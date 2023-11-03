using Core.DTOs.Admin;
using Core.Prodocers;
using Core.Security;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Blogs;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("aliannews")]
    public class BlogsController : Controller
    {

        private readonly IBlogService _blogService;
        private readonly ICompService _compService;
        private readonly IProductService _productService;
        public BlogsController(IBlogService blogService, ICompService compService, IProductService productService)
        {

            _blogService = blogService;
            _compService = compService;
            _productService = productService;
        }

        // GET: Manage/Blogs
        public async Task<IActionResult> Index()
        {
            return View(await _blogService.GetBlogsAsync());
        }
        public async Task<IActionResult> ShowBlogComments(Guid blogId)
        {
            Blog blog = await _blogService.GetBlogByIdAsync(blogId);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog.BlogComments.ToList());
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            BlogComment blogComment = await _blogService.GetBlogCommentByIdAsync(id);
            if (blogComment == null)
                return false;
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            blogComment.IsActive = st;
            _blogService.EditBlogComment(blogComment);
            await _blogService.SaveAsync();
            return st;

        }

        // GET: Manage/Blogs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog blog = await _blogService.GetBlogByIdAsync((Guid)id);

            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Manage/Blogs/Create
        public async Task<IActionResult> Create()
        {
            List<Blog> blogs = await _blogService.GetBlogsAsync();
            List<string> keys = blogs.Select(x => x.BlogShortKey).ToList();
            Blog blog = new()
            {
                BlogShortKey = Generators.GenerateKey(keys),


            };
            UniqueKey uniqueKey = new()
            {
                TableName = "blog",
                DateTime = DateTime.Now,
                Key = blog.BlogShortKey
            };
            _compService.CreateUniqueKey(uniqueKey);
            await _compService.SaveChangesAsync();
            var bgroups = await _blogService.GetblogGroups();
            ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle");
            ViewData["Products"] = await _productService.GetProductsAsync();
            ViewData["Packages"] = await _productService.GetPackagesAsync();
            ViewData["SelectedProducts"] = null;
            ViewData["SelectedPackages"] = null;
            return View(blog);
        }

        // POST: Manage/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, List<string> SelectedProducts, List<string> SelectedPackages, IFormFile BlogImageInBlog, IFormFile BlogImageInBlogDetails)
        {
            try
            {
                List<BlogGroup> bgroups = await _blogService.GetblogGroups();
                if (!ModelState.IsValid)
                {
                    ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                    ViewData["Products"] = await _productService.GetProductsAsync();
                    ViewData["SelectedProducts"] = SelectedProducts;
                    ViewData["Packages"] = await _productService.GetPackagesAsync();
                    ViewData["SelectedPackages"] = SelectedPackages;
                    return View(blog);

                }
                if (BlogImageInBlog == null)
                {
                    ModelState.AddModelError("BlogImageInBlog", "تصویر مربوط به صفحه بلاگ مشخص نشده است !");
                    ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                    ViewData["Products"] = await _productService.GetProductsAsync();
                    ViewData["SelectedProducts"] = SelectedProducts;
                    ViewData["Packages"] = await _productService.GetPackagesAsync();
                    ViewData["SelectedPackages"] = SelectedPackages;
                    return View(blog);
                }
                if (BlogImageInBlogDetails == null)
                {
                    ModelState.AddModelError("BlogImageInBlogDetails", "تصویر مربوط به صفحه جزئیات بلاگ مشخص نشده است !");
                    ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                    ViewData["Products"] = await _productService.GetProductsAsync();
                    ViewData["SelectedProducts"] = SelectedProducts;
                    ViewData["Packages"] = await _productService.GetPackagesAsync();
                    ViewData["SelectedPackages"] = SelectedPackages;
                    return View(blog);
                }
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                if (BlogImageInBlog != null)
                {
                    FileValidation ImgValid = await BlogImageInBlog.UploadedImageValidation(0, 0, 50, ex);

                    if (!ImgValid.IsValid)
                    {
                        ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                        ViewData["Products"] = await _productService.GetProductsAsync();
                        ViewData["SelectedProducts"] = SelectedProducts;
                        ViewData["Packages"] = await _productService.GetPackagesAsync();
                        ViewData["SelectedPackages"] = SelectedPackages;
                        ModelState.AddModelError("BlogImageInBlog", ImgValid.Message);
                        return View(blog);
                    }
                }
                if (BlogImageInBlogDetails != null)
                {
                    FileValidation ImgValid = await BlogImageInBlogDetails.UploadedImageValidation(0, 0, 100, ex);

                    if (!ImgValid.IsValid)
                    {
                        ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                        ViewData["Products"] = await _productService.GetProductsAsync();
                        ViewData["SelectedProducts"] = SelectedProducts;
                        ViewData["Packages"] = await _productService.GetPackagesAsync();
                        ViewData["SelectedPackages"] = SelectedPackages;
                        ModelState.AddModelError("BlogImageInBlogDetails", ImgValid.Message);
                        return View(blog);
                    }
                }

                string pcodes = string.Empty;
                int loop = 0;
                foreach (var item in SelectedProducts)
                {
                    if (loop + 1 < SelectedProducts.Count)
                    {
                        pcodes += item + Environment.NewLine;
                    }
                    else
                    {
                        pcodes += item;
                    }
                    loop++;

                }

                blog.ProductCodes = pcodes;
                string codespk = string.Empty; int lp = 0;
                foreach (var item in SelectedPackages)
                {
                    if (lp + 1 < SelectedPackages.Count)
                    {
                        codespk += item + Environment.NewLine;
                    }
                    else
                    {
                        codespk += item;
                    }
                    lp++;

                }
                if (await _blogService.ExistBlogKey(blog.BlogShortKey))
                {
                    ModelState.AddModelError("BlogShortKey", "کلید لینک پست تکراری است !");
                    ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                    ViewData["Products"] = await _productService.GetProductsAsync();
                    ViewData["SelectedProducts"] = SelectedProducts;
                    ViewData["Packages"] = await _productService.GetPackagesAsync();
                    ViewData["SelectedPackages"] = SelectedPackages;
                    return View(blog);
                }
                blog.PackageCodes = codespk;
                blog.BlogId = Guid.NewGuid();
                blog.BlogDate = DateTime.Now;
                if (BlogImageInBlog != null)
                {
                    blog.BlogImageInBlog = BlogImageInBlog.SaveUploadedImage("wwwroot/images/blogs", true);
                }
                if (BlogImageInBlogDetails != null)
                {
                    blog.BlogImageInBlogDetails = BlogImageInBlogDetails.SaveUploadedImage("wwwroot/images/blogs", true);
                }
                _blogService.Create_Blog(blog);
                await _blogService.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                throw;
            }


        }

        // GET: Manage/Blogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync((Guid)id);
            if (blog == null)
            {
                return NotFound();
            }
            var bgroups = await _blogService.GetblogGroups();
            ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle");
            ViewData["Products"] = await _productService.GetProductsAsync();
            ViewData["SelectedProducts"] = blog.ProductCodeList.ToList();
            ViewData["Packages"] = await _productService.GetPackagesAsync();
            ViewData["SelectedPackages"] = blog.PackageCodeList.ToList();
            return View(blog);
        }

        // POST: Manage/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Blog blog, List<string> SelectedProducts, List<string> SelectedPackages, string chdate, IFormFile BlogImageInBlog, IFormFile BlogImageInBlogDetails)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                    if (BlogImageInBlog != null)
                    {
                        FileValidation ImgValid = await BlogImageInBlog.UploadedImageValidation(0, 0, 50, ex);
                        if (!ImgValid.IsValid)
                        {
                            List<BlogGroup> blgroups = await _blogService.GetblogGroups();
                            ViewData["BlogGroupId"] = new SelectList(blgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                            ViewData["Products"] = await _productService.GetProductsAsync();
                            ViewData["SelectedProducts"] = SelectedProducts;
                            ViewData["Packages"] = await _productService.GetPackagesAsync();
                            ViewData["SelectedPackages"] = SelectedPackages;
                            ModelState.AddModelError("BlogImageInBlog", ImgValid.Message);
                            return View(blog);
                        }
                        blog.BlogImageInBlog = BlogImageInBlog.SaveUploadedImage("wwwroot/images/blogs", true);
                    }
                    if (BlogImageInBlogDetails != null)
                    {
                        FileValidation ImgValid = await BlogImageInBlogDetails.UploadedImageValidation(0, 0, 100, ex);
                        if (!ImgValid.IsValid)
                        {
                            var blgroups = await _blogService.GetblogGroups();
                            ViewData["BlogGroupId"] = new SelectList(blgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                            ViewData["Products"] = await _productService.GetProductsAsync();
                            ViewData["SelectedProducts"] = SelectedProducts;
                            ViewData["Packages"] = await _productService.GetPackagesAsync();
                            ViewData["SelectedPackages"] = SelectedPackages;
                            ModelState.AddModelError("BlogImageInBlogDetails", ImgValid.Message);
                            return View(blog);
                        }
                        blog.BlogImageInBlogDetails = BlogImageInBlogDetails.SaveUploadedImage("wwwroot/images/blogs", true);
                    }
                    if (blog.ProductCodeList != SelectedProducts)
                    {
                        string pcodes = string.Empty;
                        foreach (string item in SelectedProducts)
                        {
                            if (item != SelectedProducts.LastOrDefault())
                            {
                                pcodes += item + Environment.NewLine;
                            }
                            else
                            {
                                pcodes += item;
                            }
                        }
                        string pkcodes = string.Empty;
                        foreach (string item in SelectedPackages)
                        {
                            if (item != SelectedPackages.LastOrDefault())
                            {
                                pkcodes += item + Environment.NewLine;
                            }
                            else
                            {
                                pkcodes += item;
                            }
                        }
                        string codespk = string.Join(Environment.NewLine, SelectedPackages.ToArray());
                        if (chdate == "yes")
                        {
                            blog.BlogDate = DateTime.Now;
                        }

                        blog.ProductCodes = pcodes;
                        blog.PackageCodes = pkcodes;
                    }
                    if (!await _blogService.PermisionToUseKey(blog.BlogShortKey, blog.BlogId))
                    {
                        ModelState.AddModelError("BlogShortKey", "کلید لینک پست تکراری است !");
                        List<BlogGroup> blgroups = await _blogService.GetblogGroups();
                        ViewData["BlogGroupId"] = new SelectList(blgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
                        ViewData["Products"] = await _productService.GetProductsAsync();
                        ViewData["SelectedProducts"] = SelectedProducts;
                        ViewData["Packages"] = await _productService.GetPackagesAsync();
                        ViewData["SelectedPackages"] = SelectedPackages;
                        return View(blog);
                    }
                    _blogService.Edit_Blog(blog);
                    await _blogService.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
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
            var bgroups = await _blogService.GetblogGroups();
            ViewData["BlogGroupId"] = new SelectList(bgroups.Where(w => w.IsActive), "BlogGroupId", "BlogGroupTitle", blog.BlogGroupId);
            ViewData["Products"] = await _productService.GetProductsAsync();
            ViewData["SelectedProducts"] = blog.ProductCodeList.ToList();
            ViewData["Packages"] = await _productService.GetPackagesAsync();
            ViewData["SelectedPackages"] = SelectedPackages;
            return View(blog);

        }

        // GET: Manage/Blogs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync((Guid)id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Manage/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            blog.IsDeleted = true;
            _blogService.Edit_Blog(blog);
            await _blogService.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(Guid id)
        {
            return _blogService.ExistBlogbyId(id);
        }
    }
}
