using System;
using Client.Commands.Responses;
using Client.Session;

namespace Client.Commands.Commands
{
    public class BXInfo : IBXCommand
    {
        private const string command = "info";

        public string Name()
        {
            return command;
        }
        
        public IBXCommandResponse Execute(BXSession session)
        {
            Console.WriteLine("Executing INFO command");
            
            session.Send(command);

            string result = session.Receive();
            string info = session.Receive();
            
            bool ok = session.ReceiveByte() is 0;

            return new BXCommandResponse(result, info, ok);
        }
    }
}