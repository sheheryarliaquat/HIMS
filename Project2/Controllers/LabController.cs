using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project2.Models;

namespace Project2.Controllers
{
    public class LabController : Controller
    {
        // GET: Lab
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DashBoard()
        {
            return View();
        }


        public ActionResult WORKLOAD()
        {
            return View();
        }
        public ActionResult GetLOAD()
        {
            using (Pathology_dbEntities1 db = new Pathology_dbEntities1())
            {
                List<VIEW_LAB> empList = db.VIEW_LAB.ToList<VIEW_LAB>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}