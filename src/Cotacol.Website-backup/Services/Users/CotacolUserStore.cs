using Cotacol.Website.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cotacol.Website.Services.Users;

public class CotacolUserStore : IUserStore<CotacolUser>
{
    public void Dispose()
    {
    }

    public Task<string> GetUserIdAsync(CotacolUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id);
    }

    public Task<string> GetUserNameAsync(CotacolUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(CotacolUser user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string> GetNormalizedUserNameAsync(CotacolUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user?.UserName ?? user?.Id ?? "");
    }

    public Task SetNormalizedUserNameAsync(CotacolUser user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> CreateAsync(CotacolUser user, CancellationToken cancellationToken)
    {
        //TODO
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityResult> UpdateAsync(CotacolUser user, CancellationToken cancellationToken)
    {
        //TODO
        return Task.FromResult(IdentityResult.Success);
        
    }

    public Task<IdentityResult> DeleteAsync(CotacolUser user, CancellationToken cancellationToken)
    {
        //TODO
        return Task.FromResult(IdentityResult.Success);
        
    }

    public Task<CotacolUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = new CotacolUser()
        {
            Id = userId, Email = $"{Guid.NewGuid()}@cotacol.eu", FirstName = "Joseph", LastName = "Pinxten", UserName = "jopi"
        };
        return Task.FromResult(user);
    }

    public Task<CotacolUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var user = new CotacolUser()
        {
            Id = Guid.NewGuid().ToString("N")[9..], Email = $"{Guid.NewGuid()}@cotacol.eu", FirstName = "Joseph", LastName = "Pinxten", UserName = normalizedUserName
        };
        return Task.FromResult(user);
    }
}