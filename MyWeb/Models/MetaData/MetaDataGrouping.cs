using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Domain
{
    internal class MetaDataGrouping
    {
        public int ID { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; }

        [DisplayName("والد")]
        public int ParentID { get; set; }

        [DisplayName("تصویر")]
        public string Image { get; set; }
    }

    [MetadataType(typeof(MetaDataGrouping))]
    public partial class tbl_Grouping
    {
    }
}