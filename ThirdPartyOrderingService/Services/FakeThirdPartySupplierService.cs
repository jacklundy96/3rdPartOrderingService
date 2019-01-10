using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyOrderingService.Dtos;

namespace ThirdPartyOrderingService.Services
{
    public class FakeThirdPartySupplierService : IThirdPartySupplierService
    {
        List<Order> _orders = new List<Order>();

        public FakeThirdPartySupplierService()
        {
            _orders = new List<Order>()
            {
                new Order()
                {


                },
                new Order(){},
                new Order(){},
                new Order(){},
                new Order(){}
            };
        }

        //public int OrderId { get; set; }

        //public int ProductId { get; set; }
        //public int Quantity { get; set; }
        //public DateTime When { get; set; }
        //public string ProductName { get; set; }
        //public string ProductEan { get; set; }
        //public decimal TotalPrice { get; set; }

        public async Task<IActionResult> DeleteOrderAsync(int OrderID,string url)
        {
            return new OkResult();
        }

        public async Task<IActionResult> GetOrderAsync(int OrderID,string url,string supplierName)
        {
            return new JsonResult(_orders.FirstOrDefault(o => o.Id == OrderID));
        }

        public async Task<IActionResult> MakeOrderAsync(Order order,string url,string supplierName)
        {
            return new OkResult();
        }
    }
}
