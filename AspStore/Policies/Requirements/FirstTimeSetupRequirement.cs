using Microsoft.AspNetCore.Authorization;

namespace AspStore.Policies.Requirements;

public class FirstTimeSetupRequirement : IAuthorizationRequirement
{
    public int UserCount { get; }

    public FirstTimeSetupRequirement(int userCount)
    {
        UserCount = userCount;
    }

    public bool RequirementValid()
    {
        if (UserCount < 1)
        {
            return false;
        }

        else
        {
            return true;
        }
    }
}