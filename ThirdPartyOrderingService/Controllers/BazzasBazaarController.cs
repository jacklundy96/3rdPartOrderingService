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
    public class BazzasBazaarController : ControllerBase, ISupplierController
    {
        private Services.BazzasBazaarService _bbs;
        private readonly string SupplierName = "BazzasBazaar";

        public BazzasBazaarController(IBazzasBazaarService bbs)
        {
            _bbs = (BazzasBazaarService)bbs;
        }

        [HttpPost("Order")]
        public async Task<IActionResult> Order([FromBody]Order order)
        {
            order.SupplierName = SupplierName;
            return await _bbs.MakeOrderAsync(order);
        }

        [HttpGet("Orders/{id}")]
        public Task<IActionResult> GetOrderById([FromBody] int OrderID)
        {
            return _bbs.GetOrder(OrderID);
        }

        [HttpDelete("Orders")]
        public async Task<IActionResult> DeleteOrderAsync([FromBody] int OrderID)
        {
            return await _bbs.DeleteOrderAsync(OrderID);
        }

    }
}