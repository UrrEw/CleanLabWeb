using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class HomeworkCheck
    {
        public Guid homeworkcheck_id {get;set;}
        public Guid homework_id{get; set;}
        public Guid student_name {get;set;}
        public Guid check_member {get;set;}
        public string? name {get;set;}
        public bool check_result {get;set;}
        public string? check_note {get;set;}
        public DateTime finishtime {get;set;}
        public string? check_file {get; set;}
        public DateTime create_time {get;set;}

        public Guid create_id   {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id   {get;set;}

        public bool is_delete {get;set;}
        public IFormFile? formfile {get; set;}
    }
}