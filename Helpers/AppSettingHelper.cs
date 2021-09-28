namespace NUNA.Helpers
{
    public class AppSettingHelper
    {
        public static Appsettings GetValue { get; set; } = new();
    }

    public class Appsettings
    {
        public Emailsetting EmailSetting { get; set; }
    }

    public class Emailsetting
    {
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpCred { get; set; }
        public string From { get; set; }
    }
}
