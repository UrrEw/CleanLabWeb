using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.models;
using Newtonsoft.Json;
using LabWeb.ViewModel;


namespace LabWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeniorProjectController : ControllerBase
    {
        private readonly SeniorProjectService _seniorprojectService;
        private readonly GetImageService _getImageService;
        private readonly GetLoginClaimService _getLoginClaimService;
        private readonly SeniorProject_MemberService _seniorProject_MemberService;
        public SeniorProjectController(SeniorProjectService seniorprojectService,GetImageService getImageService,GetLoginClaimService getLoginClaimService,SeniorProject_MemberService seniorProject_MemberService)
        {
            _seniorprojectService = seniorprojectService;
            _getImageService = getImageService;
            _getLoginClaimService = getLoginClaimService;
            _seniorProject_MemberService = seniorProject_MemberService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var SeniorData = _seniorprojectService.GetAllData();
            var SeniorMemberData = _seniorProject_MemberService.GetAllData();

            var DataList = SeniorData.Join(SeniorMemberData, 
                    seniorData => seniorData.seniorproject_id, 
                    memberData => memberData.seniorproject_id,
                    (seniorData, memberData) => new SeniorProjectViewModel {
                    seniorproject_id = seniorData.seniorproject_id,
                    senior_title = seniorData.senior_title,
                    senior_year = seniorData.senior_year,
                    senior_content = seniorData.senior_content,
                    senior_image = seniorData.senior_image,
                    name = memberData.name
                    }).ToList();
            
            return Ok(DataList);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateSeniorProject([FromForm]SeniorProjectViewModel Data)
        {
            try
            {
                SeniorProject seniorData = new SeniorProject();
                SeniorProject_Member memberData = new SeniorProject_Member();

                seniorData.senior_title = Data.senior_title;
                seniorData.senior_year = Data.senior_year;
                seniorData.senior_content = Data.senior_content;
                seniorData.senior_image = _getImageService.CreateOneImage(Data.FormImage);
                seniorData.create_id = _getLoginClaimService.GetMembers_id();
                seniorData.update_id = _getLoginClaimService.GetMembers_id();
                _seniorprojectService.InsertSeniorProject(seniorData);

                foreach(var item in Data.members_id)
                {
                    memberData.seniorproject_id = seniorData.seniorproject_id;
                    memberData.members_id = item;
                    memberData.create_id = _getLoginClaimService.GetMembers_id();
                    memberData.update_id = _getLoginClaimService.GetMembers_id();
                    _seniorProject_MemberService.InsertSeniorProject_Member(memberData);
                }
                
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadSeniorProject([FromQuery]Guid Id)
        {
            try
            {
                var seniorData = _seniorprojectService.GetDataById(Id);
                if(seniorData == null)
                {
                    return BadRequest("NODATA");
                }
                var memberData = _seniorProject_MemberService.GetDataById(Id);

                SeniorProjectViewModel Data = new SeniorProjectViewModel();
                Data.seniorproject_id = seniorData.seniorproject_id;
                Data.senior_title = seniorData.senior_title;
                Data.senior_year = seniorData.senior_year;
                Data.senior_content = seniorData.senior_content;
                Data.senior_image = seniorData.senior_image;
                
                Data.name = memberData.name;

                return Ok(Data);
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpPut("UpdateData")]
        public IActionResult UpdateSeniorProject([FromQuery]Guid Id,[FromForm]SeniorProjectViewModel updateData)
        {
            var seniorData = _seniorprojectService.GetDataById(Id);
            var memberData = _seniorProject_MemberService.GetDataBySeniorProjectId(Id);

            if (seniorData == null)
            {
                return NotFound();
            }

            seniorData.senior_title = updateData.senior_title;
            seniorData.senior_year = updateData.senior_year;
            seniorData.senior_content = updateData.senior_content;
            if (updateData.FormImage != null)
            {
                _getImageService.OldFileCheck(seniorData.senior_image);
                seniorData.senior_image = _getImageService.CreateOneImage(updateData.FormImage);
            }
            seniorData.update_id = _getLoginClaimService.GetMembers_id();
            seniorData.seniorproject_id = Id;
            _seniorprojectService.UpdateSeniorProject(seniorData);

                foreach(var item in memberData)
                {
                    _seniorProject_MemberService.SoftDeleteSeniorProject_MemberByMemberId(item.members_id);
                }
                foreach(var newid in updateData.members_id)
                {
                    var i = 0;
                    var member = memberData[i];
                    var item = updateData.members_id[i];
                    
                    member.seniorproject_id = seniorData.seniorproject_id;
                    member.members_id = newid;
                    member.create_id = _getLoginClaimService.GetMembers_id();
                    member.update_id = _getLoginClaimService.GetMembers_id();
                    _seniorProject_MemberService.InsertSeniorProject_Member(member);
                    i+= 1;        
                }
            
            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteSeniorProject([FromQuery]Guid id)
        {
            _seniorprojectService.SoftDeleteSeniorProjectById(id);
            return Ok();
        }
    }
}