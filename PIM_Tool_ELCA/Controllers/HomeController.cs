﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIM_Tool_ELCA.Controllers
{
    public class HomeController : CustomController
    {
        public ActionResult Index()
        {
            return Redirect("Project/ProjectList");
        }
        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}