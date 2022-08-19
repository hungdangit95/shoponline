using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopOnline.Api.ActionFilter;
using ShopOnline.Api.Authorization;
using ShopOnline.Api.Helper;
using ShopOnline.Api.Initialization;
using ShopOnlineApp.Application.Implementation;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Data.EF;
using ShopOnlineApp.Data.EF.Repositories;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ShopOnline.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var host = "mssqlserver";
            var port = "1433";
            var password = Configuration["DBPASSWORD"] ?? "Pa55w0rd2021";

            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer($"server={host},{port};user id=sa;password={password};"
                    + $"Database=Products",
               o =>
               {
                   o.MigrationsAssembly("ShopOnlineApp.Data.EF");
                   o.EnableRetryOnFailure();
               }));


            services.AddIdentity<AppUser, AppRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 8;
                o.User.RequireUniqueEmail = true;

            })
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();
            //  services.Configure<CloudinaryImage>(Configuration.GetSection("CloudinarySettings"));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
            });

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtSettings["validIssuer"],
                     ValidAudience = jwtSettings["validAudience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Base64UrlEncoder.DecodeBytes("sddddddddwewewewwwwwwwwwwwwwwwwrerevvasasaawqw"))
                 };
             });
            services.AddAutoMapper(typeof(Startup));
            //services.AddMemoryCache();
            // Add application services.
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            // services.AddSingleton(AutoMapperConfig.Config.CreateMapper() );
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddInitializationStages();
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionHandler>();
            });
            services.AddScoped<ValidationFilterAttribute>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddDistributedMemoryCache();

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("vi-VN")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en-US");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });
            //add config system
            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));
            //end config

            //repository
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IFunctionRepository, FunctionRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IProductQuantityRepository, ProductQuantityRepository>();

            services.AddTransient<IProductTagRepository, ProductTagRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IBusinessRepository, BusinessRepository>();
            services.AddTransient<IBusinessActionRepository, BusinessActionRepository>();
            services.AddTransient<IBillRepository, BillRepository>();
            services.AddTransient<IBillDetailRepository, BillDetailRepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<ISizeRepository, SizeRepository>();
            services.AddTransient<IProductImageRepository, ProductImageRepository>();
            services.AddTransient<IWholePriceRepository, WholePriceRepository>();
            services.AddTransient<IGrantPermissionRepository, GrantPermissionRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IBlogTagRepository, BlogTagRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ISystemConfigRepository, SystemConfigRepository>();
            services.AddTransient<ISlideRepository, SlideRepository>();
            services.AddTransient<IFooterRepository, FooterRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();
            services.AddTransient<IBlogCategoryRepository, BlogCategoryRepository>();
            services.AddTransient<IBlogCommentRepository, BlogCommentRepository>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            //services.AddTransient<IAppUserRoleRepository, AppUserRoleRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //service
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, ShopOnlineClaimsPrincipalFactory>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IBusinessService, BusinessService>();
            services.AddTransient<IBusinessActionService, BusinessActionService>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<IGrantPermissionService, GrantPermissionService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICommonService, CommonService>();
            //services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IPageService, PageService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<ISizeService, SizeService>();
            services.AddTransient<IBlogCategoryService, BlogCategoryService>();
            services.AddTransient<IBlogCommentService, BlogCommentService>();
            services.AddTransient<ISlideService, SlideService>();
            services.AddTransient<IProductQuantityService, ProductQuantityService>();
            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            services.AddTransient<IOAuthenticationService, OAuthenticationService>();
            //.AddNewtonsoftJson(options =>
            //{

            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //});

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ShopOnline API",
                    Version = "v1",
                    Description = "An API to perform ShopOnline operations",
                    Contact = new OpenApiContact
                    {
                        Name = "hungdh",
                        Email = "hungdangit95@gmail.com",
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                     {
                        {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        },
                        Name = "Bearer",
                        },
                        new List<string>()
                        }
                });
            });

            services.AddMediatR(typeof(AssemblyReference).Assembly);

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "redis:6379,abortConnect=False";
            });

            //services.AddControllers(config =>
            //{
            //    config.RespectBrowserAcceptHeader = true;
            //    config.ReturnHttpNotAcceptable = true;
            //}).AddXmlDataContractSerializerFormatters();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            //logger.AddFile("Logs/shoponline-{Date}.txt");
            app.UseSwagger();

            app.UseSwaggerUI(c => { 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "";
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseCors(opts =>
            {
                opts.AllowAnyHeader();
                opts.AllowAnyMethod();
                opts.AllowCredentials();
                opts.SetIsOriginAllowed(origin => true);
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("api", "api/v1/{controller=Home}");
            });

            app.UseCors(MyAllowSpecificOrigins);

        }

    }
}
