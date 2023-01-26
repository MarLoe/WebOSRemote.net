using System;

namespace WebOsRemote.Net.Device
{
    /// <summary>
    /// Device information for the WebOS device to control.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Can be the host name or the IP address.
        /// </summary>
        string HostName { get; set; }

        /// <summary>
        /// The host IP address.
        /// </summary>
        string IPAddress { get; set; }

        /// <summary>
        /// The mac address - can be used for Wake.On-Lan (WOL) command
        /// to power on the WebOS device.
        /// </summary>
        string MacAddress { get; set; }

        /// <summary>
        /// The pairing key received from the first pairing.
        /// </summary>
        string PairingKey { get; set; }
    }
}

