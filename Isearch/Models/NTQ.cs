using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch.Models
{
    public class NTQ
    {
        [Key]
        public string 姓名 { get; set; }
        public string 总体满意度 { get; set; }
        public string 所在办公区域 { get; set; }
        public string 部门 { get; set; }
        public string 无线稳定性 { get; set; }
        public string 无线下载速度 { get; set; }
        public string 无线打开速度 { get; set; }
        public string 无线可访问性 { get; set; }
        public string SSID { get; set; }
        public string 有线稳定性 { get; set; }
        public string 有线下载速度 { get; set; }
        public string 有线打开速度 { get; set; }
        public string 有线可访问性 { get; set; }
        public bool 是否使用有线 { get; set; }
        public DateTime updatetime { get; set; }

    }
}
