namespace MyIdIdentification.Options;

public class MyIdOptions
{
    public Security Security { get; set; }
    public Urls Urls { get; set; }
}

public class Urls
{
    public string BaseUrl { get; set; }
    public string GetAccessToken { get; set; }
    public string GetJobId { get; set; }
    public string GetJobIdResult { get; set; }
    public string RefreshToken { get; set; }
    public string GetMe { get; set; }
}

public class Security
{
    public string GrantType { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string GrantTypeSdk { get; set; }
    public string ClientIdSdk { get; set; }

}