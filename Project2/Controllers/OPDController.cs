using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project2.Models;

namespace Project2.Controllers
{
    public class OPDController : Controller
    {
        // GET: OPD
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (Pathology_dbEntities1 db = new Pathology_dbEntities1())
            {
                List<View_Visit> empList = db.View_Visit.ToList<View_Visit>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TechData()
        {
            using (Pathology_dbEntities1 db = new Pathology_dbEntities1())
            {
                List<TBL_MPROC_DETAIL> empList = db.TBL_MPROC_DETAIL.ToList<TBL_MPROC_DETAIL>();
                //return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
                return View(empList);
            }
        }
    }
}