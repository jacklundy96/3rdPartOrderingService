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
    public class UnderCuttersController : ControllerBase,ISupplierController
    {
        private Services.UnderCuttersService _ucs;
        private readonly string SupplierName = "UnderCutters";

        public UnderCuttersController(IUnderCutterService Ucs)
        {
            _ucs = (UnderCuttersService)Ucs;
        }

        [HttpPost("Order")]

        public async Task <IActionResult> Order([FromBody]Order order)
        {
            order.SupplierName = SupplierName;
            return await _ucs.MakeOrderAsync(order);
        }

        [HttpGet("Orders")]
        public IActionResult GetOrderById([FromBody] int OrderID)
        {
            return _ucs.GetOrder(OrderID);
        }

        [HttpDelete("Orders")]
        public async Task<IActionResult> DeleteOrderAsync([FromBody] int OrderID)
        {
            return await _ucs.DeleteOrderAsync(OrderID);
        }
    }
}