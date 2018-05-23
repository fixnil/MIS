using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GISCore
{
    public class User
    {
        public int Id { get; set; }

        [DisplayName("用户名")]
        [Required(ErrorMessage = "{0} 为必填项!")]
        [StringLength(3, ErrorMessage = "{0} 长度为 3!", MinimumLength = 3)]
        public string UserName { get; set; }

        [DisplayName("密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} 为必填项!")]
        [StringLength(256, ErrorMessage = "{0} 长度在 {2}-{1} 之间!", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
