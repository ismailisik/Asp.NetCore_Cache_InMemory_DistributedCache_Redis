using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDistributedCacheRedis.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }

    }
}
