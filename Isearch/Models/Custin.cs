using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch.Models
{
    public class Custin
    {
        public int Id { get; set; }
        public string 客户名称 { get; set; }
        [EmailAddress]
        public string 电子邮件 { get; set; }
        public string 地址 { get; set; }
        public string 名单来源 { get; set; }
        public string 名单日期 { get; set; }
        public string 回访情况 { get; set; }
    }

    public class Custin2
    {
        public string 姓名 { get; set; }
        public string 公司 { get; set; }
        public string 职位 { get; set; }
        public string 手机 { get; set; }
        public string 电子邮件 { get; set; }
        public string 名单来源 { get; set; }
        public DateTime 名单日期 { get; set; }
        public string 回访情况 { get; set; }
    }
}
