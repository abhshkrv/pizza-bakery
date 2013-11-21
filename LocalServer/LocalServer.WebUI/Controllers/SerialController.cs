﻿using LocalServer.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LocalServer.WebUI.Controllers
{
    public class SerialController : Controller
    {
        IProductRepository _productRepo;
        IPriceDisplayRepository _priceRepo;

         public SerialController(IProductRepository productRepo, IPriceDisplayRepository priceRepo)
         {
             _productRepo = productRepo;
             _priceRepo = priceRepo;
         }
        
        public ContentResult Product()
        {
            var products = _productRepo.Products;

            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("products",products);

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            return new ContentResult()
            {
                Content = serializer.Serialize(output),
                ContentType = "application/json",
            };
           
        }

        public ContentResult PriceDisplays()
        {
            var priceDisplayUnits = _priceRepo.PriceDisplays;

            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("priceDisplayUnits", priceDisplayUnits);

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            return new ContentResult()
            {
                Content = serializer.Serialize(output),
                ContentType = "application/json",
            };
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
