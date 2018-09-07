using MyWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Repository
{
    public class Rep_Setting
    {
        DB_MyWebEntities db = new DB_MyWebEntities();
        public tbl_Setting tools()
        {
            try
            {
                var qGetSetting = (from a in db.tbl_Setting
                                  select a).FirstOrDefault();
                return qGetSetting;
            }
            catch
            {

                return null;
            }
        }
    }
}