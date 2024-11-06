using BCL.Communicator.Messages;
using System.Threading;

namespace BCL.Communicator;

/// <summary>
/// Extension methods for the Communicator mod.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Sets the channels for the player.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <param name="inputs">The input channels.</param>
    /// <param name="outputs">The output channels.</param>
    public static void SetChannels(this PlayerControl player, string[] inputs, string[] outputs)
    {
        CommunicatorPlugin.WebSocketClient?.SendMessage(
            new ChannelMessage(player.PlayerId, new ChannelMessage.ChannelValue(inputs, outputs)),
            CancellationToken.None);
    }

    /// <summary>
    /// Resets the channels for the player.
    /// <param name="player">The player.</param>
    /// </summary>
    public static void ResetChannels(this PlayerControl player)
    {
        player.SetChannels(["default"], ["default"]);
    }

    /// <summary>
    /// Sets the target/reference player for BCL.
    /// </summary>
    /// <param name="player">The player to track/reference.</param>
    public static void SetReferencePlayer(this PlayerControl player)
    {
        CommunicatorPlugin.WebSocketClient?.SendMessage(
            new ReferencePlayerMessage(player.PlayerId),
            CancellationToken.None);
    }
}
