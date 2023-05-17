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
    public class ActivityController : ControllerBase
    {
        private readonly ActivityService _activityService;
        private readonly GetImageService _getImageService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public ActivityController(ActivityService activityService,GetImageService getImageService,GetLoginClaimService getLoginClaimService)
        {
            _activityService = activityService;
            _getImageService = getImageService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var Data = _activityService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateActivity([FromForm]Activity Data)
        {
            try
            {
                Data.first_image = _getImageService.CreateOneImage(Data.FormImage);
                //Data.images = _getImageService.CreateMultiImage(Data.MultiImages);
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _activityService.InsertActivity(Data);
                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadActivity([FromQuery]Guid Id)
        {
            try
            {
                Activity Data = _activityService.GetDataById(Id);

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
        public IActionResult UpdateActivity([FromQuery]Guid Id,[FromForm]Activity updateData)
        {
            
            var data = _activityService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            if (updateData.FormImage != null)
            {
                _getImageService.OldFileCheck(data.first_image);
                updateData.first_image = _getImageService.CreateOneImage(updateData.FormImage);
            }
            /*if(updateData.MultiImages != null)
            {
                _getImageService.OldFileButMultiCheck(data.images);
                updateData.images = _getImageService.CreateMultiImage(updateData.MultiImages);
            }*/

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.activity_id = Id;
            _activityService.UpdateActivity(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteActivity([FromQuery]Guid id)
        {
            _activityService.SoftDeleteActivityById(id);
            return Ok();
        }
    }
}