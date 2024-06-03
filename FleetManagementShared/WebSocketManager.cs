using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FleetManagementShared
{
    public class WebSocketsManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public async Task HandleWebSocketAsync(HttpContext context, WebSocket webSocket)
        {
            var socketId = Guid.NewGuid().ToString();
            _sockets.TryAdd(socketId, webSocket);

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            _sockets.TryRemove(socketId, out _);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public async Task BroadcastMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var tasks = _sockets.Values.Select(async socket =>
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }).ToArray();

            await Task.WhenAll(tasks);
        }
    }
}
