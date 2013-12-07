using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LocalServer.Domain.Entities
{
    public class PriceDisplay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [HiddenInput(DisplayValue = false)]
        public int priceDisplayID { get; set; }
        public string barcode { get; set; }
        [HiddenInput(DisplayValue = false)]
        public byte status { get; set; }
    }
}
