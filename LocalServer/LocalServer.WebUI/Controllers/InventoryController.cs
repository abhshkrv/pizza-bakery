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

            string url = "http://hqserver.azurewebsites.net/shop/getFullInventoryList";
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            JObject raw = JObject.Parse(text);
            JArray productArray = (JArray)raw["Products"];

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
                product.maxPrice = (decimal)p["costPrice"];
                product.minimumStock = (int)p["minimumStock"];
                product.productName = (string)p["productName"];
                product.sellingPrice = product.maxPrice;

                _productRepo.quickSaveProduct(product);
            }
            _productRepo.saveContext();

            return View();
        }

        public ActionResult RefreshProducts()
        {

            string url = "http://hqserver.azurewebsites.net/shop/getFullInventoryList";
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            JObject raw = JObject.Parse(text);
            JArray productArray = (JArray)raw["Products"];

            Dictionary<string, Product> productDictionary = new Dictionary<string, Product>();

            foreach (var p in productArray)
            {
                Product product = new Product();
                product.barcode = (string)p["barcode"];
                product.categoryID = (int)p["categoryID"];
                product.manufacturerID = (int)p["manufacturerID"];
                product.bundleUnit = (int)p["bundleUnit"];
                product.currentStock = (int)p["currentStock"];
                product.discountPercentage = (float)p["discountPercentage"];
                //Change this to p["maxPrice"] later
                product.maxPrice = (decimal)p["costPrice"];
                product.minimumStock = (int)p["minimumStock"];
                product.productName = (string)p["productName"];
                product.sellingPrice = product.maxPrice;

                productDictionary.Add(product.barcode, product);
            }

            List<string> discontinuedProducts = new List<string>();

            var localProductlist = _productRepo.Products.ToArray();
            foreach (var p in localProductlist)
            {
                if (productDictionary.ContainsKey(p.barcode))
                {
                    Product nP = productDictionary[p.barcode];
                    p.bundleUnit = nP.bundleUnit;
                    p.categoryID = nP.categoryID;
                    p.manufacturerID = nP.manufacturerID;
                    p.maxPrice = nP.maxPrice;
                    p.minimumStock = nP.minimumStock;
                    p.productName = nP.productName;
                    _productRepo.quickSaveProduct(p);
                }
                else
                {
                    discontinuedProducts.Add(p.barcode + " - " + p.productName);
                    _productRepo.deleteProduct(p);
                }
            }

            Dictionary<string, Product> currentProductDictionary = _productRepo.Products.ToDictionary(p => p.barcode);

            foreach (var invprod in productDictionary)
            {
                Product inp = invprod.Value;
                if (!currentProductDictionary.ContainsKey(inp.barcode))
                {
                    _productRepo.saveProduct(inp);
                }
            }

            _productRepo.saveContext();


            return View(discontinuedProducts);
        }

        public ActionResult SyncCategories()
        {

            string url = "http://hqserver.azurewebsites.net/shop/getFullCategoriesList";
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

            string url = "http://hqserver.azurewebsites.net/shop/getFullManufacturersList";
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

        public ActionResult Details(int productId)
        {
            try
            {
                ProductsDetailsViewModel viewModel = new ProductsDetailsViewModel();
                viewModel.product = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
                viewModel.manufacturer = _manufacturerRepo.Manufacturers.FirstOrDefault(m => m.manufacturerID == viewModel.product.manufacturerID);
                viewModel.category = _categoryRepo.Categories.FirstOrDefault(c => c.categoryID == viewModel.product.categoryID);

                return View(viewModel);
            }
            catch
            {
                TempData["message"] = string.Format("Product has been deleted");
                return RedirectToAction("List");
            }
        }

        public ActionResult Edit(int productId)
        {
            try
            {
                Product product = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
                return View(product);
            }
            catch {
                TempData["message"] = string.Format("Product has been deleted");
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (product.bundleUnit < 0 || product.barcode.Length < 8 || product.currentStock < 0 || product.sellingPrice < 0 || product.sellingPrice > product.maxPrice)
            {
                TempData["message"] = "Error adding product, there are invalid fields";
                return View(product);
            }
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

        public ActionResult sendInventory()
        {
            var inventory = _productRepo.Products.ToList();

            var data = from p in inventory select new { barcode = p.barcode, currentStock = p.currentStock, minimumStock = p.minimumStock, discount = p.discountPercentage, sellingPrice = p.sellingPrice };

            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("ShopID", "1");
            output.Add("Inventory", data);

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            try
            {

                sendPost(serializer.Serialize(output));
                return View();
            }
            catch
            {
                TempData["Result"] = "The connection was reset because of a timeout.Don't worry the products are being updated in the background.Please check again after a few minutes.";
                return RedirectToAction("../Home/Index");

            }

           /* return new ContentResult()
            {
                Content = serializer.Serialize(output),
                ContentType = "application/json",
            }; */

        }

        private string sendPost(string content)
        {
            HttpWebRequest httpWReq =
        (HttpWebRequest)WebRequest.Create("http://hqserver.azurewebsites.net/shop/uploadOutletInventory");
            httpWReq.Timeout = 30000000;
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "input=";
            postData += content;
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;

        }

        public ActionResult getNewActivePrices()
        {
            string url = "http://newdata.blob.core.windows.net/stock/prices.txt";
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            JObject raw = JObject.Parse(text);
            JArray priceList = (JArray)raw["PriceList"];
            var prodcutArray = _productRepo.Products.ToArray();
            //Dictionary<string, decimal> priceDictionary = new Dictionary<string, decimal>();

            foreach (var item in prodcutArray)
            {
                if ((string)raw[item.barcode] != null)
                {
                    item.sellingPrice = (decimal)raw[item.barcode];
                }
            }
            _productRepo.saveContext();

            return View();
        }

        public ActionResult addProduct()
        {
            return View();
        }

        public ActionResult updatePrices()
        {
            string url = "http://localhost:35980/shop/getNewPrices?shopID=1&Date=06/12/2013";// + DateTime.Now.AddDays(-1).ToString("dd'/'mm'/'yyyy");
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            JObject raw = JObject.Parse(text);

            var productArray = _productRepo.Products.ToArray();

            foreach (var p in productArray)
            {
                p.sellingPrice = (decimal)raw[p.barcode];
                _productRepo.quickSaveProduct(p);
            }

            _productRepo.saveContext();

            return View();
        }


        [HttpPost]
        public ActionResult Add(string barcode, string currentStock, string minimumStock, string bundleUnit, string discountPercentage)
        {

            Product product = new Product();
            try
            {
                product.barcode = barcode;
                product.currentStock = Int32.Parse(currentStock);
                product.minimumStock = Int32.Parse(minimumStock);
                product.bundleUnit = Int32.Parse(bundleUnit);
                product.discountPercentage = float.Parse(discountPercentage);
            }
            catch
            {
                TempData["message"] = string.Format("Number expected in price and stock fields");
                return RedirectToAction("List");
            }
            if (product.bundleUnit < 0 || product.barcode.Length < 8 || product.currentStock < 0 || product.sellingPrice < 0 || product.sellingPrice > product.maxPrice)
            {
                //_productRepo.saveProduct(product);
                TempData["message"] = string.Format("Invalid data entered");
                return RedirectToAction("List");
            }
            try
            { 
                Product hqProduct = new Product();
                string url = "http://hqserver.azurewebsites.net/shop/getProductDetails?barcode=" + barcode;
                var request = WebRequest.Create(url);
                request.ContentType = "application/json; charset=utf-8";
                string text;
                var response = (HttpWebResponse)request.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }

                JObject raw = JObject.Parse(text);

                if ("success" == (string)raw["Status"])
                {
                    product.manufacturerID = (int)raw["Product"]["manufacturerID"];
                    product.categoryID = (int)raw["Product"]["categoryID"];
                    product.minimumStock = (int)raw["Product"]["minimumStock"];
                    product.productName = (string)raw["Product"]["productName"];
                    product.sellingPrice = (decimal)raw["Product"]["costPrice"];
                    _productRepo.saveProduct(product);
                    TempData["Result"] = "Success";
                    return View();
                }
                else
                {
                    //_productRepo.saveProduct(product);
                    TempData["Result"] = "Invalid barcode";
                    return View();
                }
            }
            catch
            {
                TempData["Result"] = "Invalid barcode";
                return View();
            }
        }


    }
}
