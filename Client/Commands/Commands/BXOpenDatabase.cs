using Client.Commands.Responses;
using Client.Session;

namespace Client.Commands.Commands
{
    public class BXOpenDatabase : IBXCommand
    {
        private readonly string command = "open {0}";

        public BXOpenDatabase(string databaseName)
        {
            this.command = string.Format(this.command, databaseName);
        }

        public string Name()
        {
            return command.ToUpper();
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