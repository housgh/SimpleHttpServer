using System.Net;
using System.Text;
using System.Text.Json;

namespace SimpleHttpExample.Server.Models;

public class HttpResponse
{
    public HttpResponse(HttpStatusCode statusCode = HttpStatusCode.OK, object? content = null, Dictionary<string, string>? headers = null)
    {
        StatusCode = statusCode;
        Content = content;
        if (content is HttpResult result)
        {
            StatusCode = result.HttpStatusCode;
            Content = result.Content;
        }
        headers ??= new Dictionary<string, string>();
        if(!headers.ContainsKey("Date"))
            headers.Add("Date", DateTime.Now.ToString("ddd, dd MMM yyyy hh:mm:ss"));
        if(!headers.ContainsKey("Content-Type"))
            headers.Add("Content-Type", "application/json");
        if(!headers.ContainsKey("Connection"))
            headers.Add("Connection", "keep-alive");
        ResponseHeaders = headers;
    }

    public HttpStatusCode StatusCode { get; set; }
    public object? Content { get; set; }
    public Dictionary<string, string> ResponseHeaders { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"HTTP/1.1 {GetResponseCode()}").Append(Environment.NewLine);
        foreach (var keyValuePair in ResponseHeaders)
        {
            sb.Append($"{keyValuePair.Key}: {keyValuePair.Value}")
                .Append(Environment.NewLine);
        }

        sb.Append(Environment.NewLine);
        if (Content is null) return sb.ToString();
        sb.Append(JsonSerializer.Serialize(Content));
        return sb.ToString();
    }

    private string GetResponseCode()
    {
        return $"{(int)StatusCode} {StatusCode}";
    }
}