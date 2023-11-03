using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.BlogData;
using Core.Services.Interfaces;
using DataLayer.Entities.Blogs;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class BlogsModel : PageModel
    {
        private readonly IBlogService _blogService;
        private readonly IProductService _productService;
        public BlogsModel(IBlogService blogService, IProductService productService)
        {
            _blogService = blogService;
            _productService = productService;
        }


        public BlogVM BlogVM { get; set; } = new BlogVM();
        public async Task OnGetAsync(string gName, string Tag, string archive, int? pageId)
        {
            BlogVM.InitialInfo = await _productService.GetInitialInfoAsync();
            int zpage = pageId.GetValueOrDefault(1);
            int ztotalpage = 1;
            int RecperPage = 12;
            List<Product> products = await _productService.GetProductsAsync();
            List<Blog> blogs = await _blogService.GetBlogsAsync();
            BlogVM.TotalCount = blogs.Count;
            BlogVM.SearchType = "all";
            if (!string.IsNullOrEmpty(gName))
            {
                if (gName != "all")
                {
                    blogs = blogs.Where(w => w.BlogGroup.BlogGroupEnTitle.Trim().ToLower().Replace(" ","-") == gName).ToList();
                    BlogVM.BlogGroup = await _blogService.GetBlogGroupWithEnName(gName);
                    gName = gName.Replace(" ", "-");
                    BlogVM.SearchType = "group";
                }
                
            }
            if (!string.IsNullOrEmpty(Tag))
            {
                Tag = Tag.Replace("-", " ");
                blogs = blogs.Where(w => w.BlogTags.Contains(Tag)).ToList();
                BlogVM.SearchTag = Tag;
                BlogVM.SearchType = "tag";
                
               
            }
            if (!string.IsNullOrEmpty(archive))
            {
                string[] dte = archive.Split("-");
                string ye = dte[0];
                string mo = dte[1];
                int yr = int.Parse(ye);
                int mth = int.Parse(mo);
                BlogVM.Mounth = mth;
                BlogVM.Year = yr;
                PersianCalendar pc = new();
                blogs = blogs.Where(w => pc.GetYear(w.BlogDate) == yr && pc.GetMonth(w.BlogDate) == mth).ToList();
                BlogVM.SearchType = "archive";
            }
            BlogVM.CurrentPage = zpage;
            if (blogs.Count % RecperPage == 0)
            {
                ztotalpage = blogs.Count / RecperPage;
            }
            else
            {
                ztotalpage = (blogs.Count / RecperPage) + 1;
            }
            //BlogVM.TotalCount = blogs.Count;
            blogs = blogs.OrderByDescending(x => x.BlogDate).ToList();
            BlogVM.BlogGroups = await _blogService.GetblogGroups();
            BlogVM.Products = products.Where(w => w.IsActive).ToList();
            BlogVM.GroupEnName = gName;
            BlogVM.FeaturedProducts = products.Where(w => w.ProductIsFeatured).ToList();
            BlogVM.UsefulTags = await _blogService.GetBlogsUsefullTagsAsync(10);
            blogs = blogs.Skip((zpage - 1) * RecperPage).Take(RecperPage).ToList();
            BlogVM.TotalPages = ztotalpage;
            BlogVM.Blogs = blogs;
        }
        [BindProperty]
        public string Searchtype { get; set; }
        [BindProperty]
        public string GroupEnName { get; set; }
        [BindProperty]
        public int? Year { get; set; }
        [BindProperty]
        public int? Mounth { get; set; }
        [BindProperty]
        public int? PageId { get; set; }
        [BindProperty]
        public string Tag { get; set; }
        public async Task<IActionResult> OnPost()
        {
            List<Blog> blogs = await _blogService.GetBlogsAsync();
            List<Product> products = await _productService.GetProductsAsync();
            blogs = blogs.OrderByDescending(x => x.BlogDate).ToList();
            BlogVM.TotalCount = blogs.Count;
            int zpage = PageId.GetValueOrDefault(1);
            int ztotalpage = 1;
            int RecperPage = 12;
            if (blogs.Count % RecperPage == 0)
            {
                ztotalpage = blogs.Count / RecperPage;
            }
            else
            {
                ztotalpage = (blogs.Count / RecperPage) + 1;
            }
            BlogVM.GroupId = null;
            BlogVM.BlogGroup = null;
            BlogVM.BlogGroups = await _blogService.GetblogGroups();
            if (!string.IsNullOrEmpty(GroupEnName))
            {
                if(GroupEnName !="all")
                {
                    BlogGroup blogGroup = await _blogService.GetBlogGroupWithEnName(GroupEnName);

                    if (blogGroup != null)
                    {
                        BlogVM.GroupId = blogGroup.BlogGroupId;
                        blogs = await _blogService.GetBlogsWithGroupAsync(blogGroup.BlogGroupId);
                        blogs = blogs.OrderByDescending(x => x.BlogDate).ToList();
                        BlogVM.BlogGroup = blogGroup;
                        
                    }
                   
                }
               
                BlogVM.SearchType = "group";
                
            }
            if(Year != null && Mounth != null && Searchtype =="archive")
            {
                blogs = await _blogService.GetBlogsWithYearMounth((int)Year, (int)Mounth);
                blogs = blogs.OrderByDescending(x => x.BlogDate).ToList();
                BlogVM.Year = Year;
                BlogVM.Mounth = Mounth;
                BlogVM.SearchType = "archive";
            }
            if(!string.IsNullOrEmpty(Tag))
            {
                BlogVM.SearchType = "tag";
                blogs = await _blogService.GetBlogsWithTagAsync(Tag);
                blogs = blogs.OrderByDescending(x => x.BlogDate).ToList();
                BlogVM.SearchTag = Tag;
            }
            blogs = blogs.Skip((zpage - 1) * RecperPage).Take(RecperPage).ToList();
            BlogVM.CurrentPage = zpage;
            //if (blogs.Count() % RecperPage == 0)
            //{
            //    ztotalpage = blogs.Count() / RecperPage;
            //}
            //else
            //{
            //    ztotalpage = (blogs.Count() / RecperPage) + 1;
            //}
            BlogVM.TotalPages = ztotalpage;
            BlogVM.Blogs = blogs;
            BlogVM.BlogGroups = await _blogService.GetblogGroups();
            BlogVM.Products = products.Where(w => w.IsActive).ToList();
            return Page();
        }
       
    }
}
