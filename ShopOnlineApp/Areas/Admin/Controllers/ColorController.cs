using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Color;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class ColorController : BaseController
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            //var dataReturn = await _productService.GetAll();
            var model = await _colorService.GetAll();
            return new OkObjectResult(model);
        }

        public async Task<IActionResult> GetAllPaging(ColorRequest request)
        {
            var model = await _colorService.GetAllPaging(request);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _colorService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(ColorViewModel colorVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (colorVm.Id == 0)
                {
                    await _colorService.Add(colorVm);
                }
                else
                {
                    await _colorService.Update(colorVm);
                }
                _colorService.SaveChanges();
                return new OkObjectResult(colorVm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _colorService.Delete(id);
            _colorService.SaveChanges();
            return new OkObjectResult(id);
        }

    }
}