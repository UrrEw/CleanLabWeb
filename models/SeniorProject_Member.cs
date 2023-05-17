using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class SeniorProject_Member
    {
        public Guid seniormember_id  {get;set;}

        public Guid seniorproject_id {get;set;}

        public Guid members_id {get;set;}

        // public List<Guid>? updatemembers_id {get;set;}

        public string? name {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id   {get;set;}

        public DateTime update_time   {get;set;}

        public Guid update_id   {get;set;}

        public bool is_delete {get;set;}
    }
}