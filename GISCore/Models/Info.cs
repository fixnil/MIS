using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GISCore
{
    public class Info
    {
        public int Id { get; set; }

        [DisplayName("当前温度")]
        [Required(ErrorMessage = "{0} 为必填项!")]
        public string Temp { get; set; }

        [DisplayName("当前湿度")]
        [Required(ErrorMessage = "{0} 为必填项!")]
        public string Hum { get; set; }

        [DisplayName("氨气浓度")]
        [Required(ErrorMessage = "{0} 为必填项!")]
        public string NH3 { get; set; }

        [DisplayName("当前光照")]
        [Required(ErrorMessage = "{0} 为必填项!")]
        public string Light { get; set; }
        
        public DateTime Time { get; set; }
    }
}
