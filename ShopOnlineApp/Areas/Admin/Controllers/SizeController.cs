using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Size;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class SizeController : BaseController
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            //var dataReturn = await _productService.GetAll();
            var model = await _sizeService.GetAll();
            return new OkObjectResult(model);
        }

        public async Task<IActionResult> GetAllPaging(SizeRequest request)
        {
            var model = await _sizeService.GetAllPaging(request);

            return new OkObjectResult(model);

        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _sizeService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(SizeViewModel sizeVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (sizeVm.Id == 0)
                {
                    await _sizeService.Add(sizeVm);
                }
                else
                {
                    await _sizeService.Update(sizeVm);
                }
                _sizeService.SaveChanges();
                return new OkObjectResult(sizeVm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([Required] int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _sizeService.Delete(id);
            _sizeService.SaveChanges();
            return new OkObjectResult(id);
        }

    }
}