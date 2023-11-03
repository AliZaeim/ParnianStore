using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Entities.Store;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PackageProductsController : Controller
    {
        private readonly MyContext _context;

        public PackageProductsController(MyContext context)
        {
            _context = context;
        }

        // GET: Manage/PackageProducts
        public async Task<IActionResult> Index()
        {
            var myContext = _context.PackageProducts.Include(p => p.Package).Include(p => p.Product);
            return View(await myContext.ToListAsync());
        }

        // GET: Manage/PackageProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageProduct = await _context.PackageProducts
                .Include(p => p.Package)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (packageProduct == null)
            {
                return NotFound();
            }

            return View(packageProduct);
        }

        // GET: Manage/PackageProducts/Create
        public IActionResult Create()
        {
            ViewData["PkId"] = new SelectList(_context.Packages, "PkId", "PkComment");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Contraindications");
            return View();
        }

        // POST: Manage/PackageProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,PkId")] PackageProduct packageProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(packageProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PkId"] = new SelectList(_context.Packages, "PkId", "PkComment", packageProduct.PkId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Contraindications", packageProduct.ProductId);
            return View(packageProduct);
        }

        // GET: Manage/PackageProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageProduct = await _context.PackageProducts.FindAsync(id);
            if (packageProduct == null)
            {
                return NotFound();
            }
            ViewData["PkId"] = new SelectList(_context.Packages, "PkId", "PkComment", packageProduct.PkId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Contraindications", packageProduct.ProductId);
            return View(packageProduct);
        }

        // POST: Manage/PackageProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,PkId")] PackageProduct packageProduct)
        {
            if (id != packageProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(packageProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageProductExists(packageProduct.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PkId"] = new SelectList(_context.Packages, "PkId", "PkComment", packageProduct.PkId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Contraindications", packageProduct.ProductId);
            return View(packageProduct);
        }

        // GET: Manage/PackageProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageProduct = await _context.PackageProducts
                .Include(p => p.Package)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (packageProduct == null)
            {
                return NotFound();
            }

            return View(packageProduct);
        }

        // POST: Manage/PackageProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var packageProduct = await _context.PackageProducts.FindAsync(id);
            _context.PackageProducts.Remove(packageProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackageProductExists(int id)
        {
            return _context.PackageProducts.Any(e => e.Id == id);
        }
    }
}
