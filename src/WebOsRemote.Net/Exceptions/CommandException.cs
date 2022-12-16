using System;

namespace WebOsRemote.Net.Exceptions
{
    public class CommandException : Exception
    {
        public CommandException(string error) : base(error)
        {
        }
    }
}
