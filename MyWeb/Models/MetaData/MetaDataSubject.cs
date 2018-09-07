using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//MyWeb.Models.Domain dar khat paeen bayad ba NameSpace Tbl_Subject barabar bashad (az onja copy shavad)
namespace MyWeb.Models.Domain
{
    internal class MetaDataSubject
    {
        //in ghesmat ta paeen az tbl_subject copy shode

        //metadata ha ro inja ezafe miconim chon dar tbl_Subject vaghti Update beshe (update model) metadeta ha pack mishe 
        public int ID { get; set; }
        [DisplayName("عنوان")]
        public string Title { get; set; }

        [DisplayName("روتیتر")]
        public string HeadTitle { get; set; }

        [DisplayName("چکیده")]
        public string Text { get; set; }

        [DisplayName("متن کامل خبر")]
        [AllowHtml]
        public string FullText { get; set; }

        [DisplayName("عکس")]
        public string Image { get; set; }

        [DisplayName("ویدئو")]
        public string Video { get; set; }

        [DisplayName("نویسنده")]
        public string Username { get; set; }

        [DisplayName("تعداد بازدید")]
        public long Visit { get; set; }

        [DisplayName("پسندیدن")]
        public int Like { get; set; }

        [DisplayName("نپسندیدن")]
        public int DisLike { get; set; }

        [DisplayName("تاریخ")]
        public System.DateTime Date { get; set; }

        [DisplayName("نوع")]
        public string Type { get; set; }


        //ta inja
    }

    //tag MetadataType  bayad as using System.ComponentModel.DataAnnotations; using beshe
    [MetadataType(typeof(MetaDataSubject))]
    public partial class tbl_Subject
    {

    }
}