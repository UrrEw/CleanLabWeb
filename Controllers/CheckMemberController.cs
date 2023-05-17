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
    public class CheckMemberController : ControllerBase
    {
        private readonly CheckMemberService _checkMemberService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public CheckMemberController(CheckMemberService checkMemberService,GetLoginClaimService getLoginClaimService)
        {
            _checkMemberService = checkMemberService;
            _getLoginClaimService = getLoginClaimService;
        }

        #region 新增
        [HttpPost("create")]
        public IActionResult CreateCheckMember([FromBody]CheckMember Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _checkMemberService.InsertCheckMember(Data);
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
        public IActionResult ReadCheckMember([FromQuery]Guid Id)
        {
            try
            {
                CheckMember Data = _checkMemberService.GetDataById(Id);

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
            var Data = _checkMemberService.GetAllData();
            return Ok(Data);
        }
        #endregion

        #region 修改
        [HttpPut("UpdateData")]
        public IActionResult UpdateCheckMember([FromQuery]Guid Id,[FromForm]CheckMember updateData)
        {
            var data = _checkMemberService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.checkmember_id = Id;
            _checkMemberService.UpdateCheckMember(updateData);

            return Ok();
        }
        #endregion

        #region 刪除
        [HttpDelete("{id:guid}")]
        public IActionResult SoftDeleteCheckMemberById(Guid id)
        {
            _checkMemberService.SoftDeleteCheckMemberById(id);
            return Ok();
        }
        #endregion
   }
}