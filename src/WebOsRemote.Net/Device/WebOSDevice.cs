namespace WebOsRemote.Net.Device
{
    /// <summary>
    /// Class representing a WebOS device.
    /// </summary>
    /// <seealso cref="IDevice"/>
    public class WebOSDevice : IDevice
    {

        #region IDevice

        public string HostName { get; set; }

        public string MacAddress { get; set; }

        public string PairingKey { get; set; }

        #endregion

    }
}

