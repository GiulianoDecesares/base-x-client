using System;
using Client.Authentication;
using Client.Commands.Commands;
using Client.Commands.Responses;
using Client.Session;

namespace Client
{
    internal static class Program
    {
        private static void Execute(BXSession session, IBXCommand command)
        {
            Console.WriteLine($"Executing command {command.Name()}");
            
            IBXCommandResponse response = session.Execute(command);

            if (response.Ok)
            {
                Console.WriteLine($"Command {command.Name()} execution success");
             
                Console.WriteLine($"Result: {response.Result}");
                Console.WriteLine($"Info: {response.Info}");
            }
            else
            {
                Console.WriteLine($"Command {command.Name()} execution failed");
            }
        }
        
        private static void Main()
        {
            using BXSession session = new BXSession(Address.Default);

            if (!new BXAuthenticator(session, Credentials.Default).Authenticate())
            {
                Console.WriteLine("DB authentication failed\n");
                return;
            }

            Execute(session, new BXInfo());
            
            Execute(session, new BXOpenDatabase("factbook"));
            
            /*
            Console.WriteLine("Executing QUERY command");

            session.Send("XQUERY //country/name");
            
            Console.WriteLine($"Result: {session.Receive()}");
            Console.WriteLine($"Info: {session.Receive()}");

            if (session.ReceiveByte() is 1) // Final byte
            {
                Console.WriteLine("Error while executing QUERY command");
            }
            */
            
            Execute(session, new BXCloseDatabase());
        }
    }
}