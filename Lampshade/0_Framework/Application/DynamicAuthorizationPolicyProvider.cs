using System.Security.Claims;
using _0_Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
    private readonly IServiceScopeFactory _scopeFactory;

    public DynamicAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        _scopeFactory = scopeFactory;
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        using var scope = _scopeFactory.CreateScope();
        var authHelper = scope.ServiceProvider.GetRequiredService<IAuthHelper>();
        var id = PermissionsCodes.GetCode(policyName);
        var policy = new AuthorizationPolicyBuilder();
        var permissionsStrings = authHelper.GetPermissionsStrings();
        if (id == 1)
        {
            if (permissionsStrings.Count > 0)
                return policy.RequireClaim(ClaimTypes.NameIdentifier, authHelper.CurrentAccountInfo().Id.ToString())
                    .Build();
        }
        else
        {
            if (permissionsStrings.Contains(id.ToString()))
                return policy.RequireClaim(ClaimTypes.NameIdentifier, authHelper.CurrentAccountInfo().Id.ToString())
                    .Build();
        }
        return new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => false)
            .Build();
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _fallbackPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _fallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}