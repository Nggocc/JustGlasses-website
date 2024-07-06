using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopweb.Models
{
    public class TopBanChay
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
    }
}