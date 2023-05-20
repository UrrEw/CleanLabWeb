using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;


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
                    throw new FormatException("檔案格式不正確");
                }

                var maxFileSizeInBytes = 100 * 1024 * 1024; 
                if (FormFile.Length > maxFileSizeInBytes)
                {
                    throw new InvalidOperationException("檔案大小超過限制");
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

        public async Task<FileDownload> FileDownload(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/File", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return new FileDownload
            {
                Bytes = bytes,
                ContentType = contentType,
                Filename = filename
            };
        }
    }
}