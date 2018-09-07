using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//MyWeb.Models.Domain dar khat paeen bayad ba NameSpace Tbl_ContactUs barabar bashad (az onja copy shavad)
namespace MyWeb.Models.Domain
{
    internal class MetaDataContactUs
    {
        //in ghesmat ta paeen az tbl_subject copy shode

        //metadata ha ro inja ezafe miconim chon dar tbl_Subject vaghti Update beshe (update model) metadeta ha pack mishe 

        public int ID { get; set; }

        [DisplayName("نام شما")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "تعداد کاراکتر ها باید بین 2 الی 255 کاراکتر باشد.")]
        public string Name { get; set; }

        [DisplayName("ایمیل شما")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(255, ErrorMessage = "تعداد کاراکتر ها باید 255 کاراکتر باشد.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        public string Email { get; set; }

        [DisplayName("متن شما")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "این فیلد را پر نمایید")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "تعداد کاراکتر ها باید بین 10 الی 1000 کاراکتر باشد.")]
        public string Text { get; set; }
        //ta inja
    }

    //tag MetadataType  bayad as using System.ComponentModel.DataAnnotations; using beshe
    [MetadataType(typeof(MetaDataContactUs))]
    public partial class tbl_ContactUs
    {

    }
}