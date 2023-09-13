using System;
using System.Collections.Generic;
using System.Text;

namespace Io.HcxProtocol.Exceptions
{
    public class IllegalAccessException : ReflectiveOperationException
    {
        private const long serialVersionUID = 6616958222490762034L;

        /// <summary>
        /// Constructs an {@code IllegalAccessException} without a
        /// detail message.
        /// </summary>
        public IllegalAccessException() : base()
        {
        }

        /// <summary>
        /// Constructs an {@code IllegalAccessException} with a detail message.
        /// </summary>
        /// <param name="s">   the detail message. </param>
        public IllegalAccessException(string s) : base(s)
        {
        }
    }

}
