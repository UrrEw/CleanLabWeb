using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LabWeb.models
{
    public class Carousel
    {
        public Guid carousel_image_id {get;set;}

        public DateTime create_time {get;set;}

        public Guid create_id   {get;set;}

        public DateTime update_time {get;set;}

        public Guid update_id   {get;set;}

        public bool is_delete   {get;set;}

        public List<string>? carousel_image {get;set;}

        public IFormFile? FormImage { get; set; }

        public IFormFileCollection? MultiImages {get;set;}

        public string? image {get;set;}
    }
}