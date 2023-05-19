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
    public class TesterController : ControllerBase
    {
        private readonly TesterService _testerService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public TesterController(TesterService testerService,GetLoginClaimService getLoginClaimService)
        {
            _testerService = testerService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            var Data = _testerService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateTester([FromBody]Tester Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _testerService.InsertTester(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadTester(Guid Id)
        {
            try
            {
                Tester Data = _testerService.GetDataById(Id);

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
        public IActionResult UpdateTester([FromQuery]Guid Id,[FromBody]Tester updateData)
        {
            var data = _testerService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.tester_id = Id;
            _testerService.UpdateTester(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteTester([FromQuery]Guid id)
        {
            _testerService.SoftDeleteTesterById(id);
            return Ok();
        }
    }
}