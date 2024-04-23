using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrderManagementAPI.Data;
using OrderManagementClassLibrary.Contacts;
using OrderManagementClassLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static OrderManagementClassLibrary.DTOs.ServiceResponse;

namespace OrderManagementAPI.Repository
{
    public class AccountRepository(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IUserAccount
    {
        public async Task<ServiceResponse.GeneralResponse> CreateAccount(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return new GeneralResponse(false, "Model is empty.");
            }
            else
            {
                var newUser = new ApplicationUser()
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    PasswordHash = userDTO.Password,
                    UserName = userDTO.Email
                };
                var user = await userManager.FindByEmailAsync(newUser.Email);
                if (user != null)
                {
                    return new GeneralResponse(false, "User is already registered.");
                }
                else
                {
                    var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
                    if (!createUser.Succeeded)
                    {
                        return new GeneralResponse(false, "Error occured. Please try again.");
                    }
                    var checkAdmin = await roleManager.FindByNameAsync("Admin");
                    if (checkAdmin == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = "Admin"
                        });
                        await userManager.AddToRoleAsync(newUser, "Admin");
                        return new GeneralResponse(true, "Account Created");
                    }
                    else
                    {
                        var checkUser = await roleManager.FindByNameAsync("User");
                        if (checkUser == null)
                        {
                            await roleManager.CreateAsync(new IdentityRole()
                            {
                                Name = "User"
                            });
                        }
                        await userManager.AddToRoleAsync(newUser, "User");
                        return new GeneralResponse(true, "Account Created");
                    }
                }
            }

        }

        public async Task<ServiceResponse.LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return new LoginResponse(false, null!, "Login container is empty.");
            }
            var getUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser == null)
            {
                return new LoginResponse(false, null!, "User not found.");
            }
            bool checkUserPassword = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPassword)
            {
                return new LoginResponse(false, null!, "Invalid email or password.");
            }
            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "Login completed");
        }

        private string GenerateToken(UserSession userSession)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userSession.Id),
            new Claim(ClaimTypes.Name, userSession.Name),
            new Claim(ClaimTypes.Email, userSession.Email),
            new Claim(ClaimTypes.Role, userSession.Role),
        };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
