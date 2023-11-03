using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Web.Pages
{
    public class Alian_Sabz_Tracking_OrderModel : PageModel
    {
        private readonly IStoreService _storeService;
        public Alian_Sabz_Tracking_OrderModel(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [BindProperty]
        [Display(Name ="شماره سفارش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string OrderDedicatednumber { get; set; }

        public Order Order { get; set; }
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if(!string.IsNullOrEmpty(OrderDedicatednumber))
            {
                Order = await _storeService.GetOrderByDedicatedNumber(OrderDedicatednumber);
                
            }
            
            return Page();
        }
    }
}
