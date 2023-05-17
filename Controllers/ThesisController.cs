using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace LabWeb.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ThesisController : ControllerBase
    {
        private readonly ThesisService _thesisService;
        private readonly GetImageService _getImageService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public ThesisController(ThesisService thesisService,GetImageService getImageService,GetLoginClaimService getLoginClaimService)
        {
            _thesisService = thesisService;
            _getImageService = getImageService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var Data = _thesisService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateThesis([FromForm]Thesis Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                Data.thesis_image = _getImageService.CreateOneImage(Data.FormImage);
                _thesisService.InsertThesis(Data);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(555,e.Message);
            }
        }

        [Authorize(Roles="Admin")]
        [HttpGet("ReadOneData")]
        public IActionResult ReadThesis(Guid Id)
        {
            Thesis Data = _thesisService.GetDataById(Id);

            if(Data == null)
            {
                return BadRequest("NODATA");
            }

            return Ok(Data);
        }

        [HttpPut("UpdateData")]
        public IActionResult UpdateThesis([FromQuery]Guid Id,[FromForm]Thesis updateData)
        {
            var data = _thesisService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            if (updateData.FormImage != null)
            {
                _getImageService.OldFileCheck(data.thesis_image);
                updateData.thesis_image = _getImageService.CreateOneImage(updateData.FormImage);
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.thesis_id = Id;
            _thesisService.UpdateThesis(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteThesis([FromQuery]Guid id)
        {
            _thesisService.SoftDeleteThesisById(id);
            return Ok();
        }
    }
}