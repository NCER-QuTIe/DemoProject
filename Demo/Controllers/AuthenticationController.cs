using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

public class AuthenticationController : ControllerBase
{
    [HttpGet]
    [Route("api/login")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public OkResult Login() => Ok();
}
