using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ThirdPartyOrderingService.Dtos;

namespace ThirdPartyOrderingService.Services
{
    public interface IBazzasBazaarService
    {
    }
    public class BazzasBazaarService : ThirdPartySupplier, IBazzasBazaarService
    {
        public BazzasBazaarService(HttpClient Client, OrderContext Context) : base("http://undercutters.azurewebsites.net/",Client,Context)
        {
        }

        public override async Task<IActionResult> MakeOrderAsync(Order order)
        {
            try
            {
                StoreClient client = new StoreClient();
                var result = client.CreateOrder(order.AccountName, order.CardNumber, order.ProductId, order.Quantity);

                client.Close();
                // Use the 'client' variable to call operations on the service.
                return new OkResult();
                // Always close the client.
            }
            catch (Exception ex)
            { return new OkResult();
            }
            }

        public async Task<IActionResult> DeleteOrderAsync(int OrderID)
        {
            return new BadRequestResult();
        }

        /// <summary>
        /// Gets the order via the supplier's API asynchronously 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetOrderAsync(int OrderID)
        {
            return new BadRequestResult();
        }
    }
}
