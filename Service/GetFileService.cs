using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;

namespace LabWeb.Service
{
    public class GetFileService
    {
        public string? CreateOneFile(IFormFile FormFile)
        {

            if (FormFile != null)
            {
                if(!FormFile.ContentType.StartsWith("application/"))
                {
                    return "檔案格式不正確";
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(FormFile.FileName);

                var filePath = Path.Combine("File", uniqueFileName);
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    FormFile.CopyTo(stream);
                }

                return uniqueFileName;
            }
            else
            {
                return null;
            }
        }

        public void OldFileCheck(string file)
        {
            if(!string.IsNullOrEmpty(file))
            {
                var filepath = Path.Combine("File", file);
                string oldFilePath = filepath;
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
        }
    }
}