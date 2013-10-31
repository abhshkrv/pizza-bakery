using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LocalServer.WebUI.Models;
using System.Web.Script.Serialization;
using System.Text;


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

        public ActionResult SyncProducts()
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

            JObject raw = JObject.Parse(text);
            JArray  productArray = (JArray )raw["Products"];

            foreach (var p in productArray)
            {
                Product product = new Product();
                product.productID = 0;
                product.barcode = (string)p["barcode"];
                product.categoryID = (int)p["categoryID"];
                product.manufacturerID = (int)p["manufacturerID"];
                product.bundleUnit = (int)p["bundleUnit"];
                product.currentStock = (int)p["currentStock"];
                product.discountPercentage = (float)p["discountPercentage"];
                //Change this to p["maxPrice"] later
                product.maxPrice = (float)p["costPrice"];
                product.minimumStock = (int)p["minimumStock"];
                product.productName = (string)p["productName"];
                product.sellingPrice = product.maxPrice;

                _productRepo.quickSaveProduct(product);
            }
            _productRepo.saveContext();

            return View();
        }

        public ActionResult SyncCategories()
        {

            string url = "http://pizza-hq.azurewebsites.net/shop/getFullCategoriesList";
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            JObject raw = JObject.Parse(text);
            JArray productArray = (JArray)raw["Categories"];

            foreach (var p in productArray)
            {
                Category category = new Category();
                category.categoryID = (int)p["categoryID"];
                
                category.categoryName = (string)p["categoryName"];
                

                _categoryRepo.quickSaveCategory(category);
            }
            _categoryRepo.saveContext();

            return View();
        }

        public ActionResult SyncManufacturers()
        {

            string url = "http://pizza-hq.azurewebsites.net/shop/getFullManufacturersList";
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            JObject raw = JObject.Parse(text);
            JArray productArray = (JArray)raw["Manufacturers"];

            Dictionary<int, bool> d = new Dictionary<int, bool>();

            foreach (var p in productArray)
            {
                Manufacturer manufacturer = new Manufacturer();
                manufacturer.manufacturerID = (int)p["manufacturerID"];
                manufacturer.manufacturerName = (string)p["manufacturerName"];
                if (!d.ContainsKey(manufacturer.manufacturerID))
                {
                    d.Add(manufacturer.manufacturerID, true);
                    _manufacturerRepo.quickSaveManufacturer(manufacturer);
                }
            }
            _manufacturerRepo.saveContext();

            return View();
        }

        public int PageSize = 200;
        public ViewResult List(int page = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = _productRepo.Products
                .OrderBy(p => p.productID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _productRepo.Products.Count()
                }
            };

            return View(viewModel);
        }

        public ViewResult Details(int productId)
        {

            ProductsDetailsViewModel viewModel = new ProductsDetailsViewModel();
            viewModel.product = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
            viewModel.manufacturer = _manufacturerRepo.Manufacturers.FirstOrDefault(m => m.manufacturerID == viewModel.product.manufacturerID);
            viewModel.category = _categoryRepo.Categories.FirstOrDefault(c => c.categoryID == viewModel.product.categoryID);

            return View(viewModel);
        }

        public ViewResult Edit(int productId)
        {
            Product product = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepo.saveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.productName);
                return RedirectToAction("List");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProcessSearch(string name = null, string barcode = null, string manufacturer = null, string category = null)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel();
            String noResults = "No product found with ";

            if (barcode != "" && barcode != null)
            {
                viewModel.Products = _productRepo.Products.Where(p => p.barcode == barcode);
                noResults += "barcode = " + barcode;
            }
            else if (name != "" && name != null)
            {
                viewModel.Products = _productRepo.Products.Where(p => p.productName.Contains(name));
                noResults += "Name = " + name;
            }

            if (viewModel.Products.Count() != 0)
                return View("SearchResults", viewModel);
            else
            {
                TempData["results"] = noResults;
                return View("Search");
            }

        }

        public ContentResult sendInventory()
        {
            var inventory = _productRepo.Products.ToList();

            var data = from p in inventory select new { barcode = p.barcode, currentStock = p.currentStock, discount = p.discountPercentage, sellingPrice = p.sellingPrice };

            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("ShopID", "4");
            output.Add("Inventory", data);

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };


            //sendPost(serializer.Serialize(output));

            return new ContentResult()
            {
                Content = serializer.Serialize(output),
                ContentType = "application/json",
            };

        }

        private string sendPost(string content)
        {
            HttpWebRequest httpWReq =
        (HttpWebRequest)WebRequest.Create("http://pizza-hq.azurewebsites.net/shop/uploadtransactions");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "TransactionData=";
            postData += content;
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/json";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;

        }
    }
}
