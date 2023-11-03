
using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("county")]
    public class CountiesController : Controller
    {
        
        private readonly IGenericService<County> _genericService;
        private readonly IGenericService<State> _stateService;
        public CountiesController(IGenericService<County> genericService, IGenericService<State> stateService)
        {
            
            _genericService = genericService;
            _stateService = stateService;
        }

        // GET: Manage/Counties
        public async Task<IActionResult> Index()
        {            
            return View(await _genericService.GetAllAsync());
        }

        // GET: Manage/Counties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var county = await _genericService.GetByIdAsync((int)id);
            if (county == null)
            {
                return NotFound();
            }

            return View(county);
        }

        // GET: Manage/Counties/Create
        public IActionResult Create()
        {
            ViewData["StateId"] = new SelectList (_stateService.GetAll(), "StateId", "StateName");
            return View();
        }

        // POST: Manage/Counties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( County county)
        {
            if (ModelState.IsValid)
            {
                _genericService.Create(county);
                await _genericService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = new SelectList(await _stateService.GetAllAsync(), "StateId", "StateName", county.StateId);
            return View(county);
        }

        // GET: Manage/Counties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var county = await _genericService.GetByIdAsync((int)id);
            if (county == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = new SelectList(await _stateService.GetAllAsync(), "StateId", "StateName", county.StateId);
            return View(county);
        }

        // POST: Manage/Counties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,County county)
        {
            if (id != county.CountyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _genericService.Edit(county);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_genericService.ExistEntity(county.CountyId))
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
            ViewData["StateId"] = new SelectList(await _stateService.GetAllAsync(), "StateId", "StateName", county.StateId);
            return View(county);
        }

        

        private bool CountyExists(int id)
        {
            return _genericService.ExistEntity(id);
        }
    }
}
