using System.Net.WebSockets;
using System.Text;

namespace WebSocketClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (ClientWebSocket webSocket = new ClientWebSocket())
            {
                Uri serverUri = new Uri("ws://localhost:5151/");
                await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                Console.ForegroundColor = ConsoleColor.Green;   
                Console.WriteLine("Connecté au serveur WebSocket.");
                Console.ForegroundColor = ConsoleColor.White;  
                string userPseudo = UserChosePseudo();
                await Task.WhenAll(ReceiveMessages(webSocket), SendMessages(webSocket, userPseudo));
            }
        }

        static async Task ReceiveMessages(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine(message);
            }
        }

        static async Task SendMessages(ClientWebSocket webSocket, string pseudo)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                Console.ForegroundColor= ConsoleColor.Blue;
                Console.Write($"[{pseudo}] : ");
                Console.ForegroundColor = ConsoleColor.White;   
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes($"[{pseudo}] :{message}");
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static string UserChosePseudo()
        {
            string? userPseudo = "";
            Console.WriteLine("Choisissez un pseudo : ");
            while (String.IsNullOrEmpty(userPseudo))
            {
                userPseudo = Console.ReadLine();
            }

            Console.WriteLine();
            return userPseudo;
        }
    }
}
