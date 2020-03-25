using System;
using System.Collections.Generic;

namespace ProjectZero.Lib.Entities
{
    public partial class Receipt
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Manufacturer { get; set; }
    }
}
