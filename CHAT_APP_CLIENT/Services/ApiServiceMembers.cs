using SERVER_SIDE.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace CHAT_APP_CLIENT.Services
{
    public class ApiServiceMembers
    {
        private readonly HttpClient _httpClient; // dependency injection for connect between the client side and the server side

        public ApiServiceMembers()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5021/") // ASP.NET CORE WEB API back-end URL
            };
        }

        // GET MEMBERS
        public async Task<List<Member>> GetAllMembersAsync()
        {
            var response = await _httpClient.GetAsync("api/Members");
            response.EnsureSuccessStatusCode();
            var members = await response.Content.ReadAsAsync<List<Member>>();   
            return members;
        }


        // ADD 1 MEMBER
        public async Task<HttpResponseMessage> AddMemberToChatAsync(Member member)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Members/addMember", member);
            response.EnsureSuccessStatusCode();
            return response;
        }

        // Update login field of member to true (for connected memebr)
        public async Task<Member?> UpdateLoginFieldToTrue(int id, Member member)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Members/updateLoginFieldForConnectedMemebr/{id}", member);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Member?>();
            }
            return null; // Return null if the update fails
        }

        // Update login field of memebr to false (for disconnected memebr)
        public async Task<Member?> UpdateLoginFieldToFalse(int id, Member member)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Members/updateLoginFieldForDisconnectedonnectedMemebr/{id}", member);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Member?>();
            }
            return null; // Return null if the update fails
        }

        // DELETE 1 MEMBER
        public async Task<bool> DeleteMemberFromChatAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Members/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true; // return true if the member successfully deleted
                } else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response: {responseBody}");
                    Console.WriteLine("Failed to delete member. Status Code: " + response.StatusCode);
                    return false;
                }
            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request failed: {ex.Message}");
                return false;
            } catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        // DELETE ALL MEMEBRS
        public async Task<bool> DeleteAllMembersAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync("api/members/allMembers");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("All members deleted successfully.");
                    return true;
                } else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to delete members. Status code: {response.StatusCode}, Error: {errorContent}");
                    return false;
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
                Console.WriteLine($"HTTP Request error: {httpRequestEx.Message}"); // Log HTTP request exceptions (e.g., network issues)
                return false;
            } catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}"); // Print generic exceptions
                return false;
            }
        }
    }
}