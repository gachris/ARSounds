namespace Assets
{
    public class OidcClientOptionsConfiguration
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string[] Scopes { get; set; }

        public string RedirectUri { get; set; }

        public string PostLogoutRedirectUri { get; set; }

        public string ListenerUri { get; set; }
    }
}
