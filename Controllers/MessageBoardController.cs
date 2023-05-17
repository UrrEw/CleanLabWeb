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
    public class MessageBoardController : ControllerBase
    {
        private readonly MessageBoardService _messageboardService;
        private readonly GetImageService _getImageService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public MessageBoardController(MessageBoardService messageboardService,GetImageService getImageService,GetLoginClaimService getLoginClaimService)
        {
            _messageboardService = messageboardService;
            _getImageService = getImageService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var Data = _messageboardService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateMessageBoard([FromForm]MessageBoard Data)
        {
            try
            {   
                Data.messageboard_image = _getImageService.CreateOneImage(Data.FormImage);
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _messageboardService.InsertMessageBoard(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadMessageBoard([FromQuery]Guid Id)
        {
            try
            {
                MessageBoard Data = _messageboardService.GetDataById(Id);

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
        public IActionResult UpdateMessageBoard([FromQuery]Guid Id,[FromForm]MessageBoard updateData)
        {
            var data = _messageboardService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            if (updateData.FormImage != null)
            {
                _getImageService.OldFileCheck(data.messageboard_image);
                updateData.messageboard_image = _getImageService.CreateOneImage(updateData.FormImage);
            }
            
            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.messageboard_id = Id;
            _messageboardService.UpdateMessageBoard(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteMessageBoard([FromQuery]Guid id)
        {
            _messageboardService.SoftDeleteMessageBoardById(id);
            return Ok();
        }
    }
}