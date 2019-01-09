using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ThirdPartyOrderingService.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ThirdPartyOrderingService.Services
{

    public abstract class ThirdPartySupplier
    {
        protected string _Url;
        protected HttpClient _Client;
        private readonly DBService _dbs;

        public ThirdPartySupplier(string url, HttpClient HttpClient, DBService dbs)
        {
            _Url = url;
            _Client = HttpClient;
            _dbs = dbs;
        }

        /// <summary>
        /// Makes the order via the supplier's API asynchronously 
        /// </summary>
        /// <param name="order">order object to be submitted to the api</param>
        /// <param name="supplierName">The name of the supplier recieving the order</param>
        /// <returns></returns>
        public virtual async Task<IActionResult> MakeOrderAsync(Order order)
        {
            var values = new Dictionary<string, string>
            {
               { "AccountName", order.AccountName },
               { "CardNumber", order.CardNumber },
               { "ProductId", order.ProductId.ToString() },
               { "Quantity", order.Quantity.ToString() }
            };

            HttpResponseMessage response;
            do
            {
                var content = new FormUrlEncodedContent(values);
                response = await _Client.PostAsync(_Url + "api/Order", content);

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(responseBody);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    order.OrderId = (int)JObject.Parse(responseBody)["Id"];
                    order.When = (DateTime)JObject.Parse(responseBody)["When"];
                    order.ProductName = JObject.Parse(responseBody)["ProductName"].ToString();
                    order.ProductEan = JObject.Parse(responseBody)["ProductEan"].ToString();
                    order.TotalPrice = (decimal)JObject.Parse(responseBody)["TotalPrice"];
                    _dbs.SetOrder(order);

                    return new OkResult();
                }

            } while (response.StatusCode == HttpStatusCode.ServiceUnavailable);

            return new StatusCodeResult(500);
        }

        /// <summary>
        ///  Deletes the order via the supplier's API asynchronously 
        /// </summary>
        /// <param name="OrderID">The order reference as per each supplier</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteOrderAsync(int OrderID)
        {
            string url = string.Format($"{0}/{1}/{OrderID}", _Url, "api/Order", OrderID.ToString());

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
        public async Task<IActionResult> GetOrderAsync(int OrderID)
        {
            string url = string.Format($"{0}/{1}/{OrderID}", _Url, "api/Order", OrderID.ToString());

            HttpResponseMessage response;
            string responseBody; 
            do
            {
                response = await _Client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseBody = await response.Content.ReadAsStringAsync();

                    JObject j = new JObject();
                    j.Add("Id", JObject.Parse(responseBody)["Id"].ToString());
                    j.Add("ProductId", JObject.Parse(responseBody)["ProductId"].ToString());
                    j.Add("Quantity", JObject.Parse(responseBody)["Quantity"].ToString());
                    j.Add("When", JObject.Parse(responseBody)["When"].ToString());
                    j.Add("ProductName", JObject.Parse(responseBody)["ProductName"].ToString());
                    j.Add("ProductEan", JObject.Parse(responseBody)["ProductEan"].ToString());
                    j.Add("TotalPrice", JObject.Parse(responseBody)["TotalPrice"].ToString());

                    return new ObjectResult(new
                    { StatusCode = 500,
                        Message = j.ToString()
                    });
                }
            } while (response.StatusCode == HttpStatusCode.ServiceUnavailable);

            responseBody = await response.Content.ReadAsStringAsync();
            return new ObjectResult(new { StatusCode = 500, Message = JObject.Parse(responseBody)["Message"].ToString() });

        }

 

       // http://dodgydealers.azurewebsites.net/"

       // http://undercutters.azurewebsites.net/
    } 
}
