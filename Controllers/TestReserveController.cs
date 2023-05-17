using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.models;
using LabWeb.ViewModel;

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
        
        public TestReserveController(ProctorService proctorService,ReserveTimeService reserveTimeService,
                                    TesterService testerService,TestReserveService testReserveService,GetLoginClaimService getLoginClaimService)
        {
            _proctorService = proctorService;
            _reserveTimeService = reserveTimeService;
            _testerService = testerService;
            _testReserveService = testReserveService;
            _getLoginClaimService = getLoginClaimService;
        }

        [HttpGet("GetAllDataList")]
        public IActionResult GetAllData()
        {
            var proctorData = _proctorService.GetAllData();
            var reserveTimeData = _reserveTimeService.GetAllData();
            var testerData = _testerService.GetAllData();
            var DataList = new List<TestReserveViewModel>();

            foreach(var combinedData in proctorData
                        .Zip(reserveTimeData, (proctor, reserveTime) => new { Proctor = proctor, ReserveTime = reserveTime })
                        .Zip(testerData, (combined, tester) => new { Proctor = combined.Proctor, ReserveTime = combined.ReserveTime, Tester = tester }))
            {
                var Data = new TestReserveViewModel()
                {
                    test_title = combinedData.Proctor.test_title,
                    tester_name = combinedData.Tester.name,
                    proctor_name = combinedData.Proctor.name,
                    reservedate = combinedData.ReserveTime.reservedate,
                    reservetime = combinedData.ReserveTime.reservetime,
                    is_success = combinedData.Tester.is_success
                };
                DataList.Add(Data);
            }       
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
                
                reserveTime.proctor_id = Data.proctor_id;
                reserveTime.reservedate = Data.reservedate;
                reserveTime.reservetime = Data.reservetime;
                reserveTime.create_id = _getLoginClaimService.GetMembers_id();
                reserveTime.update_id = _getLoginClaimService.GetMembers_id();
                _reserveTimeService.InsertReserveTime(reserveTime);

                tester.members_id = Data.create_id;
                tester.reservetime_id = reserveTime.reservetime_id;
                tester.create_id = _getLoginClaimService.GetMembers_id();
                tester.update_id = _getLoginClaimService.GetMembers_id();
                _testerService.InsertTester(tester);

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(555, e.Message);
            }
        }

        [HttpGet("ReadOneData")]
        public IActionResult ReadTestResever([FromBody]TestReserveForReadID ReadID)
        {
            Proctor proctor = new Proctor();
            ReserveTime reserveTime = new ReserveTime();
            Tester tester = new Tester();
            TestReserveViewModel Data = new TestReserveViewModel();
            try
            {
                proctor = _proctorService.GetDataById(ReadID.proctor_id);
                reserveTime = _reserveTimeService.GetDataById(ReadID.reservetime_id);
                tester = _testerService.GetDataById(ReadID.tester_id);

                Data.proctor_name = proctor.name;
                Data.test_title = proctor.test_title;
                Data.reservedate = reserveTime.reservedate;
                Data.reservetime = reserveTime.reservetime;
                Data.tester_name = tester.name;

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

        [HttpPut("UpdateTestResever")]
        public IActionResult UpdateTestResever([FromBody]TestReserveForUpdateID updateData)
        {
            Proctor proctor = new Proctor();
            ReserveTime reserveTime = new ReserveTime();

            try
            {
                proctor = _proctorService.GetDataById(updateData.Oldproctor_id);
                reserveTime = _reserveTimeService.GetDataById(updateData.Oldreservetime_id);

                proctor.proctor_id = updateData.Newproctor_id;
                proctor.test_id = updateData.Newtest_id;
                proctor.update_id = _getLoginClaimService.GetMembers_id();

                reserveTime.reservetime_id = updateData.Newreservetime_id;
                reserveTime.proctor_id = updateData.Newproctor_id;
                reserveTime.reservedate = updateData.reservedate;
                reserveTime.reservetime = updateData.reservetime;
                reserveTime.update_id = _getLoginClaimService.GetMembers_id();

                _proctorService.UpdateProctor(proctor);
                _reserveTimeService.UpdateReserveTime(reserveTime);
            }
            catch(Exception e)
            {
                
                return StatusCode(55688, e.Message);
            }

            return Ok();
        }

        [HttpPut("UpdateTester")]
        public IActionResult UpdateTesterReserveStatus([FromQuery]Guid Id,[FromBody]TestReserveViewModel updateData)
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

        [HttpDelete("DeleteData")]
        public IActionResult DeleteTesterReserve([FromBody]TestReserveForReadID DeleteID)
        {
            _proctorService.SoftDeleteProctorById(DeleteID.proctor_id);
            _reserveTimeService.SoftDeleteReserveTimeById(DeleteID.reservetime_id);
            _testerService.SoftDeleteTesterById(DeleteID.tester_id);
            return Ok();
        }
    }
}