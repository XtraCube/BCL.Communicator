using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

namespace BCL.Communicator;

/// <summary>
/// Plugin for Communicator Mod.
/// </summary>
[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class CommunicatorPlugin : BasePlugin
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommunicatorPlugin"/> class.
    /// </summary>
    public CommunicatorPlugin()
    {
        Logger = Log;
    }

    /// <summary>
    /// Gets the Logger.
    /// </summary>
    public static ManualLogSource? Logger { get; private set; }

    private string WebSocketServerAddress { get; set; } = "http://localhost:8080";

    /// <inheritdoc />
    public override void Load()
    {
        var webSocket = new WebSocketClient(WebSocketServerAddress);
        Task.Run(webSocket.StartAsync);
    }
}
