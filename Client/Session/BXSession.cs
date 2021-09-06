using System;
using System.Net.Sockets;
using System.Text;
using Client.Commands;
using Client.Commands.Commands;
using Client.Commands.Responses;

namespace Client.Session
{
    public class BXSession : IDisposable
    {
        private readonly TcpClient socket;
        private readonly NetworkStream stream;

        public BXSession(Address address)
        {
            this.socket = new TcpClient(address.Host, address.Port);
            this.stream = this.socket.GetStream();
        }
        
        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            this.Close();

            this.stream?.Dispose();
            this.socket?.Dispose();
        }
        
        /// <summary>
        /// Close this session
        /// Called automatically by IDisposable
        /// </summary>
        public void Close()
        {
            this.Execute(new BXExit());
        }
        
        public string Receive()
        {
            string message = string.Empty;

            while (true)
            {
                int current = this.stream.ReadByte();
                
                // BaseX transfer protocol uses FF as escape byte for
                // 00 and FF bytes, so escape it while receiving message
                if (Convert.ToByte(current) == 0xFF)
                    current = this.stream.ReadByte();

                if (current == 0)
                    break;

                message += Convert.ToChar(current);
            }

            return message;
        }

        public byte? ReceiveByte()
        {
            byte? result = null;
            int current = this.stream.ReadByte();

            if (current != -1)
                result = Convert.ToByte(current);

            return result;
        }
        
        public void Send(string message)
        {
            byte[] encodedMessage = Encoding.UTF8.GetBytes(message);
            
            this.stream.Write(encodedMessage, 0, encodedMessage.Length);
            this.Send(0); // End of message byte
        }

        public void Send(byte message)
        {
            this.stream.WriteByte(message);
        }

        public IBXCommandResponse Execute(IBXCommand command)
        {
            return command.Execute(this);
        }
    }
}