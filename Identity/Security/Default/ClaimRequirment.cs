using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Security.Default
{
    public class ClaimRequirment:IAuthorizationRequirement
    {
        public ClaimRequirment(string claimType,string claimValue)
        {
            ClaimValue = claimValue;
            CliamType = CliamType;
        }
        public string CliamType { get; set; }
        public string ClaimValue { get; set; }
    }
}
