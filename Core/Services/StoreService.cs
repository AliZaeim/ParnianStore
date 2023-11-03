using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Context;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmsIrRestfulNetCore;
using NPOI.SS.UserModel;
using System.Data;
using System.ComponentModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Core.DTOs.Admin;

namespace Core.Services
{
    public class StoreService : IStoreService
    {
        private readonly MyContext _Context;
        public StoreService(MyContext Context)
        {
            _Context = Context;
        }
        #region Generic

        public async Task<InitialInfo> GetInitialInfoAsync()
        {
            return await _Context.InitialInfos.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        }
        public async Task<int> GetPackage_Inventory_InStoreAsync(int packId)
        {
            int imp = await _Context.WareHouses.Where(w => w.Package.PkId == packId).SumAsync(s => s.WareHouse_Input);
            int exp = await _Context.WareHouses.Where(w => w.Package.PkId == packId).SumAsync(s => s.WareHouse_Export);
            return (imp - exp);
        }
        public async Task<Package> GetPackageByIdAsync(int Id)
        {
            return await _Context.Packages.Include(x => x.PackageProducts).SingleOrDefaultAsync(x => x.PkId == Id);
        }
        public async Task<DiscountOptionVM> HasPackageDiscountAsync(int PId)
        {
            string cur = "ریال";
            var initialInfos = await _Context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
            if (initialInfos != null)
            {
                cur = initialInfos.SiteCurrency;
            }
            DiscountOptionVM discountOptionVM = new()
            {
                HasDiscount = false,
                DiscountValue = 0,

            };

            Package package = await _Context.Packages.Include(r => r.PackageGroup).Include(r => r.PackageGroup.Parent)
                .SingleOrDefaultAsync(x => x.PkId == PId);
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
        public async Task<int> GetPackageNetpriceAsync(int pkId)
        {
            Package package = await _Context.Packages.Include(r => r.PackageGroup).Include(r => r.PackageGroup.Parent)
                .SingleOrDefaultAsync(x => x.PkId == pkId);
            if (package == null)
            {
                return 0;
            }
            int price = (int)package.PkPrice;
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
        public async Task<County> GetCountyByName(string countyName)
        {
            return await _Context.Counties.OrderBy(x => x.State).FirstOrDefaultAsync(x => x.CountyName == countyName);
        }

        public async Task<State> GetStateByName(string stateName)
        {
            return await _Context.States.Include(x => x.Counties).FirstOrDefaultAsync(x => x.StateName == stateName);
        }

        public string GetDisplayName(PropertyInfo propertyInfo)
        {
            string result = null;
            try
            {
                var attrs = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (attrs.Any())
                    result = ((DisplayAttribute)attrs[0]).Name;
            }
            catch (Exception)
            {
                //eat the exception
            }
            return result;
        }
        public DataTable ConvertListToDataTable<T>(List<T> Data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in Data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public IWorkbook WriteExcelWithNPOI<T>(T Entity, List<T> data, string title, string extension = "xlsx")
        {
            // Get DataTable
            DataTable dt = ConvertListToDataTable(data);
            // Instantiate Wokrbook
            IWorkbook workbook;
            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("The format '" + extension + "' is not supported.");
            }
            //make top row
            ISheet sheet1 = workbook.CreateSheet("Sheet 1");
            sheet1.IsRightToLeft = true;
            IFont TopRowFont = workbook.CreateFont();
            TopRowFont.FontName = "topFont";
            TopRowFont.IsBold = true;
            TopRowFont.FontHeight = 350;

            IRow topRow = sheet1.CreateRow(0);
            var CellStyleTop = workbook.CreateCellStyle();
            CellStyleTop.Alignment = HorizontalAlignment.Center;
            CellStyleTop.VerticalAlignment = VerticalAlignment.Center;
            CellStyleTop.SetFont(TopRowFont);
            ICell cellTop = topRow.CreateCell(0);
            cellTop.CellStyle = CellStyleTop;
            cellTop.SetCellValue(title);

            var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dt.Columns.Count - 1);
            sheet1.AddMergedRegion(cra);

            //make a header row
            IFont font1 = workbook.CreateFont();
            font1.FontName = "Font1";
            font1.IsBold = true;
            font1.Color = IndexedColors.Black.Index;



            IRow row1 = sheet1.CreateRow(1);
            var CellStyleHeader = workbook.CreateCellStyle();
            CellStyleHeader.Alignment = HorizontalAlignment.Center;
            CellStyleHeader.VerticalAlignment = VerticalAlignment.Center;

            // center-align currency values
            CellStyleHeader.Alignment = HorizontalAlignment.Center;
            CellStyleHeader.VerticalAlignment = VerticalAlignment.Center;
            CellStyleHeader.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            CellStyleHeader.FillPattern = FillPattern.SolidForeground;
            CellStyleHeader.SetFont(font1);



            var CellStyleBody = workbook.CreateCellStyle();
            // center-align currency values
            CellStyleBody.Alignment = HorizontalAlignment.Center;
            CellStyleBody.VerticalAlignment = VerticalAlignment.Center;




            PropertyInfo[] props = Entity.GetType().GetProperties();
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ICell cell = row1.CreateCell(j);
                string Title = GetDisplayName(props[j]);
                if (!string.IsNullOrEmpty(Title))
                {
                    cell.SetCellValue(Title);
                    cell.CellStyle = CellStyleHeader;
                }


            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 2);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    string columnName = dt.Columns[j].ToString();
                    string columnValue = dt.Rows[i][columnName].ToString();
                    Type tpe = dt.Rows[i][columnName].GetType();

                    if (tpe.Equals(typeof(bool)))
                    {
                        if (columnValue == "True")
                        {
                            columnValue = "بله";
                        }
                        else if (columnValue == "False")
                        {
                            columnValue = "خیر";
                        }

                    }


                    //string Title = GetDisplayName(props[j]);
                    string[] cellval = columnValue.Split("|");
                    string nstr = string.Empty;
                    int loop = 1;
                    foreach (var item in cellval)
                    {
                        if (item != cellval.LastOrDefault())
                        {
                            nstr += $"{item}\n";
                        }
                        else
                        {
                            nstr += item;
                        }
                        loop++;
                    }
                    cell.SetCellValue(nstr);

                    ICellStyle cs = workbook.CreateCellStyle();
                    cs.Alignment = HorizontalAlignment.Center;
                    cs.VerticalAlignment = VerticalAlignment.Center;
                    cs.WrapText = true;
                    cs.ShrinkToFit = true;
                    cell.CellStyle = cs;
                }
                for (int j = 0; j < row1.LastCellNum; j++)
                {
                    sheet1.AutoSizeColumn(j);
                }
            }

            return workbook;
        }
        public async Task<int> GetProduct_Inventory_InStoreAsync(string productCode)
        {
            int imp = await _Context.WareHouses.Where(w => w.Product.ProductCode == productCode).SumAsync(s => s.WareHouse_Input);
            int exp = await _Context.WareHouses.Where(w => w.Product.ProductCode == productCode).SumAsync(s => s.WareHouse_Export);
            return (imp - exp);
        }
        public async Task<List<UserRole>> GetUserRolesAsync()
        {
            return await _Context.UserRoles.Include(r => r.User).Include(r => r.User.County).Include(r => r.User.County.State).Include(r => r.Role).ToListAsync();
        }
        public bool SendUserName_and_Password(string userName, string password, string phoneNumber)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(phoneNumber),
                TemplateId = 48138,
                ParameterArray = new List<UltraFastParameters>()
            {
                new UltraFastParameters()
                {
                    Parameter="username", ParameterValue=userName

                },
                new UltraFastParameters()
                {
                     Parameter = "password" , ParameterValue = password
                }
            }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            return ultraFastSendRespone.IsSuccessful;
        }
        public bool SendPassword(string pass, string phoneNumber)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(phoneNumber),
                TemplateId = 46671,
                ParameterArray = new List<UltraFastParameters>()
            {
                new UltraFastParameters()
                {
                    Parameter = "password" , ParameterValue = pass
                }
            }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            return ultraFastSendRespone.IsSuccessful;
        }
        public bool SendOrderNumber(string Fullname, string OrderNumber, string cellphone)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(cellphone),
                TemplateId = 47849,
                ParameterArray = new List<UltraFastParameters>()
            {
                new UltraFastParameters()
                {
                    Parameter = "username" , ParameterValue = Fullname,

                },
                new UltraFastParameters()
                {
                    Parameter="number",ParameterValue=OrderNumber
                }

            }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            return ultraFastSendRespone.IsSuccessful;
        }
        public bool SendBuyingFromSite(string Fullname, string Price, string Cellphone)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(Cellphone),
                TemplateId = 56639,
                ParameterArray = new List<UltraFastParameters>()
            {
                new UltraFastParameters()
                {
                    Parameter = "username" , ParameterValue = Fullname,

                },
                new UltraFastParameters()
                {
                    Parameter="value",ParameterValue=Price
                }

            }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            return ultraFastSendRespone.IsSuccessful;
        }
        public async Task<int> GetProductNetpriceAsync(int pId)
        {

            Product product = await _Context.Products.Include(r => r.ProductGroup).Include(r => r.ProductGroup.Parent)
                .SingleOrDefaultAsync(x => x.ProductId == pId);
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
        public void SaveChanges()
        {
            _Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _Context.Users.Include(x => x.County).Include(x => x.County.State).FirstOrDefaultAsync(f => f.UserName == userName);
        }
        public async Task<int> GetProductNetpriceAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return 0;
            }
            Product product = await _Context.Products.Include(r => r.ProductGroup).Include(r => r.ProductGroup.Parent)
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
            }
            if (product.ProductPercentDiscount != 0)
            {
                int discount = price - (int)(price * product.ProductPercentDiscount / 100);
                price -= discount;
                if (price > 0)
                {
                    return price;
                }
            }
            return price;

        }
        public async Task<DiscountOptionVM> HasProductDiscountAsync(string code)
        {
            string cur = "ریال";
            var initialInfos = await _Context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
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
            Product product = await _Context.Products.Include(r => r.ProductGroup).Include(r => r.ProductGroup.Parent)
                .SingleOrDefaultAsync(x => x.ProductCode == code);
            if (product == null)
            {
                return discountOptionVM;
            }
            //100 is product price
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
            //100 is product.ProductPrice
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
                            discountOptionVM.DiscountPercent = product.ProductGroup.Parent.ProductGroupDiscountPercent;
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
        public async Task<User> GetUserByCellphoneAsync(string cellphone)
        {
            return await _Context.Users.Include(r => r.County).SingleOrDefaultAsync(x => x.Cellphone == cellphone);
        }

        public async Task<List<UserRole>> GetUserRolesByCellphone(string cellphone)
        {
            return await _Context.UserRoles.Include(r => r.User).Include(r => r.Role).Include(r => r.User.County)
                 .Where(w => w.User.Cellphone == cellphone).ToListAsync();
        }
        public async Task<County> GetCountyByIdAsync(int Id)
        {
            return await _Context.Counties.Include(r => r.State).SingleOrDefaultAsync(x => x.CountyId == Id);
        }
        public async Task<Product> GetProductByIdAsync(int Id)
        {
            return await _Context.Products.Include(x => x.ProductGroup).SingleOrDefaultAsync(x => x.ProductId == Id);
        }
        #endregion
        #region Cart
        public async Task<AddToCartVM> AddToCartAsync(int productId, int count, int? userId, string cartId, string op, string kind)
        {
            Product product = null;
            Package package = null;
            DiscountOptionVM discountOptionVM = null;
            if (kind == "pr")
            {
                product = await _Context.Products.Include(r => r.ProductGroup).FirstOrDefaultAsync(f => f.ProductId == productId);
                discountOptionVM = await HasProductDiscountAsync(product.ProductCode);
            }
            if (kind == "pk")
            {
                package = await _Context.Packages.Include(r => r.PackageGroup).FirstOrDefaultAsync(f => f.PkId == productId);
                discountOptionVM = await HasPackageDiscountAsync(productId);
            }

            AddToCartVM addToCartVM = new() { Added = true, Message = "به سبد خرید اضافه شد", Type = kind };
            if (kind == "pr")
            {
                if (product == null)
                {
                    addToCartVM.Added = false;
                    addToCartVM.Message = "محصول مورد نظر یافت نشد !";
                    return addToCartVM;
                }
                if (!product.IsActive)
                {
                    addToCartVM.Added = false;
                    addToCartVM.Message = "محصول مورد نظر یافت نشد !";
                    return addToCartVM;
                }
            }
            if (kind == "pk")
            {
                if (package == null)
                {
                    addToCartVM.Added = false;
                    addToCartVM.Message = "محصول مورد نظر یافت نشد !";
                    return addToCartVM;
                }
                if (!package.IsActive)
                {
                    addToCartVM.Added = false;
                    addToCartVM.Message = "محصول مورد نظر یافت نشد !";
                    return addToCartVM;
                }
            }
            if (kind == "pr")
            {
                int ent = await _Context.WareHouses.Where(w => w.PkId == null && w.ProductId == productId).SumAsync(x => x.WareHouse_Input);
                int ex = await _Context.WareHouses.Where(w => w.PkId == null && w.ProductId == productId).SumAsync(x => x.WareHouse_Export);
                int re = ent - ex;
                if (re <= 0)
                {
                    addToCartVM.Added = false;
                    addToCartVM.Message = "پایان موجودی";
                    addToCartVM.IsLack = true;
                    addToCartVM.ProductId = productId;
                    return addToCartVM;
                }
            }
            if (kind == "pk")
            {
                int ent = await _Context.WareHouses.Where(w => w.ProductId == null && w.PkId == productId).SumAsync(x => x.WareHouse_Input);
                int ex = await _Context.WareHouses.Where(w => w.ProductId == null && w.PkId == productId).SumAsync(x => x.WareHouse_Export);
                int re = ent - ex;
                if (re <= 0)
                {
                    addToCartVM.Added = false;
                    addToCartVM.Message = "پایان موجودی";
                    addToCartVM.IsLack = true;
                    addToCartVM.ProductId = productId;
                    return addToCartVM;
                }
            }

            User user = null;
            if (userId != null)
            {
                user = await _Context.Users.FirstOrDefaultAsync(f => f.Id == (int)userId);
            }


            int price = 0;
            if (kind == "pr")
            {
                price = await GetProductNetpriceAsync(product.ProductCode);
            }
            if (kind == "pk")
            {
                price = await GetPackageNetpriceAsync(productId);
            }

            Cart User_Last_Cart = null;
            if (user != null)
            {
                User_Last_Cart = await _Context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive && !x.CheckOut);
            }
            else
            {
                if (!string.IsNullOrEmpty(cartId))
                {
                    User_Last_Cart = await _Context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.Id.ToString() == cartId && x.IsActive && !x.CheckOut);
                }
            }

            string cur = "ریال";
            InitialInfo initialInfo = await _Context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
            if (initialInfo != null)
            {
                cur = initialInfo.SiteCurrency;
            }
            if (User_Last_Cart == null)
            {
                Cart cart = new()
                {
                    DateCreated = DateTime.Now,
                    UserId = userId,
                    IsActive = true,
                    Currency = cur,
                    CartItems = new List<CartItem> { new CartItem { ProductId = productId, Quantity = count, Price = price, Kind = kind } }
                };
                _Context.Carts.Add(cart);
                await _Context.SaveChangesAsync();
                User_Last_Cart = cart;


            }
            else
            {
                if (User_Last_Cart.CartItems.Any(a => a.Kind == kind && a.ProductId == productId))
                {
                    CartItem ExistItem = User_Last_Cart.CartItems.SingleOrDefault(x => x.ProductId == productId && x.Kind == kind);
                    int prcount = User_Last_Cart.CartItems.Where(w => w.ProductId == productId && w.Kind == kind).Sum(x => x.Quantity);
                    if (op == "eq")
                    {
                        prcount = count;
                        addToCartVM.Message = "سبد خرید اصلاح شد";
                    }
                    else if (op == "plus")
                    {
                        prcount += count;
                    }

                    if (kind == "pr")
                    {
                        if (prcount > product.MaximumNumberofPurchases)
                        {
                            addToCartVM.Added = false;
                            addToCartVM.Message = "حداکثر" + " " + product.MaximumNumberofPurchases + " " + "عدد" + " " + "از محصول" + " " + product.ProductName + " " + "می توانید به سبد اضافه کنید";
                        }
                        else
                        {

                            ExistItem.Quantity = prcount;
                            _Context.CartItems.Update(ExistItem);
                            await _Context.SaveChangesAsync();
                        }
                    }
                    if (kind == "pk")
                    {
                        if (prcount > package.MaximumNumberofPurchases)
                        {
                            addToCartVM.Added = false;
                            addToCartVM.Message = "حداکثر" + " " + product.MaximumNumberofPurchases + " " + "عدد" + " " + "از محصول" + " " + package.PkTitle + " " + "می توانید به سبد اضافه کنید";
                        }
                        else
                        {

                            ExistItem.Quantity = prcount;
                            _Context.CartItems.Update(ExistItem);
                            await _Context.SaveChangesAsync();
                        }
                    }


                }
                else
                {
                    try
                    {
                        CartItem cartItem_N = new()
                        {
                            CartId = User_Last_Cart.Id,
                            ProductId = productId,
                            Kind = kind,
                            Price = price,
                            Quantity = count
                        };
                        User_Last_Cart.CartItems.Add(cartItem_N);

                        //_Context.CartItems.Add(cartItem_N);
                        int res = await _Context.SaveChangesAsync();
                        addToCartVM.Count = User_Last_Cart.CartItems.Count;
                    }
                    catch (Exception ex)
                    {
                        string m = ex.Message;
                    }



                }
            }
            addToCartVM.CartId = User_Last_Cart.Id;
            addToCartVM.Id = User_Last_Cart.Id.ToString();
            if (User_Last_Cart != null)
            {
                addToCartVM.Count = User_Last_Cart.CartItems.Sum(x => x.Quantity);
            }
            return addToCartVM;
        }
        public void EditCart(Cart cart)
        {
            _Context.Carts.Update(cart);
        }
        public async Task<Cart> GetUserLastCartAsync(string userName)
        {
            User user = await _Context.Users.FirstOrDefaultAsync(f => f.UserName == userName);
            if (user == null)
            {
                return null;
            }
            return await _Context.Carts.Include(r => r.CartItems).SingleOrDefaultAsync(w => w.CheckOut == false && w.IsActive && w.UserId == user.Id);
        }
        public async Task<Cart> GetUserCartByCookieAsync(string cartId)
        {

            Cart crt = await _Context.Carts.Include(r => r.CartItems).SingleOrDefaultAsync(x => x.Id == Guid.Parse(cartId));
            return crt;
        }
        public async Task<bool> UserHas_UnpaidCartAsync(string userName)
        {
            User user = await _Context.Users.FirstOrDefaultAsync(f => f.UserName == userName);
            if (user == null)
            {
                return false;
            }
            return await _Context.Carts.AnyAsync(a => a.CheckOut == false && a.IsActive && a.UserId == user.Id);
        }
        public async Task<int> GetUserCartItemCountAsync(string userName)
        {
            User user = await _Context.Users.FirstOrDefaultAsync(f => f.UserName == userName);
            if (user == null)
            {
                return 0;
            }
            Cart cart = await _Context.Carts.Include(r => r.CartItems).FirstOrDefaultAsync(a => a.CheckOut == false && a.IsActive && a.UserId == user.Id);
            if (cart == null)
            {
                return 0;
            }
            return cart.CartItems.Count;
        }
        public async Task<Cart> GetCartByIdAsync(Guid Id)
        {
            return await _Context.Carts.Include(r => r.CartItems).SingleOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<List<Cart>> GetCartsAsync()
        {
            return await _Context.Carts.Include(r => r.CartItems).Include(r => r.User).ToListAsync();
        }
        public async Task<Order> GetOrderByCartIdAsync(Guid cId)
        {
            return await _Context.Orders.Include(x => x.OrderDetails).SingleOrDefaultAsync(x => x.CartId == cId);
        }
        public async Task<Cart> GetUserLastCheckoutCartAsync(string userName)
        {
            return await _Context.Carts.OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync(w => w.CheckOut && w.User.UserName == userName);
        }
        #endregion Cart
        #region CartItem
        public async Task<Cart> RemoveCart_Item(int itemId)
        {
            CartItem cartItem = await _Context.CartItems.Include(r => r.Cart).SingleOrDefaultAsync(x => x.Id == itemId);
            Cart cart = await _Context.Carts.Include(r => r.CartItems).SingleOrDefaultAsync(x => x.Id == cartItem.Cart.Id);
            if (cartItem == null)
            {
                return null;
            }
            _Context.CartItems.Remove(cartItem);
            await _Context.SaveChangesAsync();
            return cart;
        }


        #endregion CartItem
        #region DiscountCoupen
        public async Task<int> GetCoupenRemianAsync(string code)
        {
            int number = 0;
            int used = 0;
            DiscountCoupen discountCoupen = await _Context.DiscountCoupens.SingleOrDefaultAsync(x => x.Code.Equals(code));
            if (discountCoupen != null)
            {
                number = (int)discountCoupen.Number;
            }
            used = await _Context.Orders.Where(r => r.Order_DiscountCode.Equals(code)).CountAsync();

            return number - used;
        }
        public async Task<ValidateDiscountCoupenVM> ValidateDiscountCoupenAsync(string code)
        {
            ValidateDiscountCoupenVM validateDiscountCoupenVM = new();
            if (string.IsNullOrEmpty(code))
            {
                validateDiscountCoupenVM.Comment = "کد کوپن تخفیف را وارد نشده است";
            }
            int number = 0;
            int used = 0;
            DiscountCoupen discountCoupen = await _Context.DiscountCoupens.SingleOrDefaultAsync(x => x.Code.Equals(code));
            if (discountCoupen != null)
            {
                number = (int)discountCoupen.Number;
                validateDiscountCoupenVM.DiscountCoupen = discountCoupen;
            }
            else
            {
                validateDiscountCoupenVM.Comment = "کوپن موجود نمی باشد !";
            }
            used = await _Context.Orders.Where(r => r.Order_DiscountCode.Equals(code)).CountAsync();
            if (number - used > 0)
            {
                validateDiscountCoupenVM.Validity = true;
                validateDiscountCoupenVM.Comment = "کوپن تخفیف معتبر است";
            }
            return validateDiscountCoupenVM;
        }

        public async Task<DiscountCoupen> GetDiscountByCodeAsync(string code)
        {
            return await _Context.DiscountCoupens.FirstOrDefaultAsync(x => x.Code.Equals(code));
        }
        public async Task<int> GetRemianofCoupen(string code)
        {
            DiscountCoupen discountCoupen = await _Context.DiscountCoupens.SingleOrDefaultAsync(x => x.Code.Equals(code));
            if (discountCoupen == null)
                return 0;
            List<Order> orders = await _Context.Orders.Where(w => w.Order_DiscountCode.Equals(code)).ToListAsync();
            if (orders == null)
                return 0;
            return ((int)discountCoupen.Number - orders.Count);
        }
        #endregion
        #region Order
        public void CreateOrder(Order order)
        {
            _Context.Orders.Add(order);
        }

        public void CreateOrderDatail(OrderDetail orderDetail)
        {
            _Context.OrderDetails.Add(orderDetail);
        }

        public void EditOrder(Order order)
        {
            _Context.Orders.Update(order);
        }

        public void EditOrderDetail(OrderDetail orderDetail)
        {
            _Context.OrderDetails.Update(orderDetail);
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _Context.Orders.Include(r => r.OrderDetails).Include(r => r.UserRole).ToListAsync();
        }

        public async Task<List<OrderDetail>> GetOrderDetails()
        {
            return await _Context.OrderDetails.Include(r => r.Order).Include(r => r.Product).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid Id)
        {
            return await _Context.Orders.Include(r => r.OrderDetails)
                .Include(r => r.UserRole).Include(r => r.UserRole.User).Include(r => r.UserRole.Role).SingleOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int Id)
        {
            return await _Context.OrderDetails.Include(r => r.Order).Include(r => r.Product).SingleOrDefaultAsync(x => x.OrderDetailId == Id);
        }

        public async Task<SaveOrderVM> CreateOrderByCartAsync(CartVm cartVm)
        {
            SaveOrderVM saveOrderVM = new();
            User user = await _Context.Users.Include(r => r.County).SingleOrDefaultAsync(x => x.Cellphone == cartVm.BuyerCellphone);
            UserRole userRole = null;
            if (user == null)
            {
                saveOrderVM.IsNewUser = true;
                string userPass = Prodocers.Generators.GenerateUniqueString(4, 0, 4, 0);
                saveOrderVM.UserPassword = userPass;
                User New_user = new()
                {
                    Name = cartVm.BuyerName,
                    UserName = cartVm.BuyerCellphone,
                    Family = cartVm.BuyerFamily,
                    CountyId = cartVm.CountyId,
                    Cellphone = cartVm.BuyerCellphone,
                    IsActive = true,
                    Password = userPass

                };
                _Context.Users.Add(New_user);
                UserRole New_userRole = new()
                {
                    RegisterDate = DateTime.Now,
                    RoleId = 2,
                    IsActive = true,
                    User = New_user
                };
                _Context.Add(New_user);
                userRole = New_userRole;
            }
            else
            {
                userRole = await _Context.UserRoles.Include(r => r.User).SingleOrDefaultAsync(x => x.User == user);
            }
            County county = await _Context.Counties.Include(r => r.State).FirstOrDefaultAsync(x => x.CountyId == cartVm.CountyId);
            Cart cart = await _Context.Carts.Include(r => r.CartItems).FirstOrDefaultAsync(x => x.Id == cartVm.CartId);
            DiscountCoupen discountCoupen = null;
            if (!string.IsNullOrEmpty(cartVm.DiscountCode))
            {
                discountCoupen = await _Context.DiscountCoupens.FirstOrDefaultAsync(x => x.IsActive && x.Code.Equals(cart.DiscountCoupen.Trim()));
            }



            int cartItems_total = 0;
            int cart_total_without_freight = 0;
            int cart_disValue = 0;

            if (cart != null)
            {
                if (cart.CartItems.Count != 0)
                {
                    foreach (var cartItem in cart.CartItems)
                    {
                        Product product = await _Context.Products.SingleOrDefaultAsync(x => x.ProductId == cartItem.ProductId);
                        int prNetPrice = await GetProductNetpriceAsync(cartItem.ProductId);
                        int total = cartItem.Quantity * prNetPrice;
                        cartItems_total += total;
                    }


                }
                if (discountCoupen != null)
                {
                    cart_disValue = (int)(cartItems_total * discountCoupen.Percent) / 100;
                }

                cart_total_without_freight = cartItems_total - cart_disValue;
            }
            Order order = new()
            {
                Order_BuyerName = cartVm.BuyerName.Trim(),
                Order_BuyerFamily = cartVm.BuyerFamily.Trim(),
                Order_BuyerCellphone = cartVm.BuyerCellphone.Trim(),
                CartId = cartVm.CartId,
                Order_StateName = county.State.StateName,
                Order_CountyName = county.CountyName,
                Order_Address = cartVm.Address.Trim(),
                Order_PostalCode = cartVm.PostalCode.Trim(),
                Order_Date = DateTime.Now,
                Order_DedicatedNumber = Prodocers.Generators.GenerateUniqueString(_Context.Orders.Select(x => x.Order_DedicatedNumber).ToList(), 0, 0, 6, 0),
                Order_DiscountCode = cart.DiscountCoupen,
                Order_DiscountValue = cart_disValue,
                UserRole = userRole,
            };

            int cart_Freight = 200000;
            if (county.Freight != null)
            {
                cart_Freight = (int)county.Freight;
            }
            else
            {
                if (county.State.Freight != null)
                {
                    cart_Freight = (int)county.State.Freight;
                }
            }
            string cur = "ریال";
            InitialInfo initialInfo = await _Context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
            if (initialInfo != null)
            {
                cur = initialInfo.SiteCurrency;
            }
            int cartsum_withoutFreight = cart_total_without_freight - cart_disValue;
            if (initialInfo != null)
            {
                if (initialInfo.FreeShipping_MinimumPurchaseAmount.HasValue)
                {
                    if (cartsum_withoutFreight > (int)initialInfo.FreeShipping_MinimumPurchaseAmount)
                    {
                        cart_Freight = 0;
                    }
                }
            }
            int cart_total = cartsum_withoutFreight + cart_Freight;
            order.Order_DeliveryCost = cart_Freight;
            order.Order_Sum = cart_total;
            order.Order_IsFinished = true;
            order.Currency = cur;
            List<OrderDetail> orderDetails = new();
            foreach (var cItem in cart.CartItems)
            {
                Product product = await _Context.Products.SingleOrDefaultAsync(x => x.ProductId == cItem.ProductId);
                if (product != null)
                {
                    OrderDetail orderDetail = new()
                    {
                        OrderDetailCount = cItem.Quantity,
                        ProductId = cItem.ProductId,
                        OrderDetailPrice = product.ProductPrice,
                        OrderDetailNetPrice = await GetProductNetpriceAsync(cItem.ProductId)
                    };
                    orderDetails.Add(orderDetail);
                }

            }
            order.OrderDetails = orderDetails;
            _Context.Orders.Add(order);
            cart.CheckOut = true;
            _Context.Carts.Update(cart);
            await _Context.SaveChangesAsync();

            saveOrderVM.IsSuccess = true;
            saveOrderVM.OrderDedicated = order.Order_DedicatedNumber;
            saveOrderVM.OrderId = order.Id;

            return saveOrderVM;
        }
        public async Task<SaveOrderVM> CreateOrderByCartAsync(Guid cartId)
        {
            Cart cart = await _Context.Carts.Include(r => r.CartItems).SingleOrDefaultAsync(x => x.Id == cartId);
            SaveOrderVM saveOrderVM = new();
            try
            {
                User user = await _Context.Users.Include(r => r.County).SingleOrDefaultAsync(x => x.Cellphone == cart.BuyerCellphone);
                if (user == null)
                {
                    return saveOrderVM;
                }
                UserRole userRole = await _Context.UserRoles.Include(r => r.User).Include(r => r.Role).SingleOrDefaultAsync(r => r.UserId == user.Id && r.RoleId == 2);
                if (userRole == null)
                {
                    UserRole NewUserRole = new()
                    {
                        UserId = user.Id,
                        RoleId = 2,
                        RegisterDate = DateTime.Now,
                        IsActive = true
                    };
                    _Context.Add(NewUserRole);
                    await _Context.SaveChangesAsync();
                    userRole = NewUserRole;
                }

                DiscountCoupen discountCoupen = null;
                if (!string.IsNullOrEmpty(cart.DiscountCoupen))
                {
                    discountCoupen = await _Context.DiscountCoupens.FirstOrDefaultAsync(x => x.IsActive && x.Code.Equals(cart.DiscountCoupen.Trim()));
                }
                int cartItems_total = 0;
                int cart_total_without_freight = 0;
                int cart_disValue = 0;
                int prNetPrice = 0;

                if (cart != null)
                {
                    if (cart.CartItems.Count != 0)
                    {
                        foreach (var cartItem in cart.CartItems)
                        {

                            if (cartItem.Kind == "pr")
                            {
                                prNetPrice = await GetProductNetpriceAsync(cartItem.ProductId);
                            }

                            if (cartItem.Kind == "pk")
                            {
                                prNetPrice = await GetPackageNetpriceAsync(cartItem.ProductId);
                            }
                            //Product product = await _Context.Products.SingleOrDefaultAsync(x => x.ProductId == cartItem.ProductId);

                            int total = cartItem.Quantity * prNetPrice;
                            cartItems_total += total;
                        }


                    }
                    if (discountCoupen != null)
                    {
                        cart_disValue = (int)(cartItems_total * discountCoupen.Percent) / 100;
                    }

                    cart_total_without_freight = cartItems_total - cart_disValue;
                }

                Order order = new()
                {
                    Order_BuyerName = cart.BuyerName.Trim(),
                    Order_BuyerFamily = cart.BuyerFamily.Trim(),
                    Order_BuyerCellphone = cart.BuyerCellphone.Trim(),
                    //RecipientName=
                    CartId = cart.Id,
                    Order_StateName = cart.StateName,
                    Order_CountyName = cart.CountyName,
                    Order_Address = cart.Address.Trim(),
                    Order_PostalCode = cart.PostalCode.Trim(),
                    Order_Date = DateTime.Now,
                    Order_DedicatedNumber = Prodocers.Generators.GenerateUniqueString(_Context.Orders.Select(x => x.Order_DedicatedNumber).ToList(), 0, 0, 8, 0),
                    Order_DiscountCode = cart.DiscountCoupen,
                    Order_DiscountValue = cart_disValue,
                    UserRole = userRole,
                };

                int cart_Freight = cart.Freight;

                string cur = "ریال";
                InitialInfo initialInfo = await _Context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
                if (initialInfo != null)
                {
                    cur = initialInfo.SiteCurrency;
                }

                if (initialInfo != null)
                {
                    if (initialInfo.FreeShipping_MinimumPurchaseAmount.HasValue)
                    {
                        if (cart_total_without_freight > (int)initialInfo.FreeShipping_MinimumPurchaseAmount)
                        {
                            cart_Freight = 0;
                        }
                    }
                }
                int cart_total = cart_total_without_freight + cart_Freight;
                order.Order_DeliveryCost = cart_Freight;
                order.Order_Sum = cart_total;
                order.Order_IsFinished = true;
                order.Currency = cur;

                order.RecipientName = cart.RecipientName;
                order.RecipientFamily = cart.RecipientFamily;
                order.RecipientPhone = cart.RecipientPhone;
                List<OrderDetail> orderDetails = new();
                foreach (var cItem in cart.CartItems)
                {
                    string pName = string.Empty;
                    Product product = null;
                    Package package = null;


                    if (cItem.Kind == "pr")
                    {
                        product = await _Context.Products.SingleOrDefaultAsync(x => x.ProductId == cItem.ProductId);
                        if (product != null)
                        {
                            pName = product.ProductName;
                        }
                    }
                    if (cItem.Kind == "pk")
                    {
                        package = await _Context.Packages.SingleOrDefaultAsync(x => x.PkId == cItem.ProductId);
                        if (package != null)
                        {
                            pName = package.PkTitle;
                        }
                    }
                    OrderDetail orderDetail = new()
                    {
                        OrderDatailProductName = pName,
                        OrderDetailCount = cItem.Quantity,
                        ProductId = cItem.ProductId,
                        OrderDetailPrice = cItem.Price,
                        OrderDetailNetPrice = prNetPrice,
                        ProductKind = cItem.Kind
                    };
                    orderDetails.Add(orderDetail);
                }
                order.OrderDetails = orderDetails;
                _Context.Orders.Add(order);
                cart.CheckOut = true;
                cart.PaymentDate = DateTime.Now;
                _Context.Carts.Update(cart);
                await _Context.SaveChangesAsync();

                saveOrderVM.IsSuccess = true;
                saveOrderVM.OrderDedicated = order.Order_DedicatedNumber;
                saveOrderVM.OrderId = order.Id;
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }

            return saveOrderVM;

        }

        public async Task<bool> CreateWareHouseWithOrder(Guid oId)
        {
            Order order = await _Context.Orders.Include(x => x.OrderDetails).SingleOrDefaultAsync(x => x.Id == oId);
            if (order == null)
                return false;
            List<WareHouse> wareHouses = new();
            foreach (var od in order.OrderDetails)
            {
                WareHouse wareHouse = new();
                if (od.ProductKind == "pr")
                {
                    wareHouse.ProductId = od.ProductId;
                }
                if (od.ProductKind == "pk")
                {
                    wareHouse.PkId = od.ProductId;
                }
                wareHouse.WareHouse_Date = DateTime.Now;
                wareHouse.WareHouse_Input = 0;
                wareHouse.WareHouse_Export = od.OrderDetailCount;
                wareHouse.OrderdetialId = od.OrderDetailId;
                wareHouses.Add(wareHouse);
            }
            _Context.WareHouses.AddRange(wareHouses);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>> GetUserOrdersByNameAsync(string UserName)
        {
            User user = await _Context.Users.SingleOrDefaultAsync(x => x.UserName == UserName);
            if (user == null)
            {
                return null;
            }
            return await _Context.Orders.Include(r => r.OrderDetails).Include(r => r.UserRole).Include(r => r.UserRole).Include(r => r.UserRole.User).Include(r => r.UserRole.Role)
                    .Where(w => w.UserRole.User.UserName.Equals(UserName)).ToListAsync();
        }

        public async Task<List<Order>> GetUserOrdersByCellphoneAsync(string Cellphone)
        {
            return await _Context.Orders.Include(r => r.OrderDetails).Include(r => r.UserRole).Include(r => r.UserRole).Include(r => r.UserRole.User).Include(r => r.UserRole.Role)
                    .Where(w => w.UserRole.User.Cellphone.Trim() == Cellphone.Trim()).ToListAsync();
        }

        public async Task<ValidateCartForPayVM> PrepareCartForPayVM(CartVm cartVm)
        {
            ValidateCartForPayVM validateCartForPayVM = new();
            bool UpdateCart = false;
            County county = await _Context.Counties.Include(r => r.State).FirstOrDefaultAsync(x => x.CountyId == cartVm.CountyId);
            Cart cart = await _Context.Carts.Include(r => r.CartItems).FirstOrDefaultAsync(x => x.Id == cartVm.CartId);
            User user = await _Context.Users.Include(r => r.County).SingleOrDefaultAsync(x => x.Cellphone == cartVm.BuyerCellphone);
            UserRole userRole = null;
            if (cart == null)
            {
                return validateCartForPayVM;
            }
            if (user == null)
            {
                string userPass = Prodocers.Generators.GenerateUniqueString(4, 0, 4, 0);
                User New_user = new()
                {
                    Name = cartVm.BuyerName,
                    UserName = cartVm.BuyerCellphone,
                    Family = cartVm.BuyerFamily,
                    CountyId = cartVm.CountyId,
                    Cellphone = cartVm.BuyerCellphone,
                    IsActive = true,
                    Password = userPass,
                    RegisteredDate = DateTime.Now

                };
                User SaveUser = await _Context.Users.FirstOrDefaultAsync(x => x.Cellphone == cartVm.BuyerCellphone);
                _Context.Users.Add(New_user);

                if (SaveUser != null)
                {
                    cart.UserId = SaveUser.Id;
                }
                UserRole New_userRole = new()
                {
                    RegisterDate = DateTime.Now,
                    RoleId = 2,
                    IsActive = true,
                    User = New_user
                };
                _Context.Add(New_user);
                userRole = New_userRole;
            }
            else
            {
                userRole = await _Context.UserRoles.Include(r => r.User).SingleOrDefaultAsync(x => x.User == user && x.RoleId == 2);

            }
            if (cart != null)
            {

                cart.BuyerName = cartVm.BuyerName;
                cart.BuyerFamily = cartVm.BuyerFamily;
                cart.BuyerCellphone = cartVm.BuyerCellphone;
                cart.StateName = county.State.StateName;
                cart.CountyName = county.CountyName;
                cart.Address = cartVm.Address;
                cart.PostalCode = cartVm.PostalCode;
                cart.RecipientName = string.IsNullOrEmpty(cartVm.RecipientName) ? cartVm.BuyerName : cartVm.RecipientName;
                cart.RecipientFamily = string.IsNullOrEmpty(cartVm.RecipientFamily) ? cartVm.BuyerFamily : cartVm.RecipientFamily;
                cart.RecipientPhone = string.IsNullOrEmpty(cartVm.RecipientPhone) ? cartVm.BuyerCellphone : cartVm.RecipientPhone;
                UpdateCart = true;
            }

            DiscountCoupen discountCoupen = null;
            if (!string.IsNullOrEmpty(cart.DiscountCoupen))
            {
                discountCoupen = await _Context.DiscountCoupens.FirstOrDefaultAsync(x => x.IsActive && x.Code.Equals(cart.DiscountCoupen.Trim()));
            }
            int cartItems_total = 0;
            int cart_total_without_freight = 0;
            int cart_disValue = 0;
            if (cart.CartItems.Count != 0)
            {
                foreach (var cartItem in cart.CartItems)
                {
                    int prNetPrice = 0;
                    if (cartItem.Kind == "pr")
                    {
                        prNetPrice = await GetProductNetpriceAsync(cartItem.ProductId);
                    }
                    if (cartItem.Kind == "pk")
                    {
                        prNetPrice = await GetPackageNetpriceAsync(cartItem.ProductId);
                    }
                    int total = cartItem.Quantity * prNetPrice;
                    cartItems_total += total;
                }


            }
            if (discountCoupen != null)
            {
                cart_disValue = (int)(cartItems_total * discountCoupen.Percent) / 100;
                cart.DiscountCoupen = discountCoupen.Code;
                UpdateCart = true;
            }

            cart_total_without_freight = cartItems_total - cart_disValue;


            int cart_Freight = 0;

            if (county.Freight != null)
            {
                cart_Freight = (int)county.Freight;
                UpdateCart = true;
            }
            else
            {
                if (county.State.Freight != null)
                {
                    cart_Freight = (int)county.State.Freight;
                    UpdateCart = true;
                }
            }
            string cur = "ریال";
            InitialInfo initialInfo = await _Context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
            if (initialInfo != null)
            {
                cur = initialInfo.SiteCurrency;
            }
            int cartsum_withoutFreight = cart_total_without_freight;
            if (initialInfo != null)
            {
                if (initialInfo.FreeShipping_MinimumPurchaseAmount.HasValue)
                {
                    if (cartsum_withoutFreight > (int)initialInfo.FreeShipping_MinimumPurchaseAmount)
                    {
                        cart_Freight = 0;
                        UpdateCart = true;
                    }
                }
            }
            int cart_total = cartsum_withoutFreight + cart_Freight;
            if (UpdateCart == true)
            {
                cart.Freight = cart_Freight;
                _Context.Update(cart);
                await _Context.SaveChangesAsync();
            }
            validateCartForPayVM.Cart = cart;
            validateCartForPayVM.BuyerName = cartVm.BuyerName;
            validateCartForPayVM.BuyerFamily = cartVm.BuyerFamily;
            validateCartForPayVM.PayerId = long.Parse(cartVm.BuyerCellphone);
            validateCartForPayVM.CartFreight = cart_Freight;
            validateCartForPayVM.CartId = cart.Id;
            if (cart.Currency == "تومان")
            {
                cart_total *= 10;
            }
            validateCartForPayVM.CartTotal = cart_total;
            validateCartForPayVM.IsPrepare = true;
            validateCartForPayVM.Curr = cart.Currency;
            return validateCartForPayVM;
        }

        public async Task<List<Order>> GetOrdersByOrderIds(List<Guid> orderIds)
        {
            List<Order> orders = await _Context.Orders.Include(x => x.OrderDetails).Include(x => x.Cart)
                                .Where(w => orderIds.Any(a => a == w.Id)).ToListAsync();

            return orders;
        }

        public async Task<Order> GetOrderByDedicatedNumber(string dedN)
        {
            if (string.IsNullOrEmpty(dedN))
            {
                return null;
            }
            Order order = await _Context.Orders.Include(x => x.OrderDetails).SingleOrDefaultAsync(x => x.Order_DedicatedNumber == dedN);
            return order;
        }

        public async Task<SaveOrderVM> CreateOrderByAdmin(AdminOrderVM adminOrderVM, List<int> PIds, List<int> PCounts, List<string> Types, int? roleId)
        {
            SaveOrderVM saveOrderVM = new();
            try
            {

                User user = await _Context.Users.Include(r => r.County).SingleOrDefaultAsync(x => x.Cellphone == adminOrderVM.BuyerCellphone);
                if (user == null)
                {
                    string pass = Prodocers.Generators.GenerateUniqueString(0, 0, 8, 0);
                    User Newuser = new()
                    {
                        Name = adminOrderVM.BuyerName,
                        Family = adminOrderVM.BuyerFamily,
                        Cellphone = adminOrderVM.BuyerCellphone,
                        RegisteredDate = DateTime.Now,
                        Password = pass,
                        IsActive = true,
                        UserName = adminOrderVM.BuyerCellphone
                    };
                    _Context.Users.Add(Newuser);
                    await _Context.SaveChangesAsync();
                    user = await _Context.Users.FirstOrDefaultAsync(x => x.Cellphone == adminOrderVM.BuyerCellphone);
                    saveOrderVM.IsNewUser = true;
                    saveOrderVM.UserPassword = pass;
                }
                saveOrderVM.BuyerFullName = adminOrderVM.BuyerName + " " + adminOrderVM.BuyerFamily;
                UserRole userRole = await _Context.UserRoles.Include(r => r.User).Include(r => r.Role).SingleOrDefaultAsync(r => r.UserId == user.Id && r.RoleId == roleId.GetValueOrDefault(2));
                if (userRole == null)
                {
                    UserRole NewUserRole = new()
                    {
                        UserId = user.Id,
                        RoleId = roleId.GetValueOrDefault(2),
                        RegisterDate = DateTime.Now,
                        IsActive = true
                    };
                    _Context.Add(NewUserRole);

                    await _Context.SaveChangesAsync();
                    userRole = NewUserRole;
                }
                State state = await _Context.States.SingleOrDefaultAsync(x => x.StateId == adminOrderVM.StateId);
                County county = await _Context.Counties.SingleOrDefaultAsync(x => x.CountyId == adminOrderVM.CountyId);
                int DeliveryCost = (int)county.Freight;
                string DedN = Prodocers.Generators.GenerateUniqueString(_Context.Orders.Select(x => x.Order_DedicatedNumber).ToList(), 0, 0, 8, 0);
                saveOrderVM.OrderDedicated = DedN;
                Order order = new()
                {
                    Order_Date = DateTime.Now,
                    Order_BuyerCellphone = adminOrderVM.BuyerCellphone,
                    Order_BuyerName = adminOrderVM.BuyerName,
                    Order_BuyerFamily = adminOrderVM.BuyerFamily,
                    RecipientName = adminOrderVM.RecipientName,
                    RecipientFamily = adminOrderVM.RecipientFamily,
                    RecipientPhone = adminOrderVM.RecipientPhone,
                    Order_StateName = state?.StateName,
                    Order_CountyName = county?.CountyName,
                    Order_Address = adminOrderVM.Address,
                    Order_PostalCode = adminOrderVM.PostalCode,
                    Order_DeliveryCost = DeliveryCost,
                    Order_DedicatedNumber = DedN,
                    Order_IsFinished = true,
                    UserRole = userRole
                };
                string cur = "ریال";
                InitialInfo initialInfo = await _Context.InitialInfos.OrderByDescending(r => r.CreatedDate).FirstOrDefaultAsync();
                if (initialInfo != null)
                {
                    cur = initialInfo.SiteCurrency;
                }
                order.Currency = cur;
                //_Context.Orders.Attach(order);

                int sum = 0;
                foreach (var item in PIds.Select((value, ind) => (value, ind)))
                {

                    Product product = null;
                    Package package = null;
                    if (Types[item.ind] == "pr")
                    {
                        product = await _Context.Products.SingleOrDefaultAsync(x => x.ProductId == item.value);
                    }
                    if (Types[item.ind] == "pk")
                    {
                        package = await _Context.Packages.SingleOrDefaultAsync(x => x.PkId == item.value);
                    }
                    string pName = string.Empty; int pNetPrice = 0; int pPrice = 0;
                    if (product != null)
                    {
                        pName = product.ProductName;
                        pNetPrice = await GetProductNetpriceAsync(item.value);
                        pPrice = product.ProductPrice;
                    }

                    if (package != null)
                    {
                        pName = package.PkTitle;
                        pNetPrice = await GetPackageNetpriceAsync(item.value);
                        pPrice = (int)package.PkPrice;
                    }
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.value,
                        OrderDetailCount = PCounts[item.ind],
                        ProductKind = Types[item.ind],
                        OrderDatailProductName = pName,
                        OrderDetailNetPrice = pNetPrice,
                        OrderDetailPrice = pPrice,

                    });
                    sum += PCounts[item.ind] * pNetPrice;
                }
                order.Order_Sum = sum + DeliveryCost;
                _Context.Orders.Add(order);
                saveOrderVM.OrderId = order.Id;
                saveOrderVM.IsSuccess = true;
            }
            catch (Exception ex)
            {
                string m = ex.Message;

            }
            return saveOrderVM;
        }
        #endregion

    }
}
