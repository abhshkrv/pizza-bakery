﻿using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using Newtonsoft.Json.Linq;
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
                product.maxPrice = (float)p["maxPrice"];
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

            foreach (var p in productArray)
            {
                Manufacturer manufacturer = new Manufacturer();
                manufacturer.manufacturerID = (int)p["manufactuerID"];

                manufacturer.manufacturerName = (string)p["manufacturerName"];


               _manufacturerRepo.quickSaveManufacturer(manufacturer);
            }
            _manufacturerRepo.saveContext();

            return View();
        }


    }
}
