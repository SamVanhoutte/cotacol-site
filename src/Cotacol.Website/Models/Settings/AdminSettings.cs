namespace Cotacol.Website.Models.Settings
{
    public class AdminSettings
    {
        public string Admins { get; set; }

        public bool IsAdmin(string userId)
        {
            if (string.IsNullOrEmpty(Admins)) return false;
            return Admins.Split(',').Contains(userId);
        }
    }
}