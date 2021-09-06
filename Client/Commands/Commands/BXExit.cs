using Client.Commands.Responses;
using Client.Session;

namespace Client.Commands.Commands
{
    public class BXExit : IBXCommand
    {
        private const string command = "exit";

        public string Name()
        {
            return command;
        }
        
        public IBXCommandResponse Execute(BXSession session)
        {
            session.Send(command);

            string result = session.Receive();
            string info = session.Receive();
            
            bool ok = session.ReceiveByte() is 0;

            return new BXCommandResponse(result, info, ok);
        }
    }
}