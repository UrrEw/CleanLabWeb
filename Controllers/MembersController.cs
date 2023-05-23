using Microsoft.AspNetCore.Mvc;
using LabWeb.Service;
using LabWeb.Secruity;
using LabWeb.models;
using LabWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace LabWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly MembersDBService _membersSerivce;
    private readonly MailService _mailService;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _config;
    private readonly GetLoginClaimService _getLoginClaimService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MembersController(MembersDBService membersSerivce, MailService mailService, JwtService jwtService, IConfiguration config,GetLoginClaimService getLoginClaimService,IHttpContextAccessor httpContextAccessor)
    {
        _membersSerivce = membersSerivce;
        _mailService = mailService;
        _jwtService = jwtService;
        _config = config;
        _getLoginClaimService = getLoginClaimService;
        _httpContextAccessor = httpContextAccessor;
    }

    #region 會員列表
    [HttpGet(Name = "GetMembersList")]
    public IActionResult GetMembersList()
    {
        List<Members> DataList=_membersSerivce.GetDataByAccountList();
        return Ok(DataList);
    }
    #endregion

    #region 註冊
    [HttpPost("register")]
    public IActionResult Register([FromBody] MembersRegisterViewModel registerMember)
    {
        if (ModelState.IsValid)
        {
            if(_membersSerivce.AccountCheck(registerMember.account))
            {
                var Data = new Members();
                Data.password = registerMember.password;
                string authCode = _mailService.GetAuthCode();
                Data.authcode = authCode;
                Data.account = registerMember.account;
                Data.email = registerMember.email;
                Data.name = registerMember.name;
                Data.level = registerMember.level;
                Data.entry_year = registerMember.entry_year;
                _membersSerivce.Register(Data);
                string filePath = "Views/RegisterEmail.html";
                string tempMail = System.IO.File.ReadAllText(filePath);
                
                string account = registerMember.account;
                string baseUrl = "http://127.0.0.1:5500/%E5%B0%8F%E5%B0%88/%E8%A8%BB%E5%86%8A/EmailValidate.html";
                string urlWithParams = $"{baseUrl}?account={account}&authCode={Data.authcode}";
                UriBuilder validateUrl = new UriBuilder(urlWithParams);

                string mailBody = _mailService.GetMailBody(tempMail, registerMember.name, 
                validateUrl.ToString().Replace("%3F", "?").Replace("[","").Replace("]",""));
                _mailService.SendRegisterMail(mailBody, registerMember.email);
                return Ok(new { message = "註冊成功，請去收信以來驗證EMAIL" });
            }
            else
            {
                return BadRequest("帳號重複了");
            }
        }
        registerMember.password = null;
        registerMember.passwordCheck = null;
        return BadRequest(registerMember);
    }

    // [HttpGet("AccountCheck")]
    // public JsonResult AccountCheck(MembersRegisterViewModel Account)
    // {
    //     bool AccountExists= _membersSerivce.AccountCheck(Account.newMember.Account);
    //     return Json(AccountExists);
    // }
    // private JsonResult Json(bool AccountExists)
    // {
    //     return new JsonResult(new { AccountExists });
    // }


    [HttpGet("EmailValidate")]
    public IActionResult EmailValidate(string Account,string AuthCode)
    {
        string ValidateResult = _membersSerivce.EmailValidate(Account,AuthCode);
        return Ok(ValidateResult);
    }
    #endregion

    #region 登入
    [HttpPost("Login")]
    public IActionResult Login(LoginViewModel Data)
    {

        string Validate=_membersSerivce.LoginCheck(Data.Account,Data.Password);
        if(!string.IsNullOrWhiteSpace(Validate))
        {
            string Role=_membersSerivce.GetRole(Data.Account);
            string token=_jwtService.GenerateToken(Data.Account,Role);
            string cookieName = _config["AppSettings:cookieName"].ToString();
            string account = Data.Account;
            string name = Validate;
            var LoginData = _membersSerivce.GetDataByAccount(Data.Account);
            int level = LoginData.level;

            var cookieOptions=new CookieOptions
            {
                HttpOnly=true,
                Expires=DateTime.Now.AddMinutes(Convert.ToInt32(_config["AppSettings:ExpireTime"]))
            };
            
            Response.Cookies.Append(cookieName,token,cookieOptions);
            return Ok(new{token,account,name,level});
        }
        else 
        {
            ModelState.AddModelError("",Validate);
            return BadRequest("登入失敗");
        }
    }
    #endregion

    #region 登出
    [HttpGet("Logout")]
    
    public IActionResult Logout()
    {
            string cookieName = _config["AppSettings:cookieName"].ToString();
            
            return Ok("登出成功");
    }
    #endregion

    #region 修改密碼
    [HttpPut("ChangePassword")]
    public IActionResult ChangePassword([FromBody] ChangePasswordViewModel ChangeData)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if(ChangeData.Account != _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name))
                {
                    return StatusCode(666);
                }
                string ChangeState = _membersSerivce.ChangePassword(ChangeData.Account, ChangeData.Password, ChangeData.NewPassword);
                return Ok(ChangeState);
            }
            else
            {
                return StatusCode(444);
            }
        }
        catch(Exception e)
        {
            return StatusCode(555, e.Message);
        }
    }
    #endregion

    #region 忘記密碼
    [HttpPut("ForgetPassword")]
    public IActionResult ForgetPassword([FromBody] ForgetPasswordViewModel resetData)
    {
        if (ModelState.IsValid)
        {
            if(_membersSerivce.AccountExit(resetData.account))
            {
                var Data = _membersSerivce.GetDataByAccount(resetData.account);
                if(Data.email != resetData.email)
                {
                    return StatusCode(555);
                }
                string password = _membersSerivce.GetTempPassword();
                _membersSerivce.ResetPassword(resetData.account ,password);
                string filePath = "Views/ChangePasswordEmail.html";
                string tempMail = System.IO.File.ReadAllText(filePath);
                string mailBody = _mailService.GetResetMailBody(tempMail,password);
                _mailService.SendRegisterMail(mailBody, resetData.email);
                return Ok(new { message = "重設成功，請去收信以來取得重設密碼" });
            }
            else
            {
                return BadRequest("帳號錯了");
            }
        }
        return BadRequest(resetData);
    }
    #endregion

    [HttpGet("GetIDList")]
    public ActionResult GetIDList()
    {
        var Data = _membersSerivce.GetDataButOnlyIdAndName();
        return Ok(Data); 
    }

    [HttpGet("GetLoginRole")]
    public ActionResult GetLoginRole([FromQuery]string Account)
    {
        string Role = _membersSerivce.GetRole(Account);
        return Ok(Role);
    }

    [HttpGet("GetSenior")]
    public ActionResult GetSenior()
    {
        var Data = _membersSerivce.GetDataButSenior();
        return Ok(Data);
    }

    [HttpGet("GetLoginInfo")]
    public ActionResult GetLoginInfo()
    {
        var loginID = _getLoginClaimService.GetMembers_id();
        var Data = _membersSerivce.GetDataByMembersID(loginID);

        return Ok(Data);
    }

    [HttpGet("GetSuccessReserve")]
    public ActionResult GetSuccessReserve()
    {
        var Data = _membersSerivce.GetDataSuccessReserve();
        return Ok(Data);
    }

    [HttpGet("GetFailReserve")]
    public ActionResult GetFailReserve()
    {
        var Data = _membersSerivce.GetDataFailReserve();
        return Ok(Data);
    }

    [HttpGet("GetMemberLevel")]
    public ActionResult GetMemberLevel()
    {
        var Data = _membersSerivce.GetDataMemberLevelList();
        return Ok(Data);
    }

    [HttpPut("ChangeMemberLevel")]
    public ActionResult ChangeMemberLevel([FromBody]Members data)
    {

            data.update_id = _getLoginClaimService.GetMembers_id();
            _membersSerivce.ChangeMemberLevel(data);

        return Ok();
    }

    [HttpGet("GetOneMemberLevel")]
    public ActionResult GetOneMemberLevel([FromQuery]Guid Id)
    {
        Members Data = _membersSerivce.GetOneDataMemberLevel(Id);
        return Ok(Data);
    }

    [HttpPut("DeleteMember")]
    public ActionResult DeleteMember([FromQuery] Guid Id)
    {
        _membersSerivce.DeleteMember(Id);
        return Ok();
    }
}