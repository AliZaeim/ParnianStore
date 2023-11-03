using Core.DTOs.General;

using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Web.Pages
{
    public class CartModel : PageModel
    {
        private readonly IStoreService _storeService;
        private readonly ICompService _compSerive;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartModel(IStoreService storeService, ICompService compService, IHttpContextAccessor httpContextAccessor)
        {
            _storeService = storeService;
            _compSerive = compService;
            _httpContextAccessor = httpContextAccessor;

        }

        [BindProperty]
        public CartVm CartVm { get; set; } = new();
        [BindProperty]
        public string Couponcode { get; set; }
        public InitialInfo InitialInfo { get; set; }

        public async Task OnGetAsync()
        {
            InitialInfo = await _compSerive.GetInitialInfoAsync();
            Core.Utility.CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);

            Cart cart = null; Cart Cookiecart = null; Cart Usercart = null;
            if (Core.Utility.CookieExtensions.ExistCookie("cartid"))
            {
                string cid = Core.Utility.CookieExtensions.ReadCookie("cartid");
                Cookiecart = await _storeService.GetUserCartByCookieAsync(cid);
            }
            CartVm.States = await _compSerive.GetStatesAsync();
            CartVm.Counties = await _compSerive.GetCountiesAsync();
            if (User.Identity.IsAuthenticated)
            {
                Cart LastCheckout_cart = await _storeService.GetUserLastCheckoutCartAsync(User.Identity.Name);
                Usercart = await _storeService.GetUserLastCartAsync(User.Identity.Name);
                if (Usercart != null)
                {
                    if (!Usercart.CheckOut)
                    {
                        
                        User user = await _storeService.GetUserByNameAsync(User.Identity.Name);
                        if (user != null)
                        {
                            CartVm.BuyerName = user.Name;
                            CartVm.BuyerFamily = user.Family;
                            CartVm.BuyerCellphone = user.Cellphone;
                            CartVm.RecipientName = user.Name;
                            CartVm.RecipientFamily = user.Family;
                            CartVm.RecipientPhone = user.Cellphone;
                            CartVm.States = await _compSerive.GetStatesAsync();
                            if(user.County == null)
                            {
                                if(!string.IsNullOrEmpty(LastCheckout_cart.CountyName))
                                {
                                    var county = await _storeService.GetCountyByName(LastCheckout_cart.CountyName);
                                    if(county != null)
                                    {
                                        CartVm.SatetId = county.StateId;
                                        CartVm.CountyId = county.CountyId;
                                    }
                                }
                            }
                            else
                            {
                                CartVm.SatetId = user.County.StateId;
                                CartVm.CountyId = user.CountyId;
                                CartVm.Counties = await _compSerive.GetCountiesOfStateAsync(user.County.StateId);
                            }
                            
                           
                            if(string.IsNullOrEmpty(user.Address))
                            {
                                CartVm.Address = LastCheckout_cart?.Address;
                            }
                            else
                            {
                                CartVm.Address = user.Address;
                            }
                            if(string.IsNullOrEmpty(user.PostalCode))
                            {
                                CartVm.PostalCode = LastCheckout_cart?.PostalCode;
                            }
                            else
                            {
                                CartVm.PostalCode = user.PostalCode;
                            }
                            
                            
                            Usercart.UserId = user.Id;
                            _storeService.EditCart(Usercart);
                            await _storeService.SaveChangesAsync();
                        }
                        
                       
                    }
                }
            }
            if (Usercart != null)
            {
                if (Usercart.IsActive && !Usercart.CheckOut)
                {
                    cart = Usercart;
                }
            }
            else
            {
                if (Cookiecart != null)
                {
                    if (Cookiecart.IsActive && !Cookiecart.CheckOut)
                    {
                        cart = Cookiecart;
                    }
                }
            }
            if (cart != null)
            {
                CartVm.Cart = cart;
                CartVm.CartId = cart.Id;
            }

            CartVm.States = await _compSerive.GetStatesAsync();

        }
        public async Task<IActionResult> OnPostAsync(CartVm cartVm)
        {
            if (!ModelState.IsValid)
            {
                cartVm.States = await _compSerive.GetStatesAsync();
                return Page();
            }
            ValidateCartForPayVM validateCartForPayVM = await _storeService.PrepareCartForPayVM(cartVm);

            if (validateCartForPayVM.IsPrepare)
            {
                return RedirectToAction("Index", "Payment", new { price = validateCartForPayVM.CartTotal, cartId = validateCartForPayVM.CartId });        

            }

           
            
            return Page();
        }
        
        public async Task<ActionResult> OnPostApplyDiscount()
        {
            if (!string.IsNullOrEmpty(Couponcode))
            {
                ValidateDiscountCoupenVM validateDiscountCoupenVM = await _storeService.ValidateDiscountCoupenAsync(Couponcode.Trim());
                if (validateDiscountCoupenVM.Validity)
                {
                    Cart Cookiecart = null;
                    Cart Usercart = null;
                    Core.Utility.CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
                    if (Core.Utility.CookieExtensions.ExistCookie("cartid"))
                    {
                        string cid = Core.Utility.CookieExtensions.ReadCookie("cartid");
                        Cookiecart = await _storeService.GetUserCartByCookieAsync(cid);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        Usercart = await _storeService.GetUserLastCartAsync(User.Identity.Name);
                    }
                    if (Usercart != null)
                    {
                        if (Usercart.IsActive && !Usercart.CheckOut)
                        {
                            if (!string.IsNullOrEmpty(Usercart.DiscountCoupen))
                            {
                                Usercart.DiscountCoupen = Couponcode;
                                _storeService.EditCart(Usercart);
                                await _storeService.SaveChangesAsync();
                                TempData["coupenMessage"] = "کوپن تخفیف وارد شده با موفقیت اعمال شد";
                            }
                            else
                            {
                                TempData["coupenMessage"] = "شما قبلا از کوپن تخفیف برای این کارت استفاده کرده اید !";
                            }

                        }
                    }
                    else
                    {
                        if (Cookiecart != null)
                        {
                            if (Cookiecart.IsActive && !Cookiecart.CheckOut)
                            {
                                if (!string.IsNullOrEmpty(Cookiecart.DiscountCoupen))
                                {
                                    Cookiecart.DiscountCoupen = Couponcode;
                                    _storeService.EditCart(Usercart);
                                    await _storeService.SaveChangesAsync();
                                    TempData["coupenMessage"] = "کوپن تخفیف وارد شده با موفقیت اعمال شد";
                                }
                                else
                                {
                                    TempData["coupenMessage"] = "شما قبلا از کوپن تخفیف برای این کارت استفاده کرده اید !";
                                }

                            }
                        }
                    }
                }
                else
                {
                    TempData["coupenMessage"] = validateDiscountCoupenVM.Comment;
                }

            }

            return RedirectToPage("Cart");


        }
        
    }
}
