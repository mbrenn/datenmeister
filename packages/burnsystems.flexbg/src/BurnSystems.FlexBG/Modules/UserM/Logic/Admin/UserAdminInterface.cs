using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.AdminInterfaceM;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.DetailView;
using BurnSystems.WebServer.Umbra.Views.DetailView.Entities;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.Admin
{
    /// <summary>
    /// Initializes a new instance of the admin interface
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class UserAdminInterface : IFlexBgRuntimeModule
    {
        /// <summary>
        /// Stores the admin root data
        /// </summary>
        [Inject(ByName = AdminRootData.Name)]
        public AdminRootData Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the view resolver
        /// </summary>
        [Inject(IsMandatory=true)]
        public BasicDetailViewResolver ViewResolver
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the UserAdminInterface
        /// </summary>
        /// <param name="activationContext">Activationcontext to be used</param>
        public UserAdminInterface()
        {
        }

        /// <summary>
        /// Starts the user admin interface
        /// </summary>
        public void Start()
        {
            this.Data.Children.Add(
                new UsersTreeView()
                {
                    Id = this.Data.GetNextChildrenId()
                });
            this.Data.Children.Add(
                new GroupsTreeView()
                {
                    Id = this.Data.GetNextChildrenId()
                });

            // Creates Entity View for Users
            this.ViewResolver.Add(
                (x) => x is UsersTreeView,
                (x) => new EntityView(
                    new EntityViewConfig(
                        new EntityViewListTable<UsersTreeView>(
                            "Users",
                            EntityViewElementProperty.Create()
                                .Labelled("Id")
                                .For("Id")
                                .AsInteger(),
                            EntityViewElementProperty.Create()
                                .Labelled("Username")
                                .For("Username")
                                .AsString())
                        .SetSelector((a, y) => y.GetChildren(a).Select(z => z.Entity)))));

            this.ViewResolver.Add(
                (x) => x is GroupsTreeView,
                (x) => new EntityView(
                    new EntityViewConfig(
                        new EntityViewListTable<GroupsTreeView>(
                            "Groups",
                            EntityViewElementProperty.Create()
                                .Labelled("Id")
                                .For("Id")
                                .AsInteger(),
                            EntityViewElementProperty.Create()
                                .Labelled("Name")
                                .For("Name")
                                .AsString())
                        .SetSelector((a, y) => y.GetChildren(a).Select(z => z.Entity)))));

            this.ViewResolver.Add(
                (x) => x.Entity is User,
                (x) => new EntityView(
                    new EntityViewConfig(
                        new EntityViewDetailTable(
                            "Info",
                            EntityViewElementProperty.Create()
                                .Labelled("Id")
                                .For("Id")
                                .AsInteger()
                                .AsReadOnly(),
                            EntityViewElementProperty.Create()
                                .Labelled("Username")
                                .For("Username")
                                .AsString(),
                            EntityViewElementProperty.Create()
                                .Labelled("E-Mail")
                                .For("EMail")
                                .AsString(),
                            EntityViewElementProperty.Create()
                                .Labelled("Activation Key")
                                .For("ActivationKey")
                                .WithWidth(20)
                                .AsString(),
                            EntityViewElementProperty.Create()
                                .Labelled("Is active")
                                .For("IsActive")
                                .AsBoolean(),
                            EntityViewElementProperty.Create()
                                .Labelled("Has agreed to TOS")
                                .For("HasAgreedToTOS")
                                .AsBoolean(),
                            EntityViewElementProperty.Create()
                                .Labelled("Premium Till")
                                .For("PremiumTill")
                                .AsDateTime(),
                            EntityViewElementProperty.Create()
                                .Labelled("Token")
                                .For("TokenId")
                                .As((z) => z.ToString(), null, PropertyDataType.String)
                                .AsReadOnly()),
                        new EntityViewDetailTable(
                            "SetPassword",
                            EntityViewElementProperty.Create()
                                .Labelled("User Id")
                                .For("Id")
                                .AsString()
                                .AsReadOnly(),
                            EntityViewElementProperty.Create()
                                .Labelled("New Password")
                                .For("NewPassword")
                                .AsString()
                                .AsWriteOnly())
                            .WithOverrideUrl("users/SetPassword"),
                        new EntityViewDetailTable(
                            "UpdatProfile",
                            EntityViewElementProperty.Create()
                                .Labelled("User Id")
                                .For("Id")
                                .AsString()
                                .AsReadOnly(),
                            EntityViewElementProperty.Create()
                                .Labelled("Displayname")
                                .For("Displayname")
                                .AsString())
                            .WithOverrideUrl("users/UpdateProfile")
                            .WithSelector<User>((a, e) =>
                             {
                                 var userManagement = a.Get<IUserManagement>();

                                 return new
                                 {
                                     Id = e.Id, 
                                     Displayname = userManagement.GetUserData<string>(e, UserDataTokens.DisplayName)
                                 };
                             }))));

            // Creates entity view for groups
            this.ViewResolver.Add(
                (x) => x.Entity is Group,
                (x) => new EntityView(
                    new EntityViewConfig(
                        new EntityViewDetailTable(
                            "Info", 
                            EntityViewElementProperty.Create()
                                .Labelled("Id")
                                .For("Id")
                                .AsInteger()
                                .AsReadOnly(),
                            EntityViewElementProperty.Create()
                                .Labelled("Name")
                                .For("Name")
                                .AsString(),
                            EntityViewElementProperty.Create()
                                .Labelled("Token")
                                .For("TokenId")
                                .As((z) => z.ToString(), null, PropertyDataType.String)
                                .AsReadOnly()))));
        }

        /// <summary>
        /// Stops the user admin interface
        /// </summary>
        public void Shutdown()
        {
        }

        public override string ToString()
        {
            return "Users";
        }
    }
}
