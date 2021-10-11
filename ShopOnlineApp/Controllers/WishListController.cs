using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Models;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers
{
    public class WishListController : Controller
    {

        private readonly IProductService _productService;
        public WishListController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("wishlist.html", Name = "WishList")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetWishList()
        {
            var session = HttpContext.Session.Get<List<WishListViewModel>>(CommonConstants.WishListSession);
            if (session == null)
                session = new List<WishListViewModel>();
            return new OkObjectResult(session);
        }

        [HttpPost]
        public async Task<IActionResult>  AddToWishList(int productId)
        {
            var product =await _productService.GetById(productId);
            if (product != null)
            {
                var session = HttpContext.Session.Get<List<WishListViewModel>>(CommonConstants.WishListSession);
                if (session != null)
                {
                    //Convert string to list object
                    bool hasChanged = false;

                    //Check exist with item product id
                    if (session.Any(x => x.Product.Id == productId))
                    {
                        foreach (var item in session)
                        {
                            //Update quantity for product if match product id
                            if (item.Product.Id == productId)
                            {
                                item.Quantity++;
                                hasChanged = true;
                            }
                        }
                    }
                    else
                    {
                        session.Add(new WishListViewModel
                        {
                            Product = product,
                            Quantity = 1

                        });
                        hasChanged = true;
                    }

                    //Update back to cart
                    if (hasChanged)
                    {
                        HttpContext.Session.Set(CommonConstants.WishListSession, session);
                    }
                }
                else
                {

                    var wishList = new List<WishListViewModel>();
                    wishList.Add(new WishListViewModel()
                    {
                        Product = product,
                        Quantity = 1,
                    });
                    HttpContext.Session.Set(CommonConstants.WishListSession, wishList);
                }
            }

            return new OkObjectResult(productId);
        }
        [HttpPost]
        public IActionResult RemoveFromWishList(int productId)
        {
            var session = HttpContext.Session.Get<List<WishListViewModel>>(CommonConstants.WishListSession);
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
                    HttpContext.Session.Set(CommonConstants.WishListSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }
    }
}