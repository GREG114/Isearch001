using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch.Models
{
    public class QMission
    {
        public int Id { get; set; }
        [Display(Name = "任务标题")]
        public string Title { get; set; }
        [Display(Name = "细节描述")]
        public string Detail { get; set; }

        [Display(Name = "取值范围")]
        public string DateRange { get; set; }

        [Display(Name = "运行周期")]
        public string RunTime{ get; set; }

        [Display(Name = "数据模型")]
        public string DateModel { get; set; }

        [Display(Name = "验证方式")]
        public string VerifyMethod { get; set; }



        [Display(Name = "创建者")]
        public string CreateBy { get; set; }
        [Display(Name = "完成者")]
        public string FinishBy { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "完成时间")]
        public DateTime FinishTime { get; set; }
        [Display(Name = "任务状态")]
        public string Status { get; set; }
    }
}
