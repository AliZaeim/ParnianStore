using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Core.Security;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Core.DTOs.Admin;
using Core.Utility;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("sitepages")]
    public class SitePagesController : Controller
    {
        private readonly IGenericService<SitePage> _genericService;

        public SitePagesController(IGenericService<SitePage> genericService)
        {
            _genericService = genericService;
            
        }

        // GET: Manage/SitePages
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }

        // GET: Manage/SitePages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitePage = await _genericService.GetByIdAsync((int)id);
            if (sitePage == null)
            {
                return NotFound();
            }

            return View(sitePage);
        }

        // GET: Manage/SitePages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/SitePages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SitePage sitePage)
        {
            
            if (ModelState.IsValid)
            {
                sitePage.DateCreated = DateTime.Now;
                var sitepages =await _genericService.FilterAsync(x => x.EnName.Equals(sitePage.EnName));
                if(sitepages.Any())
                {
                    ModelState.AddModelError("EnName", "این نام قبلا ثبت شده است !");
                    return View(sitePage);
                }
                _genericService.Create(sitePage);
                await _genericService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sitePage);
        }

        // GET: Manage/SitePages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitePage = await _genericService.GetByIdAsync((int)id);
            if (sitePage == null)
            {
                return NotFound();
            }
            return View(sitePage);
        }

        // POST: Manage/SitePages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  SitePage sitePage)
        {
            if (id != sitePage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sitepages = await _genericService.FilterAsync(x => x.EnName.Equals(sitePage.EnName));
                    if (sitepages != null)
                    {
                        ModelState.AddModelError("EnName", "این نام قبلا ثبت شده است !");
                        return View(sitePage);
                    }
                    _genericService.Edit(sitePage);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SitePageExists(sitePage.Id))
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
            return View(sitePage);
        }

        
        private bool SitePageExists(int id)
        {
            return _genericService.ExistEntity(id);
        }
    }
}
