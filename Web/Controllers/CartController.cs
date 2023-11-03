using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{

    public class CartController : Controller
    {
       
        private readonly IStoreService _storeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartController( IStoreService storeService, IHttpContextAccessor httpContextAccessor)
        {
           
            _storeService = storeService;
            _httpContextAccessor = httpContextAccessor;
        }
       
        public async Task<Cart> GetCartAsync()
        {
            Core.Utility.CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
            Cart cart = null;
            Cart cookieCart = null;
            if (Core.Utility.CookieExtensions.ExistCookie("cartid"))
            {
                string cid = Core.Utility.CookieExtensions.ReadCookie("cartid");
                cookieCart = await _storeService.GetUserCartByCookieAsync(cid);
            }
            if (User.Identity.IsAuthenticated)
            {
                User user = await _storeService.GetUserByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    Cart userCart = await _storeService.GetUserLastCartAsync(User.Identity.Name);
                    if (userCart.IsActive && !userCart.CheckOut)
                    {
                        cart = userCart;
                    }
                    else
                    {
                        if(cookieCart.IsActive && !cookieCart.CheckOut)
                        {
                            cart = cookieCart;
                        }
                    }
                }
            }
            else
            {
                if (cookieCart.IsActive && !cookieCart.CheckOut)
                {
                    cart = cookieCart;
                }
            }

            return cart;
            
        }
        public IActionResult ShowCart()
        {
            return ViewComponent("ShowCartComponent");
        }
        [HttpGet]
        public async Task<JsonResult> AddToCart(int productId, int count,string op="eq",string kind="pr")
        {
            Core.Utility.CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
            User user = null;
            Cart cart = null;
            if (User.Identity.IsAuthenticated)
            {
                user =await _storeService.GetUserByNameAsync(User.Identity.Name);
                cart = await _storeService.GetUserLastCartAsync(User.Identity.Name);
            }
            
            string cartid = Core.Utility.CookieExtensions.ReadCookie("cartid");
            if (!string.IsNullOrEmpty(cartid))
            {
                cart = await _storeService.GetUserCartByCookieAsync(cartid);
                if (cart != null)
                {
                    if (cart.CheckOut)
                    {
                        Core.Utility.CookieExtensions.RemoveCookie("cartid");
                    }
                }
            }

            AddToCartVM addToCartVM = new();
            
            if (user == null)
            {
                if (cart == null)
                {
                    addToCartVM = await _storeService.AddToCartAsync(productId, count, null, null, op,kind);
                }
                else
                {
                    addToCartVM = await _storeService.AddToCartAsync(productId, count, null, cart.Id.ToString(), op,kind);
                }
            }
            else
            {

                addToCartVM = await _storeService.AddToCartAsync(productId, count, user.Id, null, op,kind);
            }
            cartid = addToCartVM.Id;
            //if (cart == null)
            //{
            //    addToCartVM.Message = "خطایی رخ داده است !";
            //    addToCartVM.Added = false;
            //    addToCartVM.Count = 0;
            //}
            
            if(cartid == null )
            {
                if(cart != null)
                {
                    Core.Utility.CookieExtensions.SetCookie("cartid", cart.Id.ToString(), DateTime.Now.AddDays(7));
                }
                
            }
            else
            {
                if (cart != null)
                {
                    if (cartid != cart.Id.ToString())
                    {
                        Core.Utility.CookieExtensions.SetCookie("cartid", cart.Id.ToString(), DateTime.Now.AddDays(7));
                    }
                }
                else
                {
                    Core.Utility.CookieExtensions.SetCookie("cartid", cartid, DateTime.Now.AddDays(7));
                }

            }
            if(cart != null)
            {
                addToCartVM.Count = cart.CartItems.Sum(x => x.Quantity);
            }
            return Json(addToCartVM);
        }      
        public async Task<JsonResult> RemoveCartItem(int itemId)
        {
            AddToCartVM addToCartVM = new();
            
            Cart cart = await _storeService.RemoveCart_Item(itemId);
            
            if (cart == null)
            {
                addToCartVM.Message = "خطایی رخ داده است !";
                addToCartVM.Removed = false;
                addToCartVM.Count = 0;
            }
            if (cart.CartItems.Count == 0)
            {
                Core.Utility.CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
                if (Core.Utility.CookieExtensions.ExistCookie("cartid"))
                {
                    Core.Utility.CookieExtensions.RemoveCookie("cartid");
                }
            }
            addToCartVM.Message = "از سبد خرید حذف شد !";
            addToCartVM.Removed = true;
            addToCartVM.Count = cart.CartItems.Sum(x => x.Quantity);

            return Json(addToCartVM);
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupen(string coupen)
        {
            if(!string.IsNullOrEmpty(coupen))
            {
                ValidateDiscountCoupenVM validateDiscountCoupenVM = await _storeService.ValidateDiscountCoupenAsync(coupen.Trim());
                if(validateDiscountCoupenVM.Validity)
                {
                    Cart cart = await GetCartAsync();
                    if(cart != null)
                    {
                        if(string.IsNullOrEmpty(cart.DiscountCoupen))
                        {
                            cart.DiscountCoupen = coupen.Trim();
                            _storeService.EditCart(cart);
                            await _storeService.SaveChangesAsync();
                            return Json(new { success = true, message = "کوپن تخفیف به سبد خرید اعمال شد" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "به این سبد قبلا کوپن تخفیف اعمال شده است" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "سبد خرید خالی است !" });
                    }
                    
                }
                else
                {
                    return Json(new { success = false, message = "کوپن تخفیف نامعتبر است !" });
                }

            }
            else
            {
                return Json(new { success = false, message = "کوپن تخفیف را وارد کنید !" });
            }
        }
       
        
    }
}
