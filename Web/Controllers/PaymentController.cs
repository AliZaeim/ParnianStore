using Core.DTOs.General;
using Core.Prodocers;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentController(IStoreService storeService, IHttpContextAccessor httpContextAccessor)
        {
            _storeService = storeService;
            _httpContextAccessor = httpContextAccessor;
        }

        readonly PaymentResultVM paymentResultVM = new();
        public async Task<IActionResult> Index(long price,Guid cartId)
        {
            string redirectPage = "https://aliansabz.com/Payment/MellatCallBack/?cartId=" + cartId;
            //redirectPage += "MellatCallBack";

            await MellatPayment(price, redirectPage, cartId);
            return View();
        }

        public IActionResult Return()
        {
            return View(paymentResultVM);
        }

        private async Task MellatPayment(long price, string callBackUrl, Guid crtId)
        {
           
            try
            {
                
                BankMellatPayment.BypassCertificateError();               
                long TerminalId = 5946978;                
                string UserName = "aliansabz";                
                string UserPassword = "28789888";
                string addData = "خرید از سایت عالیان سبز";
                long t = DateTime.Now.Ticks;
                //long cId = Applications.ToLong(crtId);
                long oId = DateTime.Now.Ticks;
                Cart cart = await _storeService.GetCartByIdAsync(crtId);
                var payment = new BankMellat.PaymentGatewayClient();

                //فراخوانی تابع درخواست پرداخت
                var result = await payment.bpPayRequestAsync(
                    terminalId: TerminalId,
                    userName: UserName,
                    userPassword: UserPassword,
                    orderId: oId,
                    payerId: cart.BuyerCellphone,
                    amount: price,
                    localDate: BankMellatPayment.GetDate(),
                    localTime: BankMellatPayment.GetTime(),
                    additionalData: addData,
                    callBackUrl: callBackUrl
                  );
                cart.PaymentId = oId;
                _storeService.EditCart(cart);
                await _storeService.SaveChangesAsync();
                
                //بررسی نتیجه برگشتی ار تابع پرداخت
                //در صورت نول نبودن یعنی فراخوانی انجام شده
                //در صورت نول بودن یعنی فراخوانی انجام نشده
                if (result != null)
                {
                    string[] resultArray = result.Body.@return.Split(',');                    
                    if (int.Parse(resultArray[0].ToString()) == 0)
                    {
                        NameValueCollection datacollection = new();                        
                        datacollection.Add("RefId", resultArray[1]);
                        // ارسال اطلاعات به درگاه
                        await Response.WriteAsync(HttpHelper.PostData("https://bpm.shaparak.ir/pgwchannel/startpay.mellat", datacollection));

                    }
                    else
                    {
                        
                        paymentResultVM.PaymentMessage = PayResult.Mellat(resultArray[0]);
                    }

                }
                else
                {
                    paymentResultVM.PaymentMessage = "در حال حاضر امکان اتصال به این درگاه وجود ندارد ";
                }
            }
            catch 
            {
                paymentResultVM.PaymentMessage = "در حال حاضر امکان اتصال به این درگاه وجود ندارد ";                
            }
        }

        private async Task MellatReturn(Guid cartId)
        {
            // فراخوانی متد رفع پیغام های امنیتی
            BankMellatPayment.BypassCertificateError();

            if (string.IsNullOrEmpty(Request.Form["SaleReferenceId"]))
            {
                if (!string.IsNullOrEmpty(Request.Form["ResCode"]))
                {
                    paymentResultVM.PaymentMessage = PayResult.Mellat(Request.Form["ResCode"]);
                    
                }
                else
                {
                    paymentResultVM.PaymentMessage = "شماره رسید قابل قبول نیست";
                    
                }
            }
            else
            {
                try
                {
                    string TerminalId = "5946978";
                    string UserName = "aliansabz";
                    string UserPassword = "28789888";
                    
                    long SaleOrderId = 0;
                    long SaleReferenceId = 0;
                    string RefId = null;

                    try
                    {                        
                        SaleOrderId = long.Parse(Request.Form["SaleOrderId"].ToString().TrimEnd());
                        SaleReferenceId = long.Parse(Request.Form["SaleReferenceId"].ToString().TrimEnd());
                        RefId = Request.Form["RefId"].ToString().TrimEnd();
                    }
                    catch (Exception ex)
                    {
                        paymentResultVM.PaymentMessage = ex + "<br/>" + " وضعیت:مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد ";
                        return;
                    }

                    var bpService = new BankMellat.PaymentGatewayClient();

                    var Vresult = await bpService.bpVerifyRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                    if (Vresult != null)
                    {
                        if (Vresult.Body.@return == "0")
                        {
                            var IQresult = await bpService.bpInquiryRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                            if (IQresult.Body.@return == "0")
                            {
                                //در اینجا پرداخت انجام شده است و عملیات مربوط به سایت انجام می گیرد

                                paymentResultVM.PaymentMessage = "پرداخت با موفقیت انجام شد." + "<br/>" + "شناسه سفارش: " + SaleOrderId + "<br/>" + " شناسه مرجع تراکنش:" + SaleReferenceId + "<br/>" + "رسید پرداخت:" + RefId;

                                // تایید پرداخت
                                var Sresult = await bpService.bpSettleRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                                if (Sresult != null)
                                {
                                    if (Sresult.Body.@return == "0" || Sresult.Body.@return == "45")
                                    {
                                        //تراکنش تایید و ستل شده است 
                                        SaveOrderVM saveOrderVM = await _storeService.CreateOrderByCartAsync(cartId);
                                        Cart cart = await _storeService.GetCartByIdAsync(cartId);
                                        Order order = await _storeService.GetOrderByIdAsync(saveOrderVM.OrderId);
                                        paymentResultVM.Order = order;
                                        if (saveOrderVM.IsSuccess)
                                        {
                                            paymentResultVM.SaleReferenceId = SaleReferenceId.ToString();
                                            paymentResultVM.PaymentStatus = true;
                                            
                                            CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);
                                            if (CookieExtensions.ExistCookie("cartid"))
                                            {
                                                CookieExtensions.RemoveCookie("cartid");
                                            }
                                            bool ware = await _storeService.CreateWareHouseWithOrder(saveOrderVM.OrderId);
                                            if (ware)
                                            {
                                                string fullname = cart.BuyerName + " " + cart.BuyerFamily;
                                                _storeService.SendOrderNumber(fullname, saveOrderVM.OrderDedicated, cart.BuyerCellphone);
                                               
                                                _storeService.SendBuyingFromSite(fullname, order.Order_Sum.ToString("N0"), "09391235923");
                                                if (saveOrderVM.IsNewUser)
                                                {
                                                    _storeService.SendUserName_and_Password(cart.BuyerCellphone, saveOrderVM.UserPassword, cart.BuyerCellphone);
                                                }
                                            }
                                        }
                                        await _storeService.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        //تراکنش تایید شده ولی ستل نشده است
                                    }
                                }
                            }
                            else
                            {

                                //عملیات برگشت دادن مبلغ
                                var Rvresult = await bpService.bpReversalRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                                paymentResultVM.PaymentMessage = "تراکنش بازگشت داده شد";
                                paymentResultVM.PaymentStatus = false;
                            }
                        }
                        else
                        {
                            paymentResultVM.PaymentMessage = PayResult.Mellat(Vresult.Body.@return);
                            paymentResultVM.PaymentStatus = false;
                            

                            long paymentId = SaleOrderId;

                        }
                    }
                    else
                    {
                        paymentResultVM.PaymentMessage = "شماره رسید قابل قبول نیست";
                        paymentResultVM.PaymentStatus = false;
                       
                        
                    }

                }
                catch 
                {                    
                    paymentResultVM.PaymentMessage = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
                    paymentResultVM.PaymentStatus = false;
                }
            }
        }

        public async Task<ActionResult> MellatCallBack(Guid cartId)
        {
            
            paymentResultVM.BankName = "درگاه بانک ملت";
            // فراخوانی متد برگشت از درگاه
            await MellatReturn(cartId);

            return View("Return", paymentResultVM);
        }

       



    }
}
