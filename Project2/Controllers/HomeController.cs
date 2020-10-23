using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security;

namespace Project2.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        Pathology_dbEntities1 db = new Pathology_dbEntities1();
        // GET: Home
              int USerID;

        public JsonResult TotalVisit()
        {

            var user = db.TBL_R_RECEIPT.Count();
            var json = JsonConvert.SerializeObject(user);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TotalConsultant()
        {

            var user = db.TBL_CONSULTANT.Count();
            var json = JsonConvert.SerializeObject(user);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getpatient()
        {
            return View();
        }

        public JsonResult getDatapatient(string MRNO)
        {
            //List<int> li = db.TBL_R_PATIENT.Where(x => x.MR_NO.ToString().Equals("10")).Select(y => y.MR_NO).ToList();

            var lii = db.TBL_R_PATIENT.Where(x => x.MR_NO.ToString().Equals(MRNO)).Select(x => new
            {
                name = x.FIRST_NAME,
                id = x.MR_NO,
                fname = x.F_NAME,
                adress = x.ADDRESS1,
                telno = x.TEL_NO,
                dob = x.DATEOFBIRTH,
                gn = x.GENDER,
                nic = x.NIC,
                age = x.AGE

            }).ToList();
            return Json(lii, JsonRequestBehavior.AllowGet);
        }

         

            





            
        public JsonResult gettpatient(string Prefix)
        {
            List<string> li = db.TBL_CONSULTANT.Where(x => x.CON_NAME.StartsWith(Prefix)).Select(y => y.CON_NAME).ToList();
            return Json(li, JsonRequestBehavior.AllowGet);
        }

        public ActionResult index()
        {
            return View();
        }
        public JsonResult GetConsultant(int DEP)
        {

            var cons = db.TBL_CONSULTANT.Where(x=>x.DEP_ID==DEP).Select(x => new
            {
                Name = x.CON_NAME,
                Id = x.CON_JOB_ID
            }).ToList();
            return Json(cons, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConsultantFee(int con)
        {

            var cons = db.TBl_CONS_FEE.Where(x => x.fee_cons == con).Select(x => new
            {
                Name = x.fee_name,
                fee = x.fee_Fee
            }).ToList();
            return Json(cons, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllEmployee()
        {
            
            var contacts = db.TBL_R_PATIENT.Select(x => new
            {
                Name = x.FIRST_NAME,
                Id = x.MR_NO,
                ProjectName = x.F_NAME,
                ManagerName = x.ADDRESS1,
                city = x.TEL_NO
            }).ToList();
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PatientData( string searchTerm)
        {

            var pateint = db.TBL_R_PATIENT.Where(x => x.TEL_NO.Contains(searchTerm)).ToList();
            var aa = pateint.Select(x => new
            {
                id = x.MR_NO,
                text = x.TEL_NO + "      " + x.FIRST_NAME + "       " + x.MR_NO
                  //text =  x.FIRST_NAME 
            });
            return Json(aa, JsonRequestBehavior.AllowGet);
        }


        public JsonResult TotalDep()
        {

            var user = db.TBL_R_DEPT.Count();
            var json = JsonConvert.SerializeObject(user);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult User_Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult User_Login(TBL_USER_D objUser)
        {

            var user = db.TBL_USER_D.Where(x => x.USER_ID == objUser.USER_ID && x.USER_PASSWORD == objUser.USER_PASSWORD).FirstOrDefault();
            if (user != null)
            {
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                ViewBag.Message = "Unsuccessfull!...Try Again";
                return View();
            }
        }
        public ActionResult AdminDashboard()

        {
            
            var user = db.TBL_USER_D.Where(x => x.USER_NAME == User.Identity.Name).ToList();
            return View(user);
        }

        public ActionResult Add_Departments()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add_Departments(TBL_R_DEPT objdept)
        {
            db.TBL_R_DEPT.Add(objdept);
            db.SaveChanges();
            return RedirectToAction("Departments");
        }
        public ActionResult Departments()
        {
            var dept = db.TBL_R_DEPT.ToList();
            return View(dept);
        }


        [HttpGet]
        public ActionResult Edit_Departments(int id)
        {
            var dept = db.TBL_R_DEPT.Find(id);
            return View(dept);
        }
        [HttpPost]
        public ActionResult Edit_Departments(TBL_R_DEPT objdept)
        {
            db.Entry(objdept).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Departments");
        }

        public ActionResult Delete_Departments(int id)
        {
            var dept = db.TBL_R_DEPT.Find(id);

            db.TBL_R_DEPT.Remove(dept);
            db.SaveChanges();
            return RedirectToAction("Departments");
        }



        public JsonResult indexx(List<TBL_R_PATIENT> customers)
        {
            
            {
                //Truncate Table to delete all old records.
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [TBL_R_PATIENT]");


                //Check for NULL.
                if (customers == null)
                {
                    customers = new List<TBL_R_PATIENT>();
                }

                //Loop and insert records.
                foreach (TBL_R_PATIENT customer in customers)
                {
                    db.TBL_R_PATIENT.Add(customer);
                }
                int insertedRecords = db.SaveChanges();
                return Json(insertedRecords);
            }


        }



        [HttpGet]

        public ActionResult Patient_Visit(string SearchString)
        {
           
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");
            ViewBag.consultants = new SelectList(db.TBL_CONSULTANT, "CON_JOB_ID", "CON_NAME");
            ViewBag.gendr = new SelectList(db.TBL_Gender, "G_id", "G_name");

            //var st = from c in db.TBL_R_RECEIPT select c;

            //if (!string.IsNullOrEmpty(SearchString))
            //{
            //    st = st.Where(c => c.TBL_R_PATIENT.FIRST_NAME.Contains(SearchString) || c.TBL_R_PATIENT.F_NAME.Contains(SearchString) || c.TBL_R_PATIENT.TEL_NO.Contains(SearchString));
            //    return View(st);
            //}
            return View();
  
        }
        [HttpPost]
        public ActionResult Patient_Visit(TBL_R_RECEIPT objRecept)
        {
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");
            ViewBag.consultants = new SelectList(db.TBL_CONSULTANT, "CON_JOB_ID", "CON_NAME");
            ViewBag.gendr = new SelectList(db.TBL_Gender, "G_id", "G_name");

            TBL_R_PATIENT patient = new TBL_R_PATIENT();
            patient.AGE = objRecept.TBL_R_PATIENT.AGE;
            patient.FIRST_NAME = objRecept.TBL_R_PATIENT.FIRST_NAME;
            patient.ADDRESS1 = objRecept.TBL_R_PATIENT.ADDRESS1;
            patient.TEL_NO = objRecept.TBL_R_PATIENT.TEL_NO;
            patient.EMAIL = objRecept.TBL_R_PATIENT.EMAIL;
            patient.GENDER = objRecept.TBL_R_PATIENT.GENDER;
            patient.F_NAME = objRecept.TBL_R_PATIENT.F_NAME;
            patient.PATIENTTYPE = objRecept.TBL_R_PATIENT.PATIENTTYPE;
            patient.CNIC_NO = objRecept.TBL_R_PATIENT.CNIC_NO;
            patient.DATEOFBIRTH = objRecept.TBL_R_PATIENT.DATEOFBIRTH;

            db.TBL_R_PATIENT.Add(patient);
            db.SaveChanges();

            int last_insert_id = patient.MR_NO;

            TBL_R_RECEIPT recept = new TBL_R_RECEIPT();
            recept.MR_NO = last_insert_id;
            recept.DEPT_ID = objRecept.DEPT_ID;
            recept.CONSULTANT_ID = objRecept.CONSULTANT_ID;
            recept.PAYMENT_AMOUNT = objRecept.PAYMENT_AMOUNT;
            recept.rec_datetime = System.DateTime.Now;
            recept.R_LOGIC_DATE = System.DateTime.Now;
            recept.R_LOGIC_ID = Convert.ToInt32(Session["user_id"]);
            recept.org_id = Convert.ToInt32(Session["user_org"]);
            db.TBL_R_RECEIPT.Add(recept);
            db.SaveChanges();
            int last_insert_idd = recept.R_ID;
           
            return RedirectToAction("reporting",new { id = "pdf",sub= last_insert_idd });
        }

        public ActionResult List_Patients()
        {
            var patient = db.TBL_R_RECEIPT.ToList();
            return View(patient);
        }

        [HttpPost]
        public ActionResult List_Patients(String searchTxt)
        {
            var patient = db.TBL_R_RECEIPT.ToList();
            if (searchTxt != null)
            {
                patient = db.TBL_R_RECEIPT.Where(x => x.MR_NO.ToString().Equals(searchTxt) || x.TBL_R_PATIENT.FIRST_NAME.Contains(searchTxt)||x.TBL_R_PATIENT.TEL_NO.Contains(searchTxt)).ToList();
                
            }
            return View(patient);
        }

        [HttpGet]
        public ActionResult Edit_Patient(int id)
        {
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");
            ViewBag.consultants = new SelectList(db.TBL_CONSULTANT, "CON_JOB_ID", "CON_NAME");

            var rcpt = db.TBL_R_RECEIPT.Find(id);
            return View(rcpt);
        }
        [HttpPost]
        public ActionResult Edit_Patient(TBL_R_RECEIPT objrcpt)
        {
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");
            ViewBag.consultants = new SelectList(db.TBL_CONSULTANT, "CON_JOB_ID", "CON_NAME");

            db.Entry(objrcpt).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            int last_insert_idd = objrcpt.R_ID;
            return RedirectToAction("reporting", new { id = "pdf", sub = last_insert_idd });
           
        }


        public ActionResult Add_Consultants()
        {
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");
            return View();
        }

        [HttpPost]
        public ActionResult Add_Consultants(TBL_CONSULTANT objConslt)
        {
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");

            db.TBL_CONSULTANT.Add(objConslt);
            db.SaveChanges();
            return RedirectToAction("Consultants");
        }
        public ActionResult Consultants()
        {
            var conslt = db.TBL_CONSULTANT.ToList();
            return View(conslt);
        }


        [HttpGet]
        public ActionResult Edit_Consultants(int id)
        {
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");

            var conslt = db.TBL_CONSULTANT.Find(id);
            return View(conslt);
        }
        [HttpPost]
        public ActionResult Edit_Consultants(TBL_CONSULTANT objConslt)
        {
            ViewBag.departments = new SelectList(db.TBL_R_DEPT, "DEPT_ID", "DEPT_NAME");

            db.Entry(objConslt).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Consultants");
        }

        public ActionResult Delete_Consultants(int id)
        {
            var conslt = db.TBL_CONSULTANT.Find(id);

            db.TBL_CONSULTANT.Remove(conslt);
            db.SaveChanges();
            return RedirectToAction("Consultants");
        }
        public ActionResult reporting(int sub,string id)
        {
            var lr = new LocalReport();
            String path = Path.Combine(Server.MapPath("~/Report"), "Report_Patient.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;

            }
            else
            {
                return View("index");
            }
            List<View_Visit> att = new List<View_Visit>();
            using (Pathology_dbEntities1 entities = new Pathology_dbEntities1())
            {
                att = entities.View_Visit.Where(a => a.R_ID == sub).ToList();
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
            renderedbytes = lr.Render("PDF", deviceinfo, out minetype, out encoding, out filenameextension, out stream, out warning);

            return File(renderedbytes, minetype);

        }
    }
}