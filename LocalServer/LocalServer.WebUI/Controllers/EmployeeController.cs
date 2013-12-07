using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalServer.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        IEmployeeRepository _employeeRepo;

        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string hashID = null, string role = null, string userID = null,
                                   string password = null)
        {
            Employee employee = new Employee();

            if (ModelState.IsValid)
            {

                employee.hashID = hashID;
                employee.role = role;
                employee.userID = userID;
                employee.password = password;
                _employeeRepo.saveEmployee(employee);
                 TempData["message"] = string.Format("{0} has been saved", employee.userID);
                return RedirectToAction("../Home/Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(employee);
            }


        }

    }
}
