using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        #region private method
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IConfigurationProvider _mapConfig;
        #endregion

        #region Constructor
        public ProductCategoryService(IProductCategoryRepository productCategoryRepository,
            IUnitOfWork unitOfWork, IProductRepository productRepository, IConfigurationProvider mapConfig)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _mapConfig = mapConfig;
        }

        #endregion

        #region public method
        public async Task<ProductCategoryViewModel> Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = new ProductCategoryViewModel().Map(productCategoryVm);
            await _productCategoryRepository.Add(productCategory);

            return productCategoryVm;
        }

        public async Task Delete(int id)
        {
            await _productCategoryRepository.Remove(id);
        }
        public async Task<List<ProductCategoryViewModel>> GetAll()
        {
            try
            {
                var dataReturn = (await _productCategoryRepository.FindAll()).OrderBy(x => x.ParentId).AsNoTracking().ToList();
                var items = new ProductCategoryViewModel().Map(dataReturn).ToList();
                return items;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<List<ProductCategoryViewModel>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return (await _productCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword)))
                    .OrderBy(x => x.ParentId).AsNoTracking().ProjectTo<ProductCategoryViewModel>(_mapConfig).ToList();

            return (await _productCategoryRepository.FindAll()).AsNoTracking().OrderBy(x => x.ParentId)
                .ProjectTo<ProductCategoryViewModel>(_mapConfig)
                .ToList();
        }

        public async Task<List<ProductCategoryViewModel>> GetAllByParentId(int parentId)
        {
            var dataReturn = await _productCategoryRepository.FindAll(x => x.Status == Status.Active
              && x.ParentId == parentId);

            return new ProductCategoryViewModel().Map(dataReturn).ToList();
        }

        public async Task<ProductCategoryViewModel> GetById(int id)
        {
            return new ProductCategoryViewModel().Map(await _productCategoryRepository.FindById(id));
        }

        public async Task<List<ProductCategoryViewModel>> GetHomeCategories(int top)
        {
            var categories = new ProductCategoryViewModel().Map((await _productCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Products)).AsNoTracking().AsParallel().AsOrdered()
                .OrderBy(x => x.HomeOrder)
                .Take(top)).ToList();

            return categories;
        }

        public async Task ReOrder(int sourceId, int targetId)
        {
            var source = await _productCategoryRepository.FindById(sourceId);
            var target = await _productCategoryRepository.FindById(targetId);
            int temp;
            if (source != null && target != null)
            {
                temp = source.SortOrder;
                source.SortOrder = target.SortOrder;
                target.SortOrder = temp;
            }
            await _productCategoryRepository.Update(source);
            await _productCategoryRepository.Update(target);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        //public List<ProductCategoryViewModel> Unflatern()
        //{
        //    var listCategory = new ProductCategoryViewModel().Map(_productCategoryRepository.FindAll());
        //    List<ProductCategoryViewModel> lstProductCategoryViewModels= new List<ProductCategoryViewModel>();

        //    var productCategoryViewModels = listCategory as ProductCategoryViewModel[] ?? listCategory.ToArray();
        //    foreach (var item in productCategoryViewModels)
        //    {
        //        if (item.ParentId == null)
        //        {
        //            item.Children = productCategoryViewModels.Where(x => x.ParentId == item.Id).ToList();

        //        }
        //        else
        //        {

        //        }

        //    }

        //    return lstProductCategoryViewModels;

        //}

        public async Task Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = new ProductCategoryViewModel().Map(productCategoryVm);

            await _productCategoryRepository.Update(productCategory);
        }

        public async Task UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = await _productCategoryRepository.FindById(sourceId);

            sourceCategory.ParentId = targetId;

            await _productCategoryRepository.Update(sourceCategory);

            var sibling = (await _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id))).AsNoTracking().AsParallel().AsOrdered();
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];

                await _productCategoryRepository.Update(child);
            }
        }
        #endregion
    }
}
