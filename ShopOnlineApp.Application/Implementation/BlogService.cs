using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Application.ViewModels.Tag;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Constants;
using ShopOnlineApp.Utilities.Enum;
using ShopOnlineApp.Utilities.Helpers;

namespace ShopOnlineApp.Application.Implementation
{
    public class BlogService : IBlogService
    {
        #region private method
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogCategoryRepository _categoryRepository;
        private readonly IConfigurationProvider _mapperConfig;
        #endregion

        #region Constructor
        public BlogService(IBlogRepository blogRepository,
            IBlogTagRepository blogTagRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork, IBlogCategoryRepository categoryRepository, IConfigurationProvider mapperConfig)
        {
            _blogRepository = blogRepository;
            _blogTagRepository = blogTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapperConfig = mapperConfig;
        }
        #endregion

        #region Public method
        public async Task<BlogViewModel> Add(BlogViewModel blogVm)
        {
            var blog = new BlogViewModel().Map(blogVm);

            if (!string.IsNullOrEmpty(blog.Tags))
            {
                var tags = blog.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!(await _tagRepository.FindAll(x => x.Id == tagId)).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.BlogTag
                        };
                        await _tagRepository.Add(tag);
                    }

                    var blogTag = new BlogTag { TagId = tagId };
                    blog.BlogTags.Add(blogTag);
                }
            }
            await _blogRepository.Add(blog);
            return blogVm;
        }

        public async Task Delete(int id)
        {
            await _blogRepository.Remove(id);
        }

        public async Task<List<BlogViewModel>> GetAll()
        {
            return new BlogViewModel().Map(await _blogRepository.FindAll(c => c.BlogTags)).ToList();
        }

        public async Task<BaseReponse<ModelListResult<BlogViewModel>>> GetAllPaging(BlogRequest request)
        {
            try
            {
                var response = from c in await _categoryRepository.FindAll()
                               join p in (await _blogRepository.FindAll()).AsNoTracking() on c.Id equals p.BlogCategoryId
                               select new BlogViewModel
                               {
                                   Name = p.Name,
                                   Id = p.Id,
                                   BlogCategoryId = p.BlogCategoryId,
                                   BlogCategory = new BlogCategoryViewModel
                                   {
                                       Name = c.Name
                                   },
                                   Description = p.Description,
                                   Content = p.Content,
                                   DateCreated = p.DateCreated,
                                   DateModified = p.DateModified,
                                   HomeFlag = p.HomeFlag,
                                   HotFlag = p.HotFlag,
                                   SeoAlias = p.SeoAlias,
                                   SeoDescription = p.SeoDescription,
                                   SeoKeywords = p.SeoKeywords,
                                   SeoPageTitle = p.SeoPageTitle,
                                   ViewCount = p.ViewCount,
                                   Status = p.Status,
                                   Image = p.Image
                               };

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    response = response.Where(x => x.Name.Contains(request.SearchText));
                }
                else if (!string.IsNullOrEmpty(request.Name))
                {
                    response = response.Where(x => x.Name.Contains(request.Name));
                }
                else if (request.CategoryId > 0)
                {
                    response = response.Where(x => x.BlogCategoryId == request.CategoryId);
                }

                var items = response.ToList();
                var totalCount = items.Count();
                if (request.IsPaging)
                {
                    response = response.OrderByDescending(x => x.DateModified)
                        .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
                }

                return new BaseReponse<ModelListResult<BlogViewModel>>
                {
                    Data = new ModelListResult<BlogViewModel>()
                    {
                        Items = items,
                        Message = Message.Success,
                        RowCount = totalCount,
                        PageSize = request.PageSize,
                        PageIndex = request.PageIndex
                    },
                    Message = Message.Success,
                    Status = (int)QueryStatus.Success
                };
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw;
            }
            
        }
        public async Task<BlogViewModel> GetById(int id)
        {
            return new BlogViewModel().Map(await _blogRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task Update(BlogViewModel blog)
        {
            await _blogRepository.Update(new BlogViewModel().Map(blog));
            if (!string.IsNullOrEmpty(blog.Tags))
            {
                var blogTags = await _blogTagRepository.FindAll(x => x.BlogId == blog.Id);
                await _blogTagRepository.RemoveMultiple(blogTags.ToList());

                string[] tags = blog.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!(await _tagRepository.FindAll(x => x.Id == tagId)).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        await _tagRepository.Add(tag);
                    }
                    await _blogTagRepository.RemoveMultiple((await _blogTagRepository.FindAll(x => x.Id == blog.Id)).ToList());
                    BlogTag blogTag = new BlogTag
                    {
                        BlogId = blog.Id,
                        TagId = tagId
                    };
                    await _blogTagRepository.Add(blogTag);
                }
            }
        }

        public async Task<IEnumerable<BlogViewModel>> GetLastest(int top)
        {
            return new BlogViewModel().Map((await _blogRepository.FindAll(x => x.Status == Status.Active)).OrderByDescending(x => x.DateCreated)
                .Take(top)).ToList();
        }

        public async Task<List<BlogViewModel>> GetHotProduct(int top)
        {
            return new BlogViewModel().Map((await _blogRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true))
                    .OrderByDescending(x => x.DateCreated)
                    .Take(top)).ToList();
        }

        public async Task<List<BlogViewModel>> GetListPaging(int page, int pageSize, string sort)
        {
            var query = await _blogRepository.FindAll(x => x.Status == Status.Active);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            return new BlogViewModel().Map(query.Skip((page - 1) * pageSize)
                    .Take(pageSize)).ToList();
        }

        public async Task<List<string>> GetListByName(string name)
        {
            return (await _blogRepository.FindAll(x => x.Status == Status.Active
            && x.Name.Contains(name))).Select(y => y.Name).ToList();
        }

        public async Task<List<BlogViewModel>> Search(string keyword, int page, int pageSize, string sort)
        {
            var query = await _blogRepository.FindAll(x => x.Status == Status.Active
            && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            return new BlogViewModel().Map(query.Skip((page - 1) * pageSize)
                    .Take(pageSize)).ToList();
        }

        public async Task<List<BlogViewModel>> GetReatedBlogs(int id, int top)
        {
            return new BlogViewModel().Map((await _blogRepository
                    .FindAll(x => x.Status == Status.Active && x.Id != id))
                    .OrderByDescending(x => x.DateCreated)
                    .Take(top))
            .ToList();
        }

        public async Task<List<TagViewModel>> GetListTagById(int id)
        {
            return (await _blogTagRepository.FindAll(x => x.BlogId == id, c => c.Tag))
                .Select(y => y.Tag)
                .ProjectTo<TagViewModel>(_mapperConfig)
                .ToList();
        }

        public async Task IncreaseView(int id)
        {
            var product = await _blogRepository.FindById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount += 1;
            else
                product.ViewCount = 1;
        }

        public Task<List<BlogViewModel>> GetListByTag(string tagId, int page, int pagesize, out int totalRow)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<BlogViewModel>> GetListByTag(string tagId, int page, int pageSize)
        {
            var query = from p in await _blogRepository.FindAll()
                        join pt in await _blogTagRepository.FindAll()
                        on p.Id equals pt.BlogId
                        where pt.TagId == tagId && p.Status == Status.Active
                        orderby p.DateCreated descending
                        select p;
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new BlogViewModel().Map(query).ToList();

        }

        public async Task<TagViewModel> GetTag(string tagId)
        {
            return new TagViewModel().Map(await _tagRepository.FindSingle(x => x.Id == tagId));
        }

        public async Task<List<BlogViewModel>> GetList(string keyword)
        {
            var query = !string.IsNullOrEmpty(keyword) ?
               new BlogViewModel().Map(await _blogRepository.FindAll(x => x.Name.Contains(keyword))).ToList()
                : new BlogViewModel().Map(await _blogRepository.FindAll());
            return query.ToList();
        }

        public async Task<List<TagViewModel>> GetListTag(string searchText)
        {
            return new TagViewModel().Map(await _tagRepository.FindAll(x => x.Type == CommonConstants.ProductTag
                                                                      && searchText.Contains(x.Name))).ToList();
        }

        #endregion
    }
}
