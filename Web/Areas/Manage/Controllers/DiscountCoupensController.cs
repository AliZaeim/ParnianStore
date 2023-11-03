using Core.Convertors;
using Core.DTOs.Admin;
using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Context;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("discountcoupen")]
    public class DiscountCoupensController : Controller
    {
        private readonly MyContext _context;
        private readonly IGenericService<DiscountCoupen> _discountCoupen;

        public DiscountCoupensController(IGenericService<DiscountCoupen> discountCoupen, MyContext context)
        {
            _context = context;
            _discountCoupen = discountCoupen;
        }

        // GET: Manage/DiscountCoupens
        public async Task<IActionResult> Index()
        {
            return View(await _discountCoupen.GetAllAsync());
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            DiscountCoupen discount = await _discountCoupen.GetByIdAsync(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            discount.IsActive = st;
            _discountCoupen.Edit(discount);
            await _discountCoupen.SaveChangesAsync();
            return st;

        }
        // GET: Manage/DiscountCoupens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountCoupen = await _discountCoupen.GetByIdAsync((int)id);
            if (discountCoupen == null)
            {
                return NotFound();
            }

            return View(discountCoupen);
        }
        public  JsonResult GetCoupen()
        {
            List<DiscountCoupen> discountCoupens = _discountCoupen.GetAll().ToList();
            List<string> coupens = new();
            coupens = discountCoupens.Select(x => x.Code).ToList();

            string getc = Core.Prodocers.Generators.GenerateUniqueString(coupens, 3, 3, 4, 2);
            return Json(getc);
        }
        // GET: Manage/DiscountCoupens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/DiscountCoupens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountCoupenVM discountCoupenVM)
        {
            if (ModelState.IsValid)
            {
                DiscountCoupen discountCoupen = new()
                {
                    Code = discountCoupenVM.Code,
                    Comment = discountCoupenVM.Comment,
                    Number = discountCoupenVM.Number,
                    IsActive= discountCoupenVM.IsActive,
                    Percent = discountCoupenVM.Percent

                };
                if (!string.IsNullOrEmpty(discountCoupenVM.EndDate))
                {
                    discountCoupen.ExpiredDate = discountCoupenVM.EndDate.ToMiladiWithTime(discountCoupenVM.EndTime);
                }
                _discountCoupen.Create(discountCoupen);
                await _discountCoupen.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discountCoupenVM);
        }

        // GET: Manage/DiscountCoupens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountCoupen = await _context.DiscountCoupens.FindAsync(id);
            if (discountCoupen == null)
            {
                return NotFound();
            }
            DiscountCoupenVM discountCoupenVM = new()
            {
                Id = discountCoupen.Id,
                Code = discountCoupen.Code,
                Number = discountCoupen.Number,
                Comment = discountCoupen.Comment,
                Percent = discountCoupen.Percent,
                IsActive = discountCoupen.IsActive
            };
            if(discountCoupen.ExpiredDate.HasValue)
            {
                discountCoupenVM.EndDate = discountCoupen.ExpiredDate.ToShamsiN();
                discountCoupenVM.EndTime = discountCoupen.ExpiredDate.Value.ToString("hh:ss");
            }
            return View(discountCoupenVM);
        }

        // POST: Manage/DiscountCoupens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DiscountCoupenVM discountCoupenVM)
        {
            if (id != discountCoupenVM.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    DiscountCoupen discountCoupen = await _discountCoupen.GetByIdAsync(id);
                    discountCoupen.Number = discountCoupenVM.Number;
                    discountCoupen.IsActive = discountCoupenVM.IsActive;
                    discountCoupen.Comment = discountCoupenVM.Comment;
                    discountCoupen.Percent = discountCoupenVM.Percent;
                    if (!string.IsNullOrEmpty(discountCoupenVM.EndDate))
                    {
                        discountCoupen.ExpiredDate = discountCoupenVM.EndDate.ToMiladiWithTime(discountCoupenVM.EndTime);
                    }
                    _discountCoupen.Edit(discountCoupen);
                    await _discountCoupen.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountCoupenExists(discountCoupenVM.Id))
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
            return View(discountCoupenVM);
        }

        // GET: Manage/DiscountCoupens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountCoupen = await _context.DiscountCoupens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discountCoupen == null)
            {
                return NotFound();
            }

            return View(discountCoupen);
        }

        // POST: Manage/DiscountCoupens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discountCoupen = await _context.DiscountCoupens.FindAsync(id);
            _context.DiscountCoupens.Remove(discountCoupen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountCoupenExists(int id)
        {
            return _context.DiscountCoupens.Any(e => e.Id == id);
        }
    }
}
