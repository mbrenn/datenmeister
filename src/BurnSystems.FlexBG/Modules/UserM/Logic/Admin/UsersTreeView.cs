using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.Admin
{
    /// <summary>
    /// Gets the users treeview
    /// </summary>
    public class UsersTreeView : BaseTreeViewItem
    {
        public override IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            var userManagement = activates.Get<IUserManagement>();
            Ensure.IsNotNull(userManagement, "No IUserManagement bound");
            var users = userManagement.GetAllUsers();

            return users.Select(x =>
                new GenericTreeViewItem()
            {
                Id = x.Id,
                Title = x.Username,
                Entity = x,
                ApplyChangeFunction = (container) =>
                {
                    var user = x as User;
                    Ensure.IsNotNull(user, "Entity is not a user");

                    userManagement.UpdateUser(user);
                }
            });
        }

        public override string ToString()
        {
            return "Users";
        }
    }
}
