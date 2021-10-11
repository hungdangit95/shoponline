using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        #region Private property
        private readonly IFunctionRepository _functionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;

        #endregion

        public FunctionService(IMapper mapper,
            IFunctionRepository functionRepository,
            IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, RoleManager<AppRole> roleManager)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public async Task<bool> CheckExistedId(string id)
        {
            return await _functionRepository.FindById(id) != null;
        }

        public async Task Add(FunctionViewModel functionVm)
        {
            if (!await CheckExistedId(functionVm.Id))
            {
                var function = new FunctionViewModel().Map(functionVm);
                await _functionRepository.Add(function);
            }
        }

        public async Task Delete(string id)
        {
            await _functionRepository.Remove(id);
        }

        public async Task<FunctionViewModel> GetById(string id)
        {
            var function = await _functionRepository.FindSingle(x => x.Id == id);
            return new FunctionViewModel().Map(function);
        }

        public async Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = (await _functionRepository.FindAll(x => x.Status == Status.Active)).AsNoTracking().AsParallel();

            if (!string.IsNullOrEmpty(filter))
                query = query.AsParallel().AsUnordered().WithDegreeOfParallelism(3).Where(x => x.Name.Contains(filter));

            var results = query.OrderBy(x => x.ParentId);

            return new FunctionViewModel().Map(results).ToList();
        }

        public async Task<List<FunctionViewModel>> GetFunctionByRoles(FunctionRequest request)
        {
            string[] roles = request.Roles.ToArray();
            var functionIds = (await _permissionRepository.FindAll(await BuildingQuery(roles))).AsNoTracking().AsParallel().AsUnordered().WithDegreeOfParallelism(3).Where(x => x.CanRead).Select(x => x.FunctionId);
            var ids = new List<string>();
            foreach (var id in functionIds)
            {
                var functionDetail = await _functionRepository.FindById(id);
                if (functionDetail.ParentId != null)
                {
                    ids.AddRange(new List<string>
                    {
                        id,
                        functionDetail.ParentId
                    });
                }
                else
                {
                    ids.Add(id);
                }
            }

            var function = from fun in (await _functionRepository.FindAll()).AsNoTracking()
                           join id in ids.Distinct()
                           on fun.Id equals id
                           select fun;

            return new FunctionViewModel().Map(function).ToList();
        }

        public async Task<IEnumerable<FunctionViewModel>> GetAllWithParentId(string parentId)
        {
            return new FunctionViewModel().Map(await _functionRepository.FindAll(x => x.ParentId == parentId)).ToList();
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task Update(FunctionViewModel functionVm)
        {
            var functionDb = await _functionRepository.FindById(functionVm.Id);
            if (functionDb != null)
            {
                functionDb.Name = functionVm.Name;
                functionDb.IconCss = functionVm.IconCss;
                functionDb.ParentId = functionVm.ParentId;
                functionDb.SortOrder = functionVm.SortOrder;
                functionDb.URL = functionVm.URL;
                functionDb.Status = functionVm.Status;
                await _functionRepository.Update(functionDb);
                _unitOfWork.Commit();
            }
        }

        public async Task ReOrder(string sourceId, string targetId)
        {
            var source = await _functionRepository.FindById(sourceId);
            var target = await _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;
            await _functionRepository.Update(source);
            await _functionRepository.Update(target);
        }

        public async Task UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            var category = await _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            await _functionRepository.Update(category);

            var sibling = (await _functionRepository.FindAll(x => items.ContainsKey(x.Id))).AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(3);
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                await _functionRepository.Update(child);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async Task<Expression<Func<Permission, bool>>> BuildingQuery(params string[] keywords)
        {
            var predicate = PredicateBuilder.False<Permission>();

            foreach (string keyword in keywords)
            {
                var role = await _roleManager.FindByNameAsync(keyword);

                predicate = predicate.Or(p => p.RoleId == role.Id);
            }
            return predicate;
        }
    }
}
