using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LabWeb.models;
using LabWeb.Service;

namespace LabWeb.models
{
    public class Paging
    {
      public PagingService Page { get; set; }
    }
}