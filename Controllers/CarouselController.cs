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
    public class CarouselController : ControllerBase
    {
        private readonly CarouselService _carouselService;
        private readonly GetImageService _getImageService;
        private readonly GetLoginClaimService _getLoginClaimService;
        public CarouselController(CarouselService carouselService,GetImageService getImageService,GetLoginClaimService getLoginClaimService)
        {
            _carouselService = carouselService;
            _getImageService = getImageService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var Data = _carouselService.GetAllData();
            return Ok(Data);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateCarousel([FromForm]Carousel Data)
        {
            try
            {
                Data.carousel_image = _getImageService.CreateMultiImage(Data.MultiImages);
                Data.create_id = _getLoginClaimService.GetMembers_id();
                Data.update_id = _getLoginClaimService.GetMembers_id();
                foreach(var item in Data.carousel_image)
                {
                    Data.image = item;
                    _carouselService.InsertCarousel(Data);
                }
                
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadCarousel([FromQuery]Guid Id)
        {
            try
            {
                Carousel Data = _carouselService.GetDataById(Id);

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
        public IActionResult UpdateCarousel([FromQuery]Guid Id,[FromForm]Carousel updateData)
        {
            
            var data = _carouselService.GetDataById(Id);
            
            if (data == null)
            {
                return NotFound();
            }
            
            if(updateData.MultiImages != null)
            {
                _getImageService.OldFileButMultiCheck(data.carousel_image);
                updateData.carousel_image = _getImageService.CreateMultiImage(updateData.MultiImages);
            }

            updateData.update_id = _getLoginClaimService.GetMembers_id();
            updateData.carousel_image_id = Id;
            _carouselService.UpdateCarousel(updateData);

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteCarousel([FromQuery]Guid id)
        {
            _carouselService.SoftDeleteCarouselById(id);
            return Ok();
        }
    }
}