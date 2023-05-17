using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabWeb.ViewModel
{
    public class SeniorProjectViewModel
    {
        public Guid seniorproject_id {get;set;}
        public string? senior_title  {get;set;}

        public int senior_year  {get;set;}

        public List<Guid>? members_id {get;set;}

        public IFormFile? FormImage { get; set; }

        public string? senior_content   {get;set;}

        public string? name {get;set;}

        public string? senior_image   {get;set;}
    }
}