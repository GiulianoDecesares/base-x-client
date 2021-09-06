using Client.Commands.Responses;
using Client.Session;

namespace Client.Commands.Commands
{
    public interface IBXCommand
    {
        string Name();
        
        IBXCommandResponse Execute(BXSession session);
    }
}