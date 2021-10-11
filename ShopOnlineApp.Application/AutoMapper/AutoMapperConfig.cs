using System;
using AutoMapper;
using ShopOnlineApp.Application.ViewModels;
using ShopOnlineApp.Application.ViewModels.Annoucement;
using ShopOnlineApp.Application.ViewModels.Bill;
using ShopOnlineApp.Application.ViewModels.BlogComment;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Application.ViewModels.BlogTag;
using ShopOnlineApp.Application.ViewModels.Business;
using ShopOnlineApp.Application.ViewModels.BusinessAction;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Application.ViewModels.Contact;
using ShopOnlineApp.Application.ViewModels.Feedback;
using ShopOnlineApp.Application.ViewModels.Footer;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Application.ViewModels.GranPermission;
using ShopOnlineApp.Application.ViewModels.Page;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Application.ViewModels.Role;
using ShopOnlineApp.Application.ViewModels.Size;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Application.ViewModels.SystemConfig;
using ShopOnlineApp.Application.ViewModels.Tag;
using ShopOnlineApp.Application.ViewModels.User;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            try
            {
                #region Product

                cfg.CreateMap<Product, ProductViewModel>();

                cfg.CreateMap<ProductViewModel, Product>()
                    .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
                        c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
                        c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));
                #endregion

                #region Function
                cfg.CreateMap<Function, FunctionViewModel>();
                cfg.CreateMap<FunctionViewModel, Function>();

                #endregion

                #region ProductCategory

                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>().PreserveReferences();
                cfg.CreateMap<ProductCategoryViewModel, ProductCategory>(); ;

                #endregion

                #region user
                cfg.CreateMap<AppUser, AppUserViewModel>();
                cfg.CreateMap<AppUserViewModel, AppUser>();

                #endregion
                #region approle
                cfg.CreateMap<AppRole, AppRoleViewModel>();
                cfg.CreateMap<AppRoleViewModel, AppRole>();
                #endregion approle

                #region Permission
                cfg.CreateMap<Permission, PermissionViewModel>();
                cfg.CreateMap<PermissionViewModel, Permission>();
                #endregion

                #region Bussiness
                cfg.CreateMap<Business, BusinessViewModel>();
                cfg.CreateMap<BusinessViewModel, Business>();
                #endregion

                #region BussinessAction
                cfg.CreateMap<BusinessAction, BusinessActionViewModel>();
                cfg.CreateMap<BusinessActionViewModel, BusinessAction>();
                #endregion

                #region BusinessAction

                #endregion

                #region Bill
                cfg.CreateMap<Bill, BillViewModel>();
                cfg.CreateMap<BillViewModel, Bill>();
                cfg.CreateMap<BillDetailViewModel, BillDetail>();
                #endregion
                #region BillDetail
                cfg.CreateMap<BillDetail, BillDetailViewModel>().ForMember(t => t.Bill, opt => opt.Ignore());

                #endregion

                #region Size
                cfg.CreateMap<Size, SizeViewModel>();
                cfg.CreateMap<SizeViewModel, Size>();
                #endregion

                #region Size
                cfg.CreateMap<Color, ColorViewModel>();
                cfg.CreateMap<ColorViewModel, Color>();
                #endregion

                #region ProductQuantity

                cfg.CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);
                cfg.CreateMap<ProductQuantityViewModel, ProductQuantity>();
                #endregion

                #region ProductImage

                cfg.CreateMap<ProductImage, ProductImageViewModel>();
                cfg.CreateMap<ProductImageViewModel, ProductImage>();
                #endregion

                #region WholePrice

                cfg.CreateMap<WholePrice, WholePriceViewModel>();
                cfg.CreateMap<WholePriceViewModel, WholePrice>();
                #endregion

                #region GrantPermission

                cfg.CreateMap<GrantPermission, GrantPermissionViewModel>();
                cfg.CreateMap<GrantPermissionViewModel, GrantPermission>();

                #endregion

                #region Footer

                cfg.CreateMap<Footer, FooterViewModel>();
                cfg.CreateMap<FooterViewModel, Footer>();

                #endregion

                #region Slider
                cfg.CreateMap<Slide, SlideViewModel>();
                cfg.CreateMap<SlideViewModel, Slide>();
                #endregion

                #region MyRegion

                cfg.CreateMap<SystemConfig, SystemConfigViewModel>();
                cfg.CreateMap<SystemConfigViewModel, SystemConfig>();

                #endregion

                #region Blog
                cfg.CreateMap<Blog, BlogViewModel>();
                cfg.CreateMap<BlogViewModel, Blog>();
                #endregion

                #region Tag
                cfg.CreateMap<Tag, TagViewModel>();
                cfg.CreateMap<TagViewModel, Tag>();
                #endregion

                #region Contact
                cfg.CreateMap<Contact, ContactViewModel>();
                cfg.CreateMap<ContactViewModel, Contact>();
                #endregion

                #region Feedback
                cfg.CreateMap<Feedback, FeedbackViewModel>();
                cfg.CreateMap<FeedbackViewModel, Feedback>();
                #endregion

                #region Page
                cfg.CreateMap<Page, PageViewModel>();
                cfg.CreateMap<PageViewModel, Page>();

                #endregion

                #region Rating

                cfg.CreateMap<Rating, RatingViewModel>();
                cfg.CreateMap<RatingViewModel, Rating>();

                #endregion

                #region BlogCategory
                cfg.CreateMap<BlogCategory, BlogCategoryViewModel>();
                cfg.CreateMap<BlogCategoryViewModel, BlogCategory>();
                #endregion

                #region BlogComment

                cfg.CreateMap<BlogComment, BlogCommentViewModel>();
                    
                cfg.CreateMap<BlogCommentViewModel, BlogComment>();
                #endregion

                #region BlogTag

                cfg.CreateMap<BlogTag, BlogTagViewModel>();

                cfg.CreateMap<BlogTagViewModel, BlogTag>();
                #endregion

                #region Annoucement

                cfg.CreateMap<Announcement, AnnouncementViewModel>();

                cfg.CreateMap<AnnouncementViewModel, Announcement>();

                #endregion

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

}
