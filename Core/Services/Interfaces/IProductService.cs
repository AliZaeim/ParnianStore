using Core.DTOs.General;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IProductService
    {
        #region General
        public Task<User> GetUserByUserNameAsync(string userName);
        public Task<InitialInfo> GetInitialInfoAsync();
        public Task<List<Notification>> GetNotifications();
        public Task<ValidateDiscountCoupenVM> ValidateDiscountCoupenAsync(string code);
        public Task<DiscountCoupen> GetDiscountByCodeAsync(string code);
        public Task<SitePage> GetSitePageByEnNameAsync(string EnName);
        public Task<bool> ExistFractionSliderAsync();
        public Task<bool> ExistSliderAsync();
        public void SaveChanges();
        public Task SaveChangesAsync();

        #endregion
        #region Product
        public void CreateProduct(Product product);
        public Task<List<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(int Id);
        public void EditProduct(Product product);
        public Task<bool> ExistProductAsync(int Id);
        public Task<bool> RemoveProductAsync(int Id);
        public Task<string> GetProductCodeAsync(int pgId);
        public Task<Product> GetProductWithCodeAsync(string code);
        public Task<bool> ExistProductByCodeAsync(string code);
        
        public Task<DiscountOptionVM> HasProductDiscountAsync(string code);
        public Task<DiscountOptionVM> HasProductDiscountAsync(int PId);
        public Task<int> GetProductNetpriceAsync(string code);
        public bool DetachProduct(Product Deproduct);
        public Task<List<Product>> GetAllProductofGroupAsync(int gId);
        public Task<Product> GetProductByName(string name);
        public Task<Product> GetProductByEnName(string Ename);
        public Task<List<string>> SearchProductByTextAsync(string search);
        public Task<int> AddPopularityToProductAsync(int Pr_id, string Kind);
        public int GetProductCountinCart(int pr_id,string userName);
        public Task<List<Product>> GetFeaturedProductsAsync ();
        public Task<List<Product>> GetProductsByGroupMarkAsync(string gMark);
        public Task<List<Product>> GetDiscountedProductsAsync();
        public Task<bool> ExistSpeceficProdutsAsync();
        public Task<List<Product>> GetProductsByTagAsync(string tag);
        public Task<bool> ExistActiveProductAsync();
        public Task<List<Package>> GetPackagesbyProductAsyncByName(string EnName);
        #endregion Product
        #region ProductGroup
        public bool DetachProductGroup(ProductGroup productGroup);
        public Task<ProductGroup> GetProductGroupByIdAsync(int productGroupId);
        public void CreateProductGroup(ProductGroup productGroup);
        public void EditProductGroup(ProductGroup productGroup);
        public Task<bool> RemoveProductGroupAsync(int Id);
        public Task<List<ProductGroup>> GetProductGroupsAsync();
        public Task<bool> ExistProductGroupAsync(int Id);
        public Task<bool> ExistProductGroupCodeAsync(string Code);
        public Task<ProductGroup> GetProductGroupByTitleAsync(string title);
        public Task<ProductGroup> GetProductGroupByEnTitleAsync(string Entitle);
        public Task<List<ProductGroup>> GetRelatedProductGroup_of_ProductGroupAsync(int gid);
        public Task<ProductGroup> GetProductGroupByMarkAsync(string mark);
        #endregion ProductGroup
        #region WareHouse
        public void CreateWareHouse(WareHouse wareHouse);
        public void CreateWareHouseList(List<WareHouse> wareHouses);
        public Task<WareHouse> GetWareHouseByIdAsync(int wareHouseId);
        public Task<List<WareHouse>> GetWareHousesAsync();
        public Task<List<WareHouse>> GetWareHousesByProductCodeAsync(string productCode);
        public void EditWareHouse(WareHouse wareHouse);
        public Task DeleteWareHouseAsync(int wareHouseId);
        public Task<int> GetProduct_Inventory_InStoreAsync(string productCode);
        public Task<List<Product>> GetProductsWithLowInventory();
        #endregion WareHouse
        #region ProductComment
        public void CreateProductComment(ProductComment productComment);
        public Task<List<ProductComment>> GetProductCommentsAsync();
        public void EditProductComment(ProductComment productComment);
        public Task<ProductComment> GetProductCommentByIdAsync(int Id);
        #endregion ProductComment
        #region PackageComment
        public void CreatePackageComment(PackageComment packageComment);
        public Task<List<PackageComment>> GetPackageCommentsAsync();
        public void EditPackageComment(PackageComment packageComment);
        public Task<PackageComment> GetPackageCommentByIdAsync(int Id);
        #endregion
        #region Banners
        public void CreateBanner(Banner banner);
        public void UpdateBanner(Banner banner);
        public void RemoveBanner(Banner banner);
        public Task<List<Banner>> GetBannersAsync();
        public Task<Banner> GetBannerByIdAsync(int Id);
        public bool ExistBanner(int Id);
        public bool ExitAnyBanner();
        public Task<Banner> GetLastBanner();
        /// <summary>
        /// انتخاب بنر بر اساس تعداد عکس ها
        /// </summary>
        /// <returns></returns>
        public Task<Banner> GetLastBannerbyBannerCount(int count);
        /// <summary>
        /// انتخاب بنر بر اساس تعداد و ترتیب، مثلا اولین 3 تایی 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<Banner> GetBannerByOrderandCount(int order, int count);
        #endregion Banners
       
        #region Ingredients
        public void CreateIngredient(Ingredient ingredient);
        public void Updateungredient(Ingredient ingredient);
        public Task<List<Ingredient>> GetIngredientsAsync();
        public Task<Ingredient> GetIngredientByIdAsync(int Id);
        public bool ExitIngredientById(int Id);
        public void DeleteIngredient(Ingredient ingredient);
        #endregion Ingredients
        #region ProductIngredient
        public Task<List<ProductIngredient>> GetProductIngredientsByProductIdAsync(int productId);
        public Task<bool> AddIngredientsToProduct(int productId, List<int> Ingredints);
        public Task<bool> RemoveIngredientsFromProduct(int productId, List<int> Ingredints);
        public Task<List<int>> IngredientsofProduct(int productId);
        public Task<bool> UpdateProductIngredients(int productId, List<int> NewIngredients);
        public Task<List<Product>> GetProductsofInderendientAsync(int ingId);

        #endregion
        #region Packages
        public Task CreatePackageAsync(Package package, List<int> products);
        public Task<List<Package>> GetPackagesAsync();
        public Task<Package> GetPackageByIdAsync(int Id);
        public bool UpdatePackageProducts(int pId, List<int> products);
        public void UpdatePackage(Package package);
        public bool ExistPackage(int Id);
        public void RemovePackage(Package package);
        public Task<DiscountOptionVM> HasPackageDiscountAsync(int packId);
        public Task<int> GetPackage_Inventory_InStoreAsync(int packId);
        public Task<List<Package>> GetPackagesWithLowInventory();
        public Task<Package> GetPackageByEnName(string EnName);
        
        public int GetPackage_Count_inCart(int packId, string userName);
        public Task<int> GetPackageNetPriceAsync(int pkId);
        public Task<List<Product>> GetPackge_Products(int pkId);
        public Task<List<Package>> GetPackagesByTagAsync(string tag);
        public Task<bool> ExisActivePackageAsync();
        #endregion Packages
        #region PackageProduct

        #endregion
        #region PackageGroup
        public Task<List<PackageGroup>> GetPackageGroupsAsync();
        public Task<PackageGroup> GetPackageGroupByIdAsync(int Id);
        public Task<PackageGroup> GetPackageGroupByEnNameAsync(string EnName);
        public void CreatePackageGroup(PackageGroup packageGroup);
        public void UpdatePackageGroup(PackageGroup packageGroup);
        public void RemovePackageGroup(PackageGroup packageGroup);
        public bool ExistPackageGroup(int Id);
        #endregion
    }
}
