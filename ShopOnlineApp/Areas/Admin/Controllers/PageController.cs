using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Page;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class PageController : BaseController
    {
        public readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var model = await _pageService.GetAll();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _pageService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(PageRequest request)
        {
            var model = await _pageService.GetAllPaging(request);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(PageViewModel pageVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (pageVm.Id == 0)
            {
                await _pageService.Add(pageVm);
            }
            else
            {
                await _pageService.Update(pageVm);
            }
            _pageService.SaveChanges();
            return new OkObjectResult(pageVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _pageService.Delete(id);
            _pageService.SaveChanges();

            return new OkObjectResult(id);
        }
    }
}