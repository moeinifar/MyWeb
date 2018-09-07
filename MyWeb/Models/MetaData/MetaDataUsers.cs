using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Models.Domain
{
    internal class MetaDataUsers
    {
        public int ID { get; set; }

        [DisplayName("نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "تعداد کاراکتر ها باید بین 3 الی 255 کاراکتر باشد.")]
        [Remote("DupUserName","Home",ErrorMessage="نام کاربری تکراری است",HttpMethod="Post")]
        public string Username { get; set; }

        [DisplayName("کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "تعداد کاراکتر ها باید بین 3 الی 255 کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("نام ایمیل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, ErrorMessage = "تعداد کاراکتر ها باید 255 کاراکتر باشد.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        [Remote("DupEmail", "Home", ErrorMessage = "ایمیل تکراری است", HttpMethod = "Post")]

        public string Email { get; set; }

        [DisplayName("نوع دسترسی")]
        public string Access { get; set; }

        [DisplayName("توضیحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "تعداد کاراکتر ها باید بین 5 الی 255 کاراکتر باشد.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "تعداد کاراکتر ها باید بین 3 الی 255 کاراکتر باشد.")]
        public string Name { get; set; }

        [DisplayName("نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "تعداد کاراکتر ها باید بین 3 الی 255 کاراکتر باشد.")]
        public string Family { get; set; }

        [DisplayName("تصویر")]
        public string Image { get; set; }

        [DisplayName("کد ملی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "تعداد کاراکتر ها باید 10 کاراکتر باشد.")]
        public string MeliCode { get; set; }

        [DisplayName("شماره موبایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "تعداد کاراکتر ها باید 11 کاراکتر باشد.")]
        [DataType(DataType.PhoneNumber)]
        public string MobilNumber { get; set; }

        [DisplayName("فعال بودن")]
        public bool Enable { get; set; }
    }

    [MetadataType(typeof(MetaDataUsers))]
    public partial class tbl_Users
    {

    }
}