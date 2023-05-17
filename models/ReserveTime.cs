using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class ReserveTime
    {
        public Guid reservetime_id {get;set;}

        public Guid proctor_id {get;set;}

        [Required]
        public DateTime reservedate {get;set;}

        [Required]
        public TimeSpan reservetime {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id {get;set;}

        public bool is_delete {get;set;}
    }
}