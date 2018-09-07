using MyWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Repository
{
    public class Rep_Comments
    {
        DB_MyWebEntities db = new DB_MyWebEntities();
        Rep_Setting RSet = new Rep_Setting();
        public List<tbl_Comments> GetComment(int id)
        {
            try
            {
                if (RSet.tools().ShowConfComm == false)
                {
                    var q = (from a in db.tbl_Comments
                             where a.SubjectID.Equals(id)
                             select a).ToList();
                    return q;
                }
                else
                {
                    var q = (from a in db.tbl_Comments
                             where a.SubjectID.Equals(id) && a.Confirm == true
                             select a).ToList();
                    return q;
                }


            }
            catch
            {

                return null;
            }
        }

        public int Like(int id)
        {
            var q = (from a in db.tbl_Comments
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();
            try
            {
                q.Like++;
                db.tbl_Comments.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return q.Like;
            }
            catch
            {

                return q.Like--;
            }
        }
        public int DisLike(int id)
        {
            var q = (from a in db.tbl_Comments
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();
            try
            {
                q.DisLike++;
                db.tbl_Comments.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return q.DisLike;
            }
            catch
            {

                return q.DisLike--;
            }
        }

    }
}