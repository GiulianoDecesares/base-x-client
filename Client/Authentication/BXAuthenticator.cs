using System;
using System.Security.Cryptography;
using System.Text;

namespace Client.Authentication
{
    public class BXAuthenticator : IAuthenticator
    {
        private readonly Session.BXSession currentSession;
        private readonly Credentials credentials;

        public BXAuthenticator(Session.BXSession session, Credentials credentials)
        {
            this.currentSession = session;
            this.credentials = credentials;
        }
        
        public bool Authenticate()
        {
            bool result = false;
            
            // Receive welcome message
            string message = this.currentSession.Receive();
            
            // Parse
            string realm = this.GetRealm(message);
            string nonce = this.GetNonce(message);

            if (!string.IsNullOrEmpty(realm) && !string.IsNullOrEmpty(nonce))
            {
                /*
                Console.WriteLine($"Realm: {realm}");
                Console.WriteLine($"Nonce: {nonce}");
                */
                
                string authHash = GenerateAuthHash(this.credentials.UserName, this.credentials.Password, realm, nonce);
                
                // Console.WriteLine($"Authenticating with: {authHash}");

                this.currentSession.Send(this.credentials.UserName);
                this.currentSession.Send(authHash);

                // Console.WriteLine("Info sent. Waiting response");
                
                byte? response = this.currentSession.ReceiveByte();

                if (response.HasValue)
                {
                    // Console.WriteLine($"Response is: {response.Value}");
                    result = response.Value == 0;
                    
                    // Console.WriteLine(result ? "Permission granted" : "Permission denied");   
                }
                /*
                else
                {
                    Console.WriteLine("Error in response");
                }
                */
            }

            return result;
        }
        
        private string GetRealm(string welcomeMessage)
        {
            string realm = string.Empty;

            if (!string.IsNullOrEmpty(welcomeMessage))
            {
                string[] responseComponents = welcomeMessage.Split(':');

                if (responseComponents.Length > 0)
                {
                    realm = responseComponents[0];
                }
            }

            return realm;
        }

        private string GetNonce(string welcomeMessage)
        {
            string nonce = string.Empty;

            if (!string.IsNullOrEmpty(welcomeMessage))
            {
                string[] responseComponents = welcomeMessage.Split(':');

                if (responseComponents.Length > 1)
                {
                    nonce = responseComponents[1];
                }
            }
            
            return nonce;
        }
        
        private string EncodeToMD5(string message)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] hash = provider.ComputeHash(Encoding.UTF8.GetBytes(message));
            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (byte current in hash)
            {
                stringBuilder.Append(current.ToString("x2"));
            }
            
            return stringBuilder.ToString();
        }
        
        private string GenerateAuthHash(string userName, string password, string realm, string nonce)
        {
            string hash = this.EncodeToMD5($"{userName}:{realm}:{password}") + nonce;
            return this.EncodeToMD5(hash); // Rehash everything
        }
    }
}