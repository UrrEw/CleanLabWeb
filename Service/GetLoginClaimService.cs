using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LabWeb.Service
{
    public class GetLoginClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly MembersDBService _membersDBService;

        public GetLoginClaimService(IHttpContextAccessor httpContextAccessor,MembersDBService membersDBService)
        {
            _httpContextAccessor = httpContextAccessor;
            _membersDBService = membersDBService;
        }

        public Guid GetMembers_id()
        {
            var Account = string.Empty;
            if(_httpContextAccessor.HttpContext is not null)
            {
                Account = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            var Data = _membersDBService.GetDataByAccount(Account);
            var result = Data.members_id;
            
            return result;
        }
    }
}