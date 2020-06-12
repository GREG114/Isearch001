using System.ComponentModel.DataAnnotations;

namespace Isearch.Controllers
{
    public class pdc
    {
        [Key]
        [Required(ErrorMessage = "账号不能为空")]
        public string userid { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string pd { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string newpd { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string newpd2 { get; set; }
    }
}