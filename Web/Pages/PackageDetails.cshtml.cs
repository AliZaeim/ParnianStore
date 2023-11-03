using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class PackageDetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStoreService _storeService;
        public PackageDetailsModel(IProductService productService, IStoreService storeService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _storeService = storeService;
        }
        public PackageDetailsVM PackageDetailsVM { get; set; } = new PackageDetailsVM();
        public DiscountOptionVM DiscountOptionVM { get; set; }
        public InitialInfo InitialInfo { get; set; }
        public int Inventory { get; set; }
        public async Task<IActionResult> OnGetAsync(string name)
        {
            Core.Utility.CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
            if (Core.Utility.CookieExtensions.ExistCookie("cartid"))
            {
                string cookieCartId = Core.Utility.CookieExtensions.ReadCookie("cartid");
                if (cookieCartId != null)
                {

                    var cart = await _storeService.GetUserCartByCookieAsync(cookieCartId);
                    if (cart != null)
                    {
                        int DifDays = (DateTime.Now - cart.DateCreated).Days;
                        if (DifDays > 3)
                        {
                            cart.IsActive = false;
                            _storeService.EditCart(cart);
                            await _storeService.SaveChangesAsync();
                            cart = null;
                            bool res = Core.Utility.CookieExtensions.RemoveCookie("cartid");
                        }
                        if (cart.CheckOut)
                        {
                            bool res = Core.Utility.CookieExtensions.RemoveCookie("cartid");

                        }
                    }


                }
            }
            Package package = await _productService.GetPackageByEnName(name);
            if (package == null)
            {
                return NotFound();
            }
            if (!package.IsActive)
            {
                return RedirectToPage("404");
            }
            InitialInfo = await _productService.GetInitialInfoAsync();
            DiscountOptionVM = await _productService.HasPackageDiscountAsync(package.PkId);
            Inventory = await _productService.GetPackage_Inventory_InStoreAsync(package.PkId);
            PackageDetailsVM.PackageId = package.PkId;
            PackageDetailsVM.Package = package;
            PackageDetailsVM.PackageName = package.PkTitle;
            PackageGroup group = await _productService.GetPackageGroupByIdAsync((int)package.PgId);
            List<Package> Gpackages = group.Packages.ToList();
            Gpackages = Gpackages.Where(w => w.IsActive).ToList();
            Gpackages = Gpackages.Where(w => w.PkId != package.PkId).ToList();
            PackageDetailsVM.GroupPackages = Gpackages;
            PackageDetailsVM.Products = await _productService.GetPackge_Products(package.PkId);
            if (User.Identity.IsAuthenticated)
            {
                User user = await _productService.GetUserByUserNameAsync(User.Identity.Name);
                if (user != null)
                {
                    PackageDetailsVM.CountInCart = _productService.GetPackage_Count_inCart(package.PkId, User.Identity.Name);
                    PackageDetailsVM.Name = user.Name;
                    PackageDetailsVM.Family = user.Family;
                    PackageDetailsVM.Cellphone = user.Cellphone;
                    if (PackageDetailsVM.CountInCart != 0)
                    {
                        PackageDetailsVM.CartHasPackage = true;
                    }
                }
            }
            else
            {
                Cart Cookiecart = null;
                if (Core.Utility.CookieExtensions.ExistCookie("cartid"))
                {
                    string cid = Core.Utility.CookieExtensions.ReadCookie("cartid");
                    Cookiecart = await _storeService.GetUserCartByCookieAsync(cid);
                    if (Cookiecart != null)
                    {
                        if (Cookiecart.IsActive && !Cookiecart.CheckOut)
                        {
                            int pcount = 0;
                            if (Cookiecart.CartItems.Any(a => a.ProductId == package.PkId && a.Kind=="pk"))
                            {
                                pcount = Cookiecart.CartItems.SingleOrDefault(x => x.ProductId == package.PkId).Quantity;
                                PackageDetailsVM.CartHasPackage = true;
                            }
                            PackageDetailsVM.CountInCart = pcount;
                        }

                    }
                }
            }
            package.Views += 1;
            _productService.UpdatePackage(package);
            await _productService.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPost(PackageDetailsVM packageDetailsVM)
        {
            if (!ModelState.IsValid)
                return Page();

            PackageComment packageComment = new()
            {
                CreatedDate = DateTime.Now,
                Name = packageDetailsVM.Name,
                Family = packageDetailsVM.Family,
                PkId = packageDetailsVM.PackageId,
                Cellphone = packageDetailsVM.Cellphone,
                Comment = packageDetailsVM.Comment
            };
            _productService.CreatePackageComment(packageComment);
            await _productService.SaveChangesAsync();
            Package package = await _productService.GetPackageByIdAsync(packageDetailsVM.PackageId);
            TempData["saved"] = true;
            return RedirectToPage("/PackageDetails", new { name = package.PkEnTitle.Replace(" ","-") });
        }
    }
}
