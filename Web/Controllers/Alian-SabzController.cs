using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class Alian_SabzController : Controller
    {
        [Route("Terms")]
        public IActionResult Terms()
        {
            return Redirect("Alian-Sabz-Terms");
        }
        [Route("FAQ")]
        public IActionResult FAQ()
        {
            return Redirect("Alian-Sabz-FAQ");
        }
        [Route("AlianSabz_FAQ")]
        public IActionResult AlianSabz_FAQ()
        {
            return Redirect("Alian-Sabz-FAQ");
        }
        [Route("/products/generalpackages")]
        public IActionResult generalpackages()
        {
            return Redirect("/Packages/General-Packages");
        }
        [Route("/Package/d/Men's-sexual-enhancement-pack")]
        public IActionResult mensextualpack()
        {
            return Redirect("/Package/d/men-sexual-enhancement-pack");
        }
        [Route("/Package/d/Women's-sexual-enhancement-pack")]
        public IActionResult womensextualpack()
        {
            return Redirect("/Package/d/women-sexual-enhancement-pack");
        }
        [Route("/Products/General-(Men-and-Women)")]
        public IActionResult Generalmenwomen()
        {
            return Redirect("/Products/General-Men-Women");
        }
        [Route("/blogs/medicinal plants")]
        public IActionResult Medicinalplants()
        {
            return Redirect("/blogs/medicinal-plants");
        }
    }
}
