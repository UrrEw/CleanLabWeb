using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LabWeb.models;

namespace LabWeb.ViewModel
{
    public class ChangePasswordViewModel
    {
        public string Account { get; set; }
        
        [DisplayName("舊密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
        [DisplayName("新密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string NewPassword { get; set; }

    }
}