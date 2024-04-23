using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementClassLibrary.Contacts;
using OrderManagementClassLibrary.DTOs;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        //[HttpPost("Register")]
        //[Route("Registration")]
        //public async Task<IActionResult> Register(UserDTO userDTO)
        //{
        //    var response = await userAccount.CreateAccount(userDTO);
        //    return Ok(response);
        //}
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginDTO loginDTO)
        //{
        //    var response = await userAccount.LoginAccount(loginDTO);
        //    return Ok(response);
        //}
    }
}
