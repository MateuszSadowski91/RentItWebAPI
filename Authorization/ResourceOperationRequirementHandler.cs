using Microsoft.AspNetCore.Authorization;
using RentItAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentItAPI.Authorization
{
    public class ResourceOperationRequirementHandler: AuthorizationHandler<ResourceOperationRequirement, Business>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Business business)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
                if(business.CreatedById == int.Parse(userId))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
       
}
}
