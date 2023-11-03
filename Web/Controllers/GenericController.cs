using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Blogs;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class GenericController : Controller
    {
        private readonly ICompService _compService;
        private readonly IProductService _productService;
        private readonly IGenericService<EmailBank> _genericService; 
        private readonly IBlogService _blogService;
        private readonly IStoreService _storeService;
        public GenericController(ICompService compService, IProductService productService, IGenericService<EmailBank> genericService,IBlogService blogService, IStoreService storeService)
        {
            
            _compService = compService;
            _productService = productService;
            _genericService = genericService;
            _blogService = blogService;
            _storeService = storeService;
        }
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        public IActionResult GetCountiesofState(int stId)
        {

            return ViewComponent("ShowCountiesComponent", new { stateId = stId });
        }
        public async Task<IActionResult> SearchResult(string text)
        {
            List<string> products = await _productService.SearchProductByTextAsync(text);

            return Json(products);
        }
        [HttpPost]
        public async Task<IActionResult> ProductSort(int? gId, int recperpage, string sortOption, string priceSelect)
        {
            List<Product> products = null;
            if (gId == null)
            {
                products = await _productService.GetProductsAsync();
            }
            else
            {
                ProductGroup productGroup = await _productService.GetProductGroupByIdAsync((int)gId);

                if (productGroup == null)
                {
                    return null;
                }


                products = await _productService.GetAllProductofGroupAsync(productGroup.ProductGroupId);
            }
            
            int recPerpage = recperpage;

            if (!string.IsNullOrEmpty(sortOption))
            {
                //if (sortOption == "defaultsortoption")
                //{
                //    products = await _productService.GetAllProductofGroupAsync(productGroup.ProductGroupId);
                //}
                if (sortOption == "popularity")
                {
                    products = products.OrderByDescending(x => x.Popularity).ToList();
                }
                if (sortOption == "lowpriceup")
                {
                    products = products.OrderBy(x => _productService.GetProductNetpriceAsync(x.ProductCode).Result).ToList();
                }
                if (sortOption == "highpricelow")
                {
                    products = products.OrderByDescending(x => _productService.GetProductNetpriceAsync(x.ProductCode).Result).ToList();
                }

            }
            if (!string.IsNullOrEmpty(priceSelect))
            {
                //if (priceSelect == "defaltprice")
                //{
                //    products = await _productService.GetAllProductofGroupAsync(productGroup.ProductGroupId);
                //}
                if (priceSelect == "discounted")
                {
                    products = products.Where(w => _productService.HasProductDiscountAsync(w.ProductCode).Result.HasDiscount).ToList();
                }
                if (priceSelect == "price0to20")
                {
                    products = products.Where(w => _productService.GetProductNetpriceAsync(w.ProductCode).Result > 0 && _productService.GetProductNetpriceAsync(w.ProductCode).Result <= 200000).ToList();
                }
                if (priceSelect == "price20to30")
                {
                    products = products.Where(w => _productService.GetProductNetpriceAsync(w.ProductCode).Result > 200000 && _productService.GetProductNetpriceAsync(w.ProductCode).Result <= 300000).ToList();
                }
                if (priceSelect == "price30to40")
                {
                    products = products.Where(w => _productService.GetProductNetpriceAsync(w.ProductCode).Result > 300000 && _productService.GetProductNetpriceAsync(w.ProductCode).Result <= 400000).ToList();
                }
                if (priceSelect == "price40to50")
                {
                    products = products.Where(w => _productService.GetProductNetpriceAsync(w.ProductCode).Result > 400000 && _productService.GetProductNetpriceAsync(w.ProductCode).Result <= 500000).ToList();
                }
                if (priceSelect == "priceup50")
                {
                    products = products.Where(w => _productService.GetProductNetpriceAsync(w.ProductCode).Result > 500000).ToList();
                }
            }
            products = products.Take(recPerpage).ToList();
            return PartialView(products);
        }
        //public int GetProductSortCount(List<prod>)
        public async Task<int> AddPopularity(int prId, string kind)
        {
            return await _productService.AddPopularityToProductAsync(prId, kind);
        }
        //[HttpPost]
        public async Task<JsonResult> AddEmail(string email)
        {
            AddEmailVM addEmailVM = new ();

            if (!string.IsNullOrEmpty(email))
            {
                addEmailVM.Email = email;

                EmailBank ex = _genericService.Filter(x => x.Email == email.ToString().Trim()).FirstOrDefault();
                if (ex == null)
                {
                    EmailBank emailBank = new()
                    {
                        Email = email,
                        CreatedDate = DateTime.Now
                    };
                    _genericService.Create(emailBank);
                    await _genericService.SaveChangesAsync();
                    addEmailVM.Eresult = true;
                    addEmailVM.Message = "ایمیل وارد شده با موفقیت ثبت شد !";


                }
                else
                {
                    addEmailVM.Eresult = false;
                    addEmailVM.Message = "ایمیل وارد شده قبلا ثبت شده است !";

                }

            }
            else
            {
                addEmailVM.Eresult = false;
                addEmailVM.Message = "لطفا ایمیل خود را وارد کنید !";

            }

            return Json(addEmailVM);
        }
        public IActionResult AddProductComment()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProductComment(NonMemberProductCommentVM nonMemberProductCommentVM)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(nonMemberProductCommentVM);
            }
            return PartialView();
        }
        public async Task<int> AddViews(string table, string id)
        {
            if (string.IsNullOrEmpty(table))
            {
                return 0;
            }
            int result = 0;
            switch (table.Trim())
            {
                case "slider":
                    {
                        Slider slider = await _compService.GetSliderById(int.Parse(id));
                        if (slider == null) break;
                        result = slider.Views + 1;
                        slider.Views += 1;
                        _compService.UpdateSlider(slider);
                        await _compService.SaveChangesAsync();                        
                        break;
                    }
                case "product":
                    {
                        Product product = await _productService.GetProductByIdAsync(int.Parse(id));
                        if (product == null) break;
                        result = product.Views + 1;
                        product.Views += 1;                        
                        _productService.EditProduct(product);
                        await _productService.SaveChangesAsync();
                        break;
                    }
                case "blog":
                    {
                        
                        Blog blog = await _blogService.GetBlogByIdAsync(id);
                        if (blog == null) break;
                        result = blog.BlogViewsCount + 1;
                        blog.BlogViewsCount += 1;
                        _blogService.Edit_Blog(blog);
                        await _blogService.SaveAsync();
                        break;
                    }
                default:
                    break;
                case "banner":
                    {
                        BannerNextSlider bannerNextSlider = await _compService.GetBannerNextSliderById(int.Parse(id));                        
                        if (bannerNextSlider == null) break;
                        result = bannerNextSlider.Views + 1;
                        bannerNextSlider.Views += 1;
                        _compService.EditBanner(bannerNextSlider);
                        await _compService.SaveChangesAsync();
                        break;
                    }
            }
            return result;

        }
        public async Task<int> GetCountyFreight(int cId)
        {
            int fr = 250000;
            if (cId == 0)
            {
                return fr;
            }
            
            County county = await _compService.GetCountyByIdAsync(cId);
            if(county == null)
            {
                return fr;
            }
            if(county.Freight != null)
            {
                fr= (int)county.Freight;
            }
            else
            {
                if(county.State.Freight != null)
                {
                    fr= (int)county.State.Freight;
                }
            }
            return fr;
        }
        public async Task<int> GetStateFreight(int sId)
        {
            int fr = 300000;
            if(sId == 0)
            {
                return fr;
            }
            State state = await _compService.GetStateByIdAsync(sId);
            if(state == null)
            {
                return fr;
            }
            if(state.Freight !=null)
            {
                fr = (int)state.Freight;
            }
            return fr;
        }
        public async Task<JsonResult> GetNotifications()
        {
            List<Notification> notifications = await _productService.GetNotifications();
            var noti = notifications.Where(w => w.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return Json(noti);
        }
        public async Task<IActionResult> TrackingOrder(string dedN)
        {
            Order order = await _storeService.GetOrderByDedicatedNumber(dedN);
            TrackingOrderVM trackingOrderVM = new();
            if(order == null)
            {
                trackingOrderVM.ExistOrder = false;
                trackingOrderVM.Message = "شماره سفارش نامعتبر است!";
            }
            else
            {
                trackingOrderVM.ExistOrder = true;
                trackingOrderVM.Order = order;

            }

            return Json(trackingOrderVM);
        }

    }
}
