using Core.Services;
using Core.Services.Interfaces;
using DataLayer.Context;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebMarkupMin.AspNetCore5;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(option => option.MultipartBodyLengthLimit = 1073741824);
            services.AddWebMarkupMin(
                options =>
                {
                    options.AllowMinificationInDevelopmentEnvironment = true;
                    options.AllowCompressionInDevelopmentEnvironment = true;
                })
                .AddHtmlMinification(
                    options =>
                    {
                        options.MinificationSettings.RemoveRedundantAttributes = true;
                        options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                        options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
                    })
                .AddHttpCompression();

            
            services.AddSession();

            services.AddMvc(option => option.EnableEndpointRouting = true);
            services.AddControllersWithViews();
            var razorBuilder=services.AddRazorPages();
            
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = true;

            });
            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);

            });

            #endregion
            #region tempkey
            var keysFolder = Path.Combine(WebHostEnvironment.ContentRootPath, "wwwroot/temp-keys");
            services.AddDataProtection()
           .SetApplicationName("Web")
           .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
           .SetDefaultKeyLifetime(TimeSpan.FromDays(180));
            #endregion
            #region DatabaseContext
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ParnianStoreConnection"));
            }
            );
            #endregion DatabaseContext

            #region IOC   
            //services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<ICompService, CompService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IStoreService, StoreService>();

            #endregion IOC

            #region Encoder

            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                    UnicodeRanges.Arabic }));

            #endregion Encoder

            services.AddElmah(options =>
            {                
                options.Path = @"parnianStoreElmah";
                options.OnPermissionCheck = context => context.User.Identity.IsAuthenticated;               

            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/404";
                    await next();
                }
            });
            app.UseStatusCodePages();
            app.UseStatusCodePagesWithRedirects("/404");
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();            
            app.UseWebMarkupMin();
            app.UseElmah();  
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "AdminArea",
                    areaName: "Manage",
                    pattern: "Manage/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();


            });
        }
       
    }
}
