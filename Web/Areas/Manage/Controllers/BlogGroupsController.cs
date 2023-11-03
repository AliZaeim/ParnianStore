using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("webloggroups")]
    public class BlogGroupsController : Controller
    {
       
        private readonly IBlogService _blogService;

        public BlogGroupsController(IBlogService blogService)
        {
            _blogService = blogService;
           
        }

        // GET: Manage/BlogGroups
        public async Task<IActionResult> Index()
        {
            return View(await _blogService.GetblogGroups());
        }

        // GET: Manage/BlogGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogGroup = await _blogService.GetBlogGroupWithId((int)id);
            if (blogGroup == null)
            {
                return NotFound();
            }

            return View(blogGroup);
        }

        // GET: Manage/BlogGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/BlogGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogGroup blogGroup)
        {
            if (!ModelState.IsValid)
            {
                return View(blogGroup);
            }
            if(await _blogService.GetBlogGroupWithEnName(blogGroup.BlogGroupEnTitle) != null)
            {
                ModelState.AddModelError("BlogGroupEnTitle", "نام تکراری است !");
                return View(blogGroup);
            }
            await _blogService.Create_BlogGroup(blogGroup);
            _blogService.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Manage/BlogGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogGroup = await _blogService.GetBlogGroupWithId((int)id);
            if (blogGroup == null)
            {
                return NotFound();
            }
            return View(blogGroup);
        }

        // POST: Manage/BlogGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogGroup blogGroup)
        {
            if (id != blogGroup.BlogGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _blogService.Edit_BlogGroup(blogGroup);
                    await _blogService.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogGroupExists(blogGroup.BlogGroupId))
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
            return View(blogGroup);
        }

        // GET: Manage/BlogGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogGroup = await _blogService.GetBlogGroupWithId((int)id);
            if (blogGroup == null)
            {
                return NotFound();
            }

            return View(blogGroup);
        }

        // POST: Manage/BlogGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogGroup = await _blogService.GetBlogGroupWithId(id);
            if(blogGroup==null)
            {
                return NotFound();
            }
            blogGroup.IsDeleted = true;
            _blogService.Edit_BlogGroup(blogGroup);
            await _blogService.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogGroupExists(int id)
        {
            return _blogService.ExistBlogGroup(id);
        }
    }
}
