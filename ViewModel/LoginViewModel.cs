using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.ViewModel
{
    public class LoginViewModel
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage ="ENTER")]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "ENTER")]
        public string Password { get; set; }
    }
}