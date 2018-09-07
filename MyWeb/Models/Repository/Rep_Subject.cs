using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Models.Domain;

namespace MyWeb.Models.Repository
{
    public class Rep_Subject
    {
        DB_MyWebEntities db = new DB_MyWebEntities();
        Rep_Setting RepSetting = new Rep_Setting();

        public List<tbl_Subject> GetSubject()
        {
            try
            {
                List<tbl_Subject> qGetSubject = (from a in db.tbl_Subject
                                                 select a).OrderByDescending(s => s.ID).ToList();
                return qGetSubject;
            }
            catch
            {
                return null;
            }
        }




        public List<tbl_Subject> GetByVisit()
        {
            try
            {
                var qVisit = db.tbl_Subject.Select(a => a).OrderByDescending(a => a.Visit).Skip(0).Take(3);
                return qVisit.ToList();
            }
            catch
            {

                return null;
            }
        }

        public tbl_Subject GetDeatilsSubject(int ID)
        {
            try
            {
                var q = (from a in db.tbl_Subject
                         where a.ID.Equals(ID)
                         select a).SingleOrDefault();
                return q;
            }
            catch
            {

                return null;
            }
        }

        public int Like(int id)
        {
            var q = (from a in db.tbl_Subject
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();
            try
            {
                q.Like++;
                db.tbl_Subject.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    return q.Like;
                }
                else
                {
                    return q.Like--;
                }
            }
            catch
            {

                return q.Like;
            }
        }

        public int DisLike(int id)
        {
            var q = (from a in db.tbl_Subject
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();
            try
            {
                q.DisLike++;
                db.tbl_Subject.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    return q.DisLike;
                }
                else
                {
                    return q.DisLike--;
                }
            }
            catch
            {

                return q.DisLike;
            }
        }

        public void SetVisit(int id)
        {
            var q = (from a in db.tbl_Subject
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();
            q.Visit++;
            db.tbl_Subject.Attach(q);
            db.Entry(q).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}