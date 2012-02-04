using System;

namespace FluentAdo.Oledb.Domain
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public string Manufacturer { get; set; }

        public string BarCode { get; set; }

        public string ProductCode { get; set; }

        public string Size { get; set; }
    }
}