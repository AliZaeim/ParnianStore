using Core.Services.Interfaces;
using DataLayer.Entities.Blogs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowBlogsComponent : ViewComponent
    {
        private readonly IBlogService _blogService;
        public ShowBlogsComponent(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Count)
        {
            List<Blog> blogs = await _blogService.GetBlogsAsync();
            blogs = blogs.OrderByDescending(x => x.BlogDate).Take(Count).ToList();
            return await Task.FromResult(View("/Pages/Components/_showBlogs.cshtml", blogs));
        }
    }
}
