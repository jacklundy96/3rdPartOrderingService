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
        private readonly OrderContext _context;

        public ThirdPartySupplier(string url, HttpClient HttpClient, OrderContext context)
        {
            _Url = url;
            _Client = HttpClient;
            _context = context;
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
                    SetOrder(order);

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
                    DeleteOrder(OrderID);
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

        /// <summary>
        /// Deletes the order stored in the local SQL store
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public IActionResult DeleteOrder(int OrderID)
        {
            Order order;
            try
            {
                 order = _context.Orders.Find(OrderID);
                _context.Orders.Remove(order);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
            return new OkResult();
        }

        /// <summary>
        /// Sets the order stored in the local SQL store
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        private IActionResult SetOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
            return new OkResult();
        }

        /// <summary>
        /// Gets the order stored in the local SQL store
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetOrder(int OrderID)
        {
            Order order = new Order();
            try
            {
                order =  _context.Orders.Find(OrderID);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
            return new OkObjectResult(order.GetAll());
        }


        http://dodgydealers.azurewebsites.net/"

            http://undercutters.azurewebsites.net/

    } 
}
