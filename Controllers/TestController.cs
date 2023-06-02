using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.models;
using LabWeb.Secruity;

namespace LabWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestService _testService;
        private readonly GetLoginClaimService _getLoginClaimService;
        private readonly JwtService _jwtService;

        private readonly MembersDBService _membersDBService;
        public TestController(TestService testService,GetLoginClaimService getLoginClaimService,JwtService jwtService,MembersDBService membersDBService)
        {
            _testService = testService;
            _getLoginClaimService = getLoginClaimService;
            _jwtService = jwtService;
            _membersDBService = membersDBService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var members_id = _getLoginClaimService.GetMembers_id();
            var Data = _testService.GetAllData(members_id);

            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateTest([FromBody]Test Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _testService.InsertTest(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadTest(Guid Id)
        {
            try
            {
                Test Data = _testService.GetDataById(Id);

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

        [HttpPut("UpdateData")]
        public IActionResult UpdateTest([FromQuery]Guid Id,[FromBody]Test updateData)
        {
            var data = _testService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }
            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.test_id = Id;
            _testService.UpdateTest(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteTest([FromQuery]Guid id)
        {
            _testService.SoftDeleteTestById(id);
            return Ok();
        }
    }
}