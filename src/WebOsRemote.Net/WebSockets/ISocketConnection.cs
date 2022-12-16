using System;
using WebSocketSharp;

namespace WebOsRemote.Net.WebSockets
{
    /// <summary>
    /// Socket connection for talking to a WebOS device.
    /// </summary>
    public interface ISocketConnection
    {
        /// <summary>
        /// The url of teh device.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Are we still connected.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Raised when data is reeived from the WebOS device.
        /// </summary>
        event EventHandler<SocketMessageEventArgs> OnMessage;

        /// <summary>
        /// Connecto to the WebOS device.
        /// </summary>
        /// <param name="url">
        /// The url identifiying the device to connect to.
        /// </param>
        void Connect(string url);

        /// <summary>
        /// Send content to the WebOS device.
        /// </summary>
        /// <param name="content">
        /// The content to send.
        /// </param>
        void Send(string content);

        /// <summary>
        /// Close the socket connection.
        /// </summary>
        void Close();
    }

    /// <summary>
    /// Event arguemnts for <see cref="ISocketConnection.OnMessage"/>.
    /// </summary>
    public class SocketMessageEventArgs : EventArgs
    {
        internal SocketMessageEventArgs(string data)
        {
            Data = data;
        }

        /// <summary>
        /// Data recieved.
        /// </summary>
        public string Data { get; }
    }
}