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

        // public async Task<IActionResult> GetFileAsync(string filePath)
        // {
        //     var memoryStream = new MemoryStream();

        //     try
        //     {
        //         using (var fileStream = new FileStream(filePath, FileMode.Open))
        //         {
        //             await fileStream.CopyToAsync(memoryStream);
        //         }

        //         memoryStream.Position = 0;

        //         var contentType = "application/octet-stream";

        //         var fileName = Path.GetFileName(filePath);

        //         return File(memoryStream, contentType, fileName);
        //     }
        //     catch (Exception e)
        //     {
        //         throw new Exception(e.Message.ToString());
        //     }
        //     finally
        //     {
        //         memoryStream.Dispose();
        //     }
        // }

        // public async Task<byte[]> ReadFileAsync(string filePath)
        // {
        //     if (string.IsNullOrEmpty(filePath))
        //     {
        //         return null;
        //     }

        //     try
        //     {
        //         using (var fileStream = new FileStream(filePath, FileMode.Open))
        //         {
        //             var memoryStream = new MemoryStream();

        //             await fileStream.CopyToAsync(memoryStream);

        //             return memoryStream.ToArray();
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error: {ex.Message}");
        //         return null;
        //     }
        // }

    }
}