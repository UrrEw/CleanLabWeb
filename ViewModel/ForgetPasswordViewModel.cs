using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LabWeb.models;

namespace LabWeb.ViewModel
{
    public class ForgetPasswordViewModel
    {
        public string account { get; set; }
        public string? email {get;set;}
    }
}