using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalServer.WebUI.Models
{
    public class PriceDisplayListViewModel
    {
        public IEnumerable<PriceDisplay> PriceDisplays { get; set; }
    }
}