using System;
using Microsoft.AspNetCore.Components;

namespace CotacolApp.Services.Extensions;

public static class NavigationManagerExtensions
{
    public static bool IsLocal(this NavigationManager navigationManager)
    {
        var host = new Uri(navigationManager.BaseUri).Host;
        return host == "localhost" || host == "127.0.0.1" || host == "::1";
    }
}