using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Contest_Award
    {
        public Guid contest_id  {get;set;}

        [Required]
        public int contest_year {get;set;}

        [Required]
        [StringLength(100,ErrorMessage ="不能超過100個字")]
        public string? contest_name {get;set;}

        [Required]
        [StringLength(100,ErrorMessage ="不能超過100個字")]
        public string? contest_work  {get;set;}

        [Required]
        [StringLength(100,ErrorMessage ="不能超過10個字")]
        public string? contest_rank  {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id   {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id   {get;set;}

        public bool is_delete   {get;set;}
    }
}