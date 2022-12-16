using System;

namespace WebOsRemote.Net.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException(string message) : base(message){}
    }
}
