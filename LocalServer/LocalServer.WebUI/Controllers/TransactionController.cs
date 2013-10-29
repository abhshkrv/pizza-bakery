﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocalServer.Domain;
using LocalServer.Domain.Entities;
using LocalServer.Domain.Abstract;
using LocalServer.WebUI.Infrastructure;
using LocalServer.WebUI.Models;

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
            var productDictionary = _productRepo.Products.Select(p => new { p.barcode, p.sellingPrice }).AsEnumerable().ToDictionary(kvp => kvp.barcode, kvp => kvp.sellingPrice);


            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"~/Content/TransactionData/" + fileName));
            List<string> inputList = lines.Cast<string>().ToList();
            string id = "";
            foreach (string i in inputList)
            {
                string[] tokens = i.Split(':');

                if (tokens[0] != id)
                {
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

                    _transactionRepo.quickSaveTransaction(transaction);

                    _transactionDetailRepo.quickSaveTransactionDetail(transactionDetail);

                    id = tokens[0];
                }
                else
                {
                    TransactionDetail transactionDetail = new TransactionDetail();

                    transactionDetail.transactionID = Int32.Parse(id);
                    transactionDetail.barcode = tokens[3];
                    transactionDetail.unitSold = Int32.Parse(tokens[4]);

                    String barcode = tokens[3];
                    //Product product = _productRepo.Products.FirstOrDefault(p => p.barcode.Contains(barcode));
                    transactionDetail.cost = 0; //product.sellingPrice * transactionDetail.unitSold;

                    _transactionDetailRepo.quickSaveTransactionDetail(transactionDetail);

                }
            }
            _transactionRepo.saveContext();
            _transactionDetailRepo.saveContext();
            return true;
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProcessSearch(string transactionID = null, string barcode = null, string date = null)
        {
            TransactionListViewModel viewModel = new TransactionListViewModel();
            String noResults = "No Transactions found with ";

            if (transactionID != "" && transactionID != null)
            {
                int id = Int32.Parse(transactionID);
                viewModel.Transactions = _transactionRepo.Transactions.Where(t => t.transactionID == id);
                viewModel.TransactionDetail = _transactionDetailRepo.TransactionDetails.Where(td => td.transactionID == Int32.Parse(transactionID));
                noResults += "transactionID = " + transactionID;
            }
            /* else if (barcode != "" && barcode != null)
             {
                 viewModel.TransactionDetail = _transactionDetailRepo.TransactionDetails.Where(td=>td.barcode == barcode);
                foreach(var item in viewModel.TransactionDetail)
                {
                     viewModel.Transactions = 
                }
             }*/

            if (viewModel.Transactions.Count() != 0)
                return View("SearchResults", viewModel);
            else
            {
                TempData["results"] = noResults;
                return View("Search");
            }



        }

        public ActionResult Detail(int transactionID)
        {
                //int id = Int32.Parse(transactionID);
                TransactionDetailsListViewModel viewModel = new TransactionDetailsListViewModel();
                viewModel.transaction = _transactionRepo.Transactions.First(t => t.transactionID == transactionID);
                viewModel.TransactionDetail = _transactionDetailRepo.TransactionDetails.Where(td => td.transactionID == transactionID);
                if (viewModel.TransactionDetail.Count() == 0)
                {
                    TempData["results"] = "Invalid transaction ID";
                    return View();
                }
                return View(viewModel);
        }

        public ViewResult List(int page = 1)
        {
            int PageSize = 300;
            TransactionListViewModel viewModel = new TransactionListViewModel
            {
                Transactions = _transactionRepo.Transactions
                .OrderBy(t => t.transactionID)
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

    }

}
