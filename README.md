# Cotacol Web App
An attempt to learn Blazor, by setting up a Cotacol web site

## Identity login

1. Set up razor server app, with individual identiy
1. Scaffolded two pages, based on this article: https://andrewlock.net/customising-aspnetcore-identity-without-editing-the-pagemodel/ 
    - Account.Login, which enables the customization of the actual login page `dotnet aspnet-codegenerator identity -dc CotacolApp.Data.ApplicationDbContext --files "Account.Login"`
    - Account.ExternalLogin, which enables the customization of the actual strava page `dotnet aspnet-codegenerator identity -dc CotacolApp.Data.ApplicationDbContext --files "Account.ExternalLogin"`

1. Add OAuth nuget package: `Microsoft.AspNetCore.Authentication.OAuth`
1. Update Startup.cs to include Strava login
    ```csharp
    services.AddAuthentication().AddOAuth("Strava",
        oAuthOptions =>
        {
            oAuthOptions.ClientId = "myappid"; 
            oAuthOptions.ClientSecret = "myclientsecret";
            oAuthOptions.Scope.Clear();        
            oAuthOptions.Scope.Add("read");
            oAuthOptions.CallbackPath = "/profile"; 
            oAuthOptions.AuthorizationEndpoint = "https://www.strava.com/oauth/authorize";
            oAuthOptions.TokenEndpoint = "https://www.strava.com/api/v3/oauth/token";
            oAuthOptions.SignInScheme = IdentityConstants.ExternalScheme;
            oAuthOptions.Events = new OAuthEvents()
            {
                OnRemoteFailure = loginFailureHandler =>
                {
                    Console.WriteLine("Remote Error");
                    Console.WriteLine(loginFailureHandler.Failure.Message);
                    return Task.FromResult(0);
                }, 
                OnAccessDenied = handler =>
                {
                    Console.WriteLine(handler.Response.StatusCode);
                    return Task.FromResult(0);
                }
            };
        });
    ```

1. Issue when logging on : `var info = await _signInManager.GetExternalLoginInfoAsync();` returns `null`. Similar issue found here : https://stackoverflow.com/questions/40227643/signinmanager-getexternallogininfoasync-always-returns-null-with-open-id-to-a

1. Issue when getting null in `GetExternalLoginInfoAsync` : https://stackoverflow.com/questions/46158462/instagram-oauth-getexternallogininfoasync-always-returns-null-in-net-core-2-0/55970100

## Access to tokens

It is important to get access to the OAuth Tokens, since they have to be passed to the API Backend, where they will be refreshed on a daily basis.  The `SaveTokens` property  does not seem to work, initially, so here are some posts/threads:

1. Access tokens in asp.net core: https://blog.maartenballiauw.be/post/2020/01/13/making-api-calls-using-the-access-token-and-refresh-token-from-an-aspnet-core-authentication-handler.html

1. https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/additional-claims?view=aspnetcore-5.0



InvalidOperationException: No authenticationScheme was specified, and there was no DefaultAuthenticateScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).