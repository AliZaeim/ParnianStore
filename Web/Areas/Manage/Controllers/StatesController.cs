using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Entities.Supplementary;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Core.Security;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("state")]
    public class StatesController : Controller
    {
       
        private readonly IGenericService<State> _genericService;
        public StatesController(IGenericService<State> generateService)
        {
            
            _genericService = generateService;
        }

        // GET: Manage/States
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }

        // GET: Manage/States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _genericService.GetByIdAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: Manage/States/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/States/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StateId,StateName,Freight,IsDeleted")] State state)
        {
            if (ModelState.IsValid)
            {
                _genericService.Create(state);
                await _genericService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: Manage/States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _genericService.GetByIdAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        // POST: Manage/States/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  State state)
        {
            if (id != state.StateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _genericService.Edit(state);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.StateId))
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
            return View(state);
        }

        
        private bool StateExists(int id)
        {
            return _genericService.ExistEntity(id);
        }
    }
}
