using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using ThirdPartyOrderingService.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace ThirdPartyOrderingService.Services
{
    public interface IThirdPartySupplierService
    {
        Task<IActionResult> MakeOrderAsync(Order order, string url,string supplierName);
        Task<IActionResult> DeleteOrderAsync(int OrderID, string url);
        Task<IActionResult> GetOrderAsync(int OrderID, string url,string supplierName);
    }
    public class ThirdPartySupplierService : IThirdPartySupplierService
    {
        protected HttpClient _Client;
        private readonly DBService _dbs;

        public ThirdPartySupplierService( HttpClient HttpClient, DBService dbs)
        {
            _Client = HttpClient;
            _dbs = dbs;
        }

        /// <summary>
        /// Makes the order via the supplier's API asynchronously 
        /// </summary>
        /// <param name="order">order object to be submitted to the api</param>
        /// <param name="supplierName">The name of the supplier recieving the order</param>
        /// <returns></returns>
        public async Task<IActionResult> MakeOrderAsync(Order order, string url,string supplierName)
        {
            try
            {

                var values = new Dictionary<string, string>
                {
                    {"AccountName", order.AccountName},
                    {"CardNumber", order.CardNumber},
                    {"ProductId", order.ProductId.ToString()},
                    {"Quantity", order.Quantity.ToString()}
                };

                HttpResponseMessage response;
                do
                {
                    var content = new FormUrlEncodedContent(values);
                    response = await _Client.PostAsync(url + "api/Order", content);

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        order.OrderId = (int) JObject.Parse(responseBody)["Id"];
                        order.When = (DateTime) JObject.Parse(responseBody)["When"];
                        order.ProductName = JObject.Parse(responseBody)["ProductName"].ToString();
                        order.ProductEan = JObject.Parse(responseBody)["ProductEan"].ToString();
                        order.TotalPrice = (decimal) JObject.Parse(responseBody)["TotalPrice"];
                        order.SupplierName = supplierName;
                        _dbs.SetOrder(order);

                        return new OkResult();
                    }

                } while (response.StatusCode == HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
            return new OkResult();
        }

        /// <summary>
        ///  Deletes the order via the supplier's API asynchronously 
        /// </summary>
        /// <param name="OrderID">The order reference as per each supplier</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteOrderAsync(int OrderID, string url)
        {
            string _url = string.Format($"{url}/api/Order/{OrderID}");

            HttpResponseMessage response;
            do
            {
                response = await _Client.DeleteAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _dbs.DeleteOrder(OrderID);
                    return new OkResult();
                }
            } while (response.StatusCode == HttpStatusCode.ServiceUnavailable);

            string responseBody = await response.Content.ReadAsStringAsync();
            return new ObjectResult(new { StatusCode = 500, Message = JObject.Parse(responseBody)["Message"].ToString() });
 
        }

        /// <summary>
        /// Gets the order via the supplier's API asynchronously 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetOrderAsync(int OrderID, string url, string supplierName)
        {
            Order order = _dbs.GetOrder(OrderID);

            if (order != null && order.SupplierName.Equals(supplierName))
                return new JsonResult(order);
            else
                return new NotFoundResult();
        }
    } 
}
