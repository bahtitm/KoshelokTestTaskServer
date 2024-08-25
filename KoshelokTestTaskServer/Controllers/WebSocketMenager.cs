using System.Net.WebSockets;
using System.Text;

namespace KoshelokTestTaskServer.Controllers
{
    public class WebSocketMenager
    {
        private ClientWebSocket _client = new ClientWebSocket();

        public async Task ConnectAsync(Uri uri)
        {
            await _client.ConnectAsync(uri, CancellationToken.None);
        }
        public async Task SendMessageAsync(string message)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await _client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        //public async Task Send(string message)
        //{
        //    using (var client = new ClientWebSocket())
        //    {
        //        try
        //        {
        //            // Connect to the WebSocket server
        //            Uri serverUri = new Uri("wss://localhost:7075//ws"); // Adjust the URL as needed
        //            await client.ConnectAsync(serverUri, CancellationToken.None);
        //            Console.WriteLine("Connected to WebSocket server");

        //            //// Start receiving messages
        //            //_ = Task.Run(async () =>
        //            //{
        //            //    await ReceiveMessagesAsync(client);
        //            //});

        //            // Send messages                  

        //            await SendMessageAsync(client, message);


        //            //// Close the connection
        //            //await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
        //            //Console.WriteLine("Connection closed");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Exception: {ex.Message}");
        //        }
        //    }
        //}

        //private static async Task SendMessageAsync(ClientWebSocket client, string message)
        //{
        //    var buffer = Encoding.UTF8.GetBytes(message);
        //    var segment = new ArraySegment<byte>(buffer);

        //    await client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        //    Console.WriteLine($"Sent: {message}");
        //}

        //private static async Task ReceiveMessagesAsync(ClientWebSocket client)
        //{
        //    var buffer = new byte[1024 * 4];

        //    while (client.State == WebSocketState.Open)
        //    {
        //        var segment = new ArraySegment<byte>(buffer);
        //        var result = await client.ReceiveAsync(segment, CancellationToken.None);

        //        if (result.MessageType == WebSocketMessageType.Text)
        //        {
        //            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        //            Console.WriteLine($"Received: {message}");
        //        }
        //        else if (result.MessageType == WebSocketMessageType.Close)
        //        {
        //            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closed", CancellationToken.None);
        //            Console.WriteLine("Connection closed by server");
        //        }
        //    }
        //}
    }
}
