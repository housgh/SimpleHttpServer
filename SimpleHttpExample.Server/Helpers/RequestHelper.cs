using System.Linq;
using SimpleHttpExample.Server.Exceptions;

namespace SimpleHttpExample.Server.Helpers;

public static class RequestHelper
{
    public static SimpleHttpRequest Parse(string request)
    {
        request = request.Replace("\0", string.Empty);
        var method = request.StartsWith("GET") ? HttpMethod.Get :
            request.StartsWith("POST") ? HttpMethod.Post :
            request.StartsWith("PUT") ? HttpMethod.Put :
            request.StartsWith("DELETE") ? HttpMethod.Delete :
            throw new InvalidMethodException();
        var httpRequest = new SimpleHttpRequest(method);
        var requestValues = request.Split(Environment.NewLine);
        foreach (var requestValue in requestValues[1..])
        {
            if(requestValue.Equals(string.Empty)) break;
            var headerKeyValuePair = requestValue.Split(':');
            if(headerKeyValuePair.Length != 2) continue;
            httpRequest.Headers.Add(headerKeyValuePair[0].Trim(), headerKeyValuePair[1].Trim());
        }

        httpRequest.Route = requestValues[0].Split(" ")[1][1..].Trim();
        var contentLengthExists = httpRequest.Headers.TryGetValue("Content-Length", out var contentLengthStr);
        if (!contentLengthExists || contentLengthStr is null) return httpRequest;
        var contentLength = int.Parse(contentLengthStr);
        httpRequest.StringContent = string.Join(string.Empty, request.TakeLast(contentLength));
        return httpRequest;
    }
}

public class SimpleHttpRequest
{
    internal SimpleHttpRequest(HttpMethod method)
    {
        RequestMethod = method;
    }

    public HttpMethod RequestMethod { get; set; }
    public string Route { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public string? StringContent { get; set; }
}