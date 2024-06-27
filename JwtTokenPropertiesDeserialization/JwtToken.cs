using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace JwtTokenPropertiesDeserialization;

public class JwtTokenService
{
	private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    
	public string Serialize(IEnumerable<Claim> claims)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(nameof(JwtTokenPropertiesDeserialization)));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var jwtToken = new JwtSecurityToken(
			issuer: nameof(JwtTokenPropertiesDeserialization),
			audience: nameof(JwtTokenPropertiesDeserialization),
			claims: claims,
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: credentials
		);

		return _jwtSecurityTokenHandler.WriteToken(jwtToken);
	}
	
	public TToken? Deserialize<TToken>(string hash)
		where TToken : class
	{
		JwtSecurityToken? jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(hash);

		if (jwtSecurityToken is null)
		{
			return null;
		}
        
		return JsonSerializer.Deserialize<TToken>(jwtSecurityToken.Payload.SerializeToJson());
	}
}