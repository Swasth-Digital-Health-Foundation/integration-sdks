namespace Io.HcxProtocol.Dto
{
    /**
     * Library  : Io.Hcx.Protocol.Core
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The ResponseError class which wrap the error details and helps in creating an instance of it.
    /// </summary>
    public class ResponseError
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public string Trace { get; private set; }

        public ResponseError(string code, string message, string trace)
        {
            Code = code;
            Message = message;
            Trace = trace;
        }
    }
}
