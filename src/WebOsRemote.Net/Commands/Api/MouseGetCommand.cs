namespace WebOsRemote.Net.Commands.Api
{

    /// <summary>
    /// Command for getting if the WebOS device supports mouse.
    /// </summary>
    public class MouseGetCommand : CommandBase
    {
        public override string Uri => "ssap://com.webos.service.networkinput/getPointerInputSocket";
    }


    /// <summary>
    /// Response for <seealso cref="MouseGetCommand"/>.
    /// </summary>
    public class MouseGetResponse : ResponseBase
    {
        public string SocketPath { get; set; }
    }
}
