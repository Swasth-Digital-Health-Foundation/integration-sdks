using System;


namespace Io.HcxProtocol.Exceptions
{
    /// <summary>
    /// The exception to capture the client errors.
    /// </summary>
    public class ClientException : Exception
    {
        public ClientException(string message) : base(message)
        {
        }
    }

}

