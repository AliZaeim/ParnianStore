using Core.DTOs.General;
using DataLayer.Entities.Store;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowHPackagesComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Package> packages, string returnUrl, string title, string titleClass, string link)
        {
            HPackageComponentVM hPackageComponentVM = new()
            {
                Packages = packages,
                ReturnUrl = returnUrl,
                Title = title,
                TitleClass = titleClass,
                Link = link,
                

            };
            return await Task.FromResult(View("/Pages/Components/_showHPackages.cshtml", hPackageComponentVM));
           
        }
    }
}
