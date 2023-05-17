using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class SeniorProject
    {
        public Guid seniorproject_id   {get;set;}

        [Required]
        [StringLength(50,ErrorMessage ="不能超過50個字")]
        public string? senior_title  {get;set;}

        [Required]
        [StringLength(200,ErrorMessage ="不能超過200個字")]
        public string? senior_content   {get;set;}

        public string? senior_image   {get;set;}

        [Required]
        public int senior_year  {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id   {get;set;}

        public DateTime update_time   {get;set;}

        public Guid update_id   {get;set;}

        public bool is_delete {get;set;}

        public IFormFile? FormImage { get; set; }

    }
}