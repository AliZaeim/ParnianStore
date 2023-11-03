using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("manageroles")]
    public class RolesController : Controller
    {
       
        private readonly IUserService _userService;
        public RolesController(IUserService userService)
        {
            _userService = userService;
            
        }

        // GET: Manage/Roles
        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetRolesAsync());
        }

        // GET: Manage/Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _userService.GetRoleByIdAsync((int)id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Manage/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role)
        {
            if (ModelState.IsValid)
            {
                if(await _userService.ExistRoleByName(role.RoleName) )
                {
                    ModelState.AddModelError("RoleName", "نام نقش موجود است !");
                    return View(role);
                }
                _userService.CreateRole(role);
                await _userService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Manage/Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _userService.GetRoleByIdAsync((int)id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Manage/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userService.EditRole(role);
                    await _userService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userService.ExistRoleById(role.RoleId))
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
            return View(role);
        }

        // GET: Manage/Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _userService.GetRoleByIdAsync((int)id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Manage/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _userService.GetRoleByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            role.IsDeleted = true;
            _userService.EditRole(role);
            await _userService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
