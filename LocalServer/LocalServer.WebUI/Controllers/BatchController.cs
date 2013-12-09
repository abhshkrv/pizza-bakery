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
        IBatchDispatchRepository _batchDispatchRepo;
        IBatchDispatchDetailRepository _batchDispatchDetailRepo;
        public BatchController(IBatchRequestRepository brepo, IBatchRequestDetailRepository bdrepo, IProductRepository prepo, IBatchDispatchDetailRepository bdd, IBatchDispatchRepository ib)
        {
            _batchDispatchDetailRepo = bdd;
            _batchDispatchRepo = ib;
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
            int id = Int32.Parse(batchRequestId);
            BatchRequest br = _batchRequestRepo.BatchRequests.FirstOrDefault(b => b.batchRequestID == id);
            br.status = 1;
            _batchRequestRepo.saveBatchRequest(br);

            try
            {
                using (var wb = new WebClient())
                {
                    string url = "http://localhost:35980/batch/acknowledge";
                    var data = new NameValueCollection();
                    data["outlet"] = "1";
                    data["requestID"] = batchRequestId;
                    var response = wb.UploadValues(url, "POST", data);
                }
            }
            catch
            {
                TempData["Result"] = "Error : HQ Server Error";
            }

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

            BatchDispatch batchDispatch = new BatchDispatch();
            batchDispatch.status = 1;
            batchDispatch.timeStamp = DateTime.Now;

            try
            {
                _batchDispatchRepo.saveBatchDispatch(batchDispatch);
            }
            catch {
                ;
            }
            
            foreach (var item in prodcutArray)
            {
                BatchDispatchDetail bdd = new BatchDispatchDetail();
                if ((string)raw[item.barcode] != null)
                {
                    item.currentStock = (int)raw[item.barcode];
                    item.temporaryStock = item.currentStock;
                    item.afterUpdateStock = item.currentStock;
                    _productRepo.quickSaveProduct(item);
                    updates.Add(item.barcode + " : " + item.currentStock);
                    try
                    {
                        bdd.barcode = item.barcode;
                        bdd.quantity = item.currentStock;
                        bdd.batchDispatchID = batchDispatch.batchDispatchID;
                        _batchDispatchDetailRepo.quickSaveBatchDispatchDetail(bdd);
                    }
                    catch {

                        ;
                    }
                }
            }
            _batchDispatchDetailRepo.saveContext();
            _productRepo.saveContext();

            return View(updates);
        }

        public ActionResult viewDispatches()
        {
            var bd = _batchDispatchRepo.BatchDispatchs.ToList();
            return View(bd);
        }

        public ActionResult viewDispatchDetails(string batchDispatchID)
        {
            try
            {
                int id = Int32.Parse(batchDispatchID);
                var items = _batchDispatchDetailRepo.BatchDispatchDetails.Where(b => b.batchDispatchID == id).ToList();
                var result = new List<String>();
                foreach (var item in items)
                {
                    result.Add(item.barcode + " | " + item.quantity);
                }
                return View(items);
            }
            catch
            {
                return View();
            }
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
