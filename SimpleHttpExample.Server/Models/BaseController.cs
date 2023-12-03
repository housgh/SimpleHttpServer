using System.Net;
using SimpleHttpExample.Server.Helpers;

namespace SimpleHttpExample.Server.Models;

public abstract class BaseController
{
    public SimpleHttpRequest Request { get; set; }
}

public class HttpResult
{
    public HttpResult(HttpStatusCode httpStatusCode, object? content = null)
    {
        HttpStatusCode = httpStatusCode;
        Content = content;
    }

    public HttpStatusCode HttpStatusCode { get; }
    public object? Content { get; }
}