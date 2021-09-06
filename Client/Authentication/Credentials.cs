namespace Client.Authentication
{
    public class Credentials
    {
        public static readonly Credentials Default = new("admin", "admin");
            
        public string UserName { get; }
        public string Password { get; }

        public Credentials(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}