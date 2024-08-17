using GmodOWOIntegration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

[Serializable]
class GmodOWOData
{
    public required string damageType;
    public required string hitbox;
    public required string direction;
}
class GmodUdpServer
{
    UdpClient? udpClient;
    private CancellationTokenSource? cancellationTokenSource;

    public void StartServer(int port = 54321)
    {
        udpClient = new UdpClient();
        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udpClient.Client.Bind(new IPEndPoint(IPAddress.Loopback, port));

        Console.WriteLine("Server started on port " + port + ". Waiting for incoming UDP packets...");

        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;

        Task serverTask = Task.Run(() =>
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    while (true)
                    {
                        IPEndPoint remoteEndPoint = new(IPAddress.Any, 0);
                        byte[] data = udpClient.Receive(ref remoteEndPoint);

                        if (data != null)
                        {
                            //Console.WriteLine($"Received data from {remoteEndPoint}");
                            string jsonString = Encoding.UTF8.GetString(data);
                            var deserializedObject = JsonConvert.DeserializeObject<GmodOWOData>(jsonString);

                            if (deserializedObject != null)
                            {
                                GmodOWOData json = deserializedObject;
                                OWOIntegration.ParseOWOData(json.damageType, json.hitbox, json.direction);
                                //Console.WriteLine("Damage type: " + json.damageType);
                                //Console.WriteLine("Hitbox: " + json.hitbox);
                                //Console.WriteLine("Direction: " + json.direction);
                            }
                            else
                            {
                                Console.WriteLine("Invalid JSON data received.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred with the UDP server: " + ex.Message);
                }
            }
        }, token);
    }
    public void StopServer()
    {
        cancellationTokenSource?.Cancel();
    }
}
