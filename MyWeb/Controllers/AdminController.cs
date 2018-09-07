using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWeb.Models.Domain;
using System.IO;

namespace MyWeb.Controllers
{
    public class AdminController : Controller
    {
        DB_MyWebEntities db = new DB_MyWebEntities();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                ViewBag.Style = "color:red";
                ViewBag.Message = "شما باید ابتدا وارد سایت شوید";
                return RedirectToAction("Login", "Home");
            }
        }


        public ActionResult Logout()
        {
            if (Session["UserName"] != null)
            {
                Session["UserName"] = null;
                Session["Access"] = null;
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditPassword(string txt_OldPassword, string txt_NewPassword, string txt_ConfirmPassword)
        {
            if (Session["UserName"] != null)
            {
                if (txt_OldPassword == "" || txt_OldPassword == null || txt_NewPassword == "" || txt_NewPassword == null || txt_ConfirmPassword == "" || txt_ConfirmPassword == null)
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "همه فیلد ها را پر نمایید";
                    return View();
                }
                if (txt_NewPassword.Trim() != txt_ConfirmPassword.Trim())
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "کلمه عبور جدید با تکرار آن برابر نمی باشد";
                    return View();
                }
                //txtSearch = txtSearch.Replace("|", "").Replace("'", "").Replace('"', ' ');
                string Username = Session["UserName"].ToString();
                var q = (from a in db.tbl_Users
                         where a.Username.Equals(Username) && a.Password.Equals(txt_OldPassword)
                         select a).SingleOrDefault();
                if (q != null)
                {
                    q.Password = txt_NewPassword.Trim();
                    db.tbl_Users.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() <= 0)
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "متاسفانه عملیات ویرایش کلمه عبور موفقیت آمیز نبود";
                        return View();
                    }
                    else
                    {
                        ViewBag.Style = "color:green";
                        ViewBag.Message = "عملیات ویرایش کلمه عبور با موفقیت انجام شد";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "کلمه عبور فعلی را به درستی وارد کنید";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        [HttpGet]
        public ActionResult EditProfile()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    string Username = Session["UserName"].ToString();
                    var q = (from a in db.tbl_Users
                             where a.Username.Equals(Username)
                             select a).SingleOrDefault();
                    return View(q);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch
            {
                return Content("خطا");
            }
        }

        [HttpPost]
        public ActionResult EditProfile(tbl_Users tUsers, HttpPostedFileBase Pic)
        {

            try
            {
                if (Session["UserName"] != null)
                {
                    string Username = Session["UserName"].ToString();
                    var q = (from a in db.tbl_Users
                             where a.Username.Equals(Username)
                             select a).SingleOrDefault();

                    q.Description = tUsers.Description;
                    q.MobilNumber = tUsers.MobilNumber;
                    q.Name = tUsers.Name;
                    q.Family = tUsers.Family;
                    q.MeliCode = tUsers.MeliCode;
                    q.Email = tUsers.Email;

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
                                q.Image = rndName;
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
                    db.tbl_Users.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    if (Convert.ToBoolean(db.SaveChanges()))
                    {
                        ViewBag.Style = "color:green";
                        ViewBag.Message = "پروفایل با موفقیت ویرایش شد";
                        return View();
                    }
                    else
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "متاسفانه پروفایل با موفقیت ویرایش نشد";
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch
            {
                return Content("خطا");
            }
        }


        [HttpGet]
        public ActionResult ManageSubject()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    if (Session["Access"].ToString() == "Admin")
                    {
                        var q = (from a in db.tbl_Subject
                                 orderby a.ID descending
                                 select a);
                        return View(q);
                    }
                    else
                    {
                        string Username = Session["UserName"].ToString();
                        var q = from a in db.tbl_Subject
                                where a.Username.Equals(Username)
                                orderby a.ID descending
                                select a;
                        return View(q);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch
            {
                return Content("خطا");
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    if (Session["Access"].ToString() == "Admin")
                    {
                        var qComment = (from a in db.tbl_Comments
                                        where a.SubjectID.Equals(id)
                                        select a);
                        var qGroup = (from a in db.tbl_RelationshipGroup_Subject
                                      where a.SubjectID.Equals(id)
                                      select a);
                        var qSubject = (from a in db.tbl_Subject
                                        where a.ID.Equals(id)
                                        select a).SingleOrDefault();

                        db.tbl_Comments.RemoveRange(qComment.AsEnumerable());
                        db.tbl_RelationshipGroup_Subject.RemoveRange(qGroup.AsEnumerable());
                        db.tbl_Subject.Remove(qSubject);

                        db.SaveChanges();
                        return RedirectToAction("ManageSubject");
                    }
                    else
                    {
                        string UserName = Session["UserName"].ToString();

                        var qComment = (from a in db.tbl_Subject
                                        join b in db.tbl_Comments on a.ID equals b.SubjectID
                                        where a.Username.Equals(UserName) && a.ID.Equals(id)
                                        select b);
                        var qGroup = (from a in db.tbl_Subject
                                      join b in db.tbl_RelationshipGroup_Subject on a.ID equals b.SubjectID
                                      where a.Username.Equals(UserName) && a.ID.Equals(id)
                                      select b);
                        var qSubject = (from a in db.tbl_Subject
                                        where a.Username.Equals(UserName) && a.ID.Equals(id)
                                        select a).SingleOrDefault();

                        if (qSubject != null)
                        {
                            db.tbl_Comments.RemoveRange(qComment.AsEnumerable());
                            db.tbl_RelationshipGroup_Subject.RemoveRange(qGroup.AsEnumerable());
                            db.tbl_Subject.Remove(qSubject);
                            db.SaveChanges();
                        }
                        return RedirectToAction("ManageSubject");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch
            {

                return Content("Error");
            }
        }

        [HttpGet]
        public ActionResult EditSubject(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                //if (Session["Access"].ToString() != "Writer")
                //{
                //    return RedirectToAction("Logout", "Admin");
                //}

                string UserName = Session["UserName"].ToString();
                string Access = Session["Access"].ToString();

                var q = (from a in db.tbl_Subject
                         where a.ID.Equals(id) && (a.Username.Equals(UserName) || Access.Equals("Admin"))
                         select a).SingleOrDefault();

                return View(q);
            }
            catch
            {
                return Content("Error1");
            }
        }

        [HttpPost]
        public ActionResult EditSubject(tbl_Subject tSubject, HttpPostedFileBase Pic, int Grouping = 0)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                //if (Session["Access"].ToString() != "Writer")
                //{
                //    return RedirectToAction("Logout", "Admin");
                //}

                if (Grouping == 0)
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "یک از دسته بندی ها را انتخاب نمایید";
                    return View();
                }

                var q3 = (from a in db.tbl_Grouping
                          where a.ID.Equals(Grouping)
                          select a).SingleOrDefault();
                if (q3 == null)
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "یک دسته معتبر را انتخاب نمایید";
                    return View();
                }

                tSubject.Date = DateTime.Now.Date;

                if (tSubject.Type != "Film" && tSubject.Type != "Article")
                {
                    tSubject.Type = "null";
                }
                if (ModelState.IsValid)
                {
                    string UserName = Session["UserName"].ToString();
                    string Access = Session["Access"].ToString();

                    var q = (from a in db.tbl_Subject
                             where a.ID.Equals(tSubject.ID) && (a.Username.Equals(UserName) || Access.Equals("Admin"))
                             select a).SingleOrDefault();

                    if (q != null)
                    {
                        q.Date = tSubject.Date;
                        q.FullText = tSubject.FullText;
                        q.HeadTitle = tSubject.HeadTitle;
                        q.Text = tSubject.Text;
                        q.Title = tSubject.Title;
                        q.Type = tSubject.Type;

                        if (Pic != null)
                        {
                            if (Pic.ContentLength <= 512000)
                            {
                                if (Pic.ContentType == "image/jpeg")
                                {
                                    Random rnd = new Random();
                                    string PicName = (rnd.Next().ToString() + ".jpg");
                                    string path = System.IO.Path.Combine(Server.MapPath("~") + "Content/Images/Subject/");
                                    Pic.SaveAs(path + PicName);
                                    q.Image = PicName;
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
                                ViewBag.Style = "color:red";
                                ViewBag.Message = "حجم فایل بیشتر از 512کیلو بایت است";
                                return View();
                            }
                        }

                        var q2 = (from a in db.tbl_RelationshipGroup_Subject
                                  where a.SubjectID.Equals(tSubject.ID)
                                  select a).FirstOrDefault();

                        q2.GroupID = Grouping;
                        db.tbl_RelationshipGroup_Subject.Attach(q2);
                        db.Entry(q2).State = System.Data.Entity.EntityState.Modified;

                        db.tbl_Subject.Attach(q);
                        db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges()))
                        {
                            ViewBag.Style = "color:green";
                            ViewBag.Message = "ویرایش با موفقیت انجام شد";
                            return RedirectToAction("ManageSubject", "Admin");
                        }
                        else
                        {
                            ViewBag.Style = "color:red";
                            ViewBag.Message = "متاسفانه ویرایش با موفقیت انجام نشد";
                            return RedirectToAction("ManageSubject", "Admin");
                        }


                    }
                    else
                    {
                        return RedirectToAction("Logout", "Admin");
                    }

                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "تمامی فیلدها را به درستی پر نمایید";
                    return View();
                }
            }
            catch
            {
                return Content("Error2");
            }
        }


        [HttpGet]
        public ActionResult CreateSubject()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                //if (Session["Access"].ToString() != "Writer")
                //{
                //    return RedirectToAction("Logout", "Admin");
                //}
                return View();
            }
            catch
            {
                return Content("Error");
            }
        }



        [HttpPost]
        public ActionResult CreateSubject(tbl_Subject tSubject, HttpPostedFileBase Pic, int Grouping)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                //if (Session["Access"].ToString() != "Writer")
                //{
                //    return RedirectToAction("Logout", "Admin");
                //}

                if (Pic == null)
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "یک عکس را انتخاب نمایید";
                    return View();

                }

                if (Grouping == 0)
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "یک از دسته بندی ها را انتخاب نمایید";
                    return View();
                }

                var q3 = (from a in db.tbl_Grouping
                          where a.ID.Equals(Grouping)
                          select a).SingleOrDefault();
                if (q3 == null)
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "یک دسته معتبر را انتخاب نمایید";
                    return View();
                }

                tSubject.Date = DateTime.Now.Date;
                tSubject.DisLike = 0;
                tSubject.Like = 0;
                tSubject.Visit = 0;
                tSubject.Username = Session["UserName"].ToString();

                if (tSubject.Type != "Film" && tSubject.Type != "Article")
                {
                    tSubject.Type = null;
                }
                if (ModelState.IsValid)
                {

                    if (Pic.ContentLength <= 512000)
                    {
                        if (Pic.ContentType == "image/jpeg")
                        {
                            Random rnd = new Random();
                            string PicName = (rnd.Next().ToString()) + ".jpg";
                            string path = System.IO.Path.Combine(Server.MapPath("~") + "Content/Images/Subject/");
                            Pic.SaveAs(path + PicName);
                            tSubject.Image = PicName;

                            db.tbl_Subject.Add(tSubject);
                            if (Convert.ToBoolean(db.SaveChanges()))
                            {
                                var q = (from a in db.tbl_Subject
                                         orderby a.ID descending
                                         select a).FirstOrDefault();
                                tbl_RelationshipGroup_Subject tbl_GS = new tbl_RelationshipGroup_Subject();
                                tbl_GS.SubjectID = q.ID;
                                tbl_GS.GroupID = Grouping;
                                db.tbl_RelationshipGroup_Subject.Add(tbl_GS);
                                if (Convert.ToBoolean(db.SaveChanges()))
                                {
                                    ViewBag.Style = "color:green";
                                    ViewBag.Message = "ثبت با موفقیت انجام شد";
                                    return View();
                                }
                                else
                                {
                                    ViewBag.Style = "color:red";
                                    ViewBag.Message = "متاسفانه ثبت نشد";
                                    return View();
                                }

                            }
                            else
                            {
                                ViewBag.Style = "color:red";
                                ViewBag.Message = "متاسفانه ثبت نشد";
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
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "حجم فایل بیشتر از 512کیلو بایت است";
                        return View();
                    }

                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "تمامی فیلدها را به درستی پر نمایید";
                    return View();
                }

            }
            catch
            {
                return Content("Error");
            }
        }



        public ActionResult ManageComments(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() == "Admin")
                {
                    var q = from a in db.tbl_Comments
                            where a.SubjectID.Equals(id)
                            select a;
                    return View(q);
                }
                else if (Session["Access"].ToString() == "Writer")
                {
                    string Username = Session["UserName"].ToString();
                    var q = (from a in db.tbl_Subject
                             join b in db.tbl_Comments on a.ID equals b.SubjectID
                             where a.ID.Equals(id) && a.Username.Equals(Username)
                             select b);
                    return View(q);
                }
                else
                {
                    Logout();
                    return View();
                }
            }
            catch
            {

                return Content("Error");
            }
        }

        public ActionResult DetailsComment(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (Session["Access"].ToString() == "Admin")
                {
                    var q = (from a in db.tbl_Comments
                             where a.ID.Equals(id)
                             select a).SingleOrDefault();

                    q.Read = true;
                    db.tbl_Comments.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();



                    return View(q);
                }
                else if (Session["Access"].ToString() == "Writer")
                {
                    string Username = Session["Access"].ToString();
                    var q = (from a in db.tbl_Subject
                             join b in db.tbl_Comments on a.ID equals b.SubjectID
                             where b.ID.Equals(id) && a.Username.Equals(Username)
                             select b).SingleOrDefault();

                    q.Read = false;
                    db.tbl_Comments.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return View(q);
                }
                else
                {
                    Logout();
                    return View();
                }
            }
            catch
            {

                return Content("Error");
            }
        }


        public ActionResult ConfirmComment(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (Session["Access"].ToString() == "Admin")
                {
                    var q = (from a in db.tbl_Comments
                             where a.ID.Equals(id)
                             select a).SingleOrDefault();

                    q.Confirm = true;
                    db.tbl_Comments.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges()))
                    {
                        ViewBag.Style = "color:green";
                        ViewBag.Message = "نظر با موفقیت تایید شد";
                        return View("DetailsComment", q);
                    }
                    else
                    {
                        ViewBag.Style = "color:green";
                        ViewBag.Message = "متاسفانه نظر با موفقیت تایید نشد";
                        return View("DetailsComment", q);
                    }
                }
                else if (Session["Access"].ToString() == "Writer")
                {
                    string Username = Session["Access"].ToString();
                    var q = (from a in db.tbl_Subject
                             join b in db.tbl_Comments on a.ID equals b.SubjectID
                             where b.ID.Equals(id) && a.Username.Equals(Username)
                             select b).SingleOrDefault();

                    q.Confirm = true;
                    db.tbl_Comments.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges()))
                    {
                        ViewBag.Style = "color:green";
                        ViewBag.Message = "نظر با موفقیت تایید شد";
                        return View("DetailsComment", q);
                    }
                    else
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "متاسفانه نظر با موفقیت تایید نشد";
                        return View("DetailsComment", q);
                    }

                }
                else
                {
                    Logout();
                    return View();
                }
            }
            catch
            {
                return Content("Error");
            }
        }

        public IQueryable<tbl_Grouping> GetListGrouping()
        {
            if (Session["UserName"] == null)
            {
                RedirectToAction("Login", "Home");
            }

            if (Session["Access"].ToString() != "Admin")
            {
                RedirectToAction("Logout", "Admin");
            }

            var q = from a in db.tbl_Grouping
                    select a;
            return q;
        }

        public ActionResult ManageGrouping()
        {
            try
            {
                return View("ManageGrouping", GetListGrouping());
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult DeleteGrouping(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q2 = from a in db.tbl_Grouping
                         where a.ParentID.Equals(id)
                         select a;
                foreach (var item in q2)
                {
                    item.ParentID = 1;
                    db.tbl_Grouping.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();

                var q = (from a in db.tbl_Grouping
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();

                db.tbl_Grouping.Remove(q);
                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Style = "color:green";
                    ViewBag.Message = "دسته با موفقیت حذف شد";
                    return View("ManageGrouping", GetListGrouping());
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "متاسفانه دسته با موفقیت حذف نشد";
                    return View("ManageGrouping", GetListGrouping());
                }
            }
            catch
            {
                return Content("Error");
            }
        }


        [HttpGet]
        public ActionResult CreateGrouping(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Grouping
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();
                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }


        [HttpPost]
        public ActionResult CreateGrouping(tbl_Grouping tGrouping, HttpPostedFileBase Pic)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                if (ModelState.IsValid)
                {

                    tbl_Grouping tGrouping2 = new tbl_Grouping();
                    tGrouping2.ParentID = tGrouping.ParentID;
                    tGrouping2.Title = tGrouping.Title;

                    if (Pic != null)
                    {
                        if (Pic.ContentLength <= 512000)
                        {
                            if (Pic.ContentType == "image/jpeg")
                            {
                                Random rnd = new Random();
                                string picname = rnd.Next() + ".jpg";
                                string Path = System.IO.Path.Combine(Server.MapPath("~")) + "Content/Images/Grouping/" + picname;
                                Pic.SaveAs(Path);
                                tGrouping2.Image = picname;
                            }
                            else
                            {
                                ViewBag.Style = "color:red";
                                ViewBag.Message = "فرمت عکس باید jpg باشد";
                                return View("ManageGrouping", GetListGrouping());
                            }
                        }
                        else
                        {
                            ViewBag.Style = "color:red";
                            ViewBag.Message = "حجم تصویر نباید بیشتر از 512 کیلو بایت باشد";
                            return View("ManageGrouping", GetListGrouping());
                        }
                    }

                    db.tbl_Grouping.Add(tGrouping2);
                    if (Convert.ToBoolean(db.SaveChanges()))
                    {
                        ViewBag.Style = "color:green";
                        ViewBag.Message = "دسته بندی با موفقیت ثبت شد";
                        return View("ManageGrouping", GetListGrouping());
                    }
                    else
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "متاسفانه دسته بندی با موفقیت ثبت نشد";
                        return View("ManageGrouping", GetListGrouping());
                    }
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "تمامی فیلدها را به درستی وارد نمایید";
                    return View("ManageGrouping", GetListGrouping());
                }
            }
            catch
            {
                return Content("Error");
            }
        }


        [HttpGet]
        public ActionResult EditGrouping(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Grouping
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();
                return View(q);
            }
            catch
            {

                return Content("Error");
            }
        }


        [HttpPost]
        public ActionResult EditGrouping(tbl_Grouping tGrouping, HttpPostedFileBase Pic)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                if (ModelState.IsValid)
                {

                    var q = (from a in db.tbl_Grouping
                             where a.ID.Equals(tGrouping.ID)
                             select a).SingleOrDefault();

                    q.ParentID = tGrouping.ParentID;
                    q.Title = tGrouping.Title;

                    if (Pic != null)
                    {
                        if (Pic.ContentLength <= 512000)
                        {
                            if (Pic.ContentType == "image/jpeg")
                            {
                                Random rnd = new Random();
                                string picname = rnd.Next() + ".jpg";
                                string Path = System.IO.Path.Combine(Server.MapPath("~")) + "Content/Images/Grouping/" + picname;
                                Pic.SaveAs(Path);
                                q.Image = picname;
                            }
                            else
                            {
                                ViewBag.Style = "color:red";
                                ViewBag.Message = "فرمت عکس باید jpg باشد";
                                return View("ManageGrouping", GetListGrouping());
                            }
                        }
                        else
                        {
                            ViewBag.Style = "color:red";
                            ViewBag.Message = "حجم تصویر نباید بیشتر از 512 کیلو بایت باشد";
                            return View("ManageGrouping", GetListGrouping());
                        }
                    }

                    db.tbl_Grouping.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges()))
                    {
                        ViewBag.Style = "color:green";
                        ViewBag.Message = "دسته بندی با موفقیت ویرایش شد";
                        return View("ManageGrouping", GetListGrouping());
                    }
                    else
                    {
                        ViewBag.Style = "color:red";
                        ViewBag.Message = "متاسفانه دسته بندی با موفقیت ویرایش نشد";
                        return View("ManageGrouping", GetListGrouping());
                    }
                }
                else
                {
                    ViewBag.Style = "color:red";
                    ViewBag.Message = "تمامی فیلدها را به درستی وارد نمایید";
                    return View("ManageGrouping", GetListGrouping());
                }
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult ManageUsers()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = from a in db.tbl_Users
                        where a.Access.Equals("Writer")
                        select a;
                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }


        public ActionResult DetailsUser(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    return RedirectToAction("Logout");
                }
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Users
                         where a.Access.Equals("Writer") && a.ID.Equals(id)
                         select a).SingleOrDefault();
                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult EnableUser(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Users
                         where a.Access.Equals("Writer") && a.ID.Equals(id)
                         select a).SingleOrDefault();

                q.Enable = true;
                db.tbl_Users.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ManageUsers");
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult DisableUser(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Users
                         where a.Access.Equals("Writer") && a.ID.Equals(id)
                         select a).SingleOrDefault();

                q.Enable = false;
                db.tbl_Users.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ManageUsers");
            }
            catch
            {
                return Content("Error");
            }
        }


        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Users
                         where a.Access.Equals("Writer") && a.ID.Equals(id)
                         select a).SingleOrDefault();

                db.tbl_Users.Remove(q);
                db.SaveChanges();

                return RedirectToAction("ManageUsers");
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult ManageContactUs()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_ContactUs
                         orderby a.ID descending
                         select a);

                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult DetailsContactUs(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_ContactUs
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();

                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult DeleteContactUs(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_ContactUs
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();

                db.tbl_ContactUs.Remove(q);
                db.SaveChanges();

                var q2 = (from a in db.tbl_ContactUs
                          orderby a.ID
                          select a);
                return View("ManageContactUs", q2);
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult ManageAdvertising()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = from a in db.tbl_Advertising
                        orderby a.ID descending
                        select a;

                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult DetailsAdvertising(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Advertising
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();

                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }

        [HttpGet]
        public ActionResult CreateAdvertising()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }


                return View();
            }
            catch
            {
                return Content("Error");
            }
        }

        [HttpPost]
        public ActionResult CreateAdvertising(tbl_Advertising tAdvertising)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                tAdvertising.Date = DateTime.Now.Date;
                db.tbl_Advertising.Add(tAdvertising);
                db.SaveChanges();

                var q = from a in db.tbl_Advertising
                        orderby a.ID descending
                        select a;

                return View("ManageAdvertising", q);
            }
            catch
            {
                return Content("Error");
            }
        }

        [HttpGet]
        public ActionResult EditAdvertising(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }



                var q = (from a in db.tbl_Advertising
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();

                return View("EditAdvertising", q);
            }
            catch
            {
                return Content("Error");
            }
        }


        [HttpPost]
        public ActionResult EditAdvertising(tbl_Advertising tAdvertising)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }



                var q = (from a in db.tbl_Advertising
                         where a.ID.Equals(tAdvertising.ID)
                         select a).SingleOrDefault();

                q.DataEnd = tAdvertising.DataEnd;
                q.DataStart = tAdvertising.DataStart;
                q.Description = tAdvertising.Description;
                q.Enable = tAdvertising.Enable;
                q.Image = tAdvertising.Image;
                q.Link = tAdvertising.Link;
                q.Title = tAdvertising.Title;

                db.tbl_Advertising.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var q2 = from a in db.tbl_Advertising
                        orderby a.ID descending
                        select a;

                return View("ManageAdvertising", q2);

            }
            catch
            {
                return Content("Error");
            }
        }

        public ActionResult DeleteAdvertising(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Advertising
                         where a.ID.Equals(id)
                         select a).SingleOrDefault();

                db.tbl_Advertising.Remove(q);
                db.SaveChanges();

                var q2 = (from a in db.tbl_Advertising
                          orderby a.ID descending
                          select a);
                return View("ManageAdvertising", q2);
            }
            catch
            {
                return Content("Error");
            }
        }

        [HttpGet]
        public ActionResult Settings()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }

                var q = (from a in db.tbl_Setting
                         select a).FirstOrDefault();
                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }

        [HttpPost]
        public ActionResult Settings(tbl_Setting tSetting)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (Session["Access"].ToString() != "Admin")
                {
                    return RedirectToAction("Logout", "Admin");
                }


                var q = (from a in db.tbl_Setting
                         select a).FirstOrDefault();

                q.CountNewer = tSetting.CountNewer;
                q.CountSubjectInPage = tSetting.CountSubjectInPage;
                q.CountOfMemo = tSetting.CountOfMemo;
                q.CountPopular = tSetting.CountPopular;
                q.Description = tSetting.Description;
                q.DisableComment = tSetting.DisableComment;
                q.MaxUploadSize = tSetting.MaxUploadSize;
                q.MetaTag = tSetting.MetaTag;
                q.ShowConfComm = tSetting.ShowConfComm;
                q.Title = tSetting.Title;

                db.tbl_Setting.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return View(q);
            }
            catch
            {
                return Content("Error");
            }
        }

    }
}