namespace RiverLinkReporter.models
{
    public interface IRiverLinkReporter_JWTSettings
    {
        string Audience { get; set; }
        string Issuer { get; set; }
        string SecretKey { get; set; }
        string Username { get; set; }
    }

    public class RiverLinkReporter_JWTSettings : IRiverLinkReporter_JWTSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Username { get; set; }
    }
}
