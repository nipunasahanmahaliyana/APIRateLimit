using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace APIRateLimit
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLimitMiddleware> _logger;
        private readonly int _requetstLimit = 5;
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1); 
        
        public RateLimitMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<RateLimitMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ipAddress))
            {
                // If we can't determine the user IP, reject the request
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Unable to determine user IP address.");
                return;
            }

            var cacheKey = $"RequestCount_{ipAddress}";
            // Try to get the current request count for this IP address
            var currentRequestCount = _cache.Get<int>(cacheKey);

            if (currentRequestCount >= _requetstLimit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                return;
            }

            // If the user hasn't exceeded the limit, increment the count
            _cache.Set(cacheKey, currentRequestCount + 1, DateTimeOffset.Now.Add(_timeWindow));

            await _next(context);
        }   

    }
}
