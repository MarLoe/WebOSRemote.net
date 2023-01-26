
using WebOsRemote.Net;
using WebOsRemote.Net.Device;

namespace WebOSRemote.Net.Console
{

    class Program
    {
        static async Task Main(string[] args)
        {
            //var device = new WebOSDevice() { HostName = "LGwebOSTV.local" };
            var device = new WebOSDevice() { IPAddress = "192.168.100.116" };
            using var client = new WebOSClient();
            await client.Attach(device);
            await client.ConnectAsync();

        }
    }
}

