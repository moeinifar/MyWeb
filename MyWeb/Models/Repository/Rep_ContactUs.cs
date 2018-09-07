using MyWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Repository
{
    public class Rep_ContactUs
    {
        DB_MyWebEntities db = new DB_MyWebEntities();
        public bool Insert(tbl_ContactUs tContact)
        {
            try
            {
                db.tbl_ContactUs.Add(tContact);
                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }
    }
}