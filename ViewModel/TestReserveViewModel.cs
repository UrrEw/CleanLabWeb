using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabWeb.ViewModel
{
    public class TestReserveViewModel
    {
        public Guid test_id {get;set;}
        public Guid proctor_id {get;set;}
        public Guid members_id {get;set;}
        public Guid create_id {get;set;}
        public Guid update_id {get;set;}
        public Guid reservetime_id {get;set;}
        public string? test_title {get;set;}
        public string? tester_name {get;set;}
        public string? proctor_name {get;set;}
        public DateTime reservedate {get;set;}
        public TimeSpan reservetime {get;set;}
        
        public bool is_success {get;set;}
    }
}