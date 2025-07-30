namespace WebAppTest.Exceptions;

using System.Net;

public class HttpException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}