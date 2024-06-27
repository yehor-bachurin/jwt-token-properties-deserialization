using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Bogus;
using Microsoft.IdentityModel.Tokens;

namespace JwtTokenPropertiesDeserialization;

[MemoryDiagnoser]
public class Benchmarks
{
	private readonly JwtTokenService _jwtTokenService = new();
	
	[Benchmark]
	public EmailConfirmationToken? AutoDeserialization()
	{
		return _jwtTokenService.Deserialize<EmailConfirmationToken>(GenerateToken());
	}
	
	private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
	private JwtSecurityToken JwtSecurityToken => _jwtSecurityTokenHandler.ReadJwtToken(GenerateToken());
	
	[Benchmark]
	public EmailConfirmationToken? ManualDeserialization()
	{
		if (!Guid.TryParse(JwtSecurityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out Guid userId))
		{
			return null;
		}
		
		string? email = JwtSecurityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
		
		if (string.IsNullOrEmpty(email))
		{
			return null;
		}

		string? firstName = JwtSecurityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
		
		if (string.IsNullOrEmpty(firstName))
		{
			return null;
		}
		
		string? lastName = JwtSecurityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
		
		if (string.IsNullOrEmpty(lastName))
		{
			return null;
		}
		
		return new EmailConfirmationToken()
		{
			UserId = userId,
			Email = email,
			FirstName = firstName,
			LastName = lastName,
			ExpiresOnUtc = JwtSecurityToken.ValidTo
		};
	}
	
	[Benchmark]
	public EmailConfirmationToken? OptimizedManualDeserialization()
	{
		var token = new EmailConfirmationToken();

		foreach (Claim? claim in JwtSecurityToken.Claims)
		{
			switch (claim.Type)
			{
				case ClaimTypes.NameIdentifier:
				{
					token.UserId = Guid.Parse(claim.Value);
					break;
				}
				case ClaimTypes.Email:
				{
					token.Email = claim.Value;
					break;
				}
				case ClaimTypes.GivenName:
				{
					token.FirstName = claim.Value;
					break;
				}
				case ClaimTypes.Surname:
				{
					token.LastName = claim.Value;
					break;
				}
				case ClaimTypes.Expiration:
				{
					token.ExpiresOnUtc = DateTime.Parse(claim.Value);
					break;
				}
			}
		}

		return token;
	}
	
	private string GenerateToken()
	{
		var faker = new Faker();
	
		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.Email, faker.Internet.Email()),
			new Claim(ClaimTypes.GivenName, faker.Name.FirstName()),
			new Claim(ClaimTypes.Surname, faker.Name.LastName())
		};

		return _jwtTokenService.Serialize(claims);
	}
}