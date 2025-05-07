using Demo20.App_Code;
using Demo20.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Demo20.Controllers
{
    [AuthorizeAdmin]
    public class AdminController : Controller
    {
        // GET: Admin
        EStudyDBEntities dBEntities = new EStudyDBEntities();
        public ActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StudentManagement()
        {
            List<StudyMaster>lst=dBEntities.StudyMasters.OrderBy(x=>x.Registered_On.ToString()).ToList();
            return View(lst);
        }
        [HttpPost]
        public ActionResult StudentManagement( string SearchValue)
        {
            List<StudyMaster> lst = dBEntities.StudyMasters.Where(x => x.Name.Contains(SearchValue) || x.EmailId.Contains(SearchValue) || x.DOB.Contains(SearchValue)).ToList();
            return View(lst);
        }
        public ActionResult UploadAssignment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadAssignment(AdminAssignmentMaster am)
        {
            string msg = "";
             try
            {
                //To upload file in server
                string fpath = Server.MapPath("/Contant/AdminUpload");
                HttpPostedFileBase myfile = Request.Files["Userfile"];   //file Name
                if (myfile != null)
                {
                    if (myfile.ContentLength > 0)
                    {
                        string fname = Path.GetRandomFileName() + myfile.FileName;
                        string filetype = fname.Substring(fname.LastIndexOf('.')).ToUpper();
                        if (filetype == ".DOC" || filetype == ".PDF" || filetype == ".ZIP" || filetype == ".TXT" || filetype == ".PNG" || filetype == ".JPG")
                        {
                            if (!Directory.Exists(fpath))
                                Directory.CreateDirectory(fpath);
                            myfile.SaveAs(fpath + "/" + fname);
                            //save in database
                            am.Upload_DT = DateTime.Now;
                            am.FileName = fname;   //file name
                            dBEntities.AdminAssignmentMasters.Add(am);
                            dBEntities.SaveChanges();
                            msg = "Your Assignment submitted Successfully.";
                        }
                        else
                        {
                            msg = "Invalid file type.Please choose a valid file.";
                        }
                    }
                    else
                    {
                        msg = "Please upload a Assignment in Document.";
                    }
                }
                else
                {
                    msg = "Please upload assignment.";
                }
            }
            catch
            {
                msg = "Sorry! due to some technical issue; we are unable to upload your post.";
            }
            ViewBag.Message = msg;
            return View();
        }
        public ActionResult StudyMaterials()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StudyMaterials(UploadStudyMaterial sm)
        {
            string msg = string.Empty;
            try
            {
                //To upload file in server
                string fpath = Server.MapPath("/Contant/UploadStudyMaterial");
                HttpPostedFileBase myfile = Request.Files["FileName"];
                if (myfile != null)
                {
                    if (myfile.ContentLength > 0)
                    {
                        string fname = Path.GetRandomFileName() + myfile.FileName;
                        string filetype = fname.Substring(fname.LastIndexOf('.')).ToUpper();
                        if (filetype == ".DOC" || filetype == ".PDF" || filetype == ".ZIP" || filetype == ".TXT" || filetype == ".JPG" || filetype == ".PNG")
                        {
                            if (!Directory.Exists(fpath))
                                Directory.CreateDirectory(fpath);
                            myfile.SaveAs(fpath + "/" + fname);
                            //save in database
                            
                            sm.FileName = fname;   //file name
                            sm.Upload_DT = DateTime.Now;
                            dBEntities.UploadStudyMaterials.Add(sm);
                            dBEntities.SaveChanges();
                            msg = "Your post submitted Successfully.";
                        }
                        else
                        {
                            msg = "Invalid file type.Please choose a valid file.";
                        }
                    }
                    else
                    {
                        msg = "Please upload a Assignment in Document.";
                    }
                }
                else
                {
                    msg = "Please upload a file.";
                }
            }
            catch
            {
                msg = "Sorry! due to some technical issue; we are unable to upload your post.";
            }
            ViewBag.Message=msg;
            return View();
        }
        public ActionResult DownloadTasks()
        {
            List<UploadAssignmentMaster> uam = dBEntities.UploadAssignmentMasters.OrderByDescending(x=>x.Upload_Id).ToList();
            return View(uam);
        }
        public ActionResult NotificationManagement()
        {
            List<NotificationMaster> lst=dBEntities.NotificationMasters.OrderByDescending(x=> x.Noti_Dt).ToList();
            return View(lst);
        }
        [HttpPost]
        public ActionResult NotificationManagement(NotificationMaster nm)
        {
            string msg = "";
            try
            {
                nm.Noti_Dt=DateTime.Now.ToString();
                dBEntities.NotificationMasters.Add(nm);
                dBEntities.SaveChanges();
                msg = "Notification Added sucessfully.";
            }
            catch
            {
                msg = "Sorry! unable to add notification.";
            }
            ViewBag.Message = msg;
            List<NotificationMaster> lst = dBEntities.NotificationMasters.OrderByDescending(x => x.Noti_Dt).ToList();
            return View(lst);
        }
        public JsonResult DeleteNoti(int NID)
        {
            string msg = "";
            try
            {
                NotificationMaster nm = dBEntities.NotificationMasters.Find(NID);
                dBEntities.NotificationMasters.Remove(nm);
                dBEntities.SaveChanges();
                msg = "SUCCESS";
            }
            catch
            {
                msg = "FAIL";
            }
            return Json(msg,JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnquiryManagement()
        {
            List<EnquiryMaster> em =dBEntities.EnquiryMasters.OrderByDescending(x=>x.Enquiry_Id.ToString()).ToList();
            return View(em);
        }
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(string SendTo,string Subject,string Message)
        {
            string msg = "";
            EmailSender em = new EmailSender();
            try
            {
                bool res=em.SendMyEmail(SendTo, Subject, Message);
                if(res==true)
                {
                    msg = "Email Send Successfully.";
                }
                else
                {
                    msg = "Sorry! unable to send Email.";
                }
            }
            catch
            {
                msg = "Sorry! We can pass it.";
            }
            ViewBag.Message=msg;
            return View();
        }
        public ActionResult AssignmentManagement()
        {
            List<UploadAssignmentMaster> uam = dBEntities.UploadAssignmentMasters.OrderByDescending(x => x.Upload_Id).ToList();
            return View(uam);
        }
        public ActionResult FeedbackManagement()
        {
            List<FeedbackMaster> list = dBEntities.FeedbackMasters.OrderByDescending(x=>x.Feedback_Id.ToString()).ToList();
            return View(list);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string Pass, string ConfPass, string NewPass)
        {
            string msg = "";
            if (ConfPass == NewPass)
            {
                string uid = Session["adminid"].ToString();
                Cryptography cg = new Cryptography();
                Pass = cg.EncryptMyPassword(Pass);
                NewPass = cg.EncryptMyPassword(NewPass);
                LoginMaster l = dBEntities.LoginMasters.SingleOrDefault(x => x.EmailId == uid && x.Pass == Pass);
                if (l != null)
                {
                    l.Pass = NewPass;
                    dBEntities.Entry(l);
                    dBEntities.SaveChanges();
                    msg = "Password changed Successfully";
                }
                else
                {
                    msg = "Invalid userid and password";
                }
            }
            else
            {
                msg = "Password and confirm password must be same";
            }
            ViewBag.Message = msg;
            return View();
        }
        public ActionResult RemoveUser(string uid)
        {
            string msg = "";
            try
            {
                StudyMaster um = dBEntities.StudyMasters.Find(uid);
                dBEntities.StudyMasters.Remove(um);
                dBEntities.SaveChanges();
                msg = "User Account delete Successfully.";
            }
            catch
            {
                msg = "Sorry! unable to delete the Account";
            }
            TempData["Message"]=msg;
            return RedirectToAction("StudentManagement", "Admin");
        }
        public FileResult DownloadTasks1(string fname) 
        {
            string Pathfile = "/Contant/UploadAssignment/" + fname;
            return File(Pathfile,"application/force-download",Path.GetFileName(Pathfile));
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.Remove("adminid");
            return View();
        }
    }
}