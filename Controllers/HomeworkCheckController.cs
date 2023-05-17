using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.models;

namespace LabWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeworkCheckController : ControllerBase
    {
        private readonly HomeworkCheckService _homeworkCheckService;
        private readonly GetFileService _getFileService;
        private readonly GetLoginClaimService _getLoginClaimService;
        private readonly HomeworkService _homeworkservice;
        private readonly IWebHostEnvironment _env;

        public HomeworkCheckController(HomeworkCheckService homeworkCheckService,GetLoginClaimService getLoginClaimService, GetFileService getFileService,IWebHostEnvironment env,HomeworkService homeworkservice)
        {
            _homeworkCheckService = homeworkCheckService;
            _getLoginClaimService = getLoginClaimService;
            _getFileService = getFileService;
            _homeworkservice = homeworkservice;
            _env = env;
        }

        #region 新增
        [HttpPost("create")]
        public IActionResult CreateHomeworkCheck([FromForm]HomeworkCheck Data)
        {
            try
            {
                var data = _homeworkservice.GetDataById(Data.homework_id);
                Data.check_file = _getFileService.CreateOneFile(Data.formfile);
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                Data.student_name = _getLoginClaimService.GetMembers_id();
                Data.check_note = "加油";
                Data.check_member = data.check_id;
                _homeworkCheckService.InsertHomeworkCheck(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }
        #endregion

        #region 搜尋
        [HttpGet("ReadOneData")]
        public IActionResult ReadHomeworkCheck([FromQuery]Guid Id)
        {
            try
            {
                HomeworkCheck Data = _homeworkCheckService.GetDataById(Id);

                if(Data == null)
                {
                    return BadRequest("NODATA");
                }

                return Ok(Data);
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadByName")]
        public IActionResult ReadByName([FromQuery]Guid Id)
        {
            try
            {
                var Data = _homeworkCheckService.GetDataByName(Id);

                if(Data == null)
                {
                    return BadRequest("NODATA");
                }

                return Ok(Data);
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadByHomework")]
        public IActionResult ReadByHomework([FromQuery]Guid Id)
        {
            try
            {
                var Data = _homeworkCheckService.GetDataByHomework(Id);

                if(Data == null)
                {
                    return BadRequest("NODATA");
                }

                return Ok(Data);
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }
        
        [HttpGet]
        public IActionResult GetAllData()
        {
            var Data = _homeworkCheckService.GetAllData();
            return Ok(Data);
        }
        #endregion

        #region 修改
        [HttpPut("UpdateData")]
        public IActionResult UpdateHomeworkCheck([FromQuery]Guid Id,[FromForm]HomeworkCheck updateData)
        {
            var data = _homeworkCheckService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

             if (updateData.formfile != null)
            {
                _getFileService.OldFileCheck(data.check_file);
                updateData.check_file = _getFileService.CreateOneFile(updateData.formfile);
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.homeworkcheck_id = Id;
            _homeworkCheckService.UpdateHomeworkCheck(updateData);

            return Ok();
        }
        #endregion

        #region 刪除
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteHomeworkCheck(Guid id)
        {
            _homeworkCheckService.SoftDeleteHomeworkCheckById(id);
            return Ok();
        }
        #endregion

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            string filePath = Path.Combine(_env.ContentRootPath, "Files", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

   }
}