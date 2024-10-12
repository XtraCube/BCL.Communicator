namespace BCL.Communicator.Messages;

public class ChannelMessage : MessageBase
{
    public override string Type => "channels";
    public override object Value { get; }

    public ChannelMessage(byte id, ChannelValue value)
    {
        Id = id;
        Value = value;
    }

    public struct ChannelValue(string[] input, string[] output)
    {
        public string[] Input { get; } = input;
        public string[] Output { get; } = output;
    }
}
