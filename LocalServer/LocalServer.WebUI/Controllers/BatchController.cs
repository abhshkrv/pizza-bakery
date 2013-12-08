using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LocalServer.WebUI.Controllers
{
    public class BatchController : Controller
    {
        IBatchRequestRepository _batchRequestRepo;
        IProductRepository _productRepo;
        IBatchRequestDetailRepository _batchRequestDetailRepo;
        public BatchController(IBatchRequestRepository brepo, IBatchRequestDetailRepository bdrepo, IProductRepository prepo)
        {

            _productRepo = prepo;
            _batchRequestRepo = brepo;
            _batchRequestDetailRepo = bdrepo;
        }

        public ActionResult create()
        {
            return View();
        }

        public ActionResult sendRequest(string barcode, string qty, string comment)
        {
            Product product = _productRepo.Products.FirstOrDefault(p => p.barcode == barcode);
            if (product == null)
            {
                TempData["Result"] = "Error : Product Not Found";
                return RedirectToAction("Create");
            }
            else
            {
                BatchRequest batchRequest = new BatchRequest();
                BatchRequestDetail batchRequestDetail = new BatchRequestDetail();
                batchRequest.status = 0;
                batchRequest.timeStamp = DateTime.Now;
                _batchRequestRepo.saveBatchRequest(batchRequest);
                batchRequestDetail.batchRequestID = batchRequest.batchRequestID;
                batchRequestDetail.barcode = product.barcode;
                batchRequestDetail.quantity = Int32.Parse(qty);
                _batchRequestDetailRepo.saveBatchRequestDetail(batchRequestDetail);
               //send post
                try
                {
                    using (var wb = new WebClient())
                    {
                        string url = "http://localhost:35980/batch/send";
                        var data = new NameValueCollection();
                        data["shopID"] = "1";
                        data["requestID"] = batchRequest.batchRequestID.ToString();
                        data["barcode"] = barcode;
                        data["qty"] = qty;
                        data["comment"] = comment;
                        var response = wb.UploadValues(url, "POST", data);
                    }
                }
                catch {
                    TempData["Result"] = "Error : HQ Server Error";
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult ViewDetail(string batchRequestId)
        {
            int id = Int32.Parse(batchRequestId);
            var brd = _batchRequestDetailRepo.BatchRequestDetails.Where(b => b.batchRequestID== id).ToList();
            return View(brd);
        }

        public ActionResult Acknowledge(string batchRequestId)
        {
            BatchRequest br = _batchRequestRepo.BatchRequests.FirstOrDefault(b => b.batchRequestID == Int16.Parse(batchRequestId));
            br.status = 1;
            _batchRequestRepo.saveBatchRequest(br);

            return RedirectToAction("Index");
        }

        public ActionResult receiveStock()
        {
            string url = "http://newdata.blob.core.windows.net/stock/stock1.txt";
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            
            JObject raw = JObject.Parse(text);
            //JArray priceList = (JArray)raw["PriceList"];
            var prodcutArray = _productRepo.Products.ToArray();
            //Dictionary<string, decimal> priceDictionary = new Dictionary<string, decimal>();
            var updates = new List<string>();
            foreach (var item in prodcutArray)
            {
                if ((string)raw[item.barcode] != null)
                {
                    item.sellingPrice = (decimal)raw[item.barcode];
                    _productRepo.quickSaveProduct(item);
                    updates.Add(item.barcode + " : " + item.sellingPrice);
                }
            }
            _productRepo.saveContext();

            return View(updates);
        
        }

        //
        // GET: /Batch/

        public ActionResult Index()
        {
            var br = _batchRequestRepo.BatchRequests;
            return View(br);
        }

    }
}
