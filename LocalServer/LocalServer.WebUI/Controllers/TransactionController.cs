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
using LocalServer.WebUI.Models;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;

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
            var productDictionary = _productRepo.Products.ToDictionary(p => p.barcode);


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
                    //Product product = _productRepo.Products.FirstOrDefault(p => p.barcode.Contains(barcode));
                    transactionDetail.cost = productDictionary[transactionDetail.barcode].sellingPrice* transactionDetail.unitSold;

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
                    transactionDetail.cost = productDictionary[transactionDetail.barcode].sellingPrice * transactionDetail.unitSold;

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
            int PageSize = 5000;
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
                    TotalItems = _transactionRepo.Transactions.Count()
                }
            };

            return View(viewModel);
        }

        public ViewResult AddTransaction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTransaction(string transactionString = null)
        {
            if (ModelState.IsValid)
            {
                string[] tokens = transactionString.Split(':');
                Transaction transaction = new Transaction();
                TransactionDetail transactionDetail;

                transaction.cashierID = Int32.Parse(tokens[0]);
                transaction.date = DateTime.Today.Date;
                transaction.transactionID = _transactionRepo.Transactions.Max(t => t.transactionID) + 1;

                int transactionID = getTransactionID(transaction);
                

                string[] products = tokens[1].Split(';');
                int i = 0;
                while (products.Length>i)
                {
                    transactionDetail = new TransactionDetail();
                    transactionDetail.transactionID = transactionID;
                    string[] item = products[i].Split('#');
                    transactionDetail.barcode = item[0];
                    transactionDetail.unitSold = Int32.Parse(item[1]);
                    i++;
                    Product p = _productRepo.Products.First(pd => pd.barcode == transactionDetail.barcode);
                    transactionDetail.cost = p.sellingPrice * transactionDetail.unitSold;
                    p.currentStock -= transactionDetail.unitSold;
                    if (p.currentStock >= 0)
                    {
                        _transactionDetailRepo.quickSaveTransactionDetail(transactionDetail);

                        _productRepo.saveProduct(p);
                    }

                }

                _transactionDetailRepo.saveContext();
                var user = new UserTransaction
                {
                    TransactionDetail=_transactionDetailRepo.TransactionDetails,
                    Products=_productRepo.Products,
                    firstName = "Saran",
                    email = "ksk.3393@gmail.com"

                };
                string subject = "Transaction details for transaction ID:"+transactionID+" dated :"+transaction.date;
                new MailController().SampleEmail(user,subject).DeliverAsync();

                return RedirectToAction("List");
            }
            else
            {
                // there is something wrong with the data values
                return View();
            }
        }

       /* public string SendEmail()
        {
            var user = new User
            {
                firstName = "Saran",
                email = "ksk.3393@gmail.com"
            };
            new MailController().SampleEmail(user).DeliverAsync();
            return "SUCCESS";

        } */

        private int getTransactionID(Transaction transaction)
        {
            _transactionRepo.saveTransaction(transaction);
            return transaction.transactionID;
        }

        public class SellingDetails
        {
            
            public int quantity { get; set; }
            public float unitPrice { get; set; }
        }

        public ContentResult sendSummary(string date)
        {
            DateTime inDate = new DateTime();
            try
            {
                inDate = DateTime.Parse(date);
            }
            catch
            {

                return new ContentResult() { Content = "Invalid Date" };
            }

            Dictionary<string, SellingDetails> d = new Dictionary<string, SellingDetails>();

            var transactions1 = _transactionRepo.Transactions.Where(t => t.date.Day ==inDate.Day&&t.date.Month == inDate.Month&&t.date.Year==inDate.Year);
            var transactions = transactions1.ToList();
            var transactionDetails = _transactionDetailRepo.TransactionDetails.ToList();

            var result = from td in transactionDetails
                         join t in transactions on td.transactionID equals t.transactionID
                         select new { barcode = td.barcode, qty = td.unitSold, price = td.cost };


            foreach (var item in result)
            {
                if (d.ContainsKey(item.barcode))
                {
                    var detail = d[item.barcode];
                    detail.quantity += item.qty;
                }
                else
                {
                    SellingDetails sd = new SellingDetails();
                    sd.quantity = item.qty;
                    sd.unitPrice = item.price;
                    d.Add(item.barcode, sd);
                }
            }

            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("Date", inDate.Date.ToShortDateString());
            output.Add("OutletID", "1");
            output.Add("TransactionDetails", d.ToList());
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
           

            sendPost(serializer.Serialize(output));

            return new ContentResult()
           {
               Content = serializer.Serialize(output),
               ContentType = "application/json",
           };

        }

        public string deleteTransaction(string transactionID)
        {
            int id = Int32.Parse(transactionID);
            Transaction t = _transactionRepo.Transactions.First(t1 => t1.transactionID == id);
            _transactionRepo.deleteTransaction(t);
            return "Success";
        }

        private string sendPost(string content)
        {
            HttpWebRequest httpWReq =
        (HttpWebRequest)WebRequest.Create("http://pizza-hq.azurewebsites.net/shop/uploadtransactions");

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



    }


}
