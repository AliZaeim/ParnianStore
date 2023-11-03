using Core.DTOs.Admin;
using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Context;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("warehouse")]
    public class WareHousesController : Controller
    {

        private readonly IProductService _productService;
        public WareHousesController(IProductService productService)
        {
            _productService = productService;

        }

        // GET: Manage/WareHouses
        public async Task<IActionResult> Index(string search,int? page,int? recCount)
        {
            List<WareHouse> wareHouses = await _productService.GetWareHousesAsync();
            ViewData["TotalCount"] = wareHouses.Count;
            
            int zpage = page.GetValueOrDefault(1);

            if (!string.IsNullOrEmpty(search))
            {
                wareHouses = wareHouses.Where(w => (w.Product != null && w.Product.ProductName.Contains(search)) || (w.Package != null && w.Package.PkTitle.Contains(search))).ToList();
            }
            List<WareHouseVM> wareHouseVMs = wareHouses.GroupBy(g => new { g.Product, g.Package }).Select(x => new WareHouseVM()
            {
                Product = x.Key.Product,
                Package=x.Key.Package,
                WHouses = x.Select(x => new WHouse
                {
                    Id = x.WareHouseId,
                    Export = x.WareHouse_Export,
                    Import = x.WareHouse_Input,
                    WDate = (DateTime)x.WareHouse_Date,
                    OD_Id = x.OrderdetialId,
                    Product = x.Product,
                    Package = x.Package,
                    Comment = x.WareHouse_Comment
                }).ToList()
            }).ToList();
            
            
            int RecCount = recCount.GetValueOrDefault(10);
            ViewData["RecCount"] = RecCount;
            ViewData["zPage"] = zpage;
            int TPage = 1;
            
            if(wareHouseVMs.Count % RecCount==0)
            {
                TPage = wareHouseVMs.Count / RecCount;
            }
            else
            {
                TPage = (wareHouseVMs.Count / RecCount)+1;
            }
            ViewData["zTotalPage"] = TPage;
            wareHouseVMs = wareHouseVMs.Skip((zpage - 1) * RecCount).Take(zpage * RecCount).ToList();

            return View(wareHouseVMs);

        }
        public async Task<IActionResult> GetProductsWithLowInventory()
        {
            ViewData["products"] = await _productService.GetProductsWithLowInventory();
            ViewData["packages"] = await _productService.GetPackagesWithLowInventory();
            return View();
        }
        // GET: Manage/WareHouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wareHouse = await _productService.GetProductByIdAsync((int)id);
            if (wareHouse == null)
            {
                return NotFound();
            }

            return View(wareHouse);
        }

        // GET: Manage/WareHouses/Create
        public async Task<IActionResult> Create()
        {

            ViewData["Products"] = await _productService.GetProductsAsync();
            ViewData["Packages"] = await _productService.GetPackagesAsync();

            return View();
        }

        // POST: Manage/WareHouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WareHouse wareHouse)
        {
            ViewData["Products"] = await _productService.GetProductsAsync();
            ViewData["Packages"] = await _productService.GetPackagesAsync();
            if (ModelState.IsValid)
            {
                if (wareHouse.ProductId == null && wareHouse.PkId == null)
                {
                    ModelState.AddModelError("ProductId", "محصول یا پکیج انتخاب نشده است !");
                    return View(wareHouse);
                }
                if (wareHouse.ProductId == null && wareHouse.PkId == null)
                {
                    ModelState.AddModelError("PkId", "محصول یا پکیج انتخاب نشده است !");
                    return View(wareHouse);
                }
                wareHouse.WareHouse_Date = DateTime.Now;
                _productService.CreateWareHouse(wareHouse);
                await _productService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(await _productService.GetProductsAsync(), "ProductId", "ProductName", wareHouse.ProductId);
            return View(wareHouse);


        }

        // GET: Manage/WareHouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wareHouse = await _productService.GetWareHouseByIdAsync((int)id);
            if (wareHouse == null)
            {
                return NotFound();
            }

            ViewData["ProductId"] = new SelectList(await _productService.GetProductsAsync(), "ProductId", "ProductName", wareHouse.ProductId);
            return View(wareHouse);
        }

        // POST: Manage/WareHouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Manage/WareHouses/Delete/5



    }
}
