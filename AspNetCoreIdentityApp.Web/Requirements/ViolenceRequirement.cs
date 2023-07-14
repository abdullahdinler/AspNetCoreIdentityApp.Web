using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Web.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int ThresholdAge { get; set; }
    }

    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ViolenceRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "birthdate"))
            {
                var birthdateClaim = context.User.FindFirst(c => c.Type == "birthdate");
                var today = DateTime.Now;
                var birthdate = Convert.ToDateTime(birthdateClaim!.Value);
                var age = today.Year - birthdate.Year;
                

                if (age >= requirement.ThresholdAge)
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
