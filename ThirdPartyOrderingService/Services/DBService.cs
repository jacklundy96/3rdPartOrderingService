using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyOrderingService.Dtos;

namespace ThirdPartyOrderingService.Services
{
    public class DBService
    {
        private readonly OrderContext _context;

        public DBService(OrderContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes the order stored in the local SQL store
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public IActionResult DeleteOrder(int OrderID)
        {
            Order order = _context.Orders.Find(OrderID);
            _context.Orders.Remove(order);

            return new OkResult();
        }

        /// <summary>
        /// Sets the order stored in the local SQL store
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        public IActionResult SetOrder(Order order)
        {

            _context.Orders.Add(order);
            _context.SaveChanges();

            return new OkResult();
        }

        /// <summary>
        /// Gets the order stored in the local SQL store
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetOrder(int OrderID)
        {
            Order order = _context.Orders.Find(OrderID);

            return new JsonResult(order.GetAll());
        }

    }
}
