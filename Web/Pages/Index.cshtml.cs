using Core.DTOs.General;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        public IndexModel(IUserService userService, IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _productService = productService;
        }
        public string Code { get; set; }

        public List<Product> SearchProducts { get; set; } = new List<Product>();
        public List<Package> SearchPackages { get; set; } = new List<Package>();
        public string HowToUse { get; set; }
        public SitePage SitePage { get; set; }
        public string FavIcon { get; set; } = "favicon.png";
        public List<Notification> Notifications { get; set; } = new();
        public List<ProductGroup> ProductGroups { get; set; }
        public List<PackageGroup> PackageGroups { get; set; }
        public List<Product> DiscountedProducts { get; set; }
        public List<Product> PopularProducts { get; set; }
        public List<Product> Products { get; set; }
        public List<Package> Packages { get; set; }
        public string Cur { get; set; } = "ریال";
        public bool ExistFractionSlider { get; set; }
        public bool ExistSlider { get; set; }
        public async Task OnGetAsync()
        {
            CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
            if (CookieExtensions.ExistCookie("cartid"))
            {
                string cookieCartId = CookieExtensions.ReadCookie("cartid");
                if (cookieCartId != null)
                {

                    var cart = await _userService.GetUserCartByCookieAsync(cookieCartId);
                    if(cart != null)
                    {
                        int DifDays = (DateTime.Now - cart.DateCreated).Days;
                        if (DifDays > 3)
                        {
                            cart.IsActive = false;
                            _userService.EditCart(cart);
                            await _userService.SaveChangesAsync();
                            cart = null;
                            bool res=CookieExtensions.RemoveCookie("cartid");
                        }
                        if(cart.CheckOut)
                        {
                            bool res = CookieExtensions.RemoveCookie("cartid");
                           
                        }
                    }


                }
            }
            SitePage = await _userService.GetSitePageByEnNameAsync("index");
            InitialInfo initial = await _userService.GetInitialInfoAsync();
            ExistFractionSlider = await _productService.ExistFractionSliderAsync();
            ExistSlider = await _productService.ExistSliderAsync();
            if (initial != null)
            {
                if (!string.IsNullOrEmpty(initial.FavIcon))
                {
                    FavIcon = initial.FavIcon;
                }
                
            }

            if (!string.IsNullOrEmpty(Search))
            {
                if (!Search.Contains("طریقه مصرف"))
                {
                    List<Product> products = await _productService.GetProductsAsync();
                    products = products.Where(w => w.IsActive).ToList();
                    List<Product> pr = products.Where(w => w.ProductName.Contains(Search.Trim())).ToList();
                    List<Product> prg = products.Where(w => w.ProductGroup.ProductGroupTitle.Contains(Search.Trim())).ToList();
                    if (pr != null)
                    {
                        SearchProducts.AddRange(pr);
                    }
                    if (prg != null)
                    {
                        SearchProducts.AddRange(prg);
                    }
                    SearchProducts = SearchProducts.Distinct().ToList();
                }
                else
                {
                    //List<Product> products = await _productService.GetProductsAsync();
                    string[] srchList = Search.Replace(" ", "-").Split("-");
                    string productName = string.Empty;
                    if (srchList.Length > 2)
                    {
                        productName = srchList[2].ToString();
                    }

                    Product product = await _productService.GetProductByName(productName);
                    SearchProducts.Add(product);
                    if (product != null)
                    {
                        HowToUse = product.HowToUse;
                    }
                    else
                    {
                        HowToUse = "محصولی با این نام موجود نمی باشد !";
                    }
                }

            }
            else
            {
                ProductGroups = await _productService.GetProductGroupsAsync();
                PackageGroups = await _productService.GetPackageGroupsAsync();
                Products = await _productService.GetProductsAsync();
                Products = Products.Where(w => w.IsActive).ToList();
                PopularProducts = Products.OrderByDescending(x => x.Popularity).Take(12).ToList();
                Packages = await _productService.GetPackagesAsync();
                Packages = Packages.Where(w => w.IsActive).ToList();

            }



        }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Search { get; set; }
        public async Task<IActionResult> OnPost()
        {
            SitePage sitePage = await _userService.GetSitePageByEnNameAsync("index");
            var initial = await _productService.GetInitialInfoAsync();
            ExistFractionSlider = await _productService.ExistFractionSliderAsync();
            ExistSlider = await _productService.ExistSliderAsync();
            FavIcon = "favicon.png";
            if (initial != null)
            {
                FavIcon = initial.FavIcon;
                Cur = initial.SiteCurrency;
            }
            ProductGroups = await _productService.GetProductGroupsAsync();
            Products = await _productService.GetProductsAsync();
            Products = Products.Where(w => w.IsActive).ToList();
            PackageGroups = await _productService.GetPackageGroupsAsync();
            Packages = await _productService.GetPackagesAsync();
            Packages = Packages.Where(w => w.IsActive).ToList();
            DiscountedProducts = await _productService.GetDiscountedProductsAsync();
            DiscountedProducts = DiscountedProducts.Where(w => w.IsActive).ToList();
            Notifications = await _productService.GetNotifications();
            Notifications = Notifications.Where(w => w.IsActive).ToList();
            CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);

            if (!string.IsNullOrEmpty(Search))
            {

                string[] srchList = Search.Split(" ");
                List<Product> products = await _productService.GetProductsAsync();
                products = products.Where(w => w.IsActive).ToList();
                List<Package> packages = await _productService.GetPackagesAsync();
                packages = packages.Where(w => w.IsActive).ToList();
                List<Product> pr = null;
                if (srchList.Length == 1)
                {
                    pr = products.Where(w =>
                       w.ProductName.Trim() == Search.Trim() ||
                       w.ProductNameList.Intersect(srchList).Any() ||
                       w.FeaturessList.Intersect(srchList).Any() ||
                       w.FeaturessList.Any(a => a == Search) ||
                       w.FeaturessListcompleteSplit.Intersect(srchList).Any() ||
                       w.ProductTopFeature.Contains(Search)
                      ).ToList();
                    packages = packages.Where(w =>
                       w.PkTitle.Trim().Contains(Search.Trim()) ||   w.PkFeature.Contains(Search.Trim()) ||
                       w.PkFeature.Contains(Search)).ToList();
                }
                else
                {
                    pr = products.Where(w =>
                       w.ProductName.Trim() == Search.Trim() ||
                       w.ProductFeatures.Contains(Search.Trim()) ||
                       w.ProductTopFeature.Contains(Search)
                       ).ToList();
                    packages = packages.Where(w =>
                        w.PkTitle.Trim().Contains(Search.Trim()) || w.PkFeature.Contains(Search.Trim()) ||
                        w.PkFeature.Contains(Search)).ToList();
                }
                if (pr != null)
                {
                    if (pr.Count != 0)
                    {
                        SearchProducts.AddRange(pr);
                    }

                }
                SearchProducts = SearchProducts.Distinct().ToList();
                SearchPackages = packages.ToList();

            }
            else
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    if (Email.IsValidEmail())
                    {
                        if (await _userService.ExistEmail(Email))
                        {
                            TempData["email"] = "ایمیل وارد شده قبلا ثبت شده است !";
                            return Page();
                        }
                        EmailBank Eb = new()
                        {
                            Email = Email,
                            CreatedDate = DateTime.Now
                        };
                        _userService.CreateEmail(Eb);
                        await _userService.SaveChangesAsync();
                        TempData["email"] = "ایمیل شما با موفقیت ثبت شد";
                    }
                }
            }
            return Page();
        }


    }
}
