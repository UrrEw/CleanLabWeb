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
    public class ProfessorController : ControllerBase
    {
        private readonly ProfessorService _professorService;
        private readonly GetImageService _getImageService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public ProfessorController(ProfessorService professorService,GetImageService getImageService,GetLoginClaimService getLoginClaimService)
        {
            _professorService = professorService;
            _getImageService = getImageService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var Data = _professorService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateProfessor([FromForm]Professor Data)
        {
            try
            {
                Data.professor_image = _getImageService.CreateOneImage(Data.FormImage);
                _professorService.InsertProfessor(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadProfessor([FromQuery]Guid Id)
        {
            try
            {
                Professor Data = _professorService.GetDataById(Id);

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
        public IActionResult UpdateProfessor([FromQuery]Guid Id,[FromForm]Professor updateData)
        {
            var data = _professorService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            if (updateData.FormImage != null)
            {
                _getImageService.OldFileCheck(data.professor_image);
                updateData.professor_image = _getImageService.CreateOneImage(updateData.FormImage);
            }

            updateData.professor_id = Id;
            _professorService.UpdateProfessor(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteProfessor([FromQuery]Guid id)
        {
            _professorService.DeleteProfessorById(id);
            return Ok();
        }
    }
}