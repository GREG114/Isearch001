using RestSharp.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string 课程名称 { get; set; }
        public float 培训时长 { get; set; }
        public string 培训讲师 { get; set; }
        public DateTime 培训时间 { get; set; }
        IEnumerable<TrainingFeedBack> TrainingFeedBacks { get; set; }
        public string 整合信息 { get; set; }
    }

    public class TrainingFeedBack
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb1 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb2 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb3 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb4 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb5 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb6 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb7 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb8 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb9 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb10 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb11 { get; set; }
        [Required(ErrorMessage = "此为必填项")]
        public int fb12 { get; set; }
        public int fb13 { get; set; }
        public int fb14 { get; set; }
        public int fb15 { get; set; }
        public string fb16 { get; set; }
        public string fb17 { get; set; }
        public string fb18 { get; set; }
        public string fb19 { get; set; }
        public string fb20 { get; set; }
        [Required(ErrorMessage ="参与方式必填")]
        public string fb21 { get; set; }
        public string fb22 { get; set; }
        public string fb23 { get; set; }
        public string fb24 { get; set; }
        public string fb25 { get; set; }
        public string fb26 { get; set; }
        public string fb27 { get; set; }
        public string fb28 { get; set; }
        public string fb29 { get; set; }
        public string fb30 { get; set; }
        public double 真实培训时间 { get; set; }

        public Training Training { get; set; }
        public int TrainingID { get; set; }


    }
}
