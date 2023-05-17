using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class TestReserve
    {
        public Guid testreserve_id {get;set;}

        public Guid test_id {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id {get;set;}
        
        public DateTime update_time {get;set;}

        public Guid update_id {get;set;}

        public bool is_delete {get;set;}
    }
}