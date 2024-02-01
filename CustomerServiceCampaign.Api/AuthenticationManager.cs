using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CustomerServiceCampaign.Api;
public class AuthenticationManager : IAuthenticationManager
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User _user;

    public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager; 
        _configuration = configuration;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName);
        return _user != null && 
            await _userManager.CheckPasswordAsync(_user, userForAuth.Password);
    }

        // Creates a JWT token for the authenticated user
    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

        // Retrieves signing credentials for JWT token
    private SigningCredentials GetSigningCredentials() 
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    // Retrieves claims for JWT token
    private async Task<List<Claim>> GetClaims() 
    {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.UserData, _user.SSN) 
        };
        var roles = await _userManager.GetRolesAsync(_user); 
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); 
        }

        return claims; 
    }

    // Generates options for JWT token
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken 
        (
            //issuer: jwtSettings.GetSection("validIssuer").Value, 
            audience: jwtSettings.GetSection("validAudience").Value, 
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
            signingCredentials: signingCredentials
        );
        
        return tokenOptions; 
    }


}
