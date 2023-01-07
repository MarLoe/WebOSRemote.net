namespace WebOsRemote.Net.Device
{
    /// <summary>
    /// Class representing a WebOS device.
    /// </summary>
    /// <seealso cref="IDevice"/>
    public class WebOSDevice : IDevice
    {
        private string _hostName;

        #region IDevice

        public string HostName
        {
            get => string.IsNullOrEmpty(_hostName) ? IPAddress : _hostName;
            set => _hostName = value;
        }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }

        public string PairingKey { get; set; }

        #endregion

    }
}

