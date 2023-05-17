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
    public class HomeworkController : ControllerBase
    {
        private readonly HomeworkService _homeworkservice;
        private readonly GetLoginClaimService _getLoginClaimService;
        public HomeworkController(HomeworkService homeworkservice,GetLoginClaimService getLoginClaimService)
        {
            _homeworkservice = homeworkservice;
            _getLoginClaimService = getLoginClaimService;
        }

        #region 新增
        [HttpPost("create")]
        public IActionResult CreateHomework([FromBody]Homework Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _homeworkservice.InsertHomework(Data);
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
        public IActionResult ReadHomework([FromQuery]Guid Id)
        {
            try
            {
                Homework Data = _homeworkservice.GetDataById(Id);

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
            var Data = _homeworkservice.GetAllData();
            return Ok(Data);
        }
        #endregion

        #region 修改
        [HttpPut("UpdateData")]
        public IActionResult UpdateHomework([FromQuery]Guid Id,[FromForm]Homework updateData)
        {
            var data = _homeworkservice.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.homework_id = Id;
            _homeworkservice.UpdateHomework(updateData);

            return Ok();
        }
        #endregion

        #region 刪除
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteTest(Guid id)
        {
            _homeworkservice.SoftDeleteHomeworkById(id);
            return Ok();
        }
        #endregion
   }
}