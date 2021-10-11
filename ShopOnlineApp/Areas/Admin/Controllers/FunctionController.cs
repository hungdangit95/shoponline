using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Extensions;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    [CheckPermission]
    public class FunctionController : BaseController
    {
        #region Initialize

        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        #endregion Initialize

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFillter(string filter)
        {
            var model = await _functionService.GetAll(filter);
            return new ObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _functionService.GetAll(string.Empty);
            var rootFunctions = model.Where(c => c.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var function in rootFunctions)
            {
                //add the parent category to the item list
                items.Add(function);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), function, items);
            }
            return new ObjectResult(items);
        }


        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await _functionService.GetById(id);

            return new ObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(FunctionViewModel functionVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (functionVm.IsUpdated)
            {
                await _functionService.Update(functionVm);
            }
            else
            {
                await _functionService.Add(functionVm);
            }

            _functionService.Save();
            return new OkObjectResult(functionVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    await _functionService.UpdateParentId(sourceId, targetId, items);
                    _functionService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReOrder(string sourceId, string targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    await _functionService.ReOrder(sourceId, targetId);
                    _functionService.Save();
                    return new OkObjectResult(sourceId);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            else
            {
                await _functionService.Delete(id);
                _functionService.Save();
                return new OkObjectResult(id);
            }
        }

        #region Private Functions
        private void GetByParentId(IEnumerable<FunctionViewModel> allFunctions,
            FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var functionsEntities = allFunctions as FunctionViewModel[] ?? allFunctions.ToArray();
            var subFunctions = functionsEntities.Where(c => c.ParentId == parent.Id);
            foreach (var cat in subFunctions)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(functionsEntities, cat, items);
            }
        }
        #endregion
    }
}