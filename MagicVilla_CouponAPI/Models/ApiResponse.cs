using System.Net;

namespace MagicVilla_CouponAPI.Models;

public class ApiResponse
{
    public ApiResponse()
    {
        ErrorMessages = new List<string>();
    }
    public bool IsSuccess { get; set; }
    public object Result { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public List<string> ErrorMessages { get; set; }
}