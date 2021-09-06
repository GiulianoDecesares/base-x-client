namespace Client.Session
{
    public class Address
    {
        public static readonly Address Default = new("localhost", 1984);
        
        public string Host { get; }
        public int Port { get; }

        public Address(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }
    }
}