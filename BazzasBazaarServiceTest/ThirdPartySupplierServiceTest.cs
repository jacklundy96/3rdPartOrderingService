using BazzasBazaar.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ThirdPartyOrderingService;
using Xunit;

namespace ThirdPartyOrderingServiceTest
{
    public class ThirdPartySupplierServiceTest
    {
        private List<Order> orders;
        private static int DbIndex = 1;

        private static DbContextOptions<OrderContext> options;

        public ThirdPartySupplierServiceTest()
        {
            var now = DateTime.Now;
            orders = new List<Order>()
            {
                new Order
                {
                    AccountName = "4",
                    CardNumber = "3",
                    ProductId = 5,
                    Quantity = 1
                }
            };
            options = new DbContextOptionsBuilder<OrderContext>()
               .UseInMemoryDatabase(databaseName: "In memory Test database")
               .Options;
        }

        [Fact]
        public void GetOrder_InvalidCall()
        {
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new OrderContext(options))
            {
                //Arrange
                //var _bbs = new DBService(context);
                //var _ds = new DispatchService(_dbs);
                //context.Orders.Add(orders[0]);
                //context.SaveChanges();
                //Act
                //var order = _ds.GetOrder(-1);
                //Assert 
                //Assert.Null(order);
            }
        }


    }
}
