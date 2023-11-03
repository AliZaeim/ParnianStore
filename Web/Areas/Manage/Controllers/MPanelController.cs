using Core.DTOs.Admin;
using Core.Security;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class MPanelController : Controller
    {
        private readonly IStoreService _storeService;
        public MPanelController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [PermissionCheckerByPermissionName("deliverlabel")]
        public async Task<IActionResult> DeliversLabel(string filter = "all")
        {
            var orders = await _storeService.GetOrdersAsync();
            if(orders != null)
            {
                orders = orders.OrderByDescending(x => x.Order_Date.Date).ThenByDescending(x => x.Order_Date.TimeOfDay).Where(w => !w.Order_IsCanceled).ToList();
            }
            
            var init = await _storeService.GetInitialInfoAsync();
            PersianCalendar pc = new();
            DeliverLabelsInfo deliverLabelsInfo = new()
            {
                InitialInfo = init
            };
            
            if (!string.IsNullOrEmpty(filter))
            {
                if (filter == "day")
                {
                    orders = orders.Where(w =>w.Order_IsFinished && w.Order_Date.Date == DateTime.Now.Date).ToList();
                }
                if(filter =="week")
                {
                    orders = orders.Where(w => w.Order_IsFinished && pc.GetWeekOfYear(w.Order_Date, CalendarWeekRule.FirstDay, DayOfWeek.Saturday) == pc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Saturday)).ToList();
                }
                if (filter == "mounth")
                {
                    orders = orders.Where(w => w.Order_IsFinished && pc.GetMonth(w.Order_Date) == pc.GetMonth(DateTime.Now)).ToList();
                }
            }
            TempData["filter"] = filter;
           
            List<DeliverLabel> labels = orders.Select(x =>
                new DeliverLabel
                {
                    RecipientName = x.RecipientName,
                    RecipientFamily = x.RecipientFamily,
                    RecipientPhone = x.RecipientPhone,
                    Order_StateName = x.Order_StateName,
                    Order_CountyName = x.Order_CountyName,
                    Order_Address = x.Order_Address,
                    Order_PostalCode = x.Order_PostalCode,
                    BuyerName=x.Order_BuyerName,
                    BuyerFamily=x.Order_BuyerFamily,
                    Order_DedicatedNumber=x.Order_DedicatedNumber,
                    Order_Date=x.Order_Date

                }).ToList();
            deliverLabelsInfo.DeliverLabels = labels;
            deliverLabelsInfo.Filter = filter;
            return View(deliverLabelsInfo);
        }
        public async Task<IActionResult> PrintDeliversLabel(string filter = "all")
        {
            var orders = await _storeService.GetOrdersAsync();
            if (orders != null)
            {
                orders = orders.OrderByDescending(x => x.Order_Date.Date).ThenByDescending(x => x.Order_Date.TimeOfDay).Where(w => !w.Order_IsCanceled).ToList();
            }
            var init = await _storeService.GetInitialInfoAsync();
            PersianCalendar pc = new();
            DeliverLabelsInfo deliverLabelsInfo = new()
            {
                InitialInfo = init
            };

            if (!string.IsNullOrEmpty(filter))
            {
                if (filter == "day")
                {
                    orders = orders.Where(w => w.Order_IsFinished && w.Order_Date.Date == DateTime.Now.Date).ToList();
                }
                if (filter == "week")
                {
                    orders = orders.Where(w => w.Order_IsFinished && pc.GetWeekOfYear(w.Order_Date, CalendarWeekRule.FirstDay, DayOfWeek.Saturday) == pc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Saturday)).ToList();
                }
                if (filter == "mounth")
                {
                    orders = orders.Where(w => w.Order_IsFinished && pc.GetMonth(w.Order_Date) == pc.GetMonth(DateTime.Now)).ToList();
                }
            }
            
            List<DeliverLabel> labels = orders.Select(x =>
                new DeliverLabel
                {
                    RecipientName = x.RecipientName,
                    RecipientFamily = x.RecipientFamily,
                    RecipientPhone = x.RecipientPhone,
                    Order_StateName = x.Order_StateName,
                    Order_CountyName = x.Order_CountyName,
                    Order_Address = x.Order_Address,
                    Order_PostalCode = x.Order_PostalCode,
                    BuyerName = x.Order_BuyerName,
                    BuyerFamily = x.Order_BuyerFamily,
                    Order_DedicatedNumber = x.Order_DedicatedNumber,
                    Order_Date = x.Order_Date

                }).ToList();
            deliverLabelsInfo.DeliverLabels = labels;
            deliverLabelsInfo.Filter = filter;
            return View(deliverLabelsInfo);
        }
    }
}
