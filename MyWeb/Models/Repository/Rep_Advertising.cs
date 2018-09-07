using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Models.Domain;


namespace MyWeb.Models.Repository
{
    public class Rep_Advertising
    {
        DB_MyWebEntities db = new DB_MyWebEntities();


        public List<tbl_Advertising> GetAds()
        {
            try
            {
                DateTime dt = DateTime.Now.Date;
                var q = (from a in db.tbl_Advertising
                         where a.Enable.Equals(true) && dt >= a.DataStart && dt <= a.DataEnd
                         select a).ToList();
                return q;
            }
            catch
            {

                return null;
            }
        }

    }
}