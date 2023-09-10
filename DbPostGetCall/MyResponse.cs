using System.Net;

namespace DbPostGetCall
{
    public class MyResponse
    {
        public HttpStatusCode Status { get; set; }

        public object? Data { get; set; }
    }
}
