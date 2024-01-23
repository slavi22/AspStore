using AspStore.Data;
using AspStore.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace AspStore.Policies.Handlers;

public class FirstTimeSetupHandler : AuthorizationHandler<FirstTimeSetupRequirement>
{
    private readonly AppDbContext _dbContext;

    public FirstTimeSetupHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        FirstTimeSetupRequirement requirement)
    {
        var userCount = _dbContext.Users.Count();
        if (userCount < requirement.UserCount) //if user count in db is less than 1
            context.Fail();
        else
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}