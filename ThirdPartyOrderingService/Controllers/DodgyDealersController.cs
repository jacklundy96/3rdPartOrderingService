using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyOrderingService.Dtos;
using ThirdPartyOrderingService.Services;

namespace ThirdPartyOrderingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Staff"]
    public class DodgyDealersController : ControllerBase, ISupplierController
    {
        private Services.ThirdPartySupplierService _tts;
        private readonly string SupplierName = "DodgyDealers";

        public DodgyDealersController(ThirdPartySupplierService Tts)
        {
            _tts = Tts;
        }

        [HttpPost("Order")]
        //[Authorize(Roles = "Staff"]
        public async Task<IActionResult> Order([FromBody]Order order)
        {

            order.SupplierName = SupplierName;
            return await _tts.MakeOrderAsync(order);

        }

        [HttpGet("Orders")]
        public async Task<IActionResult> GetOrderById([FromBody] int OrderID)
        {

            return await _tts.GetOrderAsync(OrderID);

        }

        [HttpDelete("Orders")]
        public async Task<IActionResult> DeleteOrderAsync([FromBody] int OrderID)
        {

            return await _tts.DeleteOrderAsync(OrderID);

        }

    }
}