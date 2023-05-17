using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Award_Members
    {
        public Guid awardmember_id {get;set;}

        public Guid members_id {get;set;}

        public Guid contest_id {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id   {get;set;}

        public DateTime update_time   {get;set;}

        public Guid update_id   {get;set;}

        public Members? members {get;set;} 

        public Contest_Award? contest_award {get;set;}
    }
}