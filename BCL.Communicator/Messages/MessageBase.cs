namespace BCL.Communicator.Messages;

public abstract class MessageBase
{
    public abstract string Type { get; }
    public abstract object Value { get; }
    public byte Id { get; set; }
}
