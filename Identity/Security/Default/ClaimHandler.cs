using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Security.Default
{
    public class ClaimHandler: AuthorizationHandler<ClaimRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ClaimRequirment requirment)
        {
            if(context.User.HasClaim(requirment.CliamType,requirment.ClaimValue))
                context.Succeed(requirment);

            return Task.CompletedTask;
        }
    }
}
