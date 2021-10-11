using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.BusinessAction;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class BusinessActionController : BaseController
    {
        private readonly IBusinessActionService _actionService;

        public BusinessActionController(IBusinessActionService actionService)
        {
            _actionService = actionService;
        }

        public async Task<IActionResult> GetAll(BusinessActionRequest request)
        {
            return new OkObjectResult(await _actionService.GetAll(request));
        }

        public async Task<IActionResult> Index([FromQuery] string businessId)
        {
            var items = await _actionService.GetByBusinessIds(businessId);
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var currentAction = await _actionService.GetByActionId(id);
            return new OkObjectResult(currentAction);
        }


        [HttpPost]
        public async Task<IActionResult> SaveEntity(BusinessActionViewModel businessVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (businessVm.Id > 0)
            {
                await _actionService.Update(businessVm);
            }
            else
            {
                await _actionService.Update(businessVm);
            }
            return new OkObjectResult(businessVm);
        }



    }
}
