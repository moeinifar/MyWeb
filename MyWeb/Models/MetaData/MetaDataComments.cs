using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Domain
{
    internal class MetaDataComments
    {
        public int ID { get; set; }

        [DisplayName("نام مطلب")]
        public int SubjectID { get; set; }

        [DisplayName("نظر والد")]
        public int ParentID { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; }

        [DisplayName("نام نظر دهنده")]
        public string Name { get; set; }

        [DisplayName("نام خانوادگی نظر دهنده")]
        public string Family { get; set; }

        [DisplayName("ایمیل")]
        public string Email { get; set; }

        [DisplayName("متن نظر")]
        public string Text { get; set; }

        [DisplayName("آی پی")]
        public string IP { get; set; }

        [DisplayName("تاریخ")]
        public System.DateTime Date { get; set; }

        [DisplayName("وضعیت")]
        public bool Confirm { get; set; }

        [DisplayName("خوانده شده")]
        public bool Read { get; set; }

        [DisplayName("پسندیدن")]
        public int Like { get; set; }

        [DisplayName("نپسندیدن")]
        public int DisLike { get; set; }
    }

    [MetadataType(typeof(MetaDataComments))]
    public partial class tbl_Comments
    {
    }

}