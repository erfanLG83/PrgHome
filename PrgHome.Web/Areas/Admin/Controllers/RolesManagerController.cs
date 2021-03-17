using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PrgHome.Web.Areas.Admin.Controllers
{
    public class RolesManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}