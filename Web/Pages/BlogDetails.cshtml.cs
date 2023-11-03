using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Blogs;
using DataLayer.Entities.Store;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class BlogDetailsModel : PageModel
    {
        private readonly IBlogService _blogService;
        private readonly IProductService _productService;

        public BlogDetailsModel(IBlogService blogService, IProductService productService)
        {
            _blogService = blogService;
            _productService = productService;

        }


        public BlogDetailsVM BlogDetailsVM { get; set; } = new();
        public List<Product> TagProducts { get; set; }
        public async Task<IActionResult> OnGetAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return NotFound();
            }

            Blog blog = await _blogService.GetBlogWithKeyAsync(code);
            if (blog == null)
            {
                return NotFound();
            }
            List<Product> products = await _productService.GetProductsAsync();
            List<Package> packages = await _productService.GetPackagesAsync();
            BlogDetailsVM.Blog = blog;
            BlogDetailsVM.BlogId = blog.BlogId;
            BlogDetailsVM.BlogCode = code;
            BlogDetailsVM.BlogGroups = await _blogService.GetblogGroups();
            BlogDetailsVM.RelatedProducts = products.Where(w => blog.ProductCodeList.Any(a => a== w.ProductCode)).ToList();
            BlogDetailsVM.RelatedPackages = packages.Where(w => blog.PackageCodeList.Any(z => z == w.PkCode)).ToList();
            if (User.Identity.IsAuthenticated)
            {
                User user = await _productService.GetUserByUserNameAsync(User.Identity.Name);
                if (user != null)
                {
                    BlogDetailsVM.Name = user.Name;
                    BlogDetailsVM.Family = user.Family;
                    BlogDetailsVM.Cellphone = user.Cellphone;
                }
            }
            blog.BlogViewsCount += 1;
            _blogService.Edit_Blog(blog);
            await _blogService.SaveAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(BlogDetailsVM blogDetailsVM)
        {
            if (!ModelState.IsValid)
                return Page();
            BlogComment blogComment = new()
            {
                CreatedDate = DateTime.Now,
                Comment = blogDetailsVM.Comment,
                Name = blogDetailsVM.Name,
                Family = blogDetailsVM.Family,
                BlogId = blogDetailsVM.BlogId,
                Cellphone = blogDetailsVM.Cellphone
            };
            _blogService.CreateBlogComment(blogComment);
            await _blogService.SaveAsync();
            TempData["saved"] = true;
           
            return RedirectToPage("/BlogDetails", new { code = blogDetailsVM.BlogCode });
        }
    }
}
