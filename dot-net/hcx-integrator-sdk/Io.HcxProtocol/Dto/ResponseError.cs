namespace Io.HcxProtocol.Dto
{
    /// <summary>
    /// The ResponseError class which wrap the error details and helps in creating an instance of it.
    /// </summary>
    public class ResponseError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Trace { get; set; }

        public ResponseError(string code, string message, string trace)
        {
            Code = code;
            Message = message;
            Trace = trace;
        }
    }
}
