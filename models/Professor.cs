using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Professor
    {
        public Guid professor_id {get;set;}

        [Required]
        [StringLength(20,ErrorMessage ="不能超過20個字")]
        public string? professor_name {get;set;}

        [Required]
        [StringLength(50,ErrorMessage ="不能超過50個字")]
        public string? professor_position {get;set;}

        [Required]
        [StringLength(50,ErrorMessage ="不能超過50個字")]
        public string? professor_school {get;set;}

        [Required]
        [StringLength(50,ErrorMessage ="不能超過50個字")]
        public string? professor_study {get;set;}
        
        [Required]
        [StringLength(100,ErrorMessage ="不能超過100個字")]
        public string? professor_major {get;set;}

        [Required]
        [EmailAddress(ErrorMessage ="不是Email格式")]
        [StringLength(50,ErrorMessage ="不能超過50個字")]
        public string? professor_email {get;set;}

        [Required]
        [StringLength(20,ErrorMessage ="不能超過20個字")]
        public string? professor_tel {get;set;}

        [Required]
        [StringLength(60,ErrorMessage ="不能超過60個字")]
        public string? professor_office {get;set;}

        public string? professor_image {get;set;}

        public IFormFile? FormImage { get; set; }
    }
}