using System;
using Newtonsoft.Json.Linq;

namespace WebOsRemote.Net.Commands
{
    /// <summary>
    /// The actual message format to be sent to the WebOS device.
    /// </summary>
    internal class Message
    {
        /// <summary>
        /// The id of the message. Resposed will be tagged
        /// with the same id to match request/response.
        /// </summary>
        public string Id { get; set; } = GenerateId();

        /// <summary>
        /// The type of message. Can be:
        /// request
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The uri of the message identifying the command being sent.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The payload of the message (e.g. command json)
        /// </summary>
        public JObject Payload { get; set; }

        /// <summary>
        /// Error status when the message is recieved as a response.
        /// </summary>
        public string Error { get; set; }


        internal static string GenerateId()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
