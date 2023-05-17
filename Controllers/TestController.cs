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
    public class TestController : ControllerBase
    {
        private readonly TestService _testService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public TestController(TestService testService,GetLoginClaimService getLoginClaimService)
        {
            _testService = testService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            var Data = _testService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("create")]
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

        [HttpGet("{id}")]
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

        [HttpPut("{id:guid}")]
        public IActionResult UpdateTest(Guid Id,Test updateData)
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

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteTest(Guid id)
        {
            _testService.SoftDeleteTestById(id);
            return Ok();
        }
    }
}