using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocalServer.Domain;
using LocalServer.Domain.Entities;
using LocalServer.Domain.Abstract;
using LocalServer.WebUI.Infrastructure;

namespace LocalServer.WebUI.Controllers
{
    public class TransactionController : Controller
    {
        ITransactionRepository _transactionRepo;
        ITransactionDetailRepository _transactionDetailRepo;
        IProductRepository _productRepo;

        public TransactionController(ITransactionRepository transactionRepo, ITransactionDetailRepository transactionDetailRepo, IProductRepository productRepo)
        {
            _transactionRepo = transactionRepo;
            _transactionDetailRepo = transactionDetailRepo;
            _productRepo = productRepo;
        }
        
        //
        // GET: /Transaction/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Setup(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            if (file.ContentLength > 0)
            {

                var path = Path.Combine(Server.MapPath("~/Content/TransactionData"), fileName);
                file.SaveAs(path);
                //emptydatabase();
            }
            else
            {
                return View();
            }

            if (parseFile(fileName))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        private bool parseFile(string fileName)
        {
            //emptydatabase();
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"~/Content/TransactionData/" + fileName));
            List<string> inputList = lines.Cast<string>().ToList();
            foreach (string i in inputList)
            {
                string[] tokens = i.Split(':');
                Transaction transaction = new Transaction();
                TransactionDetail transactionDetail = new TransactionDetail();
                transaction.transactionID = Int32.Parse(tokens[0]);
                transaction.cashierID = Int32.Parse(tokens[1]);
                transaction.date = DateTime.Parse(tokens[5]);

                transactionDetail.transactionID = transaction.transactionID;
                transactionDetail.barcode = tokens[3];
                transactionDetail.unitSold = Int32.Parse(tokens[4]);

                String barcode = tokens[3];
                Product product = _productRepo.Products.FirstOrDefault(p => p.barcode.Contains(barcode));
                transactionDetail.cost = 0; //product.sellingPrice * transactionDetail.unitSold;

                _transactionRepo.saveTransaction(transaction);
                _transactionDetailRepo.saveTransactionDetail(transactionDetail);
            }
            return true;
        }

    }

}
