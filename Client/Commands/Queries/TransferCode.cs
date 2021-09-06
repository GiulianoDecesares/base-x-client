namespace Client.Commands.Queries
{
    public static class TransferCode
    {
        // Query
        
        
        public const byte Create = 8;       // \8 {name} \0 {input} \0
        public const byte Add = 9;          // \9 {name} \0 {path} \0 {input} \0
        public const byte Watch = 10;       // \10 {name} \0
        public const byte Unwatch = Watch;  // \10 {name} \0
        public const byte Replace = 12;     // \12 {path} \0 {input} \0
    }
}