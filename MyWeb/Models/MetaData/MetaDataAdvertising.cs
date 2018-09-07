using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Domain
{
    public class MetaDataAdvertising
    {
        public int ID { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("تصویر")]
        public string Image { get; set; }

        [DisplayName("لینک")]
        public string Link { get; set; }

        [DisplayName("زمان ثبت")]
        public System.DateTime Date { get; set; }

        [DisplayName("تاریخ شروع")]
        public System.DateTime DataStart { get; set; }

        [DisplayName("تاریخ پایان")]
        public System.DateTime DataEnd { get; set; }

        [DisplayName("وضعیت")]
        public bool Enable { get; set; }
    }

    [MetadataType(typeof(MetaDataAdvertising))]
    public partial class tbl_Advertising
    {
    }
}