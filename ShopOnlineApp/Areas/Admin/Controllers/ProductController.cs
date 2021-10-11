using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Authorization;
using ShopOnlineApp.Utilities.Helpers;
using ShopOnlineApp.Application.Filter;
namespace ShopOnlineApp.Areas.Admin.Controllers
{
    //[CheckPermission]
    public class ProductController : BaseController
    {
        #region public  property 
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        private readonly IProductCategoryService _productCategoryService;
        private readonly IBillService _billService;
        private readonly IAuthorizationService _authorizationService;

        private readonly IHostingEnvironment _hostingEnvironment;
        #endregion
        #region constructer
        public ProductController(IProductService productService, IConfiguration configuration, IHostingEnvironment hostingEnvironment, IProductCategoryService productCategoryService, IBillService billService, IAuthorizationService authorizationService)
        {
            _productService = productService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _productCategoryService = productCategoryService;
            _billService = billService;
            _authorizationService = authorizationService;
        }
        #endregion
        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Authentication/NoAuthenication");
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var model = await _productService.GetAll();
            return new OkObjectResult(model);
        }

        public async Task<IActionResult> GetAllPaging(ProductRequest request)
        {
            var model = await _productService.GetAllPaging(request);

            return new OkObjectResult(model);

        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _productService.GetById(id);

            return new OkObjectResult(model);
        }
        [HttpPost]
        public async Task<IActionResult> ExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Product_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var products = await _productService.GetAll();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(ProductViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == 0)
                {
                    await _productService.Add(productVm);
                }
                else
                {
                    await _productService.Update(productVm);
                }
                _productService.Save();
                return new OkObjectResult(productVm);
            }
        }


        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _productService.ImportExcel(filePath, categoryId);
                _productService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _productService.Delete(id);
                _productService.Save();

                return new OkObjectResult(id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            await _productService.AddQuantity(productId, quantities);
            _productService.Save();
            return new OkObjectResult(quantities);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuantities(int productId)
        {
            var quantities = await _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }
        [HttpPost]
        public async Task<IActionResult> SaveImages(int productId, string[] images)
        {
            await _productService.AddImages(productId, images);
            _productService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet]
        public async Task<IActionResult> GetImages(int productId)
        {
            var images = await _productService.GetImages(productId);
            return new OkObjectResult(images);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            await _productService.AddWholePrice(productId, wholePrices);
            _productService.Save();
            return new OkObjectResult(wholePrices);
        }

        [HttpGet]
        public async Task<IActionResult> GetWholePrices(int productId)
        {
            var wholePrices = await _productService.GetWholePrices(productId);
            return new OkObjectResult(wholePrices);
        }
    }
}