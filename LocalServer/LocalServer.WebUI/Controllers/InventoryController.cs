using LocalServer.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LocalServer.WebUI.Controllers
{
    public class InventoryController : Controller
    {

        IProductRepository _productRepo;
        IManufacturerRepository _manufacturerRepo;
        ICategoryRepository _categoryRepo;

        public InventoryController(ICategoryRepository categoryRepo, IManufacturerRepository manufacturerRepo, IProductRepository productRepo)
        {
            _categoryRepo = categoryRepo;
            _manufacturerRepo = manufacturerRepo;
            _productRepo = productRepo;
        }


        //
        // GET: /Inventory/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sync()
        {
            string url = "http://pizza-hq.azurewebsites.net/shop/getFullInventoryList";
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }


            return View();
        }

    }
}
