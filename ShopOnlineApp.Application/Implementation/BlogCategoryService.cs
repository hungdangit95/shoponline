using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfigurationProvider _mapConfig;

        public BlogCategoryService(IBlogCategoryRepository blogCategoryRepository,
            IUnitOfWork unitOfWork, IBlogRepository blogRepository, IConfigurationProvider mapConfig)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _unitOfWork = unitOfWork;
            _mapConfig = mapConfig;
        }

        public async Task<BlogCategoryViewModel> Add(BlogCategoryViewModel blogCategoryVm)
        {
            var blogCategory = new BlogCategoryViewModel().Map(blogCategoryVm);
            blogCategory.DateCreated = DateTime.Now;
            blogCategory.DateModified = DateTime.Now;
            await _blogCategoryRepository.Add(blogCategory);
            return blogCategoryVm;
        }

        public async Task Delete(int id)
        {
            await _blogCategoryRepository.Remove(id);
        }

        public async Task<List<BlogCategoryViewModel>> GetAll()
        {
            try
            {
                var blogCategoryEntities = await _blogCategoryRepository.FindAll();
                var items = new BlogCategoryViewModel().Map(blogCategoryEntities.OrderBy(x => x.ParentId).AsNoTracking()).ToList();
                return items;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<BlogCategoryViewModel>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return (await _blogCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword)))
                    .OrderBy(x => x.ParentId).AsNoTracking().ProjectTo<BlogCategoryViewModel>(_mapConfig).ToList();

            return (await _blogCategoryRepository.FindAll()).AsNoTracking().OrderBy(x => x.ParentId)
                .ProjectTo<BlogCategoryViewModel>(_mapConfig)
                .ToList();
        }


        public async Task<List<BlogCategoryViewModel>> GetAllByParentId(int parentId)
        {
            var dataReturn = await _blogCategoryRepository.FindAll(x => x.Status == Status.Active
              && x.ParentId == parentId);
            return new BlogCategoryViewModel().Map(dataReturn).ToList();
        }

        public async Task<BlogCategoryViewModel> GetById(int id)
        {
            return new BlogCategoryViewModel().Map(await _blogCategoryRepository.FindById(id));
        }

        //public async Task<List<BlogCategoryViewModel>> GetHomeCategories(int top)
        //{
        //    var categories = new BlogCategoryViewModel().Map(_blogCategoryRepository
        //        .FindAll(x => x.HomeFlag == true, c => c.Blogs)
        //        .OrderBy(x => x.HomeOrder)
        //        .Take(top)).ToList();

        //    foreach (var category in categories)
        //    {
        //        var item = new BlogViewModel().Map(_blogCategoryRepository.FindAll(x => x.HomeFlag.Value && x.CategoryId == category.Id)
        //            .OrderByDescending(x => x.DateCreated)
        //            .Take(5)).ToList();
        //    }
        //    return categories;
        //}

        public async Task ReOrder(int sourceId, int targetId)
        {
            var source = await _blogCategoryRepository.FindById(sourceId);
            var target = await _blogCategoryRepository.FindById(targetId);
            if (source != null && target != null)
            {
                var temp = source.SortOrder;
                source.SortOrder = target.SortOrder;
                target.SortOrder = temp;
            }
            await _blogCategoryRepository.Update(source);
            await _blogCategoryRepository.Update(target);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        //public List<BlogCategoryViewModel> Unflatern()
        //{
        //    var listCategory = new BlogCategoryViewModel().Map(_blogCategoryRepository.FindAll());
        //    List<BlogCategoryViewModel> lstBlogCategoryViewModels= new List<BlogCategoryViewModel>();

        //    var BlogCategoryViewModels = listCategory as BlogCategoryViewModel[] ?? listCategory.ToArray();
        //    foreach (var item in BlogCategoryViewModels)
        //    {
        //        if (item.ParentId == null)
        //        {
        //            item.Children = BlogCategoryViewModels.Where(x => x.ParentId == item.Id).ToList();

        //        }
        //        else
        //        {

        //        }

        //    }

        //    return lstBlogCategoryViewModels;

        //}

        public async Task Update(BlogCategoryViewModel blogCategoryVm)
        {
            var blogCategory = new BlogCategoryViewModel().Map(blogCategoryVm);
            blogCategory.DateCreated = DateTime.Now;
            blogCategory.DateModified = DateTime.Now;
            await _blogCategoryRepository.Update(blogCategory);

        }
        public async Task UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = await _blogCategoryRepository.FindById(sourceId);

            sourceCategory.ParentId = targetId;

            await _blogCategoryRepository.Update(sourceCategory);

            var sibling = await _blogCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                await _blogCategoryRepository.Update(child);
            }

        }
    }

}
