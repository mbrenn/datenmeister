using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Modules.UserManagement;
using BurnSystems.WebServer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    public class AllowOnlyIfUserIsInGroupFilter : IRequestFilter
    {
        /// <summary>
        /// Gets or sets the required tokenset.
        /// If current logged on user does not have the tokenset, access will be denied
        /// </summary>
        public TokenSet RequiredTokenSet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the request filter. This predicate shall return true, if the filter shall be applied
        /// </summary>
        public Func<ContextDispatchInformation, bool> RequestFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the AllowOnlyIfUserIsInGroupFilter class
        /// </summary>
        /// <param name="tokenSet">Required tokens</param>
        /// <param name="filter">Filter for the function</param>
        public AllowOnlyIfUserIsInGroupFilter(TokenSet tokenSet, Func<ContextDispatchInformation, bool> filter)
        {
            this.RequiredTokenSet = tokenSet;
            this.RequestFilter = filter;
        }

        public void BeforeDispatch(IActivates container, ContextDispatchInformation information, out bool cancel)
        {
            // Is this request relevant for us?
            if (!this.RequestFilter(information))
            {
                // No match
                cancel = false;
                return;
            }

            // Gets the current user
            var user = container.GetByName<IWebUser>(CurrentWebUserHelper.Name);
            if (user == null)
            {
                // User is not logged in
                cancel = Cancel(container, information);
                return;
            }

            // Checks, if we have all credentials
            if (!TokenSet.IsSubsetOf(this.RequiredTokenSet, user.CredentialTokenSet))
            {
                // Too less credentials
                cancel = Cancel(container, information);
                return;
            }

            cancel = false;
        }

        /// <summary>
        /// Cancels the request and returns an errorresponse to browser
        /// </summary>
        /// <param name="container">Activation context</param>
        /// <param name="information">Current Http context</param>
        /// <returns>true for cancellation. </returns>
        private static bool Cancel(IActivates container, ContextDispatchInformation information)
        {
            var errorResponse = container.Create<ErrorResponse>();
            errorResponse.Set(HttpStatusCode.Forbidden);
            errorResponse.Dispatch(container, information);
            return true;
        }

        public void AfterDispatch(IActivates container, ContextDispatchInformation information, out bool cancel)
        {
            cancel = false;
        }

        public void AfterRequest(IActivates container, ContextDispatchInformation information)
        {
        }

        #region Some helper methods for better integration

        /// <summary>
        /// Creates an instance, which only accepts the given groups
        /// </summary>
        /// <param name="container">Container to be used to retrieve token</param>
        /// <param name="groupName">Name of the group</param>
        /// <returns></returns>
        public static AllowOnlyIfUserIsInGroupFilter For(IActivates container, string groupName)
        {
            var userManagement = container.Get<IUserManagement>();
            var group = userManagement.GetGroup(groupName);

            Ensure.That(group != null, "Group '" + groupName + "' not found");

            return new AllowOnlyIfUserIsInGroupFilter(
                new TokenSet(
                    new Token(group.TokenId, group.Name)), 
                DispatchFilter.All);
        }

        /// <summary>
        /// Sets the dispatchfilter to 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public AllowOnlyIfUserIsInGroupFilter MatchesUrl(string url)
        {
            this.RequestFilter = DispatchFilter.ByUrl(url);
            return this;
        }

        #endregion
    }
}
