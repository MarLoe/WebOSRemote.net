namespace WebOsRemote.Net.Commands
{
    /// <summary>
    /// The basics of a response to a command.
    /// </summary>
    /// <seealso cref="CommandBase"/>
    public abstract class ResponseBase
    {
        /// <summary>
        /// If true the command was successfull, else false.
        /// </summary>
        public bool ReturnValue { get; set; }
    }
}
