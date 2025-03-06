using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost]
    [Route("sign-in")]
    public IActionResult Signin()
    {
        return SignIn (
            new ClaimsPrincipal (
                new ClaimsIdentity (
                    [
                        new Claim("sub", Guid.NewGuid().ToString())
                    ],
                    BearerTokenDefaults.AuthenticationScheme
                )
            ),
            authenticationScheme: BearerTokenDefaults.AuthenticationScheme
        );
    }
}