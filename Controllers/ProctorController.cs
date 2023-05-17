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
    public class ProctorController : ControllerBase
    {
        private readonly ProctorService _proctorService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public ProctorController(ProctorService proctorService,GetLoginClaimService getLoginClaimService)
        {
            _proctorService = proctorService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            var Data = _proctorService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("create")]
        public IActionResult CreateProctor([FromBody]Proctor Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _proctorService.InsertProctor(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ReadProctor(Guid Id)
        {
            try
            {
                Proctor Data = _proctorService.GetDataById(Id);

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
        public IActionResult UpdateProctor(Guid Id,Proctor updateData)
        {
            var data = _proctorService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }
            
            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.proctor_id = Id;
            _proctorService.UpdateProctor(updateData);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProctor(Guid id)
        {
            _proctorService.SoftDeleteProctorById(id);
            return Ok();
        }
    }
}