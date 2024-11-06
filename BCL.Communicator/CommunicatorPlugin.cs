using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BCL.Communicator.Messages;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using UnityEngine;

namespace BCL.Communicator;

/// <summary>
/// Plugin for Communicator Mod.
/// </summary>
[BepInAutoPlugin("dev.xtracube.communicator", "BCL Communicator", "1.0.0")]
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

    private string WebSocketServerAddress { get; set; } = "ws://localhost:8080";

    internal static WebSocketClient? WebSocketClient { get; set; }

    /// <inheritdoc />
    public override void Load()
    {
        IL2CPPChainloader.Instance.Finished += () =>
        {
            if (IL2CPPChainloader.Instance.Plugins.Values.Any(x=>x.Dependencies.Any(y=>y.DependencyGUID==Id)))
            {
                WebSocketClient = new WebSocketClient(WebSocketServerAddress);
                Task.Run(WebSocketClient.StartAsync);
            }
            else
            {
                Logger.LogInfo("No dependencies found, skipping WebSocketClient initialization.");
            }
        };
    }
}
