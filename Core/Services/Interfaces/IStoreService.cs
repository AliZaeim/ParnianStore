using Core.DTOs.Admin;
using Core.DTOs.General;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IStoreService
    {
        #region Generic        
        public void SaveChanges();
        public Task SaveChangesAsync();
        public Task<User> GetUserByNameAsync(string userName);
        public Task<int> GetProductNetpriceAsync(string code);        
        public Task<DiscountOptionVM> HasProductDiscountAsync(string code);
        public Task<User> GetUserByCellphoneAsync(string cellphone);
        public Task<List<UserRole>> GetUserRolesByCellphone(string cellphone);
        public Task<County> GetCountyByIdAsync(int Id);
        public Task<County> GetCountyByName(string countyName);
        public Task<State> GetStateByName(string stateName);
        public Task<int> GetProductNetpriceAsync(int pId);
        public Task<int> GetPackageNetpriceAsync(int pkId);
        public bool SendOrderNumber(string Fullname, string OrderNumber,string cellphone);
        public bool SendUserName_and_Password(string userName, string password, string phoneNumber);
        public bool SendPassword(string pass, string phoneNumber);
        public bool SendBuyingFromSite(string Fullname, string Price, string Cellphone);
        public Task<int> GetProduct_Inventory_InStoreAsync(string productCode);
        public Task<List<UserRole>> GetUserRolesAsync();
        public Task<Product> GetProductByIdAsync(int Id);
        public DataTable ConvertListToDataTable<T>(List<T> Data);
        public IWorkbook WriteExcelWithNPOI<T>(T Entity, List<T> data, string title, string extension = "xlsx");
        public string GetDisplayName(PropertyInfo propertyInfo);
        public Task<DiscountOptionVM> HasPackageDiscountAsync(int PId);
        public Task<Package> GetPackageByIdAsync(int Id);
        public Task<int> GetPackage_Inventory_InStoreAsync(int packId);
        public Task<InitialInfo> GetInitialInfoAsync();
        #endregion Generic
        #region Cart
        public Task<AddToCartVM> AddToCartAsync(int productId, int count, int? userId, string cartId, string op,string kind);
        public Task<Cart> GetUserLastCartAsync(string userName);
        public Task<bool> UserHas_UnpaidCartAsync(string userName);
        public Task<int> GetUserCartItemCountAsync(string userName);
        public Task<Cart> GetUserCartByCookieAsync(string cartId);
        public Task<Cart> GetCartByIdAsync(Guid Id);
        public Task<List<Cart>> GetCartsAsync();
        public void EditCart(Cart cart);
        public Task<Order> GetOrderByCartIdAsync(Guid cId);
        public Task<ValidateCartForPayVM> PrepareCartForPayVM(CartVm cartVm);
        public Task<Cart> GetUserLastCheckoutCartAsync(string userName);
        #endregion Cart
        #region CartItem
        public Task<Cart> RemoveCart_Item(int itemId);
        #endregion CartItem 
        #region DiscountCoupen
        public Task<int> GetCoupenRemianAsync(string code);
        public Task<ValidateDiscountCoupenVM> ValidateDiscountCoupenAsync(string code);
        public Task<DiscountCoupen> GetDiscountByCodeAsync(string code);
        public Task<int> GetRemianofCoupen(string code);
        #endregion DiscountCoupen
        #region Order
        public void CreateOrder(Order order);
        public Task<SaveOrderVM> CreateOrderByCartAsync(CartVm cartVm);
        public Task<SaveOrderVM> CreateOrderByCartAsync(Guid cartId);
        public Task<bool> CreateWareHouseWithOrder(Guid oId);
        public void CreateOrderDatail(OrderDetail orderDetail);
        public void EditOrder(Order order);
        public void EditOrderDetail(OrderDetail orderDetail);
        public Task<List<Order>> GetOrdersAsync();
        public Task<List<OrderDetail>> GetOrderDetails();
        public Task<Order> GetOrderByIdAsync(Guid Id);
        public Task<OrderDetail> GetOrderDetailByIdAsync(int Id);
        public Task<List<Order>> GetUserOrdersByNameAsync(string UserName);
        public Task<List<Order>> GetUserOrdersByCellphoneAsync(string Cellpone);
        public Task<List<Order>> GetOrdersByOrderIds(List<Guid> orderIds);
        public Task<Order> GetOrderByDedicatedNumber(string dedN);
        public Task<SaveOrderVM> CreateOrderByAdmin(AdminOrderVM adminOrderVM, List<int> PIds, List<int> PCounts, List<string> Types, int? roleId);
        #endregion
    }
}
