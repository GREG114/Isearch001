using System.ComponentModel.DataAnnotations;

namespace Isearch.Controllers
{
    public class pdc
    {
        [Key]
        [Required(ErrorMessage = "账号不能未空")]
        public string userid { get; set; }
        [Required(ErrorMessage = "密码不能未空")]
        [RegularExpression(@"^[a-zA-Z0-9]{6,18}$", ErrorMessage = "密码必须为6位数以上")]
        public string pd { get; set; }
        [Required(ErrorMessage = "密码不能未空")]
        [RegularExpression(@"^[a-zA-Z0-9]{6,18}$", ErrorMessage = "密码必须为6位数以上")]
        public string newpd { get; set; }
        [Required(ErrorMessage = "密码不能未空")]
        [RegularExpression(@"^[a-zA-Z0-9]{6,18}$",ErrorMessage ="密码必须为6位数以上")]
        public string newpd2 { get; set; }
    }
}