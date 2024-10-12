using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BCL.Communicator.Messages;

namespace BCL.Communicator;

internal sealed class WebSocketClient(string uri) : IDisposable
{
    private ClientWebSocket _webSocket = new();
    private readonly Uri _serverUri = new(uri);
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private bool _isDisposed;

    public async Task StartAsync()
    {
        await ConnectAsync();
    }

    public Task SendMessage(MessageBase data, CancellationToken cancellation)
    {
        var encoded = JsonSerializer.SerializeToUtf8Bytes(data);
        var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
        return _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
    }

    private async Task ConnectAsync()
    {
        try
        {
            CommunicatorPlugin.Logger?.LogWarning("Attempting to connect to WebSocket server...");
            await _webSocket.ConnectAsync(_serverUri, _cancellationTokenSource.Token);

            if (_webSocket.State == WebSocketState.Open)
            {
                CommunicatorPlugin.Logger?.LogWarning("Connected to the WebSocket server.");
                await ReceiveMessagesAsync();
            }
        }
        catch (Exception ex)
        {
            CommunicatorPlugin.Logger?.LogError($"Connection failed: {ex.Message}");
            await Task.Delay(5000); // Wait 5 seconds before retrying
            await ReconnectAsync();
        }
    }

    private async Task ReconnectAsync()
    {
        if (_isDisposed || _cancellationTokenSource.IsCancellationRequested) return;

        // Create a new ClientWebSocket instance for reconnection
        _webSocket.Dispose();
        _webSocket = new ClientWebSocket();

        await ConnectAsync();
    }

    private async Task ReceiveMessagesAsync()
    {
        byte[] buffer = new byte[1024];

        try
        {
            while (_webSocket.State == WebSocketState.Open && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    CommunicatorPlugin.Logger?.LogWarning("WebSocket connection closed.");
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _cancellationTokenSource.Token);
                    await ReconnectAsync();
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    CommunicatorPlugin.Logger?.LogWarning($"Received: {message}");
                }
            }
        }
        catch (WebSocketException ex)
        {
            CommunicatorPlugin.Logger?.LogError($"WebSocket error: {ex.Message}");
            await ReconnectAsync();
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation
        }
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _cancellationTokenSource.Cancel();
        _webSocket.Dispose();
        _cancellationTokenSource.Dispose();

        _isDisposed = true;
    }
}
