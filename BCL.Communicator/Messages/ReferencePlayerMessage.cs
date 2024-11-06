namespace BCL.Communicator.Messages;

public class ReferencePlayerMessage : MessageBase
{
    public override string Type => "refplayer";
    public override object Value { get; }

    public ReferencePlayerMessage(byte id)
    {
        Id = id;
        Value = Id;
    }
}
