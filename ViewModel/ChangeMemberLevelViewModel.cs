using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabWeb.ViewModel
{
    public class ChangeMemberLevelViewModel
    {
        public Guid members_id {get;set;}
        public int? level {get;set;}

        public Guid update_id   {get;set;}
    }
}