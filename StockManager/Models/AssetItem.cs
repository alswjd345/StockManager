using System;
using System.Collections.Generic;
using System.Text;

namespace StockManager.Models
{
    public class AssetItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Memo { get; set; }
    }
}
