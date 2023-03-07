namespace Io.HcxProtocol.Dto
{
    /// <summary>
    /// The HttpResponse class to capture the REST API status code and response body.
    /// </summary>
    public class HttpResponse
    {
        public int Status { get; set; }
        public string Body { get; set; }

        public HttpResponse(int status, string body)
        {
            Status = status;
            Body = body;
        }
    }
}
