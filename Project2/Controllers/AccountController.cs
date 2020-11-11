using Project2.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Project2.Controllers
{
    public class AccountController : Controller
    {

        Pathology_dbEntities1 db = new Pathology_dbEntities1();
        public ActionResult Index()
        {
            return View(); // Return
        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Loginn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TBL_USER_D user)
        {

            

            var usr = db.TBL_USER_D.SingleOrDefault(x => x.USER_NAME == user.USER_NAME && x.USER_PASSWORD == user.USER_PASSWORD);

            if (usr != null)
            {
                Session["user_id"] = usr.USER_ID;
                Session["user_n"] = usr.USER_NAME;
                Session["user_org"] = usr.ORG;
                Session["user_rorgname"] = usr.TBL_ORGINATION.ORGNAME;

                if (usr.MAG_ACT == 1)
                {
                    FormsAuthentication.SetAuthCookie(usr.USER_NAME, false);
                    return RedirectToAction("AdminDashboard", "Home", new { user = user.USER_NAME });
                }
                else if (usr.MAG_ACT == 2)
                {
                    FormsAuthentication.SetAuthCookie(usr.USER_NAME, false);
                    return RedirectToAction("StudentDashBaord", "Home");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(usr.USER_NAME, false);
                    return RedirectToAction("staffDashboard", "Home", new { id = usr.USER_NAME });
                }

            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }
        }

        

        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

    }
}