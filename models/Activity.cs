using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Activity
    {
        public Guid activity_id {get;set;}

        [Required]
        [StringLength(50,ErrorMessage ="不可超過50個字")]
        public string? activity_title {get;set;}

        [Required]
        [StringLength(200,ErrorMessage ="不可超過200個字")]
        public string? activity_content {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id {get;set;}

        public bool is_delete {get;set;}

        public string? first_image {get;set;}

        public List<string>? images {get;set;}

        public IFormFile? FormImage { get; set; }

        //public IFormFileCollection? MultiImages {get;set;}
    }
}