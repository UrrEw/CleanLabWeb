using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace LabWeb.Service
{
    public class PagingService
    {
        public int NowPage { get; set; }
        public int MaxPage { get; set; }

        public int ItemNum
        {
            get
            {
                return 5;
            }
        }

        public PagingService()
        {
            this.NowPage = 1;
        }

        public PagingService(int Page)
        {
            this.NowPage = Page;
        }

        public void SetRightPage()
        {
            if(this.NowPage < 1)
            {
                this.NowPage = 1;
            }
            else if(this.NowPage > this.MaxPage)
            {
                this.NowPage = this.MaxPage;
            }
            if (this.MaxPage.Equals(0))
            {
                this.NowPage = 1;
            }
        }
    }
}