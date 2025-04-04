using ForexPrediction.Domain.Entities;
using ForexPrediction.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ForexPrediction.Infrastructure.Services;

public class IdentityService : IIdentityService
    {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public IdentityService ( UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration )
        {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        }

    public async Task<string> RegisterAsync ( string email, string password )
        {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return GenerateJwtToken(user);
        }

    public async Task<string> LoginAsync ( string email, string password )
        {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        if (!result.Succeeded)
            throw new Exception("Invalid login attempt");

        var user = await _userManager.FindByEmailAsync(email);
        return GenerateJwtToken(user);
        }

    private string GenerateJwtToken ( ApplicationUser user )
        {
        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }