namespace Client.Commands.Responses
{
    public interface IBXCommandResponse
    {
        string Result { get; }
        string Info { get; }
        
        bool Ok { get; }
    }
}