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
    public class ReplyController : ControllerBase
    {
        private readonly ReplyService _replyService;
        private readonly GetImageService _getImageService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public ReplyController(ReplyService replyService,GetImageService getImageService,GetLoginClaimService getLoginClaimService)
        {
            _replyService = replyService;
            _getImageService = getImageService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var Data = _replyService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateReply([FromForm]Reply Data)
        {
            try
            {
                Data.reply_image = _getImageService.CreateOneImage(Data.FormImage);
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                _replyService.InsertReply(Data);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadReply([FromQuery]Guid Id)
        {
            try
            {
                Reply Data = _replyService.GetDataById(Id);

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
        public IActionResult UpdateReply([FromQuery]Guid Id,[FromForm]Reply updateData)
        {
            var data = _replyService.GetDataById(Id);

            if (data == null)
            {
                return NotFound();
            }

            if (updateData.FormImage != null)
            {
                _getImageService.OldFileCheck(data.reply_image);
                updateData.reply_image = _getImageService.CreateOneImage(updateData.FormImage);
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.reply_id = Id;
            _replyService.UpdateReply(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteReply([FromQuery]Guid id)
        {
            _replyService.SoftDeleteReplyById(id);
            return Ok();
        }
    }
}