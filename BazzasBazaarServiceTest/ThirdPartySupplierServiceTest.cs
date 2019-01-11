using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyOrderingService;
using ThirdPartyOrderingService.Services;
using Xunit;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace ThirdPartyOrderingServiceTest
{
    public class ThirdPartySupplierServiceTest
    {
        private ThirdPartySupplierService _tss;
        private static DbContextOptions<OrderContext> options;

        public ThirdPartySupplierServiceTest()
        {
            options = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(databaseName: "In memory Test database")
                .Options;
        }

        [Fact]
        public async void GetOrderAsync_LimitTest()
        {
            using (var context = new OrderContext(options))
            {   
                //Arrange
                var _dbs = new DBService(context);
                _tss = new ThirdPartySupplierService(new HttpClient(), _dbs);
                //Act
                string ss = "UnderCutters";
                string url = "http://undercutters.azurewebsites.net/";
                var response = await _tss.GetOrderAsync(0,url,ss);
                //Assert 
                Assert.True(response.GetType() == typeof(NotFoundResult));
            }
        }

        /// <summary>
        /// Test to see if a valid order can be acces by providing a different suppliers details
        /// </summary>
        [Fact]
        public async void GetOrderAsync_WrongSupplier()
        {
            using (var context = new OrderContext(options))
            {
                //Arrange
                var _dbs = new DBService(context);
                _tss = new ThirdPartySupplierService(new HttpClient(), _dbs);
                //Act
                string ss = "DodgyDealers";
                string url = "http://dodgydealers.azurewebsites.net/";
                var response = await _tss.GetOrderAsync(186, url, ss);
                //Assert 
                Assert.True(response.GetType() == typeof(NotFoundResult));
            }
        }

        [Fact]
        public async void GetOrderAsync_IdOutOfBounds()
        {
            using (var context = new OrderContext(options))
            {
                //Arrange
                var _dbs = new DBService(context);
                _tss = new ThirdPartySupplierService(new HttpClient(), _dbs);
                //Act
                string ss = "UnderCutters";
                string url = "http://undercutters.azurewebsites.net/";
                var response = await _tss.GetOrderAsync(-1, url, ss);
                //Assert 
                Assert.True(response.GetType() == typeof(NotFoundResult));
            }
        }
    }
}



