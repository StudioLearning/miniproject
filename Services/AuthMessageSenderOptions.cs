public class AuthMessageSenderOptions
{
    public const string Email = "Email";
    public string ApiKey { get; set; } = String.Empty;
    public string SecretKey { get; set; } = String.Empty;
    public string SMTPServer { get; set; } = String.Empty;
    public int Port { get; set; } = 25;
}