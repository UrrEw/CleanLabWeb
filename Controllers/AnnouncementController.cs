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
    public class AnnouncementController : ControllerBase
    {
        private readonly AnnouncementService _announcementService;
        private readonly GetLoginClaimService _getLoginClaimService;
        private readonly PagingService _PagingService;
        public AnnouncementController(AnnouncementService announcementService,GetLoginClaimService getLoginClaimService, PagingService PagingService)
        {
            _announcementService = announcementService;
            _getLoginClaimService = getLoginClaimService;
            _PagingService = PagingService;
        }

      [HttpGet("GetAllDataList")]
        public IActionResult GetAllData(int page = 1)
        {
            _PagingService.NowPage = page;
            var data = _announcementService.GetAllData(_PagingService);
            
            return Ok(data);
        }




        [HttpPost("CreateData")]
        public IActionResult CreateAnnouncement([FromBody]Announcement Data)
        {
            try
            {
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _announcementService.InsertAnnouncement(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadAnnouncement([FromQuery]Guid Id)
        {
            try
            {
                Announcement Data = _announcementService.GetDataById(Id);

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
        public IActionResult UpdateAnnouncement([FromQuery]Guid Id,[FromBody]Announcement updateData)
        {
            var data = _announcementService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }
            
            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.announce_id = Id;
            _announcementService.UpdateAnnouncement(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteAnnouncement([FromQuery]Guid id)
        {
            _announcementService.SoftDeleteAnnouncementById(id);
            return Ok();
        }
    }
}