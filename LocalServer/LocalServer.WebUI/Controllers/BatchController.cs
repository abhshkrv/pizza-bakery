using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult sendRequest(string barcode, string qty)
        {
            Product product = _productRepo.Products.FirstOrDefault(p => p.barcode == barcode);
            if (product == null)
            {
                ViewBag["Result"] = "Error";
                return View();
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


                

                return View();
            }
        }
        //
        // GET: /Batch/

        public ActionResult Index()
        {
            return View();
        }

    }
}
