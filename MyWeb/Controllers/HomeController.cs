using MyWeb.Models.Domain;
using MyWeb.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using MyWeb.Models.Utility;
using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;

namespace MyWeb.Controllers
{
    public class HomeController : Controller
    {
        Rep_Subject RepSubject = new Rep_Subject();
        Rep_Comments RepComment = new Rep_Comments();
        DB_MyWebEntities db = new DB_MyWebEntities();
        Recovery RecEmail = new Recovery();
        //
        // GET: /Home/
        public ActionResult Index(int? page)
        {
            var qSetting = (from a in db.tbl_Setting
                            select a).SingleOrDefault();
            var q = RepSubject.GetSubject();
            var pageNumber = page ?? 1;
            var onePageOfNews = q.ToPagedList(pageNumber, qSetting.CountSubjectInPage);
            ViewBag.OnePageOfProducts = onePageOfNews;
            return View(onePageOfNews.ToList());
            //var q = RepSubject.GetSubject();
            //var pageNumber = page ?? 1;
            //var onePageOfSubject = q.ToPagedList(pageNumber, 10);
            //ViewBag.OnePageOfProducts = onePageOfSubject;
            //return View(onePageOfSubject.ToList());
            //return View(q);
        }


        public ActionResult Subject(int ID)
        {
            //paeen baray tedaad bazdid ast
            RepSubject.SetVisit(ID);
            //baray enteghal be matn kamel matlab estefade mishe , vaghti ke click mikonim ro matlab
            ////////vaghti MetaDataSubject sakhte shod view ra as haminja misazim;
            // view bayad ham name hamin tabe Subject bashad va parshial nabashad va model az deatail sakhte beshe
            return View(RepSubject.GetDeatilsSubject(ID));
        }


        public ActionResult SubjectLike(int id)
        {
            int like = RepSubject.Like(id);
            return Json(like);
        }

        public ActionResult SubjectDisLike(int id)
        {
            int dislike = RepSubject.DisLike(id);
            return Json(dislike);
        }


        public ActionResult CommentLike(int id)
        {
            int Like = RepComment.Like(id);
            return Json(Like);
        }
        public ActionResult CommentDisLike(int id)
        {
            int DisLike = RepComment.DisLike(id);
            return Json(DisLike);
        }


        public ActionResult InsertComment(tbl_Comments tComment)
        {
            try
            {
                tComment.Title = "نظر";
                tComment.Confirm = false;
                tComment.Date = DateTime.Now.Date;
                tComment.Family = "f";
                tComment.DisLike = 0;
                tComment.Like = 0;
                tComment.ParentID = 0;
                tComment.Read = false;
                tComment.IP = Request.UserHostAddress;


                db.tbl_Comments.Add(tComment);

                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Style = "color:green";
                    ViewBag.Message = "نظر ثبت شد";
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "نظر ثبت نشد";

                }

                return View("Subject", RepSubject.GetDeatilsSubject(tComment.SubjectID));
            }
            catch
            {

                return Content("Error");
            }
        }


        public ActionResult CommentReplay(int ParentID, int SubjectID)
        {
            tbl_Comments tComment = new tbl_Comments();
            tComment.SubjectID = SubjectID;
            tComment.ParentID = ParentID;
            return PartialView("P_ReplayComment", tComment);
        }


        public ActionResult InsertReplayComment(tbl_Comments tComment)
        {
            try
            {
                tComment.Title = "پاسخ نظر";
                tComment.Confirm = false;
                tComment.Date = DateTime.Now.Date;
                tComment.Family = "f";
                tComment.DisLike = 0;
                tComment.Like = 0;
                tComment.Read = false;
                tComment.IP = Request.UserHostAddress;


                db.tbl_Comments.Add(tComment);

                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Style = "color:green";
                    ViewBag.Message = "نظر ثبت شد";
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "نظر ثبت نشد";

                }

                return View("Subject", RepSubject.GetDeatilsSubject(tComment.SubjectID));
            }
            catch
            {

                return Content("Error");
            }

        }

        public ActionResult Grouping(int ID)
        {
            try
            {
                var q = from a in db.tbl_Grouping
                        join b in db.tbl_RelationshipGroup_Subject on a.ID equals b.GroupID
                        join c in db.tbl_Subject on b.SubjectID equals c.ID
                        where a.ID.Equals(ID)
                        select c;
                return View("Index", q.ToList());
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult Search(string txtSearch)
        {
            try
            {
                //sqlenjection
                //txtSearch = txtSearch.Replace("|", "").Replace("'", "").Replace('"', ' ');

                var q = db.tbl_Subject.Select(a => a).Where(a => a.Title.Contains(txtSearch));
                return View("Index", q.ToList());
            }
            catch
            {
                return Content("Error");
            }
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(tbl_ContactUs tContact)
        {
            Rep_ContactUs RepContactUs = new Rep_ContactUs();

            if (ModelState.IsValid)
            {
                if (RepContactUs.Insert(tContact))
                {
                    ViewBag.Style = "color:green";
                    ViewBag.Message = "درخواست با موفقیت ثبت شد";
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "متاسفانه درخواست با موفقیت ثبت نشد";
                }
            }
            else
            {
                ViewBag.Style = "color:red";
                ViewBag.Message = "تمامی فیلدها را به صورت صحیح پر نمایید";
            }



            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(tbl_Users tUsers, HttpPostedFileBase Pic)
        {
            tUsers.Access = "Writer";
            tUsers.Enable = false;
            if (ModelState.IsValid)
            {
                if (Pic != null)
                {
                    if (Pic.ContentType == "image/jpeg")
                    {
                        if (Pic.ContentLength <= 81920)
                        {
                            Random rnd = new Random();
                            string rndName = rnd.Next(1, 100000).ToString() + ".jpg";
                            string path = Path.Combine(Server.MapPath("~/Content/Images/Users/"));
                            Pic.SaveAs(path + rndName);
                            tUsers.Image = rndName;
                        }
                        else
                        {
                            ViewBag.Style = "color:red";
                            ViewBag.Message = "حجم تصویر نباید بیشتر از 80 کیلو بایت باشد";
                            return View();

                        }
                    }
                    else
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "فرمت تصویر باید jpg باشد";
                        return View();
                    }
                }
                else
                {
                    tUsers.Image = "Default.jpg";
                }
                db.tbl_Users.Add(tUsers);
                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Style = "color:green";
                    ViewBag.Message = "عضویت با موفقیت انجام شد";
                    return View();
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "متاسفانه عضویت با موفقیت انجام نشد";
                    return View();
                }
            }
            else
            {
                ViewBag.Style = "color:red";
                ViewBag.Message = "تمامی فیلدها را به صورت صحیح پر نمایید";
            }
            return View();
        }



        public ActionResult DupUserName(string UserName)
        {
            try
            {
                //UserName = UserName.Replace("|", "");
                var q = db.tbl_Users.Where(a => a.Username.Equals(UserName));
                if (q.Count() == 0)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {

                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult DupEmail(string Email)
        {
            try
            {
                //Email = Email.Replace("|", "");
                var q = db.tbl_Users.Where(a => a.Email.Equals(Email));
                if (q.Count() == 0)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {

                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserName"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        public ActionResult Login(string txt_UserName, string txt_Password)
        {
            if (this.IsCaptchaValid("Error"))
            {
                if (txt_UserName != "" && txt_Password != "" && txt_UserName != null && txt_Password != null)
                {
                    //txt_UserName = txt_UserName.Replace("|", "");
                    var q = (from a in db.tbl_Users
                             where a.Username.Equals(txt_UserName) && a.Password.Equals(txt_Password)
                             select a).SingleOrDefault();

                    if (q != null)
                    {
                        Session["UserName"] = q.Username;
                        Session["Access"] = q.Access;
                        Session["Enable"] = q.Enable;
                        Session["Name"] = q.Name;
                        Session["Family"] = q.Family;
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "نام کاربری یا کلمه عبور اشتباه است";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "نام کاربری و کلمه عبور را وارد نمایید";
                    return View();

                }
            }
            else
            {
                ViewBag.Style = "color:red";
                ViewBag.Message = "کد تصویری را به درستی وارد نمایید";
                return View();
            }
        }




        [HttpGet]
        public ActionResult Recovery()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Recovery(string txt_UserName, string txt_Email)
        {
            try
            {
                //txt_UserName = txt_UserName.Replace("|", "");
                //txt_Email = txt_Email.Replace("|", "");
                if (txt_UserName != "" && txt_Email != "" && txt_UserName != null && txt_Email != null)
                {
                    var q = (from a in db.tbl_Users
                             where a.Username.Equals(txt_UserName) && a.Email.Equals(txt_Email)
                             select a).SingleOrDefault();
                    if (q != null)
                    {
                        bool a = RecEmail.SendEmail("smtp.mail.yahoo.com", "EducationCode@yahoo.com", "751953852456", txt_Email, "ایمیل آزمایشی برای ریکاوری پسورد", q.Password);
                        if (a == true)
                        {
                            ViewBag.Style = "color:green";
                            ViewBag.Message = "پسورد با موفقیت ارسال شد";
                            return View();
                        }
                        else
                        {
                            ViewBag.Style = "color:red";
                            ViewBag.Message = "متاسفانه نغییر پسورد موفقیت آمیز نبود";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "کاربری با این مشخصات پیدا نشد";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "همه ی فیلد ها را وارد نمایید";
                    return View();
                }
            }
            catch
            {

                ViewBag.Style = "color:red";
                ViewBag.Message = "خطایی رخ داده است";
                return View();
            }
        }
    }
}