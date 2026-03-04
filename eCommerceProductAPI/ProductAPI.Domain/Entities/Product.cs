using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
