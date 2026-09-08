

using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using System.Net.Http.Json;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.HttpClients;
public class UsersMicroserviceClients
{
    private readonly HttpClient _httpClient;

    public UsersMicroserviceClients(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<UserDTO?> GetUserByUserID(Guid userId)
    {
      HttpResponseMessage  response=   await _httpClient.GetAsync($"/api/users{userId}");

        if (!response.IsSuccessStatusCode)
        {
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                throw new HttpRequestException($"Http request failed with status code {response.StatusCode}");

            }
        }

        UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
        if(user is null)
        {
            throw new ArgumentException("Invalid user id ");

        }
        return user;
    }
}

