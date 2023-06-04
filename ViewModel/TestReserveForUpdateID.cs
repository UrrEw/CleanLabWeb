using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabWeb.ViewModel
{
    public class TestReserveForUpdateID
    {
        public Guid Oldproctor_id {get;set;}
        public Guid Oldreservetime_id {get;set;}

        public Guid Newmember_id {get;set;}
        public Guid Newreservetime_id {get;set;}

        public DateTime reservedate {get;set;}
        public TimeSpan reservetime {get;set;}

        public Guid update_id {get;set;}
    }
}