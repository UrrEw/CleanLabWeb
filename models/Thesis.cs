using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Thesis
    {
        public Guid thesis_id {get;set;}

        [Required]
        public Guid author_id {get;set;}

        [Required]
        [StringLength(50,ErrorMessage ="BADBAD")]
        public string? thesis_title {get;set;}

        public string? thesis_image {get;set;}

        [Required]
        [StringLength(1000,ErrorMessage ="BADBAD")]
        public string? thesis_abstract {get;set;}

        [Required]
        public int thesis_year  {get;set;}

        public DateTime create_time {get;set;}

        [Required]
        public Guid create_id   {get;set;}

        public DateTime update_time   {get;set;}

        [Required]
        public Guid update_id   {get;set;}

        public bool is_delete {get;set;}

        public IFormFile? FormImage {get;set;}
    }
}