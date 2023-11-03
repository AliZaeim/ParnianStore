using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Context;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("fractionslider")]
    public class FSTextsController : Controller
    {
        
        private readonly ICompService _compService;
        public FSTextsController(ICompService compService)
        {
            
            _compService = compService;
        }

        // GET: Manage/FSTexts
        public async Task<IActionResult> Index()
        {
            var myContext = await _compService.GetFSTextsAsync();
            return View(myContext);
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            FSText fSText = await _compService.GetFSTextByIdAsync(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            fSText.IsActive = st;
            _compService.UpdateFSText(fSText);
            await _compService.SaveChangesAsync();
            return st;

        }
        // GET: Manage/FSTexts/Details/5
        public async Task<IActionResult> Details(int? id,string retUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fSText = await _compService.GetFSTextByIdAsync((int)id);
               
            if (fSText == null)
            {
                return NotFound();
            }
            fSText.ReturnUrl = retUrl;
            return View(fSText);
        }

        // GET: Manage/FSTexts/Create
        public async Task<IActionResult> Create(int? fsId, string retUrl)
        {
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title",fsId);
            FSText fSText = new()
            {
                ReturnUrl = retUrl
            };
            return View(fSText);
        }

        // POST: Manage/FSTexts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FSText fSText)
        {
            if (ModelState.IsValid)
            {
                _compService.CreateFSText(fSText);
                await _compService.SaveChangesAsync();
                if(string.IsNullOrEmpty(fSText.ReturnUrl))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect(fSText.ReturnUrl.Replace("-","/"));
                }
                
            }
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title");
            return View(fSText);
        }

        // GET: Manage/FSTexts/Edit/5
        public async Task<IActionResult> Edit(int? id,string retUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fSText = await _compService.GetFSTextByIdAsync((int)id);
            if (fSText == null)
            {
                return NotFound();
            }
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title");
            fSText.ReturnUrl = retUrl;
            return View(fSText);
        }

        // POST: Manage/FSTexts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  FSText fSText)
        {
            if (id != fSText.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _compService.UpdateFSText(fSText);
                    await _compService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FSTextExists(fSText.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (string.IsNullOrEmpty(fSText.ReturnUrl))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect(fSText.ReturnUrl.Replace("-","/"));
                }
            }
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title");
            return View(fSText);
        }

        // GET: Manage/FSTexts/Delete/5
        public async Task<IActionResult> Delete(int? id,string retUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fSText = await _compService.GetFSTextByIdAsync((int)id);
            if (fSText == null)
            {
                return NotFound();
            }
            fSText.ReturnUrl = retUrl;
            return View(fSText);
        }

        // POST: Manage/FSTexts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string ReturnUrl)
        {
            var fSText = await _compService.GetFSTextByIdAsync(id);
            _compService.RemoveFSText(fSText);
            await _compService.SaveChangesAsync();
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect(ReturnUrl.Replace("-", "/"));
            }
        }

        private bool FSTextExists(int id)
        {
            var fsTexts = _compService.GetFSTextsAsync().Result;
            return fsTexts.Any(a => a.IsActive && a.FractionSlider.IsActive && a.Id == id);
        }
    }
}
