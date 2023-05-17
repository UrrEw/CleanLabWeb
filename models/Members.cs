using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Members
    {
        public Guid members_id {get;set;}
        
        public string? account {get;set;}

        public string? password {get;set;}

        public string? name {get;set;}

        public string? authcode {get;set;}

        public string?  email {get;set;}

        public int level {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id   {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id   {get;set;}
    }
}