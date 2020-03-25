using System;
using System.Collections.Generic;

namespace ProjectZero.Lib.Entities
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
