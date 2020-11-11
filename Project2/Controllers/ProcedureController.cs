using Microsoft.Reporting.WebForms;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project2.Controllers
{
    public class ProcedureController : Controller
    {
        Pathology_dbEntities1 db = new Pathology_dbEntities1();
        // GET: Procedure
        public ActionResult Indexx()
        {
            return View();
        }

        public ActionResult order()
        {

            List<TBL_MASTER_PROCEDURE> OrderAndCustomerList = db.TBL_MASTER_PROCEDURE.ToList();
            return View(OrderAndCustomerList);
           
        }
        public JsonResult getprocedure(string MRNO)
        {
            //List<int> li = db.TBL_R_PATIENT.Where(x => x.MR_NO.ToString().Equals("10")).Select(y => y.MR_NO).ToList();

            var lii = db.TBL_MASTER_PRO_ENTRY.Where(x => x.PRO_NAME.ToString().Equals(MRNO)).Select(x => new
            {
                name = x.PRO_NAME,
                id = x.PRO_ID,
                price = x.PRO_PRICE,
                cat = x.PRO_CAT

            }).ToList();
            return Json(lii, JsonRequestBehavior.AllowGet);
        }
            public JsonResult ProcedureENT(string searchTerm)
        {

            var pateint = db.TBL_MASTER_PRO_ENTRY.Where(x => x.PRO_NAME.Contains(searchTerm)).ToList();
            var aa = pateint.Select(x => new
            {
                id = x.PRO_NAME,
                text = x.PRO_NAME
                //,
                //price=x.PRO_PRICE
                //text =  x.FIRST_NAME 
            });
            return Json(aa, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveOrder(int mr, int cons, TBL_MPROC_DETAIL[] order)
        {
            string result = "Error! Order Is Not Complete!";
            if (mr != 0 && cons != 0)
            {
                //int cutomerId = Guid.NewGuid();
                TBL_MASTER_PROCEDURE model = new TBL_MASTER_PROCEDURE();
                model.MR = mr;
                model.MP_CONS = cons;
                model.MP_USER_ID = Convert.ToInt32(Session["user_id"]);
                model.MP_USER_ORG= Convert.ToInt32(Session["user_org"]);
                model.MP_USER_dATE = DateTime.Now;
                db.TBL_MASTER_PROCEDURE.Add(model);
                db.SaveChanges();
                int last_insert_id = model.MP_CODE;
                foreach (var item in order)
                {
                    //var orderId = Guid.NewGuid();
                    TBL_MPROC_DETAIL tmd = new TBL_MPROC_DETAIL();
                    //O.OrderId = orderId;
                    tmd.MP_CODE = last_insert_id;
                    tmd.pro_id = item.pro_id;
                    tmd.PRICE = item.PRICE;
                    db.TBL_MPROC_DETAIL.Add(tmd);
                }
                db.SaveChanges();

            }
           
            result = "Success! Order Is Complete!";
            
            return Json(result, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("reporting");

        }










        public ActionResult Entry_Proc()
        {
            Pathology_dbEntities1 db = new Pathology_dbEntities1();
            ViewBag.PANEL = new SelectList(db.TBL_R_PANEL, "PANEL_ID", "PANEL_NAME");
            ViewBag.consultants = new SelectList(db.TBL_PAY_MAST, "EMP_CODE", "EMP_NAME");
            ViewBag.gendr = new SelectList(db.TBL_Gender, "G_id", "G_name");
            return View();
        }

        public ActionResult Index()

        {

            Pathology_dbEntities1 db = new Pathology_dbEntities1();
            return View(db.TBL_R_DEPT);
        }

        public ActionResult Bank()
        {
            return View();
        }


        public JsonResult InsertCustomers(List<TBL_MPROC_DETAIL> customers)
        {
            using( Pathology_dbEntities1 db = new Pathology_dbEntities1())
            {
                TBL_MASTER_PROCEDURE mp =  new TBL_MASTER_PROCEDURE();
                //Truncate Table to delete all old records.
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [TBL_MPROC_DETAIL]");
                //Check for NULL.
                if (customers == null)
                {
                    customers = new List<TBL_MPROC_DETAIL>();
                }

                //Loop and insert records.
                foreach (TBL_MPROC_DETAIL customer in customers)
                {
                    db.TBL_MPROC_DETAIL.Add(customer);
                }
                int insertedRecords = db.SaveChanges();
                return Json(insertedRecords);
            }








        }


        public ActionResult reporting()
        {
            int sub = 36;
            string id = ".pdf";
            var lr = new LocalReport();
            String path = Path.Combine(Server.MapPath("~/Report"), "ProSlip.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;

            }
            else
            {
                return View("index");
            }
            List<Procedure_slip> att = new List<Procedure_slip>();
            using (Pathology_dbEntities1 entities = new Pathology_dbEntities1())
            {
                att = entities.Procedure_slip.Where(a => a.mp_code == sub).ToList();
            }
            ReportDataSource rds = new ReportDataSource("DataSet1", att);
            lr.DataSources.Add(rds);
            string reporttype = id;
            string minetype;
            string encoding;
            string filenameextension = id;
            string deviceinfo =
            "<DeviceInfo>" +
                "<OutputFormat>DD</OutputFormat>" +
                "<PageWidth>8.5in</PageWidth>" +
                "<PageHeight>11in</PageHeight>" +
                "<MarginTop>0.5in</MarginTop>" +
                "<MarginLeft>11in</MarginLeft>" +
                "<MarginRight>11in</MarginRight>" +
                "<MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warning;
            string[] stream;
            byte[] renderedbytes;
            renderedbytes = lr.Render("pdf", deviceinfo, out minetype, out encoding, out filenameextension, out stream, out warning);

            return File(renderedbytes, minetype);

        }
    }

}
