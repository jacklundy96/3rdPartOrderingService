using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThirdPartyOrderingService.Dtos
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string SupplierName { get; set; }
        [NotMapped]
        public string AccountName { get; set; }
        [NotMapped]
        public string CardNumber { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime When { get; set; }
        public string ProductName { get; set; }
        public string ProductEan { get; set; }
        public decimal TotalPrice { get; set; }

        public List<string> GetAll()
        {
            return new List<string>(new string[] { Id.ToString(), AccountName, CardNumber, ProductId.ToString() ,Quantity.ToString() ,When.ToLongDateString() ,ProductName ,ProductEan ,TotalPrice.ToString() });
        }
    }
}
