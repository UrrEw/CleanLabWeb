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
    public class ReserveTimeController : ControllerBase
    {
        private readonly ReserveTimeService _reserveTimeService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public ReserveTimeController(ReserveTimeService reserveTimeService,GetLoginClaimService getLoginClaimService)
        {
            _reserveTimeService = reserveTimeService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            var Data = _reserveTimeService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("create")]
        public IActionResult CreateReserveTime([FromBody]ReserveTime Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _reserveTimeService.InsertReserveTime(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ReadReserveTime(Guid Id)
        {
            try
            {
                ReserveTime Data = _reserveTimeService.GetDataById(Id);

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
        public IActionResult UpdateReserveTime(Guid Id,ReserveTime updateData)
        {
            var data = _reserveTimeService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.reservetime_id = Id;
            _reserveTimeService.UpdateReserveTime(updateData);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteReserveTime(Guid id)
        {
            _reserveTimeService.SoftDeleteReserveTimeById(id);
            return Ok();
        }
    }
}