using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using System.Web.UI.WebControls;
using Demo20.App_Code;
using Demo20.Models;

namespace Demo20.Controllers
{
    public class GeneralController : Controller
    {
        EStudyDBEntities dBEntities = new EStudyDBEntities();
        static string[] PicAndName = new string[2];
        [NonAction]
        void CreateNewCaptcha()
        {
            RandomCodeGenerator rg= new RandomCodeGenerator();
            PicAndName = rg.GetCaptchaImgAndCode();
            ViewBag.CaptchaPicName = PicAndName[0];
        }
        public JsonResult GetNewCaptchaByAJAX()
        {
            CreateNewCaptcha();
            String s = PicAndName[0];
            return Json(s,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            List<NotificationMaster> lst = dBEntities.NotificationMasters.OrderByDescending(x => x.Notification_Id).Take(4).ToList();
            return View(lst);
        }
        public ActionResult Visionmission()
        {
            return View();
        }
        public ActionResult Benefits()
        {
            return View();
        }
        [NonAction]
        void BindCourseAndYear()
        {
            //first DDL
            string[] course = new string[] {"Diploma(CS)","Diploma(IT)","B.Sc(CS)","M.Sc(CS)","BCA","MCA","PGDCA","B.Tech","M.Tech"};
            List<SelectListItem> lstcourse = new List<SelectListItem>();
            foreach (string cr in course)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = cr;
                lstcourse.Add(sli);
            }
            ViewBag.Course = lstcourse;
            // Second DDL
            List<SelectListItem> lstyear=new List<SelectListItem>();
            for(int x=1;x<=4;x++)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = x.ToString();
                lstyear.Add(sli);
            }
            ViewBag.Yearofcourse = lstyear;
        }
        [HttpGet]
        public ActionResult NewStudent()
        {
            BindCourseAndYear();
            CreateNewCaptcha();
            return View();
        }
        [HttpPost]
        public ActionResult NewStudent(StudyMaster sm,string Pass,string ConfPass,string TxtCaptcha)
        {
            string msg=string.Empty;
            if (TxtCaptcha == PicAndName[1])
            {
                if(Pass ==ConfPass)
                {
                    //Reading and saving User Picture
                    HttpPostedFileBase myfile = Request.Files["UserPic"];
                    bool IsFileError = false;
                    string UserPicName = string.Empty;
                    if(myfile != null)
                    {
                        if(myfile.ContentLength > 0)
                        {
                            UserPicName = Path.GetRandomFileName() + myfile.FileName;
                            string ftype=UserPicName.Substring(UserPicName.LastIndexOf('.')).ToUpper();
                            if(ftype ==".JPG" || ftype==".JPEG" || ftype==".PNG")
                            {
                                string folderPath = Server.MapPath("/contant/UserPics");
                                if(!Directory.Exists(folderPath))
                                Directory.CreateDirectory(folderPath);
                                myfile.SaveAs(folderPath +"/"+UserPicName);
                                sm.UserPic=UserPicName;
                            }
                            else
                            {
                                msg = "Invalid picture. Please choose correct image file";
                                IsFileError=true;
                            }
                        }
                    }
                    if(IsFileError==false)
                    {
                        sm.Registered_On = DateTime.Now;
                        dBEntities.StudyMasters.Add(sm);  //Adding record in Reg table
                        //Setting login details
                        LoginMaster lg = new LoginMaster();
                        lg.EmailId = sm.EmailId;
                        Cryptography cg= new Cryptography();
                        lg.Pass = cg.EncryptMyPassword(Pass);
                        lg.Utype = "STUDENT";
                        dBEntities.LoginMasters.Add(lg);
                        dBEntities.SaveChanges();
                        msg = "Congratulation! you are registration successfully.";
                        //sending Email.....
                        EmailSender em=new EmailSender();
                        string msgs = "Hii" + sm.Name + "\nCongratulation you are registered Successfully. We welcome you E-Study Corner";
                        em.SendMyEmail(sm.EmailId, "Study corner",msgs);
                    }
                }
                else
                {
                    msg = "Password and confirm password must be same.";
                }
            }
            else
            {
                msg = "Invalide captcha code. Please try again.";
            }
            ViewBag.Message = msg;
            BindCourseAndYear();
            CreateNewCaptcha();
            return View();
        }
        public ActionResult Contactus()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginMaster lg)
        {
            Cryptography cg= new Cryptography();
            lg.Pass = cg.EncryptMyPassword(lg.Pass);
            LoginMaster lgdb = dBEntities.LoginMasters.SingleOrDefault(tbl => tbl.EmailId == lg.EmailId && tbl.Pass == lg.Pass && tbl.Utype == lg.Utype);
            if(lgdb!=null)
            {
                if(lgdb.Utype=="STUDENT")
                {
                    Session["userid"] = lgdb.EmailId;
                    return RedirectToAction("Welcome","User");
                }
                else if(lgdb.Utype=="ADMIN")
                {
                    Session["adminid"] = lgdb.EmailId;
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            ViewBag.Message = "Invalid userid or password.";
            return View();
        }
        public JsonResult SaveEnquiryByAJAX(EnquiryMaster em)
        {
            string msg=string.Empty;
            try
            {
                em.Enquiry_DT = DateTime.Now;
                dBEntities.EnquiryMasters.Add(em);
                dBEntities.SaveChanges();
                msg = "SUCCESS";
                //sending Email.....
                EmailSender es = new EmailSender();
                string msgs = "Hii " + em.Name + ",\n Thanks for your enquiry. We will contact you soon\n From-\nTeamEstudy Corner";
                es.SendMyEmail(em.Email, "Thanks from E-Study corner", msgs);
            }
            catch
            {
                msg = "FAIL";
            }
            return Json(msg,JsonRequestBehavior.AllowGet);
        }
        public ActionResult myintro()
        {
            return View();
        }
    }
}