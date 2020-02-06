using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch
{
    public class Knowlage
    {
        public int Id { get; set; }
        [Display(Name ="标题")]
        public string Title { get; set; }
        [Display(Name = "细节")]
        public string Detail { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public string username { get; set; }
        //public Manager Manager { get; set; }
        //public string ManagerId { get; set; }
    }
 

}
