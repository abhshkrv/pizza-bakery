using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LocalServer.WebUI.Controllers
{
    public class SessionController : Controller
    {
        ISessionRepository _sessionRepo;
        IEmployeeRepository _employeeRepo;

        public SessionController(ISessionRepository srepo, IEmployeeRepository erepo)
        {
            _sessionRepo = srepo;
            _employeeRepo = erepo;
        }

        [HttpPost]
        public ContentResult Login(string username, string password, string cashRegister)
        {
            Employee employee = _employeeRepo.Employees.First(e => e.userID == username);
            Dictionary<string, object> output = new Dictionary<string, object>();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            if (employee.password != password)
            {
                output.Add("Status", "Fail");

                return new ContentResult()
                {
                    Content = serializer.Serialize(output),
                    ContentType = "application/json",
                };
            }
            CRSession session = new CRSession();
            session.cashRegister = cashRegister;
            session.userID = username;
            
            session.startTime = DateTime.Now;
            _sessionRepo.saveSession(session);

            output.Add("Status", "Success");

            return new ContentResult()
            {
                Content = serializer.Serialize(output),
                ContentType = "application/json",
            };

        }

        [HttpPost]
        public ContentResult Logout(string username, string cashRegister)
        {
            Employee employee = _employeeRepo.Employees.First(e => e.userID == username);
            Dictionary<string, object> output = new Dictionary<string, object>();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            CRSession session = _sessionRepo.Sessions.Last(s => s.userID == username && s.cashRegister == cashRegister);

            session.endTime = DateTime.Now;
            _sessionRepo.saveSession(session);

            output.Add("Status", "Success");

            return new ContentResult()
            {
                Content = serializer.Serialize(output),
                ContentType = "application/json",
            };

        }

        public ContentResult employees()
        {
            Dictionary<string, object> output = new Dictionary<string, object>();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            
            output.Add("Employees", _employeeRepo.Employees.ToList());

            return new ContentResult()
            {
                Content = serializer.Serialize(output),
                ContentType = "application/json",
            };

        }



        //
        // GET: /Session/

        public ActionResult Index()
        {
            return View();
        }


    }
}
