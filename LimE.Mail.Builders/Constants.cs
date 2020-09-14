namespace LimE.Mail.Builders
{
    public static class Constants
    {
        public const string TagHelperPrefix = "lme";

        public const string DefaultConfigurationBaseKey = "LimE.Mail.Builders";

        public static string FormatAs => $"{DefaultConfigurationBaseKey}:FormatAs";

        public static string Title => $"{DefaultConfigurationBaseKey}:Title";

        public static string BodyBgColor => $"{DefaultConfigurationBaseKey}:Body:bgcolor";
    }
}