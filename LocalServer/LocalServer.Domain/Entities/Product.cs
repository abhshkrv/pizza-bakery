using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace LocalServer.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int productID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string productName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string barcode { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int categoryID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int manufacturerID { get; set; }
        public double sellingPrice { get; set; }
        [HiddenInput(DisplayValue = false)]
        public double maxPrice { get; set; }
        public int currentStock { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int minimumStock { get; set; }
        public int bundleUnit { get; set; }
        public float discountPercentage { get; set; }
    }
}
