using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.Tag;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Constants;
using ShopOnlineApp.Utilities.Enum;
using ShopOnlineApp.Utilities.Helpers;
using Status = ShopOnlineApp.Data.Enums.Status;

namespace ShopOnlineApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductQuantityRepository _productQuantityRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IWholePriceRepository _wholePriceRepository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IProductQuantityRepository _productQuantity;
        public ProductService(IProductRepository productionRepository, IProductTagRepository productTagRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork, IProductQuantityRepository productQuantityRepository, IProductImageRepository productImageRepository, IWholePriceRepository wholePriceRepository, IProductCategoryRepository categoryRepository, IRatingRepository ratingRepository, IProductQuantityRepository productQuantity)
        {
            _productRepository = productionRepository;
            _productTagRepository = productTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _productQuantityRepository = productQuantityRepository;
            _productImageRepository = productImageRepository;
            _wholePriceRepository = wholePriceRepository;
            _categoryRepository = categoryRepository;
            _ratingRepository = ratingRepository;
            _productQuantity = productQuantity;
        }
        public async Task<List<ProductViewModel>> GetAll()
        {
            try
            {
                var response = await _productRepository.FindAllProductAsync(x=>true);
                return response.Any() ? new ProductViewModel().Map(response.AsNoTracking()).ToList() : new List<ProductViewModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<BaseReponse<ModelListResult<ProductViewModel>>> GetAllPaging(ProductRequest request)
        {
            var categoryEntities = await _categoryRepository.FindAll();
            var productEntities = await _productRepository.FindAllProductAsync(x=>true);
            var response = (from c in categoryEntities.AsNoTracking()
                            join p in productEntities.AsNoTracking() on c.Id equals p.CategoryId
                            select new ProductViewModel
                            {
                                Name = p.Name,
                                Id = p.Id,
                                CategoryId = p.CategoryId,
                                ProductCategory = new ProductCategoryViewModel()
                                {
                                    Name = c.Name
                                },
                                Description = p.Description,
                                Content = p.Content,
                                DateCreated = p.DateCreated,
                                DateModified = p.DateModified,
                                HomeFlag = p.HomeFlag,
                                HotFlag = p.HotFlag,
                                Price = p.Price,
                                OriginalPrice = p.OriginalPrice,
                                PromotionPrice = p.PromotionPrice,
                                SeoAlias = p.SeoAlias,
                                SeoDescription = p.SeoDescription,
                                SeoKeywords = p.SeoKeywords,
                                SeoPageTitle = p.SeoPageTitle,
                                Unit = p.Unit,
                                ViewCount = p.ViewCount,
                                Status = p.Status,
                                Image = p.Image
                            }).AsParallel();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                response = response.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Where(x => x.Name.Contains(request.SearchText));
            }
            else if (!string.IsNullOrEmpty(request.Name))
            {
                response = response.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Where(x => x.Name.Contains(request.Name));
            }
            else if (request?.CategoryId > 0)
            {
                response = response.Where(x => x.CategoryId == request.CategoryId);
            }

            var totalCount = response.AsParallel().AsOrdered().Count();

            if (request.IsPaging)
            {
                response = response.AsParallel().AsOrdered().WithDegreeOfParallelism(3).OrderByDescending(x => x.DateModified)
                    .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            }

            return new BaseReponse<ModelListResult<ProductViewModel>>()
            {
                Data = new ModelListResult<ProductViewModel>()
                {
                    Items = response.ToList(),
                    Message = Message.Success,
                    RowCount = totalCount,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status = (int)QueryStatus.Success
            };
        }

        public async Task<ModelListResult<ProductFullViewModel>> FilterProducts(ProductRequest request)
        {
            List<ProductFullViewModel> response = new List<ProductFullViewModel>();
            var categoryEntities = await _categoryRepository.FindAll();
            var productEntities = await _productRepository.FindAllProductAsync(x=>true);
            var productQuantityEntities = await _productQuantity.FindAll();
            if (request.ColorId > 0 && request.SizeId > 0)
            {
                response = (from c in categoryEntities.AsNoTracking()
                            join p in productEntities.AsNoTracking() on c.Id equals p.CategoryId
                            join quan in productQuantityEntities.AsNoTracking() on p.Id equals quan.ProductId
                            where quan.ColorId == request.ColorId && quan.SizeId == request.SizeId
                            select new ProductFullViewModel
                            {
                                Name = p.Name,
                                Id = p.Id,
                                CategoryId = p.CategoryId,
                                ProductCategory = new ProductCategoryViewModel
                                {
                                    Name = c.Name
                                },
                                Description = p.Description,
                                Content = p.Content,
                                DateCreated = p.DateCreated,
                                DateModified = p.DateModified,
                                HomeFlag = p.HomeFlag,
                                HotFlag = p.HotFlag,
                                Price = p.Price,
                                OriginalPrice = p.OriginalPrice,
                                PromotionPrice = p.PromotionPrice,
                                SeoAlias = p.SeoAlias,
                                SeoDescription = p.SeoDescription,
                                SeoKeywords = p.SeoKeywords,
                                SeoPageTitle = p.SeoPageTitle,
                                Unit = p.Unit,
                                ViewCount = p.ViewCount,
                                Status = p.Status,
                                Image = p.Image,
                                ColorId = quan.ColorId,
                                SizeId = quan.SizeId

                            }).ToList();
            }
            else if (request.ColorId > 0)
            {
                response = (from c in categoryEntities.AsNoTracking()
                            join p in productEntities.AsNoTracking() on c.Id equals p.CategoryId
                            join quan
                                in productQuantityEntities.AsNoTracking() on p.Id equals quan.ProductId
                            where quan.ColorId == request.ColorId
                            select new ProductFullViewModel
                            {
                                Name = p.Name,
                                Id = p.Id,
                                CategoryId = p.CategoryId,
                                ProductCategory = new ProductCategoryViewModel()
                                {
                                    Name = c.Name
                                },
                                Description = p.Description,
                                Content = p.Content,
                                DateCreated = p.DateCreated,
                                DateModified = p.DateModified,
                                HomeFlag = p.HomeFlag,
                                HotFlag = p.HotFlag,
                                Price = p.Price,
                                OriginalPrice = p.OriginalPrice,
                                PromotionPrice = p.PromotionPrice,
                                SeoAlias = p.SeoAlias,
                                SeoDescription = p.SeoDescription,
                                SeoKeywords = p.SeoKeywords,
                                SeoPageTitle = p.SeoPageTitle,
                                Unit = p.Unit,
                                ViewCount = p.ViewCount,
                                Status = p.Status,
                                Image = p.Image,
                                ColorId = quan.ColorId,
                                SizeId = quan.SizeId
                            }).ToList();
            }
            else if (request.SizeId > 0)
            {
                response = (from c in categoryEntities.AsNoTracking()
                            join p in productEntities.AsNoTracking() on c.Id equals p.CategoryId
                            join quan
                                in productQuantityEntities.AsNoTracking() on p.Id equals quan.ProductId
                            where quan.SizeId == request.SizeId
                            select new ProductFullViewModel
                            {
                                Name = p.Name,
                                Id = p.Id,
                                CategoryId = p.CategoryId,
                                ProductCategory = new ProductCategoryViewModel()
                                {
                                    Name = c.Name
                                },
                                Description = p.Description,
                                Content = p.Content,
                                DateCreated = p.DateCreated,
                                DateModified = p.DateModified,
                                HomeFlag = p.HomeFlag,
                                HotFlag = p.HotFlag,
                                Price = p.Price,
                                OriginalPrice = p.OriginalPrice,
                                PromotionPrice = p.PromotionPrice,
                                SeoAlias = p.SeoAlias,
                                SeoDescription = p.SeoDescription,
                                SeoKeywords = p.SeoKeywords,
                                SeoPageTitle = p.SeoPageTitle,
                                Unit = p.Unit,
                                ViewCount = p.ViewCount,
                                Status = p.Status,
                                Image = p.Image,
                                ColorId = quan.ColorId,
                                SizeId = quan.SizeId

                            }).ToList();
            }
            else
            {
                response = (from c in categoryEntities.AsNoTracking()
                            join p in productEntities.AsNoTracking() on c.Id equals p.CategoryId

                            select new ProductFullViewModel
                            {
                                Name = p.Name,
                                Id = p.Id,
                                CategoryId = p.CategoryId,
                                ProductCategory = new ProductCategoryViewModel()
                                {
                                    Name = c.Name
                                },
                                Description = p.Description,
                                Content = p.Content,
                                DateCreated = p.DateCreated,
                                DateModified = p.DateModified,
                                HomeFlag = p.HomeFlag,
                                HotFlag = p.HotFlag,
                                Price = p.Price,
                                OriginalPrice = p.OriginalPrice,
                                PromotionPrice = p.PromotionPrice,
                                SeoAlias = p.SeoAlias,
                                SeoDescription = p.SeoDescription,
                                SeoKeywords = p.SeoKeywords,
                                SeoPageTitle = p.SeoPageTitle,
                                Unit = p.Unit,
                                ViewCount = p.ViewCount,
                                Status = p.Status,
                                Image = p.Image,
                            }).ToList();
            }
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                response = response.AsParallel().Where(x => x.Name.Contains(request.SearchText)).ToList();
            }
            else if (!string.IsNullOrEmpty(request.Name))
            {
                response = response.AsParallel().Where(x => x.Name.Contains(request.Name)).ToList();
            }
            else if (request?.CategoryId > 0)
            {
                response = response.AsParallel().Where(x => x.CategoryId == request.CategoryId).ToList();
            }
            else if (request.ColorId > 0)
            {
                response = response.AsParallel().Where(x => x.ColorId == request.CategoryId).ToList();
            }
            else if (request.SizeId > 0)
            {
                response = response.AsParallel().Where(x => x.SizeId == request.SizeId).ToList();
            }

            var totalCount = response.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).AsParallel().AsOrdered().Count();

            if (request.IsPaging)
            {
                response = response.AsParallel().OrderByDescending(x => x.DateModified)
                    .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
            }
            return new ModelListResult<ProductFullViewModel>()
            {
                Items = response.ToList(),
                Message = Message.Success,
                RowCount = totalCount,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };
        }

        public async Task<ProductViewModel> Add(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    var tagEntities = await _tagRepository.FindAll(x => x.Id == tagId);
                    var canCreate = !tagEntities.AsNoTracking().Any();
                    if (canCreate)
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        await _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId,
                    };
                    productTags.Add(productTag);
                }

                var product = new ProductViewModel().Map(productVm);

                foreach (var productTag in productTags)
                {
                    product.ProductTags.Add(productTag);
                }
                await _productRepository.Add(product);
            }
            return productVm;
        }

        public async Task<ProductViewModel> GetById(int id)
        {
            return new ProductViewModel().Map(await _productRepository.FindById(id));
        }

        public async Task Update(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();

            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    var tagEntities = await _tagRepository.FindAll(x => x.Id == tagId);
                    var canCreated = !tagEntities.AsNoTracking().Any();
                    if (canCreated)
                    {
                        Tag tag = new Tag { Id = tagId, Name = t, Type = CommonConstants.ProductTag };
                        await _tagRepository.Add(tag);
                    }

                    var productTagEntities = await _productTagRepository.FindAll(x => x.Id == productVm.Id);
                    await _productTagRepository.RemoveMultiple(productTagEntities.AsNoTracking().ToList());

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }

            var product = new ProductViewModel().Map(productVm);

            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }
            await _productRepository.Update(product);
        }

        public async Task Delete(int id)
        {
            await _productRepository.Remove(id);
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Product product;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    product = new Product();
                    product.CategoryId = categoryId;

                    product.Name = workSheet.Cells[i, 1].Value.ToString();

                    product.Description = workSheet.Cells[i, 2].Value.ToString();

                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;

                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var promotionPrice);

                    product.PromotionPrice = promotionPrice;
                    product.Content = workSheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = workSheet.Cells[i, 7].Value.ToString();

                    product.SeoDescription = workSheet.Cells[i, 8].Value.ToString();
                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var hotFlag);

                    product.HotFlag = hotFlag;
                    bool.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;

                    product.Status = Status.Active;

                    await _productRepository.Add(product);
                }
            }
        }

        public async Task AddQuantity(int productId, List<ProductQuantityViewModel> quantities)
        {
            var dataNeedDelete = await _productQuantityRepository.FindAll(x => x.ProductId == productId);
            await _productQuantityRepository.RemoveMultiple(dataNeedDelete.AsNoTracking().ToList());
            foreach (var quantity in quantities)
            {
                await _productQuantityRepository.Add(new ProductQuantity()
                {
                    ProductId = productId,
                    ColorId = quantity.ColorId,
                    SizeId = quantity.SizeId,
                    Quantity = quantity.Quantity
                });
            }
        }

        public async Task<List<ProductQuantityViewModel>> GetQuantities(int productId)
        {
            var productQuantityEntities = await _productQuantityRepository.FindAll(x => x.ProductId == productId);
            return new ProductQuantityViewModel().Map(productQuantityEntities.AsNoTracking()).ToList();
        }

        public async Task<List<ProductImageViewModel>> GetImages(int productId)
        {
            var imageEntities = await _productImageRepository.FindAll(x => x.ProductId == productId);
            return new ProductImageViewModel().Map(imageEntities.AsNoTracking()).ToList();
        }

        public async Task AddWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            var wholePriceEntities = await _wholePriceRepository.FindAll(x => x.ProductId == productId);
            await _wholePriceRepository.RemoveMultiple(wholePriceEntities.AsNoTracking().ToList());
            foreach (var wholePrice in wholePrices)
            {
                await _wholePriceRepository.Add(new WholePrice
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });
            }
        }

        public async Task<List<WholePriceViewModel>> GetWholePrices(int productId)
        {
            var wholePriceEntities = await _wholePriceRepository.FindAll(x => x.ProductId == productId);
            return new WholePriceViewModel().Map(wholePriceEntities.AsNoTracking()).ToList();
        }

        public async Task<List<ProductViewModel>> GetLastest(int top)
        {
            var productEntities = await _productRepository.FindAllProductAsync(x => x.Status == Status.Active);
            return new ProductViewModel().Map(productEntities.AsNoTracking().OrderByDescending(x => x.DateCreated)
                .Take(top)).ToList();
        }

        public async Task<List<ProductViewModel>> GetHotProduct(int top)
        {
            var productEntities = await _productRepository.FindAllProductAsync(x => x.Status == Status.Active && x.HotFlag == true);
            return new ProductViewModel().Map(productEntities.AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(2)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)).ToList();
        }

        public async Task<List<ProductViewModel>> GetRelatedProducts(int id, int top)
        {
            var product = await _productRepository.FindById(id);
            var productEntities = await _productRepository
                .FindAll(x => x.Status == Status.Active && x.Id != id && x.CategoryId == product.CategoryId);

            return new ProductViewModel().Map(productEntities.AsNoTracking().AsParallel().AsOrdered()
                .OrderByDescending(x => x.DateCreated)
                .Take(top)).ToList();
        }

        public async Task<List<ProductViewModel>> GetRatingProducts(int top)
        {
            var resultReturn = from u in (await _productRepository.FindAllProductAsync(x=>true)).AsNoTracking().AsParallel().AsOrdered()
                               join p in (from item in (await _ratingRepository.FindAll()).AsNoTracking().AsParallel().AsOrdered()
                                          group item by item.ProductId
                    into g
                                          orderby g.Sum(x => x.Quantity + x.Price + x.Value) descending
                                          select g.Key) on u.Id equals p
                               select u;

            return new ProductViewModel().Map(resultReturn.Take(top)).ToList();
        }

        public async Task<List<ProductViewModel>> GetUpsellProducts(int top)
        {
            return new ProductViewModel().Map((await _productRepository.FindAllProductAsync(x => x.PromotionPrice != null)).AsNoTracking().AsParallel().AsOrdered()
                    .OrderByDescending(x => x.DateModified)
                    .Take(top)).ToList();
        }

        public async Task<List<TagViewModel>> GetProductTags(int productId)
        {
            var tags = (await _tagRepository.FindAll()).AsNoTracking();
            var productTags = (await _productTagRepository.FindAll()).AsNoTracking();

            var query = from t in tags
                        join pt in productTags
                        on t.Id equals pt.TagId
                        where pt.ProductId == productId
                        select new TagViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name
                        };
            return query.ToList();

        }

        public async Task<bool> CheckAvailability(int productId, int size, int color)
        {
            var quantity = await _productQuantityRepository.FindSingle(x => x.ColorId == color && x.SizeId == size && x.ProductId == productId);
            if (quantity == null)
                return false;
            return quantity.Quantity > 0;
        }

        public async Task<IEnumerable<ProductViewModel>> SearchAsync(string key,int page, int pageSize = 5)
        {
            var data= await _productRepository.FindProductsAsync(key,page,pageSize);
            return new ProductViewModel().Map(data);
        }

        public async Task AddImages(int productId, string[] images)
        {
            await _productImageRepository.RemoveMultiple((await _productImageRepository.FindAll(x => x.ProductId == productId)).AsNoTracking().AsParallel().AsOrdered().ToList());
            foreach (var image in images)
            {
                await _productImageRepository.Add(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty
                });
            }

        }
    }
}
