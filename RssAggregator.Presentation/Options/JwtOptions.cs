namespace RssAggregator.Presentation.Options;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenValidityInMinutes { get; set; }
    public int RefreshTokenValidityInMinutes { get; set; }
}