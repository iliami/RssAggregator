using System.Text.Json.Serialization;

namespace Iliami.Identity.Domain.TokenGenerator;

public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    [property: JsonIgnore] DateTime AccessTokenExpiration,
    [property: JsonIgnore] DateTime RefreshTokenExpiration);