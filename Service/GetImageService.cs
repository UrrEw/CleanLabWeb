using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;

namespace LabWeb.Service
{
    public class GetImageService
    {
        public string? CreateOneImage(IFormFile FormImage)
        {

            if (FormImage != null)
            {
                if(!FormImage.ContentType.StartsWith("image/"))
                {
                    return "檔案格式不正確";
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(FormImage.FileName);

                var filePath = Path.Combine("wwwroot/Image", uniqueFileName);
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    FormImage.CopyTo(stream);
                }

                return uniqueFileName;
            }
            else
            {
                return null;
            }
        }

        public List<string>? CreateMultiImage(IFormFileCollection MultiImages)
        {
            var imagePaths = new List<string>();
            if(MultiImages != null)
            {
                
                foreach(var formfile in MultiImages)
                {
                    if(!formfile.ContentType.StartsWith("image/"))
                    {
                        return null;
                    }
                    
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(formfile.FileName);

                    var filePath = Path.Combine("wwwroot/Image", uniqueFileName);
                    using(var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formfile.CopyTo(stream);
                    }

                    imagePaths.Add(uniqueFileName);
                }
                return imagePaths;
            }
            else
            {
                return null;
            }
        }

        public void OldFileCheck(string image)
        {
            if(!string.IsNullOrEmpty(image))
            {
                var filepath = Path.Combine("wwwroot/Image", image);
                string oldFilePath = filepath;
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
        }

        public void OldFileButMultiCheck(List<string> images)
        {
            if(images != null && images.Any())
            {
                foreach (var FileName in images)
                {
                    var oldFilePath = Path.Combine("wwwroot/Image", FileName);
                    
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
        }
    }
}