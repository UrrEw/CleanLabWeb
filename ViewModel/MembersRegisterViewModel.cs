using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LabWeb.models;

namespace LabWeb.ViewModel
{
    public class MembersRegisterViewModel
    {

        public string? account {get;set;}
        public string? name {get;set;}

        public string? authcode {get;set;}

        public string?  email {get;set;}

        public int level {get;set;}
        [DisplayName("密碼")]
        [Required(ErrorMessage ="請輸入密碼")]
        public string password { get; set; }
        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        [Compare("password",ErrorMessage ="兩次密碼輸入錯誤")]
        public string passwordCheck { get; set; }
    }
}