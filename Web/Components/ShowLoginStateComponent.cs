using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowLoginStateComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            return await Task.FromResult(View("/Pages/Components/_showLoginState.cshtml", type));
        }
    }
}
