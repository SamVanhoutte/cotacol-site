namespace Cotacol.Website.Models.Settings
{
    public class StravaSettings
    {
        public string ApiUrl { get; set; }
        public string ClientOauthSecret { get; set; }
        public string ClientId { get; set; }
        public string WebhookCallbackUrl { get; set; }
        public string WebhookVerifyToken { get; set; }
        public string CommonUserId { get; set; }
        public string AccessTokenUrl { get; set; }
        public double RouteMatchingMargin { get; set; } = 0.005;
    }
}