using SpotifyAPI.Web;

namespace SpotifyMultiplayer.Server.Services
{
    public interface IAuthService
    {
        public LoginRequest AuthenticateFirstStep();
        public Task<AuthorizationCodeTokenResponse> AuthenticateSecondStep(string code);
    }
}