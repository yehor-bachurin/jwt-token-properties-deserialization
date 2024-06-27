using System.Security.Claims;
using System.Text.Json.Serialization;

namespace JwtTokenPropertiesDeserialization;

public class EmailConfirmationToken
{
	[JsonPropertyName(ClaimTypes.NameIdentifier)]
	public Guid UserId { get; set; }

	[JsonPropertyName(ClaimTypes.Email)]
	public string Email { get; set; } = string.Empty;
	
	[JsonPropertyName(ClaimTypes.GivenName)]
	public string FirstName { get; set; } = string.Empty;

	[JsonPropertyName(ClaimTypes.Surname)]
	public string LastName { get; set; } = string.Empty;

	[JsonPropertyName(ClaimTypes.Expiration)]
	public DateTime ExpiresOnUtc { get; set; }
}