using System;
using WebOsRemote.Net.Device;
using WebSocketSharp;

namespace WebOsRemote.Net.WebSockets
{
    public class SocketConnection : ISocketConnection
    {
        private WebSocket _socket;

        #region ISocketConnection

        public string Url { get; private set; }

        public bool IsAlive => _socket.IsAlive;

        public event EventHandler OnDisconnected;

        public event EventHandler<SocketMessageEventArgs> OnMessage;

        public void Connect(IDevice device)
        {
            Connect($"ws://{device.HostName}:3000");
        }

        public void Connect(string url)
        {
            Url = url;

            _socket = new WebSocket(Url);
            _socket.OnClose += (s, e) => OnDisconnected?.Invoke(this, EventArgs.Empty);
            _socket.OnMessage += (s, e) => OnMessage?.Invoke(this, new SocketMessageEventArgs(e.Data));
            _socket.Connect();
        }

        public void Send(string content)
        {
            _socket?.Send(content);
        }

        public void Close()
        {
            _socket?.Close();
            _socket = null;
        }

        #endregion
    }
}
