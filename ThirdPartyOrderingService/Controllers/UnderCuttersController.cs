using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyOrderingService.Dtos;
using ThirdPartyOrderingService.Services;

namespace ThirdPartyOrderingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Staff"]
    public class UnderCuttersController : ControllerBase
    {
        private IThirdPartySupplierService _tts;
        private readonly string SupplierName = "UnderCutters";
        private string _url = "http://undercutters.azurewebsites.net/";

        public UnderCuttersController(IThirdPartySupplierService Tts)
        {
            _tts = Tts;
        }

        [HttpPost("Order")]
        public async Task <IActionResult> Order([FromBody]Order order)
        {
            order.SupplierName = SupplierName;
            return await _tts.MakeOrderAsync(order, _url, SupplierName);
        }

        [HttpGet("Orders/Get/{OrderID}")]
        public async Task<IActionResult> GetOrderById(int OrderID)
        {
            return await _tts.GetOrderAsync(OrderID, _url,SupplierName);
        }

        [HttpDelete("Order/{OrderID}")]
        public async Task<IActionResult> DeleteOrderAsync(int OrderID)
        {
            return await _tts.DeleteOrderAsync(OrderID, _url);
        }
    }
}