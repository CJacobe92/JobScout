using Prometheus;

namespace JobScout.API.Middlewares;

public class MetricsLabelingMiddleware : IMiddleware
{
    private static readonly Counter RequestCounter = Metrics
        .CreateCounter("http_requests_by_route_total", "Total requests by route",
            new CounterConfiguration
            {
                LabelNames = ["method", "route", "status"]
            });

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var originalPath = context.Request.Path;
        var method = context.Request.Method;

        await next(context); // Let the pipeline continue

        var statusCode = context.Response.StatusCode.ToString();
        var matchedEndpoint = context.GetEndpoint();

        var route = matchedEndpoint?.DisplayName ?? originalPath;

        RequestCounter.WithLabels(method, route, statusCode).Inc();
    }
}
