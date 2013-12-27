using BurnSystems.FlexBG.Modules.UserM.Interfaces;
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
    public class GroupsTreeView : BaseTreeViewItem
    {
        public override string ToString()
        {
            return "Groups";
        }

        public override IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            var userManagement = activates.Get<IUserManagement>();
            Ensure.IsNotNull(userManagement, "No IUserManagement bound");
            var groups = userManagement.GetAllGroups();

            return groups.Select(x =>
                new GenericTreeViewItem()
            {
                Id = x.Id,
                Title = x.Name,
                Entity = x
            });
        }
    }
}
