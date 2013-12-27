using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Responses;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    /// <summary>
    /// Performs webfiltering
    /// </summary>
    public class WebAuthorisation : IRequestFilter
    {
        /// <summary>
        /// Stores the entries
        /// </summary>
        private List<WebAuthorisationEntry> entries = new List<WebAuthorisationEntry>();

        /// <summary>
        /// Restricts access to following users
        /// </summary>
        /// <param name="filter">Filter being used</param>
        /// <param name="token">List of tokens</param>
        public void RestrictTo(Func<ContextDispatchInformation, bool> filter, params Token[] tokens)
        {
            this.RestrictTo(
                filter, 
                new TokenCheckpoint(tokens));
        }
        
        /// <summary>
        /// Restricts access to following users
        /// </summary>
        /// <param name="filter">Filter being used</param>
        /// <param name="tokenCheckpoint">Checkpoint being used</param>
        public void RestrictTo(Func<ContextDispatchInformation, bool> filter, TokenCheckpoint tokenCheckpoint)
        {
            this.entries.Add(
                new WebAuthorisationEntry(filter, tokenCheckpoint));
        }

        void IRequestFilter.BeforeDispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            TokenSet cachedTokenSet = null;

            // For every entry, we need 
            foreach (var entry in this.entries.Where(x => x.Selector(information)))
            {
                if (cachedTokenSet == null)
                {
                    var auth = container.Get<IAuthentication>();
                    var user = auth.GetLoggedInUser();
                    if (user != null)
                    {
                        cachedTokenSet = user.CredentialTokenSet;
                    }
                    else
                    {
                        cachedTokenSet = new TokenSet();
                    }
                }

                // Check, if we pass
                if (!entry.Checkpoint.DoesPass(cachedTokenSet))
                {
                    var errorResponse = container.Create<ErrorResponse>();
                    errorResponse.Set(HttpStatusCode.Forbidden);
                    errorResponse.Dispatch(container, information);
                    cancel = true;
                    return;
                }
            }
            
            // We pass
            cancel = false;
        }

        void IRequestFilter.AfterDispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            cancel = false;
        }

        void IRequestFilter.AfterRequest(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information)
        {
        }
    }
}
