﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIM_Tool_ELCA.Controllers
{
    public class AngularController : Controller
    {
        // GET: Angular
        public ActionResult Index()
        {
            return new FilePathResult("~/Scripts/ClientApp/index.html", "text/html");
        }
    }
}