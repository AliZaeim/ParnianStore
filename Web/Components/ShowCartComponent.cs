using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowCartComponent : ViewComponent
    {
        private readonly IStoreService _storeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShowCartComponent(IStoreService storeService, IHttpContextAccessor httpContextAccessor)
        {
            _storeService = storeService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            Cart cart = null;
            Core.Utility.CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
            if (Core.Utility.CookieExtensions.ExistCookie("cartid"))
            {
                string cookieCartId = Core.Utility.CookieExtensions.ReadCookie("cartid");
                if (cookieCartId != null )
                {                    
                        cart = await _storeService.GetUserCartByCookieAsync(cookieCartId);  
                }
            }
            if(User.Identity.IsAuthenticated)
            {
                User user = await _storeService.GetUserByNameAsync(User.Identity.Name);
                cart = await _storeService.GetUserLastCartAsync(User.Identity.Name);                
                if (user != null)
                {
                    if(cart!=null)
                    {
                        if (cart.UserId == null)
                        {
                            cart.UserId = user.Id;
                            _storeService.EditCart(cart);
                            await _storeService.SaveChangesAsync();
                        }
                    }
                   
                   
                }
            }
            if(cart !=null)
            {
                if(cart.CheckOut==false)
                {
                    foreach (var item in cart.CartItems)
                    {
                        Package package = null;
                        Product pr = null;
                        if(item.Kind=="pr")
                        {
                            pr= await _storeService.GetProductByIdAsync((int)item.ProductId);
                        }
                        if(item.Kind=="pk")
                        {
                            package = await _storeService.GetPackageByIdAsync(item.ProductId);
                        }
                        if (pr != null)
                        {
                            int InStore = await _storeService.GetProduct_Inventory_InStoreAsync(pr.ProductCode);
                            if (!pr.IsActive || InStore <= 0)
                            {
                                await _storeService.RemoveCart_Item(item.Id);
                                await _storeService.SaveChangesAsync();
                            }
                        }
                        if(package != null)
                        {
                            int InStore = await _storeService.GetPackage_Inventory_InStoreAsync(package.PkId);
                            if(!package.IsActive || InStore<=0)
                            {
                                await _storeService.RemoveCart_Item(item.Id);
                                await _storeService.SaveChangesAsync();
                            }
                        }


                    }
                }
            }
            return View("/Pages/Components/_showCart.cshtml", cart);
        }
    }
}
