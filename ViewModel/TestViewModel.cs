using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabWeb.ViewModel
{
    public class TestViewModel
    {
        public Guid test_id {get;set;}

        public string? test_title {get;set;}

        public string? test_content {get;set;}

        public DateTime start_date {get;set;}

        public DateTime end_date {get;set;}
        public string? Status {get;set;}
    }
}