using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyOrderingService.Dtos;

namespace ThirdPartyOrderingService.Controllers
{
    interface ISupplierController
    {
        Task<IActionResult> Order([FromBody]Order order);

        IActionResult GetOrderById([FromBody] int OrderID);

        Task<IActionResult> DeleteOrderAsync([FromBody] int OrderID);
    }
}
