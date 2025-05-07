using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo20.Models;
using System.IO;
using Demo20.App_Code;
using System.Web.Security.AntiXss;

namespace Demo20.Controllers
{
    [AuthorizeUser]
    public class UserController : Controller
    {
        // GET: User
        EStudyDBEntities db =new EStudyDBEntities();
        static string uid;
        [NonAction]
        void SetUserPicAndName()
        {
            if (Session["userid"]!=null)
            {
                uid = Session["userid"].ToString();
                StudyMaster um = db.StudyMasters.Find(uid);
                string picname = "";
                if(um.UserPic==null)
                {
                    if(um.Gender.ToUpper().Trim()=="MALE")
                    {
                        picname = "/Contant/images/male.png";
                    }
                    else
                    {
                        picname = "/Contant/images/female.png";
                    }
                }
                else
                {
                    picname = "/Contant/UserPics/" + um.UserPic; 
                }

                ViewBag.Picture = picname;
                ViewBag.Name = um.Name;
            }
        }
        [NonAction]
        void BindCourseAndYear(string mycourse ,int myyear)
        {
            //first DDL
            string[] course = new string[] { "Diploma(CS)", "Diploma(IT)", "B.Sc(CS)", "M.Sc(CS)", "BCA", "MCA", "PGDCA", "B.Tech", "M.Tech" };
            List<SelectListItem> lstcourse = new List<SelectListItem>();
            foreach (string cr in course)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = cr;
                if (cr==mycourse)
                    sli.Selected = true;
                lstcourse.Add(sli);
            }
            ViewBag.Course = lstcourse;
            // Second DDL
            List<SelectListItem> lstyear = new List<SelectListItem>();
            for (int x = 1; x <= 4; x++)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = x.ToString();
                if(x==myyear)
                    sli.Selected = true;
                lstyear.Add(sli);
            }
            ViewBag.Yearofcourse = lstyear;
        }
        public ActionResult Welcome()
        {
            SetUserPicAndName();
            return View();
        }
        public ActionResult ViewAssignment()
        {
            SetUserPicAndName();
            List<AdminAssignmentMaster> am = db.AdminAssignmentMasters.OrderByDescending(x => x.Upload_Id).ToList();
            return View(am);
        }
        public FileResult Download(string fname)
        {
            string Pathfile = "/Contant/AdminUpload/" + fname;
            return File(Pathfile, "application/force-download", Path.GetFileName(Pathfile));
        }
       public FileResult DownloadTasks1(string fname)
        {
            string Pathfile = "/Contant/UploadStudyMaterial/" + fname;
            return File(Pathfile,"application/force-download",Path.GetFileName(Pathfile));
        }
         public ActionResult DownloadStudyMaterial()
        {
            List<UploadStudyMaterial> sm=db.UploadStudyMaterials.OrderByDescending(x=>x.Title).ToList();
            SetUserPicAndName();
            return View(sm);
        }
        [HttpGet]
        public ActionResult UploadAssignment()
        {
            SetUserPicAndName();
            return View();
        }
        [HttpPost]
        public ActionResult UploadAssignment(UploadAssignmentMaster uam)
        {
            SetUserPicAndName();
            string msg=string.Empty;
            try
            {
                //To upload file in server
                string fpath = Server.MapPath("/contant/UploadAssignment");
                HttpPostedFileBase myfile = Request.Files["Userfile"];
                if(myfile!=null)
                {
                    if (myfile.ContentLength > 0)
                    {
                        string fname = Path.GetRandomFileName() + myfile.FileName;
                        string filetype = fname.Substring(fname.LastIndexOf('.')).ToUpper();
                        if (filetype == ".DOC" || filetype==".PDF" || filetype == ".ZIP" || filetype == ".TXT")
                        {
                            if(!Directory.Exists(fpath))
                                Directory.CreateDirectory(fpath);
                            myfile.SaveAs(fpath+"/"+fname);
                            //save in database
                            uam.Uploaded_By = uid;    //session
                            uam.FileName = fname;   //file name
                            uam.Upload_DT =DateTime.Now;
                            db.UploadAssignmentMasters.Add(uam);
                            db.SaveChanges();
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
            ViewBag.Message = msg;
            SetUserPicAndName();
            return View();
        }
        public ActionResult Feedback()
        {
            SetUserPicAndName();
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(FeedbackMaster fm)
        {
            string msg = "";
            try
            {
                fm.UserId = Session["userid"].ToString();
                fm.Feedback_DT = DateTime.Now;
                db.FeedbackMasters.Add(fm);
                db.SaveChanges();
                msg = "Feedback uploaded successfully"; 
            }
            catch
            {
                msg = "Sorry! feedback is not uploaded";
            }
            ViewBag.Msg = msg;
            SetUserPicAndName();
            return View();
        }
        public ActionResult MyProfile()
        {
            SetUserPicAndName();
            //getting details of original database
            StudyMaster sm=db.StudyMasters.Find(uid);
            BindCourseAndYear(sm.Course,(int)sm.Yearofcourse);
            return View(sm);
        }
        [HttpPost]
        public ActionResult MyProfile(StudyMaster sm)
        {
            string msg = "";
            StudyMaster smdb = db.StudyMasters.Find(uid);
            //Reading and saving User File
            HttpPostedFileBase myfile = Request.Files["UserPic"];
            bool IsFileError = false;
            string UserPicName = string.Empty;
            if (myfile != null)
            {
                if (myfile.ContentLength > 0)
                {
                    UserPicName = Path.GetRandomFileName() + myfile.FileName;
                    string ftype = UserPicName.Substring(UserPicName.LastIndexOf('.')).ToUpper();
                    if (ftype == ".JPG" || ftype == ".JPEG" || ftype == ".PNG" || ftype == ".TXT")
                    {
                        string folderPath = Server.MapPath("/contant/UserPics");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);
                        myfile.SaveAs(folderPath + "/" + UserPicName);
                        smdb.UserPic = UserPicName;
                    }
                    else
                    {
                        msg = "Invalid picture. Please choose correct image file.";
                        IsFileError = true;
                    }
                }
            }
            if (IsFileError == false)
            {
                //Arranging values to save the data of MyProfile into User Master
                try
                {
                   smdb.Name=sm.Name;
                    smdb.DOB=sm.DOB;
                    smdb.Gender=sm.Gender;
                    smdb.CollegeName=sm.CollegeName;
                    smdb.Course=sm.Course;
                    smdb.Yearofcourse=sm.Yearofcourse;
                    smdb.MobileNo=sm.MobileNo;
                    smdb.Address=sm.Address;
                    db.Entry(smdb);
                    db.SaveChanges();
                    msg = "Profile update successfully.";
                }
                catch
                {
                    msg = "Sorrry! your Profile is not update.";
                }
            }
            ViewBag.Message = msg;
            SetUserPicAndName();
            StudyMaster u = db.StudyMasters.Find(uid);
            BindCourseAndYear(u.Course, (int)u.Yearofcourse);
            return View(sm);
        }
        public ActionResult ChangePassword()
        {
            SetUserPicAndName();
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string Pass,string ConfPass,string NewPass)
        {
            string msg = "";
            if(ConfPass==NewPass)
            {
                string uid = Session["userid"].ToString();
                Cryptography cg = new Cryptography();
                Pass=cg.EncryptMyPassword(Pass);
                NewPass=cg.EncryptMyPassword(NewPass);
                LoginMaster l=db.LoginMasters.SingleOrDefault(x => x.EmailId == uid && x.Pass == Pass);
                if(l!=null)
                {
                   l.Pass=NewPass;
                    db.Entry(l);
                    db.SaveChanges();
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
            SetUserPicAndName();
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.Remove("userid");

            return View();
        }
    }
}