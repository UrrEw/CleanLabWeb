using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Reply
    {
        public Guid reply_id {get;set;}

        [Required]
        [StringLength(200,ErrorMessage ="不能超過200個字")]
        public string? reply_content {get;set;}

        public string? reply_image {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id {get;set;}

        public Guid messageboard_id {get;set;}

        public bool is_delete {get;set;}

        public IFormFile? FormImage { get; set; }
        
    }
}