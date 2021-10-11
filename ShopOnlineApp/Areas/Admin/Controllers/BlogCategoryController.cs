using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Helpers;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class BlogCategoryController : BaseController
    {
        #region private property

        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public BlogCategoryController(IBlogCategoryService blogCategoryService, IUnitOfWork unitOfWork)
        {
            _blogCategoryService = blogCategoryService;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region method

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var items = await _blogCategoryService.GetById(id);
            if (items != null)
            {
                return new OkObjectResult(items);
            }

            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }

            await _blogCategoryService.Delete(id);
            _unitOfWork.Commit();
            return new OkObjectResult(id);
        }


        [HttpPost]
        public async Task<IActionResult> SaveEntity(BlogCategoryViewModel BlogVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            BlogVm.SeoAlias = TextHelper.ToUnsignString(BlogVm.Name);
            if (BlogVm.Id == 0)
            {
                await _blogCategoryService.Add(BlogVm);
            }
            else
            {
                await _blogCategoryService.Update(BlogVm);
            }
            _unitOfWork.Commit();
            return new OkObjectResult(BlogVm);
        }
        public async Task<IActionResult> UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (sourceId == targetId)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _blogCategoryService.UpdateParentId(sourceId, targetId, items);
            _unitOfWork.Commit();
            return new OkResult();
        }

        public async Task<IActionResult> ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (sourceId == targetId)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _blogCategoryService.ReOrder(sourceId, targetId);
            _unitOfWork.Commit();
            return new OkResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _blogCategoryService.GetAll();
            return new OkObjectResult(items);
        }
        #endregion
    }
}