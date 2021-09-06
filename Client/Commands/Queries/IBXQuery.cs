namespace Client.Commands.Queries
{
    public interface IBXQuery
    {
        /*
        void Bind(string name, string value);
        void Bind(string name, string value, string type);

        void Context(string value);
        void Context(string value, string type);


        string Options();
        */
        
        // Iteration
        bool IsMore();
        string Next();
        
        string Info();
        string Execute();
        
        void Close();
    }
}