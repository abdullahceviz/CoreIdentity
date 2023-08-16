using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Web.Requierments
{
    public class ExchangeExpireRequierement : IAuthorizationRequirement
    {
    }
    public class ExchangeExpireRequierementHandler : AuthorizationHandler<ExchangeExpireRequierement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequierement requirement)
        {

            if (!context.User.HasClaim(x => x.Type == "ExchangeExpireDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var exchangeExpireDate = context.User.FindFirst("ExchangeExpireDate")!;
            if(!(Convert.ToDateTime(exchangeExpireDate.Value) > DateTime.Now))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

}
