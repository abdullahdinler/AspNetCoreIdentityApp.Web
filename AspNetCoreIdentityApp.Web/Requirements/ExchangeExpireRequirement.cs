using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Web.Requirements
{
    public class ExchangeExpireRequirement : IAuthorizationRequirement
    {
    }

    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ExchangeExpireRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "ExchangeExpire"))
            {
                var expireDate = Convert.ToDateTime(context.User.FindFirst(c => c.Type == "ExchangeExpire")!.Value);
                if (expireDate > DateTime.Now)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
