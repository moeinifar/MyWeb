using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Models.Domain;

namespace MyWeb.Models.Repository
{
    public class Rep_Grouping
    {
        DB_MyWebEntities db = new DB_MyWebEntities();
        public List<tbl_Grouping> GetGrouping()
        {
            try
            {
                var qGroup = (from a in db.tbl_Grouping
                              where a.ID != 1
                              select a).ToList();
                return qGroup;
            }
            catch
            {

                return null;
            }
        }
    }
}