using System;


namespace Io.HcxProtocol.Exceptions
{
    /// <summary>
    /// The exception to capture the unknown errors.
    /// </summary>
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message)
        {
        }
    }

}
