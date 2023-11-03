using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("faq")]
    public class FAQsController : Controller
    {
        
        private readonly IGenericService<FAQ> _generateService;
        public FAQsController(IGenericService<FAQ> genericService)
        {            
            _generateService = genericService;
        }

        // GET: Manage/FAQs
        public async Task<IActionResult> Index()
        {
            return View(await _generateService.GetAllAsync());
        }
        public bool ChangeStatus(int id, int status)
        {
            FAQ fAQ = _generateService.GetById(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            fAQ.IsActive = st;
            _generateService.Edit(fAQ);
            _generateService.SaveChanges();
            return st;

        }
        // GET: Manage/FAQs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _generateService.GetByIdAsync((int)id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // GET: Manage/FAQs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/FAQs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                fAQ.CreateDate = DateTime.Now;
                _generateService.Create(fAQ);
                await _generateService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fAQ);
        }

        // GET: Manage/FAQs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _generateService.GetByIdAsync((int)id);
            if (fAQ == null)
            {
                return NotFound();
            }
            return View(fAQ);
        }

        // POST: Manage/FAQs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FAQ fAQ)
        {
            if (id != fAQ.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _generateService.Edit(fAQ);
                    await _generateService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(fAQ.Id))
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
            return View(fAQ);
        }

        // GET: Manage/FAQs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _generateService.GetByIdAsync((int)id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // POST: Manage/FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fAQ = await _generateService.GetByIdAsync(id);
            _generateService.Delete(fAQ);
            await _generateService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FAQExists(int id)
        {
            return _generateService.ExistEntity(id);
        }
    }
}
