using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Announcement
    {
        public Guid announce_id {get;set;}

        [Required]
        [StringLength(50,ErrorMessage ="不能超過50個字")]
        public string? announce_title {get;set;}

        [Required]
        [StringLength(500,ErrorMessage ="不能超過500個字")]
        public string? announce_content {get;set;}
        
        public DateTime create_time {get;set;}

        public Guid create_id {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id {get;set;}

        public bool is_delete {get;set;}

    }
}