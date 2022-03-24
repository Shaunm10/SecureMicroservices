namespace Movies.Client.Configuration;

    public class OpenIdConnect
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string ResponseType { get; set; }

        public List<string> Scopes { get; set; } = new List<string>();

        public bool SaveTokens { get; set; }

        public bool GetClaimsFromUserInputEndpoint { get; set; }
    }