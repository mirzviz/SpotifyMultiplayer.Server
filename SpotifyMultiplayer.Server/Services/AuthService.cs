using SpotifyAPI.Web;

namespace SpotifyMultiplayer.Server.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly string _redirectURI;
    private readonly string _clientID;
    private readonly string _clientSecret;

    public AuthService(IConfiguration config)
    {
        _config = config;
        _redirectURI = _config.GetValue<string>("RedirectURI");
        _clientID = _config.GetValue<string>("ClientID");
        _clientSecret = _config.GetValue<string>("ClientSecret");
    }

    public LoginRequest AuthenticateFirstStep()
    {
        return new LoginRequest(
            new Uri(_redirectURI),
            _clientID,
            LoginRequest.ResponseType.Code
        )
        {
            Scope = new[] 
            { 
                Scopes.PlaylistReadPrivate, 
                Scopes.PlaylistReadCollaborative,
                Scopes.UserReadRecentlyPlayed,
                Scopes.UserTopRead
            }
        };
    }

    public async Task<AuthorizationCodeTokenResponse> AuthenticateSecondStep(string code)
    {
        AuthorizationCodeTokenResponse response = await new OAuthClient().RequestToken(
                 new AuthorizationCodeTokenRequest(
                             _clientID,
                         _clientSecret,
                                code,
                         new Uri(_redirectURI)
                        )
        );

        return response;
    }
}
