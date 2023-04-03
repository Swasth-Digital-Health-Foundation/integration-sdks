namespace Io.HcxProtocol.Dto
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The HttpResponse class to capture the REST API status code and response body.
    /// </summary>
    public class HttpResponse
    {
        public int Status { get; private set; }
        public string Body { get; private set; }

        public HttpResponse(int status, string body)
        {
            Status = status;
            Body = body;
        }
    }
}
