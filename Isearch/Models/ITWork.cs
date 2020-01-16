using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch.Models
{
    public class ITWork
    {
        public string Id { get; set; }
        [Display(Name ="标题")]
        public string Title { get; set; }
        [Display(Name ="类型")]
        public string Workclass { get; set; }
        [Display(Name ="执行人")]
        public string Creator { get; set; }
        [Display(Name ="服务对象")]
        public string Target { get; set; }
        [Display(Name ="详细")]
        public string Content { get; set; }
        [Display(Name ="状态")]
        public string Status { get; set; }
        public int satisfied { get; set; }
        public DateTime Create_at { get; set; }
        public DateTime Update_at { get; set; }
        public DateTime Finish_at { get; set; }
        
    }

}
