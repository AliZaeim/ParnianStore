using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("sitefaq")]
    public class QuestionAnswersController : Controller    {
        
        private readonly IGenericService<QuestionAnswer> _genericService;
        public QuestionAnswersController(IGenericService<QuestionAnswer> genericService)
        {
            _genericService = genericService;
            
        }

        // GET: Manage/QuestionAnswers
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }

        // GET: Manage/QuestionAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _genericService.GetByIdAsync((int)id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // GET: Manage/QuestionAnswers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/QuestionAnswers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {
                questionAnswer.CreatedDate = DateTime.Now;
                _genericService.Create(questionAnswer);
                await _genericService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionAnswer);
        }

        // GET: Manage/QuestionAnswers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _genericService.GetByIdAsync((int)id);
            if (questionAnswer == null)
            {
                return NotFound();
            }
            return View(questionAnswer);
        }

        // POST: Manage/QuestionAnswers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  QuestionAnswer questionAnswer)
        {
            if (id != questionAnswer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    questionAnswer.CreatedDate = DateTime.Now;
                    _genericService.Edit(questionAnswer);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionAnswerExists(questionAnswer.Id))
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
            return View(questionAnswer);
        }

        // GET: Manage/QuestionAnswers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _genericService.GetByIdAsync((int)id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // POST: Manage/QuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionAnswer = await _genericService.GetByIdAsync(id);
            _genericService.Delete(questionAnswer);
            await _genericService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionAnswerExists(int id)
        {
            return _genericService.GetAll().Any(x => x.Id == id);
        }
    }
}
