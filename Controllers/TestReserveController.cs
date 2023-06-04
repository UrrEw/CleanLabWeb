using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.models;
using LabWeb.ViewModel;
using LabWeb.Secruity;

namespace LabWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestReserveController : ControllerBase
    {
        private readonly ProctorService _proctorService;
        private readonly ReserveTimeService _reserveTimeService;
        private readonly TesterService _testerService;
        private readonly TestReserveService _testReserveService;
        private readonly GetLoginClaimService _getLoginClaimService;
        private readonly JwtService _jwtService;
        private readonly MembersDBService _membersDBService;
        public TestReserveController(ProctorService proctorService,ReserveTimeService reserveTimeService,
                                    TesterService testerService,TestReserveService testReserveService,GetLoginClaimService getLoginClaimService
                                    ,JwtService jwtService,MembersDBService membersDBService)
        {
            _proctorService = proctorService;
            _reserveTimeService = reserveTimeService;
            _testerService = testerService;
            _testReserveService = testReserveService;
            _getLoginClaimService = getLoginClaimService;
            _jwtService = jwtService;
            _membersDBService = membersDBService;
        }

        // [HttpGet("GetAllDataList")]
        // public IActionResult GetAllData()
        // {
        //     var proctorData = _proctorService.GetAllData();
        //     var reserveTimeData = _reserveTimeService.GetAllData();
        //     var testerData = _testerService.GetAllData();
        //     var DataList = new List<TestReserveViewModel>();

        //     foreach(var combinedData in proctorData
        //                 .Zip(reserveTimeData, (proctor, reserveTime) => new { Proctor = proctor, ReserveTime = reserveTime })
        //                 .Zip(testerData, (combined, tester) => new { Proctor = combined.Proctor, ReserveTime = combined.ReserveTime, Tester = tester }))
        //     {
        //         var Data = new TestReserveViewModel()
        //         {
        //             test_id = combinedData.Proctor.test_id,
        //             test_title = combinedData.Proctor.test_title,
        //             tester_name = combinedData.Tester.name,
        //             proctor_id = combinedData.Proctor.proctor_id,
        //             proctor_name = combinedData.Proctor.name,
        //             reservedate = combinedData.ReserveTime.reservedate,
        //             reservetime = combinedData.ReserveTime.reservetime,
        //             is_success = combinedData.Tester.is_success,
        //             is_fail = combinedData.Tester.is_pass,
        //             tester_id = combinedData.Tester.tester_id,
        //             members_id = combinedData.Tester.members_id,
        //             reservetime_id = combinedData.ReserveTime.reservetime_id,
        //             create_id = combinedData.Proctor.create_id,
        //             update_id = combinedData.Proctor.update_id
        //         };
        //         DataList.Add(Data);
        //     }

        //     DataList = DataList.OrderBy(data => data.reservedate).ToList();
            
        //     return Ok(DataList);
        // }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var DataList = new List<TestReserveViewModel>();
            DataList = _testReserveService.GetAllTestReserveData();

            return Ok(DataList);
        }

        [HttpPost("CreateData")]
        public IActionResult CreateTestResever([FromBody]TestReserveViewModel Data)
        {
            Proctor proctor = new Proctor();
            ReserveTime reserveTime = new ReserveTime();
            Tester tester = new Tester();

            try
            {
                proctor.test_id = Data.test_id;
                proctor.members_id = Data.members_id;
                proctor.create_id = _getLoginClaimService.GetMembers_id();
                proctor.update_id = _getLoginClaimService.GetMembers_id();
                _proctorService.InsertProctor(proctor);
                
                reserveTime.proctor_id = proctor.proctor_id;
                reserveTime.reservedate = Data.reservedate;
                reserveTime.reservetime = Data.reservetime;
                reserveTime.create_id = _getLoginClaimService.GetMembers_id();
                reserveTime.update_id = _getLoginClaimService.GetMembers_id();
                _reserveTimeService.InsertReserveTime(reserveTime);

                tester.members_id = _getLoginClaimService.GetMembers_id();;
                tester.reservetime_id = reserveTime.reservetime_id;
                tester.create_id = _getLoginClaimService.GetMembers_id();
                tester.update_id = _getLoginClaimService.GetMembers_id();
                tester.test_id = Data.test_id;
                _testerService.InsertTester(tester);

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadTestResever([FromQuery]Guid Id)
        {
            Proctor proctor = new Proctor();
            ReserveTime reserveTime = new ReserveTime();
            TestReserveViewModel Data = new TestReserveViewModel();
            try
            {
                reserveTime = _reserveTimeService.GetDataById(Id);

                Data.proctor_id = reserveTime.proctor_id;
                proctor = _proctorService.GetDataById(Data.proctor_id);
                Data.reservetime_id = Id;
                Data.test_id = proctor.test_id;
                Data.create_id = proctor.create_id;
                Data.update_id = proctor.update_id;
                Data.members_id = proctor.members_id;
                Data.proctor_name = proctor.name;
                Data.test_title = proctor.test_title;
                Data.reservedate = reserveTime.reservedate;
                Data.reservetime = reserveTime.reservetime;

                var MemberData = _membersDBService.GetDataButOnlyIdAndName();
                if(Data == null)
                {
                    return BadRequest("NODATA");
                }

                return Ok(new{Data,MemberData});
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpPut("UpdateTestResever")]
        public IActionResult UpdateTestResever([FromQuery]Guid Id,[FromBody]TestReserveForUpdateID updateData)
        {
            Proctor proctor = new Proctor();
            ReserveTime reserveTime = new ReserveTime();

            try
            {
                reserveTime = _reserveTimeService.GetDataById(Id);
                proctor = _proctorService.GetDataById(reserveTime.proctor_id);

                proctor.members_id = updateData.Newmember_id;
                proctor.update_id = _getLoginClaimService.GetMembers_id();
                _proctorService.UpdateProctor(proctor);
                
                reserveTime.reservedate = updateData.reservedate;
                reserveTime.reservetime = updateData.reservetime;
                reserveTime.update_id = _getLoginClaimService.GetMembers_id();
                _reserveTimeService.UpdateReserveTime(reserveTime);
            }
            catch(Exception e)
            {
                
                return StatusCode(555, e.Message);
            }

            return Ok();
        }

        [HttpDelete("DeleteData")]
        public IActionResult DeleteTesterReserve([FromQuery]Guid Id)
        {
            var ReserveData = _reserveTimeService.GetDataById(Id);
            _proctorService.SoftDeleteProctorById(ReserveData.proctor_id);
            _reserveTimeService.SoftDeleteReserveTimeById(Id);
            return Ok();
        }

        [HttpPut("TesterSuccessReserve")]
        public IActionResult TesterSuccessReserve([FromQuery]Guid Id,[FromBody]TestReserveViewModel updateData)
        {
            var data = _testerService.GetDataById(Id);

            if (data == null)
            {
                return BadRequest();
            }
            data.tester_id = Id;
            data.is_success = updateData.is_success;
            _testerService.UpdateTester(data);

            return Ok();
        }

        [HttpPut("TesterFailReserve")]
        public IActionResult TesterFailReserve([FromQuery]Guid Id,[FromBody]TestReserveViewModel updateData)
        {
            var data = _testerService.GetDataById(Id);

            if (data == null)
            {
                return BadRequest();
            }
            data.tester_id = Id;
            data.is_pass = updateData.is_fail;
            _testerService.UpdateTesterFail(data);

            return Ok();
        }
    }
}