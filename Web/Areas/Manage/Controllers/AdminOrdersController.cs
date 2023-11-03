using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.Convertors;
using Core.DTOs.Admin;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using Core.DTOs.General;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("ordersforadmin")]
    public class AdminOrdersController : Controller
    {
       
        private readonly IStoreService _storeService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        
        public AdminOrdersController(IStoreService storeService, IUserService userService,IProductService productService)
        {
            _storeService = storeService;
            _userService = userService;
            _productService = productService;
           
        }
        //ordersforadmin
        // GET: Manage/AdminOrders
        //permission ordersforadmin
        public async Task<IActionResult> Index(string filter)
        {
            List<Order> orders = await _storeService.GetOrdersAsync();
            orders = orders.OrderByDescending(x => x.Order_Date).ToList();
            ViewData["OTitle"] = "تمام سفارشات";
            if (!string.IsNullOrEmpty(filter))
            { 
                PersianCalendar PC = new();
                if(filter == "today")
                {
                    orders = orders.Where(w => w.Order_Date.ToShamsi() == DateTime.Now.ToShamsi()).ToList();
                    ViewData["OTitle"] = "سفارشات امروز";
                }
                if(filter == "week")
                {
                    orders = orders.Where(w =>
                    PC.GetWeekOfYear(w.Order_Date, CalendarWeekRule.FirstDay, DayOfWeek.Saturday) ==
                    PC.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Saturday)
                    ).ToList();
                    ViewData["OTitle"] = "سفارشات هفته" + " " + PC.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
                }
                if(filter == "mounth")
                {
                    orders = orders.Where(w => PC.GetMonth(w.Order_Date) == PC.GetMonth(DateTime.Now)).ToList();
                    ViewData["OTitle"] = "سفارشات ماه" + " " + PC.GetMonth(DateTime.Now).ToString();
                }
                if (filter == "year")
                {
                    orders = orders.Where(w => PC.GetYear(w.Order_Date) == PC.GetYear(DateTime.Now)).ToList();
                    ViewData["OTitle"] = "سفارشات سال" + " " + PC.GetYear(DateTime.Now);
                }
                if(filter =="cancled")
                {
                    orders = orders.Where(w => w.Order_IsCanceled).ToList();
                    ViewData["OTitle"] = "سفارشات لغو شده";
                }

            }
            return View(orders);

        }
        
        
        //bascketsforadmin
        [PermissionCheckerByPermissionName("bascketsforadmin")]
        public async Task<IActionResult> GetCarts(string filter)
        {
            List<Cart> carts = await _storeService.GetCartsAsync();
            carts = carts.OrderByDescending(x => x.DateCreated).ToList();
            ViewData["CTitle"] = "تمام سبدهای خرید";
            if (!string.IsNullOrEmpty(filter))
            {
                PersianCalendar PC = new();
                if (filter == "today")
                {
                    carts = carts.Where(w => w.DateCreated.ToShamsi() == DateTime.Now.ToShamsi()).ToList();
                    ViewData["CTitle"] = "سبدهای امروز";
                }
                if (filter == "week")
                {
                    carts = carts.Where(w =>
                    PC.GetWeekOfYear(w.DateCreated, CalendarWeekRule.FirstDay, DayOfWeek.Saturday) ==
                    PC.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Saturday)
                    ).ToList();
                    ViewData["CTitle"] = "سبدهای هفته" + " " + PC.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
                }
                if (filter == "mounth")
                {
                    carts = carts.Where(w => PC.GetMonth(w.DateCreated) == PC.GetMonth(DateTime.Now)).ToList();
                    ViewData["CTitle"] = "سبدهای ماه" + " " + PC.GetMonth(DateTime.Now).ToString() + " سال " + PC.GetYear(DateTime.Now);
                }
                if (filter == "year")
                {
                    carts = carts.Where(w => PC.GetYear(w.DateCreated) == PC.GetYear(DateTime.Now)).ToList();
                    ViewData["CTitle"] = "سبدهای سال" + " " + PC.GetYear(DateTime.Now);
                }

            }
            return View(carts);
        }
        [PermissionCheckerByPermissionName("bascketsforadmin")]
        public async Task<IActionResult> CartDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cart = await _storeService.GetCartByIdAsync((Guid)id);
            if(cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }
        // GET: Manage/AdminOrders/Details/5
        
        public async Task<IActionResult> Details(Guid? id)
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

        public async Task<IActionResult> CreateOrder()
        {
            User user = await _userService.GetUserByUserName(User.Identity.Name);
            var products = await _productService.GetProductsAsync();
            products = products.Where(w => _productService.GetProduct_Inventory_InStoreAsync(w.ProductCode).Result > 0).ToList();
            var packages =await _productService.GetPackagesAsync();
            packages = packages.Where(w => _productService.GetPackage_Inventory_InStoreAsync(w.PkId).Result > 0).ToList();
            AdminOrderVM adminOrderVM = new()
            {
                Packages =packages,
                Products =products,
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
        public async Task<IActionResult> CreateOrder(AdminOrderVM adminOrderVM, List<int> SelectedProducts, List<int> PCount,List<string> Kinds)
        {
            var products = await _productService.GetProductsAsync();
            products = products.Where(w => _productService.GetProduct_Inventory_InStoreAsync(w.ProductCode).Result > 0).ToList();
            var packages = await _productService.GetPackagesAsync();
            packages = packages.Where(w => _productService.GetPackage_Inventory_InStoreAsync(w.PkId).Result > 0).ToList();

            adminOrderVM.Products = products;
            adminOrderVM.Packages = packages;
            adminOrderVM.States = await _userService.GetStates();
            adminOrderVM.Counties = await _userService.GetCounties();
            if(ModelState.IsValid)
            {
                SaveOrderVM saveOrderVM = await _storeService.CreateOrderByAdmin(adminOrderVM, SelectedProducts, PCount, Kinds, 2);
                if (saveOrderVM.IsSuccess == true)
                {
                    await _storeService.SaveChangesAsync();
                    adminOrderVM.Saved = true;
                    bool ware = await _storeService.CreateWareHouseWithOrder(saveOrderVM.OrderId);
                    if (ware == true)
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
        // GET: Manage/AdminOrders/Edit/5
        public async Task<IActionResult> GetCounties(string stN)
        {
            State state =await _storeService.GetStateByName(stN);
            if (stN == null)
                return null;
            var counties = await _userService.GetCountiesofState(state.StateId);
            return PartialView(counties.ToList());
        }
        public async Task<IActionResult> Edit(Guid? id)
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
            ViewData["URId"] = new SelectList(await _storeService.GetUserRolesAsync(), "URId", "FullName", order.URId);
            State state = await _storeService.GetStateByName(order.Order_StateName);
            ViewData["StateId"] = state.StateId;
            ViewData["States"] = await _userService.GetStates();
            ViewData["Counties"] = await _userService.GetCountiesofState(state.StateId);
            return View(order);
        }

        // POST: Manage/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _storeService.EditOrder(order);
                    await _storeService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if(TempData["retUrl"] == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect(TempData["retUrl"].ToString());
                }
               
            }
            ViewData["URId"] = new SelectList(await _storeService.GetUserRolesAsync(), "URId", "URId", order.URId);
            return View(order);
        }
        private bool OrderExists(Guid id)
        {
            return _storeService.GetOrdersAsync().Result.Any(x => x.Id == id);
        }
        public async Task<IActionResult> SearchOrders()
        {
            SearchOrdersVM searchOrdersVM = new()
            {                
                UserRoles = await _userService.GetUserRolesAsync()
            };
            return View(searchOrdersVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchOrders(SearchOrdersVM searchOrdersVM)
        {
            try
            {
                int recp = searchOrdersVM.RecPerPage; int pagen = searchOrdersVM.PageN;
                if (!ModelState.IsValid)
                {
                    searchOrdersVM.UserRoles = await _userService.GetUserRolesAsync();
                    return View(searchOrdersVM);
                }
                List<Order> orders = await _storeService.GetOrdersAsync();
                orders = orders.OrderByDescending(x => x.Order_Date).ToList();
                searchOrdersVM.TotalRecCount = orders.Count;
                searchOrdersVM.TotalOrdersSum = orders.Sum(x => x.Order_Sum);
                searchOrdersVM.UnPaidCount = orders.Where(x => !x.Order_IsFinished).Count();
                searchOrdersVM.UnPaidSum = orders.Where(x => !x.Order_IsFinished).Sum(x => x.Order_Sum);
                searchOrdersVM.PaidCount = orders.Where(x => x.Order_IsFinished).Count();
                searchOrdersVM.PaidSum = orders.Where(x => x.Order_IsFinished).Sum(x => x.Order_Sum);
                string title = "لیست تمام سفارشات";
                if (!string.IsNullOrEmpty(searchOrdersVM.StartDate))
                {
                    title += " از تاریخ : " + searchOrdersVM.StartDate;
                    DateTime sdt = searchOrdersVM.StartDate.ToMiladiWithoutTime();
                    orders = orders.Where(w => w.Order_Date >= sdt).ToList();
                }
                if (!string.IsNullOrEmpty(searchOrdersVM.EndDate))
                {
                    title += " تا تاریخ : " + searchOrdersVM.EndDate;
                    DateTime edt = searchOrdersVM.EndDate.ToMiladiWithoutTime();
                    orders = orders.Where(w => w.Order_Date <= edt).ToList();
                }

                if (!string.IsNullOrEmpty(searchOrdersVM.BuyerName))
                {
                    title += " | خریدار : " + searchOrdersVM.BuyerName;
                    orders = orders.Where(w => w.Order_BuyerName.Equals(searchOrdersVM.BuyerName.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(searchOrdersVM.BuyerFamily))
                {
                    title += " " + searchOrdersVM.BuyerFamily;
                    orders = orders.Where(w => w.Order_BuyerFamily.Equals(searchOrdersVM.BuyerFamily.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(searchOrdersVM.BuyerCellphone))
                {
                    title += " | " + searchOrdersVM.BuyerCellphone;
                    orders = orders.Where(w => w.Order_BuyerCellphone.Equals(searchOrdersVM.BuyerCellphone.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(searchOrdersVM.PostalCode))
                {
                    title += " | " + searchOrdersVM.PostalCode;
                    orders = orders.Where(w => w.Order_PostalCode.Equals(searchOrdersVM.PostalCode.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(searchOrdersVM.DisCoupen))
                {
                    title += " | " + searchOrdersVM.DisCoupen;
                    orders = orders.Where(w => w.Order_DiscountCode == searchOrdersVM.DisCoupen).ToList();
                }
                if (searchOrdersVM.IsFinished != null)
                {
                    if ((bool)searchOrdersVM.IsFinished)
                    {
                        title += " | " + " پرداخت شده";
                    }
                    else
                    {
                        title += " | " + " پرداخت نشده";
                    }

                    orders = orders.Where(w => w.Order_IsFinished == searchOrdersVM.IsFinished).ToList();
                }
                if (searchOrdersVM.IsCancled != null)
                {
                    if ((bool)searchOrdersVM.IsCancled)
                    {
                        title += " | " + "لغو شده";
                    }
                    else
                    {
                        title += " | " + "لغو نشده";
                    }

                    orders = orders.Where(w => w.Order_IsCanceled == searchOrdersVM.IsCancled).ToList();
                }
                if (searchOrdersVM.DeliverdToPost != null)
                {
                    if ((bool)searchOrdersVM.DeliverdToPost)
                    {
                        title += " | " + "تحویل داده شده به پست";
                    }
                    else
                    {
                        title += " | " + "تحویل داده نشده به پست";
                    }

                    orders = orders.Where(w => w.DeliveredToPost == searchOrdersVM.DeliverdToPost).ToList();
                }
                if (searchOrdersVM.DeliveredToCustomer != null)
                {
                    if ((bool)searchOrdersVM.DeliveredToCustomer)
                    {
                        title += " | " + "تحویل داده شده به مشتری";
                    }
                    else
                    {
                        title += " | " + "تحویل داده نشده به مشتری";
                    }

                    orders = orders.Where(w => w.IsDeliveredCutomer == searchOrdersVM.DeliveredToCustomer).ToList();
                }
                if (!string.IsNullOrEmpty(searchOrdersVM.DedicatedNumber))
                {
                    orders = orders.Where(w => w.Order_DedicatedNumber == searchOrdersVM.DedicatedNumber).ToList();
                }
                if (searchOrdersVM.HasDicCoupen != null)
                {
                    if ((bool)searchOrdersVM.HasDicCoupen)
                    {
                        title += " | " + "دارای کوپن تخفیف";
                    }
                    else
                    {
                        title += " | " + "بدون کوپن تخفیف";
                    }
                    orders = orders.Where(w => !string.IsNullOrEmpty(w.Order_DiscountCode)).ToList();
                }
                if (searchOrdersVM.URId != null)
                {
                    UserRole userRole = await _userService.GetUserRoleById((int)searchOrdersVM.URId);
                    title += " | " + userRole.FullName;
                    orders = orders.Where(w => w.URId == searchOrdersVM.URId).ToList();
                }

                orders = orders.Skip((pagen - 1) * recp).Take((pagen) * recp).ToList();
                searchOrdersVM.Orders = orders.ToList();
                searchOrdersVM.UserRoles = await _userService.GetUserRolesAsync();
                searchOrdersVM.ReportTitle = title;
                return View(searchOrdersVM);
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return NotFound(ex.Message);
                
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> ExportOrdersAsExcel(List<Guid> ordersid, string rtitle)
        {
            User user = await _userService.GetUserByUserName(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            List<Order> orders = await _storeService.GetOrdersByOrderIds(ordersid);
            
            List<OrdersExcelReportVM> ordersExcelReportVMsList = new();
            foreach (var od in orders)
            {
                OrdersExcelReportVM ordersExcelReportVM = new() {
                    Id = od.Id,
                    Order_Date = od.Order_Date.ToShamsiWithTime(),
                    Order_StateName = od.Order_StateName,
                    Order_CountyName = od.Order_CountyName,
                    Order_BuyerName = od.Order_BuyerName,
                    Order_BuyerFamily = od.Order_BuyerFamily,
                    ClientFullName = od.Order_BuyerName + " " + od.Order_BuyerFamily,
                    Order_BuyerCellphone = od.Order_BuyerCellphone,
                    Order_Address = od.Order_Address,
                    Order_PostalCode = od.Order_PostalCode,
                    Order_DiscountCode = od.Order_DiscountCode,
                    Order_DiscountValue = od.Order_DiscountValue,
                    Order_DeliveryCost = od.Order_DeliveryCost,
                    Order_DeliveryCostDiscount = od.Order_DeliveryCostDiscount,
                    Order_Sum = od.Order_Sum,
                    Order_IsFinished = od.Order_IsFinished,
                    Order_IsDeleted = od.Order_IsDeleted,
                    Order_IsCanceled = od.Order_IsCanceled,
                    CancellationComment = od.CancellationComment,
                    UserBusinessName = od.UserBusinessName,
                    UserBusinessPercent = od.UserBusinessPercent,
                    Order_DedicatedNumber = od.Order_DedicatedNumber,
                    DeliveryType = od.DeliveryType,
                    Comment = od.Comment,
                    DeliveredToPost = od.DeliveredToPost,
                    IsDeliveredCutomer = od.IsDeliveredCutomer,
                    Currency = od.Currency,
                    Order_RecipientFullName = od.RecipientName + " " + od.RecipientFamily,
                    Order_RecipientPhone = od.RecipientPhone,
                    CartId = od.CartId.ToString(),
                };
                string odt = string.Empty;
                foreach (var item in od.OrderDetails)
                {
                    Product product = await _storeService.GetProductByIdAsync(item.ProductId);
                    if(item != od.OrderDetails.LastOrDefault())
                    {
                        odt += item.OrderDatailProductName + " - " + item.OrderDetailCount +product.ProductUnit + " - " + item.OrderDetailPrice.ToString("N0") + " "+ item.Order.Currency  + Environment.NewLine;
                    }
                    else
                    {
                        odt += item.OrderDatailProductName + " - " + item.OrderDetailCount + product.ProductUnit + " - " + item.OrderDetailPrice.ToString("N0") + " " + item.Order.Currency;
                    }
                }
                ordersExcelReportVM.OrderDetailsString = odt;
                ordersExcelReportVMsList.Add(ordersExcelReportVM) ;
            }
            
            IWorkbook workbook = _storeService.WriteExcelWithNPOI(new OrdersExcelReportVM(), ordersExcelReportVMsList, rtitle);
            string contentType; // Scope

            MemoryStream tempStream = null;
            MemoryStream stream = null;
            try
            {
                // 1. Write the workbook to a temporary stream
                tempStream = new MemoryStream();
                workbook.Write(tempStream);
                // 2. Convert the tempStream to byteArray and copy to another stream
                var byteArray = tempStream.ToArray();
                stream = new MemoryStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Seek(0, SeekOrigin.Begin);
                // 3. Set file content type
                contentType = workbook.GetType() == typeof(XSSFWorkbook) ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/vnd.ms-excel";
                // 4. Return file
                return File(
                    fileContents: stream.ToArray(),
                    contentType: contentType,
                    fileDownloadName: "Orders " + "-" + DateTime.Now.Ticks + "-" + DateTime.Now.ToShamsi() + ((workbook.GetType() == typeof(XSSFWorkbook)) ? ".xlsx" : "xls"));
            }
            finally
            {
                if (tempStream != null) tempStream.Dispose();
                if (stream != null) stream.Dispose();
            }
        }
        
        
    }
}
