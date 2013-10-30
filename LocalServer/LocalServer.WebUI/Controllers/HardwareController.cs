using LocalServer.Domain.Abstract;
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

    }
}
