using Newtonsoft.Json;
using SERVER_SIDE.Models.DTOModels;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CHAT_APP_CLIENT.Services
{
    public class ApiServiceAuth
    {
        private readonly HttpClient _httpClient;

        public ApiServiceAuth()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:5021/") // Replace with your actual API base URL
            };
        }

        public async Task<string> LoginAsync(MemberLogin loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest._name) || string.IsNullOrEmpty(loginRequest._passwordHash))
            {
                throw new ArgumentException("Username and password must be provided.");
            }

            // Convert the DTO to JSON and send it via HTTP POST
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", new
            {
                loginRequest._name,
                loginRequest._passwordHash,
                _token = loginRequest._token ?? "", // Send an empty string if null
                _role = loginRequest._role ?? ""   // Send an empty string if null
            });


            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content into the ResponseModel
                var result = await response.Content.ReadFromJsonAsync<MemberLogin>();

                if (result != null && !string.IsNullOrEmpty(result._token))
                {
                    return result._token; // Return the token from the response model
                }

                throw new Exception("Token is missing or invalid.");
            }

            // Handle non-success responses by throwing the error message
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Server Error: {errorContent}");
            throw new Exception(errorContent);
        }



        public async Task LoginUser()
        {
            try
            {
                var username = "user1";  // Example username
                var password = "password123";  // Example password

                // Hash the password before sending
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                // Create a MemberLogin object with hashed password
                var loginRequest = new MemberLogin
                {
                    _name = username,
                    _passwordHash = hashedPassword
                };

                // Create an instance of ApiServiceAuth
                var apiServiceAuth = new ApiServiceAuth();

                // Call LoginAsync with the MemberLogin object
                string token = await apiServiceAuth.LoginAsync(loginRequest);

                // Use the token (e.g., store it for future requests or decoding)
                Console.WriteLine("JWT Token: " + token);

                // Optionally decode the token (e.g., extracting claims from the payload)
                var tokenParts = token.Split('.');
                var decodedPayload = apiServiceAuth.Base64UrlDecode(tokenParts[1]);
                Console.WriteLine("Decoded Payload: " + Encoding.UTF8.GetString(decodedPayload));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public byte[] Base64UrlDecode(string base64Url)
        {
            // Convert the Base64 URL-safe encoding to regular Base64 format
            var base64 = base64Url.Replace('-', '+').Replace('_', '/');

            // Add padding based on the length of the string (required for proper Base64 decoding)
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            // Convert the Base64 string to a byte array and return it
            return Convert.FromBase64String(base64);
        }
    }
}