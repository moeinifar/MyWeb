using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Domain
{
    internal class MetaDataSettings
    {
        [DisplayName("شماره")]
        public int ID { get; set; }

        [DisplayName("عنوان سایت")]
        public string Title { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("متا تگ ها")]
        public string MetaTag { get; set; }

        [DisplayName("نمایش نظرات تایید شده")]
        public bool ShowConfComm { get; set; }

        [DisplayName("غیر فعال سازی نظرات")]
        public bool DisableComment { get; set; }

        [DisplayName("بیشترین حجم آپلود")]
        public string MaxUploadSize { get; set; }

        [DisplayName("تعداد مطالب در صفحه")]
        public int CountSubjectInPage { get; set; }

        [DisplayName("تعداد یادداشت ها")]
        public int CountOfMemo { get; set; }

        [DisplayName("تعداد جدیدترین مطالب")]
        public int CountNewer { get; set; }

        [DisplayName("تعداد محبوب ترین ها")]
        public int CountPopular { get; set; }

    }

    [MetadataType(typeof(MetaDataSettings))]
    public partial class tbl_Setting
    {
    }
}