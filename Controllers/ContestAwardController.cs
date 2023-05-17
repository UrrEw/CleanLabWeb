using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.models;
using Newtonsoft.Json;

namespace LabWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContestAwardController : ControllerBase
    {
        private readonly ContestAwardService _contestawardService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public ContestAwardController(ContestAwardService contestawardService,GetLoginClaimService getLoginClaimService)
        {
            _contestawardService = contestawardService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var Data = _contestawardService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateContestAward([FromBody]Contest_Award Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _contestawardService.InsertContestAward(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadContestAward([FromQuery]Guid Id)
        {
            try
            {
                Contest_Award Data = _contestawardService.GetDataById(Id);

                if(Data == null)
                {
                    return BadRequest("NODATA");
                }

                return Ok(Data);
            }
            catch(Exception e)
            {
                return StatusCode(55688, e.Message);
            }
        }

        [HttpPut("UpdateData")]
        public IActionResult UpdateContestAward([FromQuery]Guid Id,[FromBody]Contest_Award updateData)
        {
            var data = _contestawardService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }
            
            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.contest_id = Id;
            _contestawardService.UpdateContestAward(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteContestAward([FromQuery]Guid id)
        {
            _contestawardService.SoftDeleteContestAwardById(id);
            return Ok();
        }
    }
}