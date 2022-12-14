using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using WebOsRemote.Net.Commands;
using WebOsRemote.Net.Commands.Api;
using WebOsRemote.Net.Commands.Tv;
using WebOsRemote.Net.Device;
using WebOsRemote.Net.Exceptions;
using WebOsRemote.Net.Factory;
using WebOsRemote.Net.Json;
using WebOsRemote.Net.WebSockets;

namespace WebOsRemote.Net
{
    public class WebOSClient : IWebOSClient, IDisposable
    {
        private readonly IFactory<ISocketConnection> _socketFactory;
        private readonly ILogger<WebOSClient> _logger;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<Message>> _completionSources = new ConcurrentDictionary<string, TaskCompletionSource<Message>>();

        private ISocketConnection _socket;
        private ISocketConnection _mouseSocket;

        private IDevice _device;

        public int CommandTimeout { get; set; } = 5000;

        public WebOSClient() : this(new NullLogger<WebOSClient>())
        {
        }

        public WebOSClient(ILogger<WebOSClient> logger) : this(new Factory<ISocketConnection>(() => new SocketConnection()), logger)
        {
        }

        internal WebOSClient(IFactory<ISocketConnection> socketFactory, ILogger<WebOSClient> logger)
        {
            _socketFactory = socketFactory;
            _logger = logger;
        }

        #region IClient

        public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;

        public event EventHandler<PairingUpdatedEventArgs> PairingUpdated;

        public virtual bool IsConnected => _socket?.IsAlive is true;

        public virtual bool IsPaired { get; private set; }


        public virtual async Task Attach(IDevice device)
        {
            if (device == _device && _socket?.IsAlive is true)
            {
                // All good - no need to reattach
                return;
            }

            CloseSockets();

            _device = device;

            _socket = _socketFactory.Create();
            _socket.OnMessage += OnMessage;
            _socket.OnDisconnected += (s, e) => ConnectionChanged?.Invoke(this, new(_device, false));

            await Task.Run(() =>
            {
                _socket.Connect(device);
                if (!_socket.IsAlive)
                {
                    throw new ConnectionException($"Unable to conenct to WebOS device at {device.HostName}.");
                }
                ConnectionChanged?.Invoke(this, new(_device, true));
            });
        }

        public virtual async Task ConnectAsync(IDevice device)
        {
            await Attach(device);

            var handshakeResponse = await SendCommandAsync<HandshakeResponse>(new HandshakeCommand(_device.PairingKey));
            if (handshakeResponse.ReturnValue)
            {
                var updated = _device.PairingKey != handshakeResponse.Key;
                IsPaired = true;
                _device.PairingKey = handshakeResponse.Key;
                PairingUpdated?.Invoke(this, new(_device, updated));
            }

            var mouseCommand = new MouseGetCommand();
            var mouseGetResponse = await SendCommandAsync<MouseGetResponse>(mouseCommand);
            if (mouseGetResponse.ReturnValue && !string.IsNullOrEmpty(mouseGetResponse.SocketPath))
            {
                _mouseSocket = _socketFactory.Create();
                await Task.Run(() => _mouseSocket.Connect(mouseGetResponse.SocketPath));
                if (!_mouseSocket.IsAlive)
                {
                    throw new ConnectionException($"Unable to conenct to television mouse service at {_device.HostName}.");
                }
            }
        }

        public virtual void Close()
        {
            _device = null;
            CloseSockets();
        }

        public virtual Task<TResponse> SendCommandAsync<TResponse>(CommandBase command) where TResponse : ResponseBase
        {
            // Create a default timeout - in case we never get a response.
            var ctsTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            return SendCommandAsync<TResponse>(command, ctsTimeout.Token);
        }

        public virtual async Task<TResponse> SendCommandAsync<TResponse>(CommandBase command, CancellationToken cancellationToken) where TResponse : ResponseBase
        {
            if (_device is null)
            {
                throw new InvalidOperationException("Please connect to a device before sending commands");
            }

            if (!IsPaired || _socket?.IsAlive is not true)
            {
                await ConnectAsync(_device);
            }

            var request = new Message
            {
                Uri = command.Uri,
                Type = "request",
                Payload = command.ToJObject()
            };

            if (!string.IsNullOrEmpty(command.CustomId))
            {
                request.Id = command.CustomId;
            }

            if (!string.IsNullOrEmpty(command.CustomType))
            {
                request.Type = command.CustomType;
            }

            var taskSource = new TaskCompletionSource<Message>();
            _completionSources.TryAdd(request.Id, taskSource);

            await Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(request, SerializationSettings.Default);
                _logger.LogTrace("Sending: {json}", json);
                _socket.Send(json);
            }, cancellationToken);

            cancellationToken.Register(() =>
            {
                if (_completionSources.TryRemove(request.Id, out var taskCompletion))
                {
                    taskCompletion.TrySetException(new TimeoutException());
                }
            });

            var response = await taskSource.Task;

            return response.Payload.ToObject<TResponse>(JsonSerializer.CreateDefault(SerializationSettings.Default));
        }

        public async Task SendButtonAsync(ButtonType type)
        {
            if (_device is null)
            {
                throw new InvalidOperationException("Please connect to a device before sending commands");
            }

            if (_mouseSocket is null)
            {
                // Mouse not supported
                return;
            }

            if (!IsPaired || _mouseSocket.IsAlive is false)
            {
                await ConnectAsync(_device);
            }

            _logger.LogTrace("Sending Button: {type}", type);

            _mouseSocket.Send($"type:button\nname:{type.ButtonCode}\n\n");
        }

        #endregion


        #region IDisposable

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            Close();
        }

        #endregion


        internal void OnMessage(object sender, SocketMessageEventArgs e)
        {
            _logger.LogTrace("Received: {data}", e.Data);

            var response = JsonConvert.DeserializeObject<Message>(e.Data, SerializationSettings.Default);

            // We may get multiple responses for register_0 - we can safely ignore this one!
            if (response.Id == "register_0" && response.Payload.Value<string>("pairingType") == "PROMPT")
            {
                return;
            }

            if (_completionSources.TryRemove(response.Id, out var taskCompletion))
            {
                if (response.Type is "error")
                {
                    taskCompletion.TrySetException(new CommandException(response.Error));
                }
                else
                {
                    taskCompletion.TrySetResult(response);
                }
            }
        }


        internal void CloseSockets()
        {
            _socket?.Close();
            _socket = null;

            _mouseSocket?.Close();
            _mouseSocket = null;
        }


        internal void SetSocketsForTesting(ISocketConnection main, ISocketConnection mouse)
        {
            _socket = main;
            _mouseSocket = mouse;
        }
    }
}
