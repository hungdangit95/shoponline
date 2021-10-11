using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Models;
using ShopOnlineApp.Utilities.Helpers;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    //[CheckPermission]
    public class BlogController : BaseController
    {
        #region public  property 
        private readonly IBlogService _blogService;
        private readonly IConfiguration _configuration;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IBillService _billService;

        private readonly IHostingEnvironment _hostingEnvironment;
        #endregion
        #region constructer
        public BlogController(IBlogService BlogService, IConfiguration configuration, IOptions<CloudinaryImage> cloudinaryConfig, IHostingEnvironment hostingEnvironment, IBlogCategoryService BlogCategoryService, IBillService billService)
        {
            _blogService = BlogService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _blogCategoryService = BlogCategoryService;
            _billService = billService;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            //var dataReturn = await _BlogService.GetAll();
            var model = await _blogService.GetAll();
            return new OkObjectResult(model);
        }

        public async Task<IActionResult> GetAllPaging(BlogRequest request)
        {
            var model = await _blogService.GetAllPaging(request);

            return new OkObjectResult(model);

        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _blogService.GetById(id);

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
            string sFileName = $"Blog_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var blogs = await _blogService.GetAll();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Blogs");
                worksheet.Cells["A1"].LoadFromCollection(blogs, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(BlogViewModel blogVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                blogVm.SeoAlias = TextHelper.ToUnsignString(blogVm.Name);
                if (blogVm.Id == 0)
                {
                    await _blogService.Add(blogVm);
                }
                else
                {
                    await _blogService.Update(blogVm);
                }
                _blogService.Save();
                return new OkObjectResult(blogVm);
            }
        }


        //[HttpPost]
        //public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        //{
        //    if (files != null && files.Count > 0)
        //    {
        //        var file = files[0];
        //        var filename = ContentDispositionHeaderValue
        //                           .Parse(file.ContentDisposition)
        //                           .FileName
        //                           .Trim('"');

        //        string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        string filePath = Path.Combine(folder, filename);

        //        using (FileStream fs = System.IO.File.Create(filePath))
        //        {
        //            file.CopyTo(fs);
        //            fs.Flush();
        //        }
        //        _BlogService.ImportExcel(filePath, categoryId);
        //        _BlogService.Save();
        //        return new OkObjectResult(filePath);
        //    }
        //    return new NoContentResult();
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _blogService.Delete(id);
                _blogService.Save();

                return new OkObjectResult(id);
            }
        }

        //[HttpPost]
        //public IActionResult SaveQuantities(int BlogId, List<BlogQuantityViewModel> quantities)
        //{
        //    _BlogService.AddQuantity(BlogId, quantities);
        //    _BlogService.Save();
        //    return new OkObjectResult(quantities);
        //}

        //[HttpGet]
        //public IActionResult GetQuantities(int BlogId)
        //{
        //    var quantities = _BlogService.GetQuantities(BlogId);
        //    return new OkObjectResult(quantities);
        //}
        //[HttpPost]
        //public IActionResult SaveImages(int BlogId, string[] images)
        //{
        //    _BlogService.AddImages(BlogId, images);
        //    _BlogService.Save();
        //    return new OkObjectResult(images);
        //}

        //[HttpGet]
        //public IActionResult GetImages(int BlogId)
        //{
        //    var images = _BlogService.GetImages(BlogId);
        //    return new OkObjectResult(images);
        //}

        //[HttpPost]
        //public IActionResult SaveWholePrice(int BlogId, List<WholePriceViewModel> wholePrices)
        //{
        //    _BlogService.AddWholePrice(BlogId, wholePrices);
        //    _BlogService.Save();
        //    return new OkObjectResult(wholePrices);
        //}

        //[HttpGet]
        //public IActionResult GetWholePrices(int BlogId)
        //{
        //    var wholePrices = _BlogService.GetWholePrices(BlogId);
        //    return new OkObjectResult(wholePrices);
        //}

    }
}