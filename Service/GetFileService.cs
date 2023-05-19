using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.AspNetCore.Mvc;
using System.IO;


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
                    return "檔案格式錯誤";
                }

                string uniqueFileName = FormFile.FileName;

                var filePath = Path.Combine("wwwroot/File", uniqueFileName);
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
                var filepath = Path.Combine("wwwroot/File", file);
                string oldFilePath = filepath;
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
        }
    }
}