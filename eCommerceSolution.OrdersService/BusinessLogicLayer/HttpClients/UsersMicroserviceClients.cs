

using DnsClient.Internal;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net.Http.Json;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.HttpClients;
public class UsersMicroserviceClients
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UsersMicroserviceClients> _logger;
    public UsersMicroserviceClients(HttpClient httpClient,ILogger<UsersMicroserviceClients> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    public async Task<UserDTO?> GetUserByUserID(Guid userId)
    {
        try
        {


            HttpResponseMessage response = await _httpClient.GetAsync($"/api/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    //throw new HttpRequestException($"Http request failed with status code {response.StatusCode}");
                    return new UserDTO(
                        PersonName: "Temporarily Unavailable",
                        Email: "Temporarily Unavailable",
                        Gender: "Temporarily Unavailable",
                        UserID: Guid.Empty

                        );

                }
            }

            UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
            if (user is null)
            {
                throw new ArgumentException("Invalid user id ");

            }
            return user;
        }catch(BrokenCircuitException ex)
        {
             _logger.LogError(ex, "Request failed because of circuit breaker is in Open state. Returning dummy data.");
            return new UserDTO(
             PersonName: "Temporarily Unavailable (circuit breaker)",
             Email: "Temporarily Unavailable (circuit breaker)",
             Gender: "Temporarily Unavailable (circuit breaker)",
             UserID: Guid.Empty);
        }
        catch (TimeoutRejectedException ex) {
            _logger.LogError(ex, "Timeout occurred while fetching user data. Returning dummy data");

            return new UserDTO(
                    PersonName: "Temporarily Unavailable (timeout)",
                    Email: "Temporarily Unavailable (timeout)",
                    Gender: "Temporarily Unavailable (timeout)",
                    UserID: Guid.Empty);
        }
    }
}

