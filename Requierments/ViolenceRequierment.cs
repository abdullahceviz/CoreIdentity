using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Web.Requierments
{
    public class ViolenceRequierment:IAuthorizationRequirement
    {
        public int TresholdAge { get; set; }
    }
    public class ViolenceRequiermentHandler : AuthorizationHandler<ViolenceRequierment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequierment requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "birthdate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var birthdayClaim = context.User.FindFirst("birthdate")!;
            var today = DateTime.Now;
            var birthday = Convert.ToDateTime(birthdayClaim.Value);
            var age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age)) age--;

            if (requirement.TresholdAge > age)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
