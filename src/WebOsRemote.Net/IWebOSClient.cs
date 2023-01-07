using System;
using System.Threading.Tasks;
using WebOsRemote.Net.Commands;
using WebOsRemote.Net.Commands.Tv;
using WebOsRemote.Net.Device;

namespace WebOsRemote.Net
{
    /// <summary>
    /// Client for communicating with a WebOS device.
    /// </summary>
    public interface IWebOSClient
    {
        /// <summary>
        /// Raised when the pairing key is updated.
        /// </summary>
        event EventHandler<PairingUpdatedEventArgs> PairingUpdated;

        /// <summary>
        /// Returns true if client has an ctive connection.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Connect to a <paramref name="device"/> and
        /// establish connection with a handshake.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        Task ConnectAsync(IDevice device);

        /// <summary>
        /// Close the connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Send command to the WebOS device.
        /// </summary>
        /// <typeparam name="TResponse">
        /// The expected response type.
        /// </typeparam>
        /// <param name="command">
        /// The command to send.
        /// </param>
        /// <returns>
        /// The response or Task failed on error.
        /// </returns>
        Task<TResponse> SendCommandAsync<TResponse>(CommandBase command) where TResponse : ResponseBase;

        /// <summary>
        /// Send remote control button press.
        /// </summary>
        /// <param name="type">
        /// The button to send.
        /// </param>
        Task SendButtonAsync(ButtonType type);
    }

    /// <summary>
    /// Event arguemnts for <see cref="IWebOSClient.PairingUpdated"/>.
    /// </summary>
    public class PairingUpdatedEventArgs : EventArgs
    {

        public PairingUpdatedEventArgs(IDevice device)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
        }

        /// <summary>
        /// The updated device.
        /// </summary>
        public IDevice Device { get; }
    }
}