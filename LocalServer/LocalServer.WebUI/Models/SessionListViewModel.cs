using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LocalServer.Domain.Entities;

namespace LocalServer.WebUI.Models
{
    public class SessionListViewModel
    {
        public IEnumerable<CRSession> Sessions { get; set; }
    }
}