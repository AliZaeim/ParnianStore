using Core.Convertors;
using Core.DTOs.Admin;
using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class MyAccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        public MyAccountController(IUserService userService, IStoreService storeService, IProductService productService)
        {
            _userService = userService;
            _storeService = storeService;
            _productService = productService;
        }

        public async Task<IActionResult> MyOrders()
        {
            User user = await _userService.GetUserByUserName(User.Identity.Name);
            if (user == null)
            {
                return NotFound("خطا رخ داده است، لطفا به مدیرسایت اطلاع دهید ! ");
            }

            var orders = await _storeService.GetUserOrdersByNameAsync(User.Identity.Name);
            return View(orders);
        }
        public async Task<JsonResult> ChangeCancleStatus(Guid id, int status)
        {
            Order order = await _storeService.GetOrderByIdAsync(id);
            CancleOrderVM cancleOrderVM = new();
            if (order != null)
            {
                if (order.Order_IsCanceled == false && status == 1)
                {
                    if(order.DeliveredToPost == false)
                    {
                        order.Order_IsCanceled = true;
                        string message = "در تاریخ " + DateTime.Now.ToShamsiWithTime() + " توسط" + Environment.NewLine +  User.Identity.Name + " لغو شده است !";
                        _storeService.EditOrder(order);
                        await _storeService.SaveChangesAsync();
                        cancleOrderVM.CResult = true;
                        cancleOrderVM.CMessage = "سفارش شماره " + order.Order_DedicatedNumber + "با موفقیت لغو شد !";
                    }
                    else
                    {
                        cancleOrderVM.CResult = false;
                        cancleOrderVM.CMessage = "این سفارش تحویل پست داده شده و امکان لغو آن وجود ندارد !";
                    }
                    
                }
                else
                {
                    cancleOrderVM.CResult = false;
                    cancleOrderVM.CMessage = "امکان لغو این سفارش وجود ندارد !";
                }
            }
            else
            {
                cancleOrderVM.CResult = false;
                cancleOrderVM.CMessage = "امکان لغو این سفارش وجود ندارد !";
            }

            return Json(cancleOrderVM);

        }
        public async Task<IActionResult> OrderDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _storeService.GetOrderByIdAsync((Guid)id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        public async Task<IActionResult> MyData()
        {
            User user = await _userService.GetUserByUserName(User.Identity.Name);
            UserInfoVM userInfoVM = new()
            {
                Name = user.Name,
                Family = user.Family,
                Cellphone = user.Cellphone,
                UserName = user.UserName,
                Sex = user.Sex,
                County = user.County,
                CountyId = user.CountyId,
                Address = user.Address,
                PostalCode = user.PostalCode
            };

            return View(userInfoVM);
        }
        public async Task<IActionResult> MyQuestions()
        {
            MyQuestionsVM myQuestionsVM = new()
            {
                FAQs = await _userService.GetFAQsByNameAsync(User.Identity.Name),
                ContactMessages = await _userService.GetContactMessagesByNameAsync(User.Identity.Name)
            };
            return View(myQuestionsVM);
        }
        public async Task<IActionResult> EditInfo()
        {
            User user = await _userService.GetUserByUserName(User.Identity.Name);
            UserInfoVM userInfoVM = new()
            {
                Name = user.Name,
                Family = user.Family,
                Cellphone = user.Cellphone,
                UserName = user.UserName,
                Sex = user.Sex,
                County = user.County,
                CountyId = user.CountyId,
                Address = user.Address,
                PostalCode = user.PostalCode
            };
            ViewData["StateId"] = new SelectList(await _userService.GetStates(), "StateId", "StateName", user.County.StateId);
            ViewData["CountyId"] = new SelectList(await _userService.GetCounties(), "CountyId", "CountyName", user.CountyId);
            return View(userInfoVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo(UserInfoVM userInfoVM)
        {
            User user = await _userService.GetUserByCellphoneAsync(userInfoVM.Cellphone);
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    ModelState.AddModelError("Cellphone", "شماره تلفن نامعتبر است !");
                    ViewData["StateId"] = new SelectList(await _userService.GetStates(), "StateId", "StateName", user.County.StateId);
                    ViewData["CountyId"] = new SelectList(await _userService.GetCounties(), "CountyId", "CountyName", user.CountyId);
                    return View();
                }
                user.Name = userInfoVM.Name;
                user.Family = userInfoVM.Family;
                user.CountyId = userInfoVM.CountyId;
                user.Email = userInfoVM.Email;
                user.PostalCode = userInfoVM.PostalCode;
                user.Address = userInfoVM.Address;
                user.UserName = userInfoVM.UserName;
                user.Sex = userInfoVM.Sex;
                _userService.UpdateUser(user);
                await _userService.SaveChangesAsync();
                return RedirectToAction(nameof(MyData));
            }
            ViewData["StateId"] = new SelectList(await _userService.GetStates(), "StateId", "StateName", user.County.StateId);
            ViewData["CountyId"] = new SelectList(await _userService.GetCounties(), "CountyId", "CountyName", user.CountyId);
            return View();
        }
        public async Task<IActionResult> CreateOrder()
        {
            User user = await _userService.GetUserByUserName(User.Identity.Name);
            var products = await _productService.GetProductsAsync();
            products = products.Where(w => _productService.GetProduct_Inventory_InStoreAsync(w.ProductCode).Result > 0).ToList();
            var packages = await _productService.GetPackagesAsync();
            packages = packages.Where(w => _productService.GetPackage_Inventory_InStoreAsync(w.PkId).Result > 0).ToList();
            AdminOrderVM adminOrderVM = new()
            {
                Packages = packages,
                Products = products,
                BuyerCellphone = user.Cellphone,
                BuyerName = user.Name,
                BuyerFamily = user.Family,
                States = await _userService.GetStates(),
                Counties = await _userService.GetCounties()
            };
            return View(adminOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(AdminOrderVM adminOrderVM, List<int> SelectedProducts, List<int> PCount, List<string> Kinds)
        {
            List<Product> products = await _productService.GetProductsAsync();
            products = products.Where(w => _productService.GetProduct_Inventory_InStoreAsync(w.ProductCode).Result > 0).ToList();
            var packages = await _productService.GetPackagesAsync();
            packages = packages.Where(w => _productService.GetPackage_Inventory_InStoreAsync(w.PkId).Result > 0).ToList();

            adminOrderVM.Products = products;
            adminOrderVM.Packages = packages;
            adminOrderVM.States = await _userService.GetStates();
            adminOrderVM.Counties = await _userService.GetCounties();
            if (ModelState.IsValid)
            {
                if (SelectedProducts == null)
                {
                    ModelState.AddModelError("SelectedProducts", "محصولی برای سفارش در نظر گرفته نشده است !");
                    return View(adminOrderVM);
                }
                else
                {
                    if (SelectedProducts.Count == 0)
                    {
                        ModelState.AddModelError("SelectedProducts", "محصولی برای سفارش در نظر گرفته نشده است !");
                        return View(adminOrderVM);
                    }
                }
                SaveOrderVM saveOrderVM = await _storeService.CreateOrderByAdmin(adminOrderVM, SelectedProducts, PCount, Kinds, 3);
                if (saveOrderVM.IsSuccess)
                {
                    await _storeService.SaveChangesAsync();
                    adminOrderVM.Saved = true;
                    bool ware = await _storeService.CreateWareHouseWithOrder(saveOrderVM.OrderId);
                    if (ware)
                    {
                        _storeService.SendOrderNumber(adminOrderVM.BuyerName + " " + adminOrderVM.BuyerFamily, saveOrderVM.OrderDedicated, adminOrderVM.BuyerCellphone);
                        if (saveOrderVM.IsNewUser)
                        {
                            _storeService.SendUserName_and_Password(adminOrderVM.BuyerCellphone, saveOrderVM.UserPassword, adminOrderVM.BuyerCellphone);
                        }
                    }
                }

                ModelState.Clear();
            }

            return View(adminOrderVM);
        }
        public async Task<JsonResult> GetUserByCellphone(string Cellphone)
        {
            User user = await _userService.GetUserByCellphoneAsync(Cellphone);
            UserInfoByCellphoneVM userInfoByCellphoneVM = new()
            {
                UserName = string.Empty,
                UserFamily = string.Empty,
                IsLock = false
            };
            if (user != null)
            {
                userInfoByCellphoneVM.UserName = user.Name;
                userInfoByCellphoneVM.UserFamily = user.Family;
                userInfoByCellphoneVM.IsLock = true;

            }
            return Json(userInfoByCellphoneVM);
        }
    }
}
