using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.Policies;

public class UsersMicroservicePolicies : IUsersMicroservicePolicies
{
    private readonly ILogger<UsersMicroservicePolicies> _logger;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
    private readonly IAsyncPolicy<HttpResponseMessage> _circuitBreakerPolicy;
    private readonly IAsyncPolicy<HttpResponseMessage> _timeoutPolicy;

    public UsersMicroservicePolicies(ILogger<UsersMicroservicePolicies> logger)
    {
        _logger = logger;

        _retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 4, //Number of retries
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)), // Delay between retries
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    _logger.LogInformation($"Retry {retryAttempt} after {timespan.TotalSeconds} seconds");
                });

        _circuitBreakerPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3, //Number of retries
                durationOfBreak: TimeSpan.FromMinutes(2), // Delay between retries
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogInformation($"Circuit breaker opened for {timespan.TotalMinutes} minutes due to consecutive 3 failures. The subsequent requests will be blocked");
                },
                onReset: () => {
                    _logger.LogInformation($"Circuit breaker closed. The subsequent requests will be allowed.");
                });
        _timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(1500));
    }

    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        

        return _timeoutPolicy;
    }
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return _retryPolicy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return _circuitBreakerPolicy;
    }
}