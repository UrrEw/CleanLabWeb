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
        public HomeworkCheckController(HomeworkCheckService homeworkCheckService,GetLoginClaimService getLoginClaimService, GetFileService getFileService)
        {
            _homeworkCheckService = homeworkCheckService;
            _getLoginClaimService = getLoginClaimService;
            _getFileService = getFileService;
        }

        #region 新增
        [HttpPost("create")]
        public IActionResult CreateHomeworkCheck([FromForm]HomeworkCheck Data)
        {
            try
            {
                Data.check_file = _getFileService.CreateOneFile(Data.formfile);
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
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
   }
}