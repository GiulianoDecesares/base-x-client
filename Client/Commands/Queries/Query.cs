using System;
using System.Collections;

namespace Client.Commands.Queries
{
    public class Query : IBXQuery, IDisposable
    {
        // Query transfer codes
        
        /// <summary>
        /// Creates a new query instance and returns its id
        /// \0 {query} \0
        /// </summary>
        private const byte QueryStart = 0;
        
        
        private const byte QueryNext = 1;    // \1 {id} \0
        
        /// <summary>
        /// Closes and unregisters the query with the specified id
        /// \2 {id} \0
        /// </summary>
        private const byte QueryClose = 2;

        /// <summary>
        /// Binds a value to a variable. The type will be ignored if the string is empty
        /// \3 {id} \0 {variable} \0 {value}\0 {type}\0
        /// </summary>
        private const byte QueryBind = 3;
        
        /// <summary>
        /// Returns all resulting items as strings, prefixed by a single byte (\xx) that represents the Type ID
        /// This command is called by the more() function of a client implementation
        /// \4 {id} \0
        /// </summary>
        private const byte QueryResults = 4;
        
        /// <summary>
        /// Executes the query and returns the result as a single string
        /// \5 {id} \0
        /// </summary>
        private const byte QueryExecute = 5;

        /// <summary>
        /// Returns a string with query compilation and profiling info
        /// \6 {id} \0
        /// </summary>
        private const byte QueryInfo = 6;

        /// <summary>
        /// Returns a string with all query serialization parameters, which can e.g. be assigned to the SERIALIZER option
        /// </summary>
        private const byte QueryOptions = 7;

        
        private readonly Session.BXSession session;
        private readonly string id;

        private ArrayList cache;
        private int cacheReadPosition;
        
        public Query(Session.BXSession session, string query)
        {
            this.session = session;
            this.id = this.Execute(QueryStart, query);
        }

        public void Dispose()
        {
            this.Close();
        }
        
        public void Close()
        {
            this.Execute(QueryClose, this.id);
        }
        
        public string Execute()
        {
            return this.Execute(QueryExecute, this.id);
        }

        public bool IsMore()
        {
            /*
            if (this.cache == null)
            {
                this.session.Send(QueryResults);
                this.session.Send(this.id);

                this.cache = new ArrayList();

                while (this.session.DataAvailable())
                {
                    this.cache.Add(this.session.Receive());
                }
                
                if (this.session.ReadByte() != 0)
                    throw new IOException(session.Receive());
                
                this.cacheReadPosition = 0;
            }

            bool result = this.cacheReadPosition < this.cache.Count;

            if (!result)
                this.cache = null;

            return result;
            */
            return false;
        }

        public string Next()
        {
            string result = null;
            
            if (this.IsMore())
            {
                result = this.cache[this.cacheReadPosition++] as string;
            }

            return result;
        }

        public string Info()
        {
            return this.Execute(QueryInfo, this.id);
        }
        
        public string Options()
        {
            return this.Execute(QueryOptions, this.id);
        }

        private string Execute(byte command, string argument)
        {
            this.session.Send(command);
            this.session.Send(argument);
            
            string response = session.Receive();
            
            /* TODO :: Review this 
            bool ok = session.Ok();
            
            if(!ok)
            {
                throw new IOException(session.Receive());
            }
            */
            
            return response;
        }
    }
}