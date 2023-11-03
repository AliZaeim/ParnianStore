using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IStoreService _storeService;
        public GeneralController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpGet]
        public async Task<string> TrackingOrder(string dedN)
        {
            var order = await _storeService.GetOrderByDedicatedNumber(dedN);
            return order.Comment;
        }
    }
}
