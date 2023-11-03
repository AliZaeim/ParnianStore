using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Core.Security;
using Core.DTOs.Admin;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("reportsforadmin")]
    public class AdminReportsController : Controller
    {
        private readonly IUserService _userService;
        public AdminReportsController(IUserService userService)
        {
            _userService = userService;
        }
        [PermissionCheckerByPermissionName("registeredusers")]
        public async Task<IActionResult> Registered()
        {
            return View(await _userService.GetUsersAsync());
        }
        [PermissionCheckerByPermissionName("createuser")]
        public async Task<IActionResult> CreateUser()
        {
            UserVM userVM = new()
            {
                Roles = await _userService.GetRolesAsync()
            };

            return View(userVM);
        }
        [PermissionCheckerByPermissionName("createuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                userVM.Roles = await _userService.GetRolesAsync();
                return View(userVM);
            }
            if (await _userService.GetUserByCellphoneAsync(userVM.Cellphone) != null)
            {
                ModelState.AddModelError("Cellphone", "تلفن همراه قبلا ثبت شده است !");
                userVM.Roles = await _userService.GetRolesAsync();
                return View(userVM);
            }
            string password = Core.Prodocers.Generators.GenerateUniqueString(0, 0, 8, 0);
           
            User user = new()
            {
                Name = userVM.Name,
                Family = userVM.Family,
                UserName = userVM.Cellphone,
                Cellphone = userVM.Cellphone,
                Password = password,
                IsActive = true,
                RegisteredDate = DateTime.Now
            };
            UserRole userRole = new()
            {
                User = user,
                RoleId = (int)userVM.RoleId,
                RegisterDate = DateTime.Now,
                IsActive = true
            };
            _userService.CreateUserRole(userRole);
            await _userService.SaveChangesAsync();
            _userService.SendUserName_and_Password(userVM.Cellphone, password, userVM.Cellphone);
            return RedirectToAction(nameof(Registered), "AdminReports");
        }
        [PermissionCheckerByPermissionName("addrole")]
        public async Task<IActionResult> AddRole(int userId)
        {
            List<Role> user_Roles = await _userService.GetRolesOfUserAsync(userId);
            List<Role> roles = await _userService.GetRolesAsync();
            User user = await _userService.GetUserByIdAsync(userId);
            if(user == null)
            {
                return NotFound("کاربر نامعتبر است !");
            }
            AddRoleVM addRoleVM = new()
            {
                UserId = userId,
                Roles = roles.Except(user_Roles).ToList(),
                User_FullName = user.FullName
            };
            return View(addRoleVM);
        }
        [PermissionCheckerByPermissionName("addrole")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddRoleVM addRoleVM)
        {
            if (!ModelState.IsValid)
            {
                addRoleVM.Roles = await _userService.GetRolesAsync();
                return View(addRoleVM);
            }
            User user = await _userService.GetUserByIdAsync(addRoleVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("RoleId", "کاربر تامعتبر است !");
                addRoleVM.Roles = await _userService.GetRolesAsync();
                return View(addRoleVM);
            }
            UserRole userRole = new()
            {
                UserId = addRoleVM.UserId,
                RoleId = (int)addRoleVM.RoleId,
                IsActive = true,
                RegisterDate = DateTime.Now
            };
            _userService.CreateUserRole(userRole);
            await _userService.SaveChangesAsync();
            return RedirectToAction(nameof(Registered), "AdminReports");
        }
        [PermissionCheckerByPermissionName("registeredusers")]
        public async Task<bool> ChangeStatus(int id, int status)
        {
            User user = await _userService.GetUserByIdAsync(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            user.IsActive = st;
            _userService.UpdateUser(user);
            await _userService.SaveChangesAsync();
            return st;

        }
        [PermissionCheckerByPermissionName("registeredusers")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [PermissionCheckerByPermissionName("registeredusers")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync((int)id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["CountyId"] = new SelectList(await _userService.GetCounties(), "CountyId", "CountyName", user.CountyId);
           
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionCheckerByPermissionName("registeredusers")]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userService.UpdateUser(user);
                    await _userService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactInfoExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Registered));
            }
            ViewData["CountyId"] = new SelectList(await _userService.GetCounties(), "CountyId", "CountyName", user.CountyId);
           
            return View(user);
        }
        private bool ContactInfoExists(int id)
        {
            User user = _userService.GetUserByIdAsync(id).Result;
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public async Task<IActionResult> GetEmails()
        {
            return View(await _userService.GetEmailBanksAsync());
        }
        public async Task<IActionResult> DeleteEmail(int id)
        {
            _userService.RemoveEmailById(id);
            await _userService.SaveChangesAsync();

            return RedirectToAction(nameof(GetEmails));
        }
    }
}
