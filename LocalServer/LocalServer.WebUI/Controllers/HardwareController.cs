using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using LocalServer.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalServer.WebUI.Controllers
{
    public class HardwareController : Controller
    {
        ICashRegisterRepository _cashRegisterRepo;
        IPriceDisplayRepository _priceDisplayRepo;

        public HardwareController(ICashRegisterRepository cashRepo, IPriceDisplayRepository pdRepo)
        {
            _cashRegisterRepo = cashRepo;
            _priceDisplayRepo = pdRepo;
        }

        //
        // GET: /Hardware/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult listCashRegisters()
        {
            CashRegisterListViewModel viewModel = new CashRegisterListViewModel
            {
                CashRegisters = _cashRegisterRepo.CashRegisters
                .OrderBy(c => c.cashRegisterID)

            };

            return View(viewModel);
        }

        public ActionResult listPriceDisplays()
        {
            PriceDisplayListViewModel viewModel = new PriceDisplayListViewModel
            {
                PriceDisplays = _priceDisplayRepo.PriceDisplays
                .OrderBy(c => c.priceDisplayID)

            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(int priceDisplayID = 0, string barcode = null)
        {
            PriceDisplay priceDisplay = new PriceDisplay();

            if (ModelState.IsValid)
            {

                priceDisplay.priceDisplayID = priceDisplayID;
                priceDisplay.barcode = barcode;
                priceDisplay.status = 1;
                _priceDisplayRepo.addPriceDisplay(priceDisplay);
                TempData["message"] = string.Format("{0} has been saved", priceDisplay.priceDisplayID);
                return RedirectToAction("listPriceDisplays");
            }
            else
            {
                // there is something wrong with the data values
                return View(priceDisplay);
            }


        }

        public ViewResult Edit(int priceDisplayID)
        {
            PriceDisplay priceDisplay = _priceDisplayRepo.PriceDisplays.FirstOrDefault(p => p.priceDisplayID==priceDisplayID);
            return View(priceDisplay);
        }
        [HttpPost]
        public ActionResult Edit(PriceDisplay priceDisplay)
        {
            if (ModelState.IsValid)
            {
                _priceDisplayRepo.editPriceDisplay(priceDisplay);
                TempData["message"] = string.Format("{0} has been saved", priceDisplay.priceDisplayID);
                return RedirectToAction("listPriceDisplays");
            }
            else
            {
                // there is something wrong with the data values
                return View(priceDisplay);
            }
        }

       public ViewResult AddCashRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCashRegister(int cashRegisterID=0)
        {
            CashRegister cashRegister = new CashRegister();

            if (ModelState.IsValid)
            {

                cashRegister.cashRegisterID = cashRegisterID;
                cashRegister.status = 1;
                _cashRegisterRepo.saveCashRegister(cashRegister);
                TempData["message"] = string.Format("{0} has been saved", cashRegister.cashRegisterID);
                return RedirectToAction("listCashRegisters");
            }
            else
            {
                // there is something wrong with the data values
                return View(cashRegister);
            }


        }


    }
}
