using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyMultiplayer.Server.Services;

namespace SpotifyMultiplayer.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public ActionResult Get()
    {
        LoginRequest loginRequest = _authService.AuthenticateFirstStep();

        return Redirect(loginRequest.ToUri().ToString());
    }

    [HttpGet]
    [Route("redirect")]
    public async Task<string> FirstCallback(string code)
    {
        AuthorizationCodeTokenResponse response = await _authService.AuthenticateSecondStep(code);

        var spotify = new SpotifyClient(response.AccessToken);

        PrivateUser privateUser = await spotify.UserProfile.Current();

        return $"{privateUser.DisplayName}";
    }

    
}
