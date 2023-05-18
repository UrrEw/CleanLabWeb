using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Test
    {
        public Guid test_id {get;set;}

        [Required]
        [StringLength(40,ErrorMessage ="BADBAD")]
        public string? test_title {get;set;}

        [Required]
        [StringLength(100,ErrorMessage ="BADBAD")]
        public string? test_content {get;set;}

        [Required]
        public DateTime start_date {get;set;}

        [Required]
        public DateTime end_date {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id {get;set;}

        public bool is_delete {get;set;}

        public string? Status {get;set;}

        public bool is_success {get;set;}
    }
}