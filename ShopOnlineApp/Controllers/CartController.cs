using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Annoucement;
using ShopOnlineApp.Application.ViewModels.Bill;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Infrastructure.NganLuongAPI;
using ShopOnlineApp.Models;
using ShopOnlineApp.Services;
using ShopOnlineApp.SignalR;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers
{
    public class CartController : Controller
    {
        public readonly IProductService _productService;
        public readonly IBillService _billService;
        public readonly IViewRenderService _viewRenderService;
        public readonly IEmailSender _emailSender;
        public readonly IConfiguration _configuration;
        public readonly IColorService _color;
        public readonly ISizeService _size;
        private readonly IProductQuantityService _quantityService;
        private readonly IHubContext<OnlineShopHub> _hubContext;
        private readonly IProductRepository _productRepository;
        //private string merchantId = ConfigHelper.GetByKey("MerchantId");
        //private string merchantPassword = ConfigHelper.GetByKey("MerchantPassword");
        //private string merchantEmail = ConfigHelper.GetByKey("MerchantEmail");


        public CartController(IProductService productService, IBillService billService, IViewRenderService viewRenderService, IEmailSender emailSender, IConfiguration configuration, IColorService color, ISizeService size, IHubContext<OnlineShopHub> hubContext, IProductQuantityService quantityService, IProductRepository productRepository)
        {
            _productService = productService;
            _billService = billService;
            _viewRenderService = viewRenderService;
            _emailSender = emailSender;
            _configuration = configuration;
            _color = color;
            _size = size;
            _hubContext = hubContext;
            _quantityService = quantityService;
            _productRepository = productRepository;
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View();
        }

        #region AJAX Request
        /// <summary>
        /// Get list item
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
                session = new List<ShoppingCartViewModel>();
            return new OkObjectResult(session);
        }
        /// <summary>
        /// Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        [Route("checkout.html", Name = "Checkout")]
        [HttpGet]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel();

            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/login.html");
            }
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                if (session.Any(x => x.Color == null || x.Size == null))
                {
                    return Redirect("/cart.html");
                }
            }

            model.Carts = session;
            return View(model);
        }
        //[Route("checkout.html", Name = "Checkout")]
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public async Task<IActionResult> Checkout(CheckoutViewModel model)
        //{
        //    var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
        //    if (ModelState.IsValid)
        //    {
        //        if (session != null)
        //        {
        //            var details = new List<BillDetailViewModel>();
        //            foreach (var item in session)
        //            {
        //                details.Add(new BillDetailViewModel()
        //                {
        //                    Product = item.Product,
        //                    Price = item.Price,
        //                    ColorId = item.Color.Id,
        //                    SizeId = item.Size.Id,
        //                    Quantity = item.Quantity,
        //                    ProductId = item.Product.Id
        //                });
        //            }
        //            var billViewModel = new BillViewModel()
        //            {
        //                CustomerMobile = model.CustomerMobile,
        //                BillStatus = BillStatus.New,
        //                CustomerAddress = model.CustomerAddress,
        //                CustomerName = model.CustomerName,
        //                CustomerMessage = model.CustomerMessage,
        //                BillDetails = details,
        //                DateCreated = DateTime.Now;
        //                PaymentMethod = model.PaymentMethod
        //            };
        //            if (User.Identity.IsAuthenticated)
        //            {
        //                billViewModel.CustomerId = Guid.Parse(User.GetSpecificDefault("UserId"));
        //            }
        //            var dataReturn = _billService.Create(billViewModel);
        //            _billService.Save();
        //            var currentLink = _configuration["CurrentLink"];
        //            RequestInfo info = new RequestInfo();
        //            info.Merchant_id = _configuration["Nganluong:MerchantId"];
        //            info.Merchant_password = _configuration["Nganluong:MerchantPassword"];
        //            info.Receiver_email = _configuration["Nganluong:MerchantEmail"];
        //            info.cur_code = "vnd";
        //            info.bank_code = model.BankCode;
        //            info.Order_code = "1";
        //            info.Total_amount = _configuration["Nganluong:money"];
        //            info.fee_shipping = "0";
        //            info.Discount_amount = "0";
        //            info.order_description = "Thanh toán đơn hàng tại Online shop";
        //            info.return_url = currentLink + "/xac-nhan-don-hang.html";
        //            info.cancel_url = currentLink + "/huy-don-hang.html";
        //            info.Buyer_fullname = model.CustomerName;
        //            info.Buyer_email = "hung.dang@cto.edu.vn";
        //            info.Buyer_mobile = model.CustomerMobile;
        //            ApiCheckoutV3 objNLChecout = new ApiCheckoutV3();
        //            ResponseInfo result = objNLChecout.GetUrlCheckout(info, model.TypePayment);
        //            if (result.Error_code == "00")
        //            {
        //                return new OkObjectResult(new
        //                {
        //                    status = true,
        //                    urlCheckout = result.Checkout_url,
        //                    message = result.Description
        //                });
        //            }
        //            try
        //            {
        //                foreach (var item in dataReturn.BillDetails)
        //                {
        //                    var colorVM = _color.GetById(item.ColorId);

        //                    if (colorVM != null)
        //                    {
        //                        item.Color = colorVM;
        //                    }
        //                    var sizeVM = _size.GetById(item.ColorId);
        //                    if (sizeVM != null)
        //                    {
        //                        item.Size = sizeVM;
        //                    }
        //                }

        //                ViewBag.BillModel = dataReturn;
        //                var announcement = new AnnouncementViewModel()
        //                {
        //                    Content = $"Bạn có một yêu cầu phê duyệt",
        //                    DateCreated = DateTime.Now,
        //                    Status = Status.Active,
        //                    Title = "User created",
        //                    UserId = User.GetUserId(),
        //                    Id = Guid.NewGuid().ToString()
        //                };
        //                await _hubContext.Clients.Client("").SendAsync("ReceiveMessage", announcement);

        //                var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail", billViewModel);
        //                await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "New bill from Panda Shop", content);
        //                ViewData["Success"] = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                ViewData["Success"] = false;
        //                ModelState.AddModelError("", ex.Message);
        //            }
        //        }
        //        model.Carts = session;
        //        HttpContext.Session.Set(CommonConstants.CartSession, new List<ShoppingCartViewModel>());
        //        return View(model);
        //    }
        //    return new OkObjectResult("");
        //}
        //test
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var sessions = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (ModelState.IsValid)
            {
                if (sessions != null)
                {
                    var details = new List<BillDetailViewModel>();

                    decimal totalMoney = 0;
                    bool canBuy = true;
                    foreach (var item in sessions)
                    {
                        totalMoney += item.Price * item.Quantity;

                        details.Add(new BillDetailViewModel()
                        {
                            Price = item.Price,
                            ColorId = item.Color.Id,
                            SizeId = item.Size.Id,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                        });

                        canBuy = await _quantityService.SellProduct(item.Product.Id, item.Quantity);

                        if (!canBuy)
                        {
                            break;
                        }
                    }

                    if (canBuy)
                    {
                        var billViewModel = new BillViewModel()
                        {
                            CustomerMobile = model.CustomerMobile,
                            BillStatus = BillStatus.New,
                            CustomerAddress = model.CustomerAddress,
                            CustomerName = model.CustomerName,
                            CustomerMessage = model.CustomerMessage,
                            BillDetails = details,
                            DateCreated = DateTime.Now,
                            PaymentMethod = model.PaymentMethod
                        };
                        if (User.Identity.IsAuthenticated)
                        {
                            billViewModel.CustomerId = Guid.Parse(User.GetSpecificDefault("UserId"));
                        }
                        var dataReturn = await _billService.Create(billViewModel);
                        _billService.Save();
                        HttpContext.Session.Set(CommonConstants.BillSession, dataReturn);
                        string vnp_Returnurl = _configuration["VNPAY:vnp_Returnurl"]; //URL nhan ket qua tra ve 
                        string vnp_Url = _configuration["VNPAY:vnp_Url"]; //URL thanh toan cua VNPAY 
                        string vnp_TmnCode = _configuration["VNPAY:vnp_TmnCode"]; //Ma website
                        string vnp_HashSecret = _configuration["VNPAY:vnp_HashSecret"]; //Chuoi bi mat
                        HttpContextAccessor accessor = new HttpContextAccessor();
                        VnPayLibrary vnpay = new VnPayLibrary(accessor);
                        vnpay.AddRequestData("vnp_Version", "2.0.0");
                        vnpay.AddRequestData("vnp_Command", "pay");
                        vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                        vnpay.AddRequestData("vnp_Locale", "vn");
                        vnpay.AddRequestData("vnp_CurrCode", "VND");
                        vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
                        vnpay.AddRequestData("vnp_OrderInfo", "Noi dung thanh toan:20190421030204");
                        vnpay.AddRequestData("vnp_OrderType", "topup"); //default value: other
                        vnpay.AddRequestData("vnp_Amount", ((int)totalMoney * 100).ToString());
                        vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                        vnpay.AddRequestData("vnp_IpAddr", vnpay.GetIpAddress());
                        vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                        vnpay.AddRequestData("vnp_BankCode", "NCB");
                        string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                        return new OkObjectResult(new
                        {
                            status = true,
                            url = paymentUrl
                        });
                    }

                    return new OkObjectResult(new
                    {
                        status = false,
                        message = "Hiện số lượng sản phẩm không đủ."
                    });
                }

                model.Carts = sessions;
                HttpContext.Session.Set(CommonConstants.CartSession, new List<ShoppingCartViewModel>());
                return View(model);
            }
            return new OkObjectResult("");
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, int color, int size)
        {
            var product = await _productService.GetById(productId);
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;

                if (session.Any(x => x.Product.Id == productId))
                {
                    foreach (var item in session)
                    {
                        if (item.Product.Id == productId)
                        {
                            item.Quantity += quantity;
                            item.Price = product.PromotionPrice ?? product.Price;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new ShoppingCartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = await _billService.GetColor(color),
                        Size = await _billService.GetSize(size),
                        Price = product.PromotionPrice ?? product.Price
                    });
                    hasChanged = true;
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                var cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel()
                {
                    Product = product,
                    Quantity = quantity,
                    Color = await _billService.GetColor(color),
                    Size = await _billService.GetSize(size),
                    Price = product.PromotionPrice ?? product.Price
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            return new OkObjectResult(productId);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        public async Task<IActionResult> UpdateCart(int productId, int quantity, int color, int size)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = await _productService.GetById(productId);
                        item.Product = product;
                        item.Size = await _billService.GetSize(size);
                        item.Color = await _billService.GetColor(color);
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _billService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public async Task<IActionResult> GetSizes()
        {
            var sizes = await _billService.GetSizes();
            return new OkObjectResult(sizes);
        }

        [HttpGet("xac-nhan-don-hang.html")]
        public async Task<ActionResult> ConfirmOrder()
        {
            //update status order
            //_orderService.UpdateStatus(int.Parse(result.order_code));
            //_orderService.Save();
            var billSession = HttpContext.Session.Get<BillViewModel>(CommonConstants.BillSession);
           
            await _billService.UpdateStatus(billSession.Id, BillStatus.InProgress);

            foreach (var bill in billSession.BillDetails)
            {
                await _quantityService.UpdateQuantityProduct(bill.ProductId, bill.Quantity);
            }

            var announcement = new AnnouncementViewModel()
            {
                Content = $"Bạn có một đơn hàng từ {billSession.CustomerName}",
                DateCreated = DateTime.Now,
                Status = Status.Active,
                Title = "New Order",
                UserId = User.GetUserId(),
                Id = Guid.NewGuid().ToString()
            };
            //  await _hubContext.Clients.Client("").SendAsync("ReceiveMessage", announcement);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);

            ViewBag.IsSuccess = true;
            ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";

            HttpContext.Session.Set(CommonConstants.CartSession, new List<ShoppingCartViewModel>());
            return View(billSession);
        }
        [HttpGet("huy-don-hang.html")]
        public ActionResult CancelOrder()
        {
            return View();
        }
        #endregion
    }
}