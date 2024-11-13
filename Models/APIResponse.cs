using System.Net;

namespace MagicVilla.Api.Models;

public class APIResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<string> ErrorMessage { get; set; }
    public object Data { get; set; }
}