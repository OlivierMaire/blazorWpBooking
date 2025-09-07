using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace blazorWpBooking.Components.Account;

public class WordPressAuthentication
{
    public string WordPressApiUrl { get; set; } = string.Empty;

    public WordPressAuthentication()
    {

    }
    
    public async Task<(SignInResult, TokenResponse?)> ValidateUserAsync(
        string username,
        string password
       )
    {
        SignInResult result = new SignInResult(); ;

        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri(WordPressApiUrl);

            // Prepare the login request
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            // Send the login request
            HttpResponseMessage response = await client.PostAsync("/wp-json/jwt-auth/v1/token", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            //Deserialize token
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

            if (response.IsSuccessStatusCode)
            {
                // User authentication succeeded
                return (SignInResult.Success, tokenResponse);
            }
            else
            {
                // User authentication failed
                return (SignInResult.Failed, null);
            }
        }
    }
 
}

   
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("user_display_name")]
        public string UserDisplayName { get; set; } = string.Empty;
        [JsonPropertyName("user_email")]
        public string UserEmail { get; set; } = string.Empty;
        [JsonPropertyName("user_nicename")]
        public string UserNiceName { get; set; } = string.Empty;
    }