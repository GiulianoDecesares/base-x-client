namespace Client.Commands.Responses
{
    public class BXCommandResponse : IBXCommandResponse
    {
        public string Result { get; }
        public string Info { get; }
        
        public bool Ok { get; }

        public BXCommandResponse(string result, string info, bool wentOk)
        {
            this.Result = result;
            this.Info = info;
            this.Ok = wentOk;
        }
    }
}