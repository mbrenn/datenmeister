using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    [Serializable]
    public class TokenSet
    {
        /// <summary>
        /// Stores the tokens
        /// </summary>
        private List<Token> tokens = new List<Token>();

        /// <summary>
        /// Gets the tokens
        /// </summary>
        public List<Token> Tokens
        {
            get { return this.tokens; }
        }

        /// <summary>
        /// Initializes a new instance of the TokenSet class.
        /// </summary>
        public TokenSet()
        {
        }

        /// <summary>
        /// Initializes a new set of token and adds the given token to the set
        /// </summary>
        /// <param name="tokens">Tokens to be added</param>
        public TokenSet(params Token[] tokens)
        {
            this.tokens.AddRange(tokens);
        }

        /// <summary>
        /// Adds a token
        /// </summary>
        /// <param name="token">Token to be added</param>
        public void Add(Token token)
        {
            this.tokens.Add(token);
        }

        /// <summary>
        /// Checks, if the given token is in tokenset
        /// </summary>
        /// <param name="token">Token to be checked</param>
        /// <returns>true, if token is in tokenset</returns>
        public bool Contains(Token token)
        {
            return this.tokens.Any(x => x.Id == token.Id);
        }

        /// <summary>
        /// Checks if all tokens of <c>subset</c> are in <c>major</c>
        /// </summary>
        /// <param name="subset">Subset to be tested</param>
        /// <param name="major">Container containing all the tokens</param>
        /// <returns>true, if all subset tokens are available at major</returns>
        public static bool IsSubsetOf(TokenSet subset, TokenSet major)
        {
            foreach (var token in subset.Tokens)
            {
                if (!major.Tokens.Any(x => x.Id == token.Id))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
