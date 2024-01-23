using Microsoft.AspNetCore.Authorization;

namespace AspStore.Policies.Requirements;

public class FirstTimeSetupRequirement : IAuthorizationRequirement
{
    public FirstTimeSetupRequirement(int userCount)
    {
        UserCount = userCount;
    }

    public int UserCount { get; }
}