using Core.Services.Interfaces;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.General;
using DataLayer.Entities.User;
using DataLayer.Entities.Supplementary;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        private readonly MyContext _context;
        public ProductService(MyContext context)
        {
            _context = context;
        }
        #region General
        public async Task<bool> ExistFractionSliderAsync()
        {
            return await _context.FractionSliders.AnyAsync(x => x.IsActive);
        }

        public async Task<bool> ExistSliderAsync()
        {
            return await _context.Sliders.AnyAsync(x => x.SliderIsActive);
        }
        public async Task<ValidateDiscountCoupenVM> ValidateDiscountCoupenAsync(string code)
        {
            ValidateDiscountCoupenVM validateDiscountCoupenVM = new();
            if (string.IsNullOrEmpty(code))
            {
                validateDiscountCoupenVM.Comment = "کد کوپن تخفیف وارد نشده است";
            }
            int number = 0;
            int used = 0;
            DiscountCoupen discountCoupen = await _context.DiscountCoupens.SingleOrDefaultAsync(x => x.Code.Equals(code));
            if (discountCoupen != null)
            {
                number = (int)discountCoupen.Number;
                validateDiscountCoupenVM.DiscountCoupen = discountCoupen;
            }
            else
            {
                validateDiscountCoupenVM.Comment = "کوپن موجود نمی باشد !";
            }
            used = await _context.Orders.Where(r => r.Order_DiscountCode.Equals(code)).CountAsync();
            if (number - used > 0)
            {
                validateDiscountCoupenVM.Validity = true;
                validateDiscountCoupenVM.Comment = "کوپن تخفیف معتبر است";
            }
            return validateDiscountCoupenVM;
        }

        public async Task<DiscountCoupen> GetDiscountByCodeAsync(string code)
        {
            return await _context.DiscountCoupens.FirstOrDefaultAsync(x => x.Code.Equals(code));
        }
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            User user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == userName);
            return user;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<InitialInfo> GetInitialInfoAsync()
        {
            return await _context.InitialInfos.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        }
        public async Task<List<Notification>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }
        public async Task<SitePage> GetSitePageByEnNameAsync(string EnName)
        {
            return await _context.SitePages.SingleOrDefaultAsync(x => x.EnName.Equals(EnName));
        }
        #endregion
        #region productGroup
        public bool DetachProductGroup(ProductGroup productGroup)
        {
            _context.Entry(productGroup).State = EntityState.Detached;
            return true;
        }
        public void CreateProductGroup(ProductGroup productGroup)
        {
            _context.ProductGroups.Add(productGroup);
        }

        public void EditProductGroup(ProductGroup productGroup)
        {
            _context.ProductGroups.Update(productGroup);
        }

        public async Task<ProductGroup> GetProductGroupByIdAsync(int productGroupId)
        {
            return await _context.ProductGroups.Include(r => r.Parent).Include(x => x.Products).SingleOrDefaultAsync(s => s.ProductGroupId == productGroupId);
        }

        public async Task<List<ProductGroup>> GetProductGroupsAsync()
        {
            return await _context.ProductGroups.Include(r => r.Products).Include(r => r.Banner).Include(r => r.Parent).OrderBy(r => r.Parent).ToListAsync();
        }

        public async Task<bool> RemoveProductGroupAsync(int Id)
        {
            ProductGroup productGroup = await _context.ProductGroups.FindAsync(Id);
            if (productGroup == null)
            {
                return false;
            }
            string ImagePath = string.Empty;
            if (!string.IsNullOrEmpty(productGroup.ProductGroupImage))
            {
                ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productGroups", productGroup.ProductGroupImage);
            }

            _context.ProductGroups.Remove(productGroup);
            if (File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
            }
            return true;
        }

        public async Task<bool> ExistProductGroupAsync(int Id)
        {
            return await _context.ProductGroups.AnyAsync(x => x.ProductGroupId == Id);
        }

        public async Task<bool> ExistProductGroupCodeAsync(string Code)
        {
            if (string.IsNullOrEmpty(Code))
            {
                return false;
            }
            ProductGroup productGroup = await _context.ProductGroups.FirstOrDefaultAsync(x => x.ProductGroupMark == Code);
            if (productGroup == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<ProductGroup> GetProductGroupByTitleAsync(string title)
        {
            ProductGroup productGroup = await _context.ProductGroups.Include(r => r.Products).Include(r => r.Parent)
                .SingleOrDefaultAsync(x => x.ProductGroupTitle.Replace(" ", "").Equals(title.Replace("-", "").Replace(" ", "")));
            return productGroup;
        }
        public async Task<ProductGroup> GetProductGroupByEnTitleAsync(string Entitle)
        {
            ProductGroup productGroup = await _context.ProductGroups.Include(r => r.Products).Include(r => r.Parent)
                .SingleOrDefaultAsync(x => x.ProductEnGroupTitle.Replace(" ", "").Equals(Entitle.Replace("-", "").Replace(" ", "")));
            return productGroup;
        }

        public async Task<List<ProductGroup>> GetRelatedProductGroup_of_ProductGroupAsync(int gid)
        {
            ProductGroup productGroup = await _context.ProductGroups.Include(r => r.Parent).SingleOrDefaultAsync(x => x.ProductGroupId == gid);
            if (productGroup == null)
            {
                return null;
            }
            List<ProductGroup> productGroups = new();
            productGroups.Add(productGroup);
            if (productGroup.Parent == null)
            {
                List<ProductGroup> subs = await _context.ProductGroups.Include(r => r.Parent).Where(w => w.ParentId == productGroup.ProductGroupId).ToListAsync();
                subs = subs.Where(w => w.ProductGroupId != productGroup.ProductGroupId).ToList();
                if (subs != null)
                {
                    productGroups.AddRange(subs);
                }
            }
            else
            {
                List<ProductGroup> sibs = await _context.ProductGroups.Include(r => r.Parent).Where(w => w.ParentId == (int)productGroup.ParentId).ToListAsync();
                sibs = sibs.Where(w => w.ProductGroupId != productGroup.ProductGroupId).ToList();
                if (sibs != null)
                {
                    productGroups.AddRange(sibs);
                }
            }
            return productGroups;
        }
        #endregion ProductGroup
        #region Product
        public void CreateProduct(Product product)
        {
            _context.Products.Add(product);
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductComments).Include(x => x.ProductIngredients).ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int Id)
        {
            Product product = await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductComments).Include(x => x.ProductIngredients).SingleOrDefaultAsync(f => f.ProductId == Id);
            return product;
        }
        public void EditProduct(Product product)
        {
            _context.Products.Update(product);
        }
        public async Task<bool> ExistProductAsync(int Id)
        {
            return await _context.Products.AnyAsync(a => a.ProductId == Id);
        }
        public async Task<bool> RemoveProductAsync(int Id)
        {
            Product product = await _context.Products.SingleOrDefaultAsync(s => s.ProductId == Id);
            if (product == null)
            {
                return false;
            }
            string ImagePath = string.Empty;
            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", product.ProductImage);
            }
            _context.Products.Remove(product);
            if (File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
            }
            return true;

        }
        public async Task<string> GetProductCodeAsync(int pgId)
        {
            List<Product> products = await _context.Products.Include(r => r.ProductGroup).Where(w => w.ProductGroupId == pgId).ToListAsync();
            ProductGroup productGroup = await _context.ProductGroups.FirstOrDefaultAsync(f => f.ProductGroupId == pgId);
            if (productGroup == null)
            {
                return null;
            }
            string code = string.Empty;
            if (products == null)
            {
                code = productGroup.ProductGroupMark + "-1";
            }
            else
            {
                if (products.Count == 0)
                {
                    code = productGroup.ProductGroupMark + "-1";
                }
                else
                {
                    string strCode = string.Join("", products.LastOrDefault().ProductCode.ToCharArray().Where(Char.IsDigit));
                    int intCode = int.Parse(strCode);
                    code = productGroup.ProductGroupMark + "-" + (intCode + 1).ToString();
                }

            }
            return code;
        }
        public async Task<Product> GetProductWithCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            Product product = await _context.Products.Include(r => r.ProductGroup).SingleOrDefaultAsync(x => x.ProductCode == code);
            return product;
        }
        public async Task<bool> ExistProductByCodeAsync(string code)
        {
            Product product = await _context.Products.SingleOrDefaultAsync(x => x.ProductCode.Trim() == code.Trim());
            if (product != null)
            {
                return true;
            }
            return false;
        }
        public bool DetachProduct(Product Deproduct)
        {
            _context.Entry(Deproduct).State = EntityState.Detached;
            return true;
        }
        public async Task<DiscountOptionVM> HasProductDiscountAsync(string code)
        {
            string cur = "ریال";
            var initialInfos = await _context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
            if (initialInfos != null)
            {
                cur = initialInfos.SiteCurrency;
            }
            DiscountOptionVM discountOptionVM = new()
            {
                HasDiscount = false,
                DiscountValue = 0,

            };
            if (string.IsNullOrEmpty(code))
            {
                return discountOptionVM;
            }
            Product product = await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductGroup.Parent)
                .SingleOrDefaultAsync(x => x.ProductCode == code);
            if (product == null)
            {
                return discountOptionVM;
            }
            if (product.ProductPercentDiscount != 0 || product.ProductValueDiscount != 0)
            {
                discountOptionVM.HasDiscount = true;
                if (product.ProductValueDiscount != 0)
                {
                    discountOptionVM.DiscountPercent = product.ProductPercentDiscount.GetValueOrDefault(0);
                    discountOptionVM.DiscountValue = product.ProductValueDiscount.GetValueOrDefault(0);
                    discountOptionVM.DiscountUnit = cur;
                    discountOptionVM.NetPrice = product.ProductPrice - product.ProductValueDiscount.GetValueOrDefault(0);
                    discountOptionVM.Note = "تخفیف مبلغی محصول" + " " + product.ProductValueDiscount.ToString() + " " + cur;
                    discountOptionVM.DiscountAmount = product.ProductValueDiscount.GetValueOrDefault(0);
                }
                if (product.ProductPercentDiscount != 0)
                {
                    discountOptionVM.DiscountPercent = product.ProductPercentDiscount.GetValueOrDefault(0);
                    discountOptionVM.DiscountValue = product.ProductValueDiscount.GetValueOrDefault(0);
                    discountOptionVM.DiscountUnit = "%";
                    discountOptionVM.NetPrice = product.ProductPrice - (int)(product.ProductPrice * product.ProductPercentDiscount / 100);
                    discountOptionVM.Note = "تخفیف درصدی محصول" + " " + product.ProductPercentDiscount.ToString() + " " + "درصد";
                    discountOptionVM.DiscountAmount = (int)(product.ProductPrice * (product.ProductPercentDiscount / 100));
                }
                return discountOptionVM;
            }
            if (product.ProductGroup != null)
            {
                if (product.ProductGroup.ProductGroupDiscountPercent != 0 || product.ProductGroup.ProductGroupDiscountValue != 0)
                {
                    discountOptionVM.HasDiscount = true;
                    if (product.ProductGroup.ProductGroupDiscountValue != 0)
                    {
                        discountOptionVM.DiscountValue = product.ProductGroup.ProductGroupDiscountValue;
                        discountOptionVM.DiscountUnit = cur;
                        discountOptionVM.NetPrice = product.ProductPrice - product.ProductGroup.ProductGroupDiscountValue;
                        discountOptionVM.Note = "تخفیف مبلغی گروه" + " " + product.ProductGroup.ProductGroupTitle + " " + product.ProductGroup.ProductGroupDiscountValue.ToString() + " " + cur;
                        discountOptionVM.DiscountAmount = product.ProductGroup.ProductGroupDiscountValue;
                    }
                    if (product.ProductGroup.ProductGroupDiscountPercent != 0)
                    {

                        discountOptionVM.DiscountPercent = product.ProductGroup.ProductGroupDiscountPercent;
                        discountOptionVM.DiscountUnit = "%";
                        discountOptionVM.NetPrice = product.ProductPrice - (int)(product.ProductPrice * product.ProductGroup.ProductGroupDiscountPercent / 100);
                        discountOptionVM.Note = "تخفیف درصدی گروه" + " " + product.ProductGroup.ProductGroupTitle + " " + product.ProductGroup.ProductGroupDiscountPercent.ToString() + " " + "درصد";
                        discountOptionVM.DiscountAmount = (int)(product.ProductPrice * (product.ProductGroup.ProductGroupDiscountPercent / 100));
                    }
                    return discountOptionVM;
                }
            }
            if (product.ProductGroup != null)
            {
                if (product.ProductGroup.Parent != null)
                {
                    if (product.ProductGroup.Parent.ProductGroupDiscountPercent != 0 || product.ProductGroup.Parent.ProductGroupDiscountValue != 0)
                    {
                        discountOptionVM.HasDiscount = true;
                        if (product.ProductGroup.Parent.ProductGroupDiscountValue != 0)
                        {
                            discountOptionVM.DiscountValue = product.ProductGroup.Parent.ProductGroupDiscountValue;
                            discountOptionVM.DiscountUnit = cur;
                            discountOptionVM.NetPrice = product.ProductPrice - product.ProductGroup.Parent.ProductGroupDiscountValue;
                            discountOptionVM.Note = "تخفیف مبلغی گروه" + " " + product.ProductGroup.ProductGroupTitle + " " + product.ProductGroup.Parent.ProductGroupDiscountValue.ToString() + " " + cur;
                            discountOptionVM.DiscountAmount = product.ProductGroup.Parent.ProductGroupDiscountValue;
                        }
                        if (product.ProductGroup.Parent.ProductGroupDiscountPercent != 0)
                        {
                            discountOptionVM.DiscountPercent = (float)product.ProductGroup.Parent.ProductGroupDiscountPercent;
                            discountOptionVM.DiscountUnit = "%";
                            discountOptionVM.NetPrice = product.ProductPrice - (int)(product.ProductPrice * product.ProductGroup.Parent.ProductGroupDiscountPercent / 100);
                            discountOptionVM.Note = "تخفیف درصدی گروه" + " " + product.ProductGroup.ProductGroupTitle + " " + product.ProductGroup.Parent.ProductGroupDiscountPercent.ToString() + " " + "درصد";
                            discountOptionVM.DiscountAmount = (int)(product.ProductPrice * (product.ProductGroup.Parent.ProductGroupDiscountPercent / 100));
                        }
                        return discountOptionVM;
                    }
                }
            }
            return discountOptionVM;

        }
        public async Task<DiscountOptionVM> HasProductDiscountAsync(int PId)
        {
            DiscountOptionVM discountOptionVM = new()
            {
                HasDiscount = false,
                DiscountValue = 0,

            };
            string cur = "ریال";
            var initialInfos = await _context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
            if (initialInfos != null)
            {
                cur = initialInfos.SiteCurrency;
            }
            if (PId == 0)
            {
                return discountOptionVM;
            }
            Product product = await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductGroup.Parent)
                .SingleOrDefaultAsync(x => x.ProductId == PId);
            if (product == null)
            {
                return discountOptionVM;
            }
            if (product.ProductPercentDiscount != 0 || product.ProductValueDiscount != 0)
            {
                discountOptionVM.HasDiscount = true;
                if (product.ProductValueDiscount != 0)
                {
                    discountOptionVM.DiscountValue = product.ProductValueDiscount.GetValueOrDefault(0);
                    discountOptionVM.DiscountUnit = cur;
                    discountOptionVM.NetPrice = product.ProductPrice - product.ProductValueDiscount.GetValueOrDefault(0);
                }
                if (product.ProductPercentDiscount != 0)
                {
                    discountOptionVM.DiscountPercent = product.ProductPercentDiscount.GetValueOrDefault(0);
                    discountOptionVM.DiscountUnit = "%";
                    discountOptionVM.NetPrice = product.ProductPrice - (int)(product.ProductPrice * product.ProductPercentDiscount / 100);
                }
                return discountOptionVM;
            }
            if (product.ProductGroup != null)
            {
                if (product.ProductGroup.ProductGroupDiscountPercent != 0 || product.ProductGroup.ProductGroupDiscountValue != 0)
                {
                    discountOptionVM.HasDiscount = true;
                    if (product.ProductGroup.ProductGroupDiscountValue != 0)
                    {
                        discountOptionVM.DiscountValue = product.ProductGroup.ProductGroupDiscountValue;
                        discountOptionVM.DiscountUnit = cur;
                        discountOptionVM.NetPrice = product.ProductPrice - product.ProductGroup.ProductGroupDiscountValue;
                    }
                    if (product.ProductGroup.ProductGroupDiscountPercent != 0)
                    {

                        discountOptionVM.DiscountPercent = (float)product.ProductGroup.ProductGroupDiscountPercent;
                        discountOptionVM.DiscountUnit = "%";
                        discountOptionVM.NetPrice = product.ProductPrice - (int)(product.ProductPrice * product.ProductGroup.ProductGroupDiscountPercent / 100);
                    }
                    return discountOptionVM;
                }
            }
            if (product.ProductGroup != null)
            {
                if (product.ProductGroup.Parent != null)
                {
                    if (product.ProductGroup.Parent.ProductGroupDiscountPercent != 0 || product.ProductGroup.Parent.ProductGroupDiscountValue != 0)
                    {
                        discountOptionVM.HasDiscount = true;
                        if (product.ProductGroup.Parent.ProductGroupDiscountValue != 0)
                        {
                            discountOptionVM.DiscountValue = product.ProductGroup.Parent.ProductGroupDiscountValue;
                            discountOptionVM.DiscountUnit = cur;
                            discountOptionVM.NetPrice = product.ProductPrice - product.ProductGroup.Parent.ProductGroupDiscountValue;
                        }
                        if (product.ProductGroup.Parent.ProductGroupDiscountPercent != 0)
                        {
                            discountOptionVM.DiscountPercent = (float)product.ProductGroup.Parent.ProductGroupDiscountPercent;
                            discountOptionVM.DiscountUnit = "%";
                            discountOptionVM.NetPrice = product.ProductPrice - (int)(product.ProductPrice * product.ProductGroup.Parent.ProductGroupDiscountPercent / 100);
                        }
                        return discountOptionVM;
                    }
                }
            }
            return discountOptionVM;
        }
        public async Task<int> GetProductNetpriceAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return 0;
            }
            Product product = await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductGroup.Parent)
                .SingleOrDefaultAsync(x => x.ProductCode == code);
            if (product == null)
            {
                return 0;
            }
            int price = product.ProductPrice;
            if (product.ProductValueDiscount != 0)
            {
                price -= product.ProductValueDiscount.GetValueOrDefault(0);
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (product.ProductPercentDiscount != 0)
            {
                int discount = (int)(price * product.ProductPercentDiscount / 100);
                price -= discount;
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (product.ProductGroup.ProductGroupDiscountValue != 0)
            {
                price -= product.ProductGroup.ProductGroupDiscountValue;
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (product.ProductGroup.ProductGroupDiscountPercent != 0)
            {
                int discount = (int)(price * product.ProductGroup.ProductGroupDiscountPercent / 100);
                price -= discount;
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (product.ProductGroup.Parent != null)
            {
                if (product.ProductGroup.Parent.ProductGroupDiscountValue != 0)
                {
                    price -= product.ProductGroup.Parent.ProductGroupDiscountValue;
                    if (price > 0)
                    {
                        return price;
                    }
                    else
                    {
                        return 0;
                    }
                }
                if (product.ProductGroup.Parent.ProductGroupDiscountPercent != 0)
                {
                    int discount = (int)(price * product.ProductGroup.Parent.ProductGroupDiscountPercent / 100);
                    price -= discount;
                    if (price > 0)
                    {
                        return price;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            return price;

        }
        public async Task<List<Product>> GetAllProductofGroupAsync(int gId)
        {
            ProductGroup productGroup = await _context.ProductGroups.Include(r => r.Parent).Include(r => r.Products).SingleOrDefaultAsync(x => x.ProductGroupId == gId); ;
            List<Product> products = new();
            if (productGroup == null)
            {
                return null;
            }
            if (productGroup.Parent != null)
            {
                products = await _context.Products.Where(w => w.ProductGroupId == gId).ToListAsync();
            }
            else
            {
                //List<ProductGroup> productGroups = await _context.ProductGroups.Include(r => r.Products).Where(w => w.ParentId == productGroup.ProductGroupId).ToListAsync();
                //foreach (var item in productGroups)
                //{
                //    if(item.Products !=null)
                //    {
                //        products.AddRange(item.Products.ToList());
                //    }

                //}
                return await _context.Products.ToListAsync();
            }
            return products.ToList();
        }
        public async Task<Product> GetProductByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            Product product = await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductComments).SingleOrDefaultAsync(x => x.ProductName.Trim().Replace(" ", "-") == name.Trim());
            return product;
        }
        public async Task<Product> GetProductByEnName(string Ename)
        {
            if (string.IsNullOrEmpty(Ename))
            {
                return null;
            }

            Product product = await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductComments).Include(x => x.ProductIngredients).SingleOrDefaultAsync(x => x.ProductEnName.Trim().Replace(" ", "-") == Ename.Trim());
            return product;
        }
        public async Task<List<string>> SearchProductByTextAsync(string search)
        {
            List<string> SearchResultList = null;
            if (!string.IsNullOrEmpty(search))
            {
                List<Product> products = await _context.Products.Where(w => w.IsActive).Include(r => r.ProductGroup).ToListAsync();
                var pr = products.Where(w => w.ProductName.Contains(search.Trim()) || w.FeaturessListcompleteSplit.Any(a => a == search)).ToList();
                //var prg = products.Where(w => w.IsActive && w.ProductGroup.ProductGroupTitle.Contains(search.Trim())).ToList();
                if (pr != null)
                {
                    if (SearchResultList == null)
                    {
                        SearchResultList = new List<string>();
                    }
                    foreach (var item in pr.ToList())
                    {
                        string str = string.Empty;
                        string strHow = string.Empty;
                        string strpid = string.Empty;
                        if (item != pr.LastOrDefault())
                        {
                            str = "<p class='srchP text-white m-t-1 pr-1  border-bottom w-100 block'>" + item.ProductName + " | " + item.ProductTopFeature + "</p>";
                            strHow = "<p class='srchP text-white m-t-1 pr-1  border-bottom w-100 block'>" + "طریقه مصرف" + " " + item.ProductName + " | " + item.ProductTopFeature + "</p>";
                            strpid = item.ProductId.ToString();
                        }
                        else
                        {
                            str = "<p class='srchP text-white m-t-1 pr-1 w-100 border-bottom block'>" + item.ProductName + " | " + item.ProductTopFeature + "</p>";
                            strHow = "<p class='srchP text-white m-t-1 pr-1 w-100 block'>" + "طریقه مصرف" + " " + item.ProductName + " | " + item.ProductTopFeature + "</p>";
                            strpid = item.ProductId.ToString();
                        }

                        SearchResultList.Add(str);
                        SearchResultList.Add(strHow);
                    }


                }
                //if (prg != null)
                //{
                //    if (SearchResultList == null)
                //    {
                //        SearchResultList = new List<string>();
                //    }
                //    SearchResultList.AddRange(prg.Select(x =>x.ProductName + "|" + x.ProductGroup.ProductGroupTitle));
                //}

            }

            return SearchResultList.Distinct().ToList();
        }
        public int GetProductCountinCart(int pr_id, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return 0;
            }
            User user = _context.Users.FirstOrDefault(f => f.UserName == userName);
            if (user == null)
            {
                return 0;
            }
            Product product = _context.Products.Include(r => r.ProductGroup).FirstOrDefault(f => f.ProductId == pr_id);
            if (product == null)
            {
                return 0;
            }
            Cart User_Last_Cart = _context.Carts.Include(r => r.CartItems).FirstOrDefault(w => w.IsActive && w.CheckOut == false && w.UserId == user.Id);
            if (User_Last_Cart == null)
            {
                return 0;
            }
            if (User_Last_Cart.CartItems.Any(x => x.ProductId == pr_id))
            {
                return User_Last_Cart.CartItems.FirstOrDefault(w => w.ProductId == pr_id).Quantity;
            }
            else
            {
                return 0;
            }
        }
        public async Task<List<Product>> GetFeaturedProductsAsync()
        {
            return await _context.Products.Where(w => w.ProductIsFeatured && w.IsActive).ToListAsync();
        }
        public async Task<List<Product>> GetProductsByGroupMarkAsync(string gMark)
        {
            ProductGroup productGroup = await _context.ProductGroups.Include(r => r.Products).SingleOrDefaultAsync(x => x.ProductGroupMark == gMark.Trim());
            if (productGroup == null)
            {
                return null;
            }
            return productGroup.Products.ToList();
        }
        public async Task<ProductGroup> GetProductGroupByMarkAsync(string mark)
        {
            return await _context.ProductGroups.SingleOrDefaultAsync(x => x.ProductGroupMark == mark.Trim());
        }
        public async Task<List<Product>> GetDiscountedProductsAsync()
        {

            List<Product> products = await _context.Products.Include(r => r.ProductGroup).Include(r => r.ProductGroup.Parent).ToListAsync();
            products = products.Where(w => w.ProductValueDiscount != 0 || w.ProductPercentDiscount != 0 || w.ProductGroup.ProductGroupDiscountValue != 0
            || w.ProductGroup.ProductGroupDiscountPercent != 0 || w.ProductGroup.Parent?.ProductGroupDiscountPercent != 0 || w.ProductGroup.Parent?.ProductGroupDiscountValue != 0).ToList();
            return products;
        }
        public async Task<bool> ExistSpeceficProdutsAsync()
        {
            return await _context.Products.AnyAsync(a => a.IsActive && a.ProductIsFeatured);
        }

        public async Task<List<Product>> GetProductsWithLowInventory()
        {
            List<Product> products = await _context.Products.ToListAsync();
            products = products.Where(w => GetProduct_Inventory_InStoreAsync(w.ProductCode).Result < (int)w.ProductMinimumInventory).ToList();
            return products.ToList();
        }
        public async Task<List<Product>> GetProductsByTagAsync(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return null;
            tag = tag.Replace("-", " ");
            List<Product> products = await _context.Products.Include(r => r.ProductGroup).ToListAsync();
            products = products.Where(w => w.ProductTagsList.Contains(tag)).ToList();
            return products;
        }
        public async Task<bool> ExistActiveProductAsync()
        {
            return await _context.Products.AnyAsync(x => x.IsActive);
        }
        public async Task<List<Package>> GetPackagesbyProductAsyncByName(string EnName)
        {
            Product product = await _context.Products.Include(x => x.PackageProducts).SingleOrDefaultAsync(x => x.ProductEnName.Replace(" ", "-") == EnName);

            if (product == null)
            {
                return null;
            }
            //return product.PackageProducts.Select(x => x.Package).ToList();

            var packs = await _context.Packages.Include(x => x.PackageProducts).Where(w => w.PackageProducts.Any(a => a.ProductId == product.ProductId)).ToListAsync();
            return packs.ToList();
        }

        #endregion Product
        #region WareHouse
        public void CreateWareHouse(WareHouse wareHouse)
        {
            _context.WareHouses.Add(wareHouse);
        }

        public async Task<WareHouse> GetWareHouseByIdAsync(int wareHouseId)
        {
            return await _context.WareHouses.Include(x => x.Product).Include(x => x.Package).Include(x => x.OrderDetail).Include(r => r.OrderDetail.Order).SingleOrDefaultAsync(s => s.WareHouseId == wareHouseId);
        }

        public async Task<List<WareHouse>> GetWareHousesAsync()
        {
            return await _context.WareHouses.Include(x => x.Product).Include(x => x.Package).Include(r => r.OrderDetail).Include(r => r.OrderDetail.Order).ToListAsync();
        }

        public async Task<List<WareHouse>> GetWareHousesByProductCodeAsync(string productCode)
        {
            return await _context.WareHouses.Include(x => x.Product).Include(x => x.Package).Include(r => r.OrderDetail).Include(r => r.OrderDetail.Order).Where(w => w.Product.ProductCode == productCode).ToListAsync();
        }

        public void EditWareHouse(WareHouse wareHouse)
        {
            _context.WareHouses.Update(wareHouse);
        }

        public async Task<int> GetProduct_Inventory_InStoreAsync(string productCode)
        {
            int imp = await _context.WareHouses.Where(w => w.Product.ProductCode == productCode).SumAsync(s => s.WareHouse_Input);
            int exp = await _context.WareHouses.Where(w => w.Product.ProductCode == productCode).SumAsync(s => s.WareHouse_Export);
            return (imp - exp);
        }

        public async Task DeleteWareHouseAsync(int wareHouseId)
        {
            WareHouse wareHouse = await _context.WareHouses.SingleOrDefaultAsync(s => s.WareHouseId == wareHouseId);
            _context.WareHouses.Remove(wareHouse);
        }

        public async Task<int> AddPopularityToProductAsync(int Pr_id, string Kind)
        {
            if (Kind == "pr")
            {
                Product product = await _context.Products.SingleOrDefaultAsync(s => s.ProductId == Pr_id);
                product.Popularity += 1;
                _context.Update(product);
                await _context.SaveChangesAsync();
                return product.Popularity;
            }
            if (Kind == "pk")
            {
                Package package = await _context.Packages.SingleOrDefaultAsync(s => s.PkId == Pr_id);
                package.Popularity += 1;
                _context.Update(package);
                await _context.SaveChangesAsync();
                return package.Popularity;
            }
            return 0;

        }
        public void CreateWareHouseList(List<WareHouse> wareHouses)
        {
            _context.WareHouses.AddRange(wareHouses);
        }
        #endregion WareHouse
        #region ProductComment
        public void CreateProductComment(ProductComment productComment)
        {
            _context.ProductComments.Add(productComment);
        }

        public async Task<List<ProductComment>> GetProductCommentsAsync()
        {
            return await _context.ProductComments.ToListAsync();
        }
        public void EditProductComment(ProductComment productComment)
        {
            _context.ProductComments.Update(productComment);
        }

        public async Task<ProductComment> GetProductCommentByIdAsync(int Id)
        {
            return await _context.ProductComments.SingleOrDefaultAsync(x => x.Id == Id);
        }

        #endregion ProductComment
        #region Banners
        public void CreateBanner(Banner banner)
        {
            _context.Banners.Add(banner);
        }
        public void UpdateBanner(Banner banner)
        {
            _context.Banners.Update(banner);
        }
        public async Task<List<Banner>> GetBannersAsync()
        {
            return await _context.Banners.ToListAsync();
        }
        public async Task<Banner> GetBannerByIdAsync(int Id)
        {
            return await _context.Banners.SingleOrDefaultAsync(x => x.Id == Id);
        }
        public bool ExistBanner(int Id)
        {
            return _context.Banners.Any(x => x.Id == Id);
        }
        public void RemoveBanner(Banner banner)
        {
            _context.Banners.Remove(banner);
        }
        public bool ExitAnyBanner()
        {
            return _context.Banners.Any();
        }
        public async Task<Banner> GetLastBanner()
        {
            return await _context.Banners.Where(w => w.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        }
        public async Task<Banner> GetLastBannerbyBannerCount(int count)
        {

            if (count == 1)
            {
                return await _context.Banners.Where(w => w.Banner1 != null && w.Banner2 == null && w.Banner3 == null && w.Banner4 == null && w.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            }
            if (count == 2)
            {
                return await _context.Banners.Where(w => w.Banner1 != null && w.Banner2 != null && w.Banner3 == null && w.Banner4 == null && w.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            }
            if (count == 3)
            {
                return await _context.Banners.Where(w => w.Banner1 != null && w.Banner2 != null && w.Banner3 != null && w.Banner4 == null && w.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            }
            if (count == 4)
            {
                return await _context.Banners.Where(w => w.Banner1 != null && w.Banner2 != null && w.Banner3 != null && w.Banner4 != null && w.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            }


            return null;

        }

        public async Task<Banner> GetBannerByOrderandCount(int order, int count)
        {
            Banner banner = null;
            List<Banner> banners = await _context.Banners.OrderBy(x => x.Id).ToListAsync();
            if (count == 1)
            {
                banner = banners.Where(w => w.Banner1 != null && w.Banner2 == null && w.Banner3 == null && w.Banner4 == null).ToList().Skip(order - 1).Take(order).FirstOrDefault();
            }
            if (count == 2)
            {
                banner = banners.Where(w => w.Banner1 != null && w.Banner2 != null && w.Banner3 == null && w.Banner4 == null).ToList().Skip(order - 1).Take(order).FirstOrDefault();
            }
            if (count == 3)
            {
                banner = banners.Where(w => w.Banner1 != null && w.Banner2 != null && w.Banner3 != null && w.Banner4 == null).ToList().Skip(order - 1).Take(order).FirstOrDefault();
            }
            if (count == 4)
            {
                banner = banners.Where(w => w.Banner1 != null && w.Banner2 != null && w.Banner3 != null && w.Banner4 != null).ToList().Skip(order - 1).Take(order).FirstOrDefault();
            }
            return banner;
        }


        #endregion Banners        
        #region Ingredient
        public void CreateIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
        }

        public void Updateungredient(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
        }

        public async Task<List<Ingredient>> GetIngredientsAsync()
        {
            return await _context.Ingredients.Include(x => x.ProductIngredients).ToListAsync();
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int Id)
        {
            return await _context.Ingredients.SingleOrDefaultAsync(x => x.IngredientId == Id);
        }

        public bool ExitIngredientById(int Id)
        {
            return _context.Ingredients.Any(x => x.IngredientId == Id);
        }

        public void DeleteIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
        }


        #endregion Ingredient
        #region ProductIngredient
        public async Task<List<ProductIngredient>> GetProductIngredientsByProductIdAsync(int productId)
        {
            return await _context.ProductIngredients.Include(x => x.Product).Include(x => x.Ingredient)
                .Where(w => w.ProductId == productId).ToListAsync();
        }

        public async Task<bool> UpdateProductIngredients(int productId, List<int> NewIngredients)
        {
            try
            {
                Product product = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == productId);
                if (product == null)
                    return false;
                List<ProductIngredient> productIngredients = await _context.ProductIngredients.Include(x => x.Product).Include(x => x.Ingredient)
                                                                .Where(w => w.ProductId == productId).ToListAsync();
                List<Ingredient> Curings = productIngredients.Select(x => x.Ingredient).ToList();
                List<int> CuringIds = Curings.Select(x => x.IngredientId).ToList();
                List<int> Remove_Newingredients_Expect_CuringIds = CuringIds.Except(NewIngredients).ToList(); //return items from permissions that not in perIds
                List<int> Add_CuringIds_Expect_Newingredients = NewIngredients.Except(CuringIds).ToList(); //return items from perIds that not in permissions

                bool RemRes = await RemoveIngredientsFromProduct(productId, Remove_Newingredients_Expect_CuringIds);
                bool AddRes = await AddIngredientsToProduct(productId, Add_CuringIds_Expect_Newingredients);
                return true;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return false;
            }
        }

        public async Task<bool> AddIngredientsToProduct(int productId, List<int> Ingredints)
        {
            List<ProductIngredient> productIngredients = new();
            foreach (var ing in Ingredints)
            {
                productIngredients.Add(new ProductIngredient
                {
                    IngredientId = ing,
                    ProductId = productId
                });

            }
            if (productIngredients.Count != 0)
            {
                await _context.ProductIngredients.AddRangeAsync(productIngredients);
            }
            return true;
        }

        public async Task<bool> RemoveIngredientsFromProduct(int productId, List<int> Ingredints)
        {
            List<ProductIngredient> productIngredients = new();
            foreach (var ing in Ingredints)
            {
                productIngredients.Add(new ProductIngredient
                {
                    IngredientId = ing,
                    ProductId = productId
                });

            }
            if (productIngredients.Count != 0)
            {
                foreach (var item in productIngredients)
                {
                    ProductIngredient remm = await _context.ProductIngredients.SingleOrDefaultAsync(x => x.ProductId == productId && x.IngredientId == item.IngredientId);
                    if (remm != null)
                    {
                        _context.ProductIngredients.Remove(remm);
                    }

                }

            }
            return true;
        }

        public async Task<List<int>> IngredientsofProduct(int productId)
        {
            return await _context.ProductIngredients.Include(x => x.Product).Include(x => x.Ingredient)
                .Where(r => r.ProductId == productId)
                .Select(r => r.IngredientId).ToListAsync();
        }
        public async Task<List<Product>> GetProductsofInderendientAsync(int ingId)
        {
            List<Product> products = await _context.ProductIngredients.Include(x => x.Product).Where(w => w.IngredientId == ingId).Select(x => x.Product).ToListAsync();
            return products;
        }

        #endregion
        #region Packages
        public async Task CreatePackageAsync(Package package, List<int> products)
        {
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
            foreach (var item in products)
            {
                _context.PackageProducts.Add(new PackageProduct { PkId = package.PkId, ProductId = item });
            }
        }

        public async Task<List<Package>> GetPackagesAsync()
        {
            return await _context.Packages.Include(x => x.PackageComments).Include(x => x.PackageProducts).Include(x => x.PackageGroup).ToListAsync();
        }

        public async Task<Package> GetPackageByIdAsync(int Id)
        {
            return await _context.Packages.Include(x => x.PackageProducts).Include(x => x.PackageComments).Include(x => x.PackageGroup).SingleOrDefaultAsync(x => x.PkId == Id);
        }

        public bool UpdatePackageProducts(int pId, List<int> products)
        {
            try
            {
                List<int> PrevPros = _context.PackageProducts.Where(w => w.PkId == pId).Select(x => x.ProductId).ToList();
                List<int> RemovePros = PrevPros.Except(products).ToList();
                List<int> AddPros = products.Except(PrevPros).ToList();
                if (RemovePros.Count != 0)
                {
                    foreach (var remPr in RemovePros)
                    {
                        PackageProduct pp = _context.PackageProducts.FirstOrDefault(f => f.PkId == pId && f.ProductId == remPr);
                        _context.PackageProducts.Remove(pp);
                    }
                }

                if (AddPros.Count != 0)
                {
                    foreach (var addPr in AddPros)
                    {
                        PackageProduct pp = new() { PkId = pId, ProductId = addPr };
                        _context.PackageProducts.Add(pp);
                    }
                }


                return true;
            }
            catch (Exception Ex)
            {
                string m = Ex.Message;
                return false;
            }

        }
        public void UpdatePackage(Package package)
        {
            _context.Packages.Update(package);
        }
        public bool ExistPackage(int Id)
        {
            return _context.Packages.Any(x => x.PgId == Id);
        }
        public void RemovePackage(Package package)
        {
            _context.Packages.Remove(package);
        }
        public async Task<DiscountOptionVM> HasPackageDiscountAsync(int packId)
        {
            string cur = "ریال";
            var initialInfos = await _context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
            if (initialInfos != null)
            {
                cur = initialInfos.SiteCurrency;
            }
            DiscountOptionVM discountOptionVM = new()
            {
                HasDiscount = false,
                DiscountValue = 0,

            };

            Package package = await _context.Packages.Include(r => r.PackageGroup).Include(r => r.PackageGroup.Parent)
                .SingleOrDefaultAsync(x => x.PkId == packId);
            if (package == null)
            {
                return discountOptionVM;
            }
            if (package.PkDiscountValue != 0 || package.PkDiscountPercent != 0)
            {
                discountOptionVM.HasDiscount = true;
                if (package.PkDiscountValue != 0)
                {
                    discountOptionVM.DiscountPercent = package.PkDiscountPercent.GetValueOrDefault(0);
                    discountOptionVM.DiscountValue = package.PkDiscountValue.GetValueOrDefault(0);
                    discountOptionVM.DiscountUnit = cur;
                    discountOptionVM.NetPrice = (int)package.PkPrice - package.PkDiscountValue.GetValueOrDefault(0);
                    discountOptionVM.Note = "تخفیف مبلغی پک" + " " + package.PkDiscountValue.ToString() + " " + cur;
                    discountOptionVM.DiscountAmount = package.PkDiscountValue.GetValueOrDefault(0);
                }
                if (package.PkDiscountPercent != 0)
                {
                    discountOptionVM.DiscountPercent = package.PkDiscountPercent.GetValueOrDefault(0);
                    discountOptionVM.DiscountValue = package.PkDiscountValue.GetValueOrDefault(0);
                    discountOptionVM.DiscountUnit = "%";
                    discountOptionVM.NetPrice = (int)package.PkPrice - (int)(package.PkPrice * package.PkDiscountPercent / 100);
                    discountOptionVM.Note = "تخفیف درصدی پک" + " " + package.PkDiscountPercent.ToString() + " " + "درصد";
                    discountOptionVM.DiscountAmount = (int)(package.PkPrice * (package.PkDiscountPercent / 100));
                }
                return discountOptionVM;
            }
            if (package.PackageGroup != null)
            {
                if (package.PackageGroup.PgDiscountPercent != 0 || package.PackageGroup.PgDiscountValue != 0)
                {
                    discountOptionVM.HasDiscount = true;
                    if (package.PackageGroup.PgDiscountValue != 0)
                    {
                        discountOptionVM.DiscountValue = package.PackageGroup.PgDiscountValue;
                        discountOptionVM.DiscountUnit = cur;
                        discountOptionVM.NetPrice = (int)package.PkPrice - package.PackageGroup.PgDiscountValue;
                        discountOptionVM.Note = "تخفیف مبلغی گروه" + " " + package.PackageGroup.PgTitle + " " + package.PackageGroup.PgDiscountValue.ToString() + " " + cur;
                        discountOptionVM.DiscountAmount = package.PackageGroup.PgDiscountValue;
                    }
                    if (package.PackageGroup.PgDiscountPercent != 0)
                    {

                        discountOptionVM.DiscountPercent = package.PackageGroup.PgDiscountPercent;
                        discountOptionVM.DiscountUnit = "%";
                        discountOptionVM.NetPrice = (int)package.PkPrice - (int)(package.PkPrice * package.PackageGroup.PgDiscountPercent / 100);
                        discountOptionVM.Note = "تخفیف درصدی گروه" + " " + package.PackageGroup.PgTitle + " " + package.PackageGroup.PgDiscountPercent.ToString() + " " + "درصد";
                        discountOptionVM.DiscountAmount = (int)(package.PkPrice * (package.PackageGroup.PgDiscountPercent / 100));
                    }
                    return discountOptionVM;
                }
            }
            if (package.PackageGroup != null)
            {
                if (package.PackageGroup.Parent != null)
                {
                    if (package.PackageGroup.Parent.PgDiscountPercent != 0 || package.PackageGroup.Parent.PgDiscountValue != 0)
                    {
                        discountOptionVM.HasDiscount = true;
                        if (package.PackageGroup.Parent.PgDiscountValue != 0)
                        {
                            discountOptionVM.DiscountValue = package.PackageGroup.Parent.PgDiscountValue;
                            discountOptionVM.DiscountUnit = cur;
                            discountOptionVM.NetPrice = (int)package.PkPrice - package.PackageGroup.Parent.PgDiscountValue;
                            discountOptionVM.Note = "تخفیف مبلغی گروه" + " " + package.PackageGroup.PgTitle + " " + package.PackageGroup.Parent.PgDiscountValue.ToString() + " " + cur;
                            discountOptionVM.DiscountAmount = package.PackageGroup.Parent.PgDiscountValue;
                        }
                        if (package.PackageGroup.Parent.PgDiscountPercent != 0)
                        {
                            discountOptionVM.DiscountPercent = package.PackageGroup.Parent.PgDiscountPercent;
                            discountOptionVM.DiscountUnit = "%";
                            discountOptionVM.NetPrice = (int)package.PkPrice - (int)(package.PkPrice * package.PackageGroup.Parent.PgDiscountPercent / 100);
                            discountOptionVM.Note = "تخفیف درصدی گروه" + " " + package.PackageGroup.PgTitle + " " + package.PackageGroup.Parent.PgDiscountPercent.ToString() + " " + "درصد";
                            discountOptionVM.DiscountAmount = (int)(package.PkPrice * (package.PackageGroup.Parent.PgDiscountPercent / 100));
                        }
                        return discountOptionVM;
                    }
                }
            }
            return discountOptionVM;
        }
        public async Task<int> GetPackage_Inventory_InStoreAsync(int packId)
        {
            int imp = await _context.WareHouses.Where(w => w.Package.PkId == packId).SumAsync(s => s.WareHouse_Input);
            int exp = await _context.WareHouses.Where(w => w.Package.PkId == packId).SumAsync(s => s.WareHouse_Export);
            return (imp - exp);
        }
        public async Task<List<Package>> GetPackagesWithLowInventory()
        {
            List<Package> packages = await _context.Packages.ToListAsync();
            packages = packages.Where(w => GetPackage_Inventory_InStoreAsync(w.PkId).Result < (int)w.PkMin_Inventory_ForAlarm).ToList();
            return packages.ToList();
        }
        public async Task<Package> GetPackageByEnName(string EnName)
        {
            if (string.IsNullOrEmpty(EnName))
                return null;
            Package package = await _context.Packages.Include(r => r.PackageGroup).Include(x => x.PackageProducts).Include(x => x.PackageComments).SingleOrDefaultAsync(x => x.PkEnTitle.Trim().Replace(" ", "-") == EnName.Trim());
            return package;
        }
        public int GetPackage_Count_inCart(int packId, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return 0;
            }
            User user = _context.Users.FirstOrDefault(f => f.UserName == userName);
            if (user == null)
            {
                return 0;
            }
            Package package = _context.Packages.Include(r => r.PackageGroup).FirstOrDefault(f => f.PkId == packId);
            if (package == null)
            {
                return 0;
            }
            Cart User_Last_Cart = _context.Carts.Include(r => r.CartItems).FirstOrDefault(w => w.IsActive && w.CheckOut == false && w.UserId == user.Id);
            if (User_Last_Cart == null)
            {
                return 0;
            }
            if (User_Last_Cart.CartItems.Any(x => x.ProductId == packId && x.Kind == "pk"))
            {
                return User_Last_Cart.CartItems.FirstOrDefault(w => w.ProductId == packId && w.Kind == "pk").Quantity;
            }
            else
            {
                return 0;
            }

        }
        public async Task<int> GetPackageNetPriceAsync(int pkId)
        {

            Package package = await _context.Packages.Include(r => r.PackageGroup).Include(r => r.PackageGroup.Parent)
                .SingleOrDefaultAsync(x => x.PkId == pkId);
            if (package == null)
            {
                return 0;
            }
            int price = package.PkPrice.GetValueOrDefault(0);
            if (package.PkDiscountValue != 0)
            {
                price -= package.PkDiscountValue.GetValueOrDefault(0);
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (package.PkDiscountPercent != 0)
            {
                int discount = (int)(price * package.PkDiscountPercent / 100);
                price -= discount;
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (package.PackageGroup.PgDiscountValue != 0)
            {
                price -= package.PackageGroup.PgDiscountValue;
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (package.PackageGroup.PgDiscountPercent != 0)
            {
                int discount = (int)(price * package.PackageGroup.PgDiscountPercent / 100);
                price -= discount;
                if (price > 0)
                {
                    return price;
                }
                else
                {
                    return 0;
                }
            }
            if (package.PackageGroup.Parent != null)
            {
                if (package.PackageGroup.Parent.PgDiscountValue != 0)
                {
                    price -= package.PackageGroup.Parent.PgDiscountValue;
                    if (price > 0)
                    {
                        return price;
                    }
                    else
                    {
                        return 0;
                    }
                }
                if (package.PackageGroup.Parent.PgDiscountPercent != 0)
                {
                    int discount = (int)(price * package.PackageGroup.Parent.PgDiscountPercent / 100);
                    price -= discount;
                    if (price > 0)
                    {
                        return price;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            return price;
        }
        public async Task<List<Product>> GetPackge_Products(int pkId)
        {
            List<PackageProduct> packageProducts = await _context.PackageProducts.Include(x => x.Package).Include(x => x.Product).Include(x => x.Product.WareHouses).Include(x => x.Product.ProductIngredients).Where(w => w.PkId == pkId).ToListAsync();

            return packageProducts.Select(x => x.Product).ToList();
        }
        public async Task<bool> ExisActivePackageAsync()
        {
            return await _context.Packages.AnyAsync(x => x.IsActive);
        }
        #endregion Packages
        #region PackageGroup
        public async Task<List<PackageGroup>> GetPackageGroupsAsync()
        {
            return await _context.PackageGroups.Include(x => x.Banner).Include(x => x.Packages).Include(x => x.Parent).ToListAsync();
        }

        public async Task<PackageGroup> GetPackageGroupByIdAsync(int Id)
        {
            return await _context.PackageGroups.Include(x => x.Banner).Include(x => x.Packages).Include(x => x.Parent).SingleOrDefaultAsync(x => x.PgId == Id);
        }

        public void CreatePackageGroup(PackageGroup packageGroup)
        {
            _context.PackageGroups.Add(packageGroup);
        }

        public void UpdatePackageGroup(PackageGroup packageGroup)
        {
            _context.PackageGroups.Update(packageGroup);
        }

        public void RemovePackageGroup(PackageGroup packageGroup)
        {
            _context.PackageGroups.Remove(packageGroup);
        }

        public bool ExistPackageGroup(int Id)
        {
            return _context.PackageGroups.Any(a => a.PgId == Id);
        }

        public async Task<PackageGroup> GetPackageGroupByEnNameAsync(string EnName)
        {
            return await _context.PackageGroups.Include(x => x.Packages).SingleOrDefaultAsync(x => x.PgEnTitle.Replace(" ", "").Equals(EnName.Replace("-", "").Replace(" ", "")));
        }

        public async Task<List<Package>> GetPackagesByTagAsync(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return null;
            tag = tag.Replace("-", " ");
            List<Package> packages = await _context.Packages.Include(r => r.PackageGroup).ToListAsync();
            packages = packages.Where(w => w.TagsList.Any(x => x.Trim() == tag.Trim())).ToList();
            return packages;
        }



        #endregion
        #region PackageComment
        public void CreatePackageComment(PackageComment packageComment)
        {
            _context.PackageComments.Add(packageComment);
        }

        public async Task<List<PackageComment>> GetPackageCommentsAsync()
        {
            return await _context.PackageComments.Include(x => x.Package).ToListAsync();
        }

        public void EditPackageComment(PackageComment packageComment)
        {
            _context.PackageComments.Update(packageComment);
        }

        public async Task<PackageComment> GetPackageCommentByIdAsync(int Id)
        {
            return await _context.PackageComments.Include(x => x.Package).SingleOrDefaultAsync(x => x.Id == Id);
        }

       




        #endregion
    }
}
