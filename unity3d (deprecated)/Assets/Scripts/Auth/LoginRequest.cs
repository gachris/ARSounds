namespace Assets
{
    public class LoginRequest
    {
        public string RedirectUri { get; set; }

        public string CallbackUri { get; set; }

        public LoginRequest(string redirectUri, string callbackUri)
        {
            RedirectUri = redirectUri;
            CallbackUri = callbackUri;
        }
    }
}
