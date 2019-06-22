using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPP1.Models
{
    public class User
    {
        [Display(Name ="账户")]
        [Required(ErrorMessage ="{0}是必填项")]
        [MinLength(7, ErrorMessage = "{0}最小长度是7")]
        [MaxLength(18,ErrorMessage ="{0}最大长度是20")]
        public string Account { get; set; }
        [Display(Name = "密码")]
        [MinLength(7, ErrorMessage = "{0}最小长度是7")]
        [MaxLength(18, ErrorMessage = "{0}最大长度是20")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
