namespace blazorWpBooking.Components.Account
{
    /// <summary>
    /// Scoped per-circuit/token holder for Blazor Server.
    /// </summary>
    public class CircuitTokenStore
    {
        private string? _token;

        public string? Token
        {
            get => _token;
            set => _token = value;
        }

        public string? DisplayName { get; set; }
        public string? Email { get; set; }

        public void SetToken(TokenResponse token)  {
            _token = token.Token;
            DisplayName = token.UserDisplayName;
            Email = token.UserEmail;
        }

        public void Clear()  {
            _token = null;
            DisplayName = null;
            Email = null;
        }
    }
}