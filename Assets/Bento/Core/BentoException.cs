using System;
using System.Runtime.Serialization;

namespace Bento
{
    public class BentoException : Exception
    {
        public BentoException() : base()
        {
        }

        public BentoException(string message) : base(message)
        {
        }

        public BentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}