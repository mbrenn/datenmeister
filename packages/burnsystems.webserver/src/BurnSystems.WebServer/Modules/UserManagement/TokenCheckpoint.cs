using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    /// <summary>
    /// Defines the token checkpoint
    /// </summary>
    public class TokenCheckpoint
    {
        private List<Token> tokens = new List<Token>();

        /// <summary>
        /// Initializes a new instance of the TokenCheckpoint
        /// </summary>
        public TokenCheckpoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TokenCheckpoint
        /// </summary>
        /// <param name="tokens">List of tokens, were one of the tokens is required to pass this checkpoint</param>
        public TokenCheckpoint(params Token[] tokens)
        {
            this.tokens.AddRange(tokens);
        }

        /// <summary>
        /// Adds a token for the checkpoint which can be used to pass
        /// </summary>
        /// <param name="token">Token that can be used to pass</param>
        public void AddRequired(Token token)
        {
            this.tokens.Add(token);
        }

        /// <summary>
        /// Checks, if the given tokenset can be used to pass the checkpoint
        /// </summary>
        /// <param name="tokenSet">Checkpoint being used</param>
        /// <returns>true, if it does pass</returns>
        public bool DoesPass(TokenSet tokenSet)
        {
            foreach (var token in this.tokens)
            {
                if (tokenSet.Contains(token))
                {
                    return true;
                }
            }

            return false;
        }

        public void CheckAndThrow(TokenSet tokenSet)
        {
            if (!this.DoesPass(tokenSet))
            {
                throw new SecurityException("TokenSet is not sufficient to pass this Checkpoint");
            }
        }
    }
}
