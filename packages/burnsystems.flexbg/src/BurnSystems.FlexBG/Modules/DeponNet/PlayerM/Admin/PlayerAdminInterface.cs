using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.AdminInterfaceM;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.DetailView;
using BurnSystems.WebServer.Umbra.Views.DetailView.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Admin
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class PlayerAdminInterface : IFlexBgRuntimeModule
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
        [Inject(IsMandatory = true)]
        public BasicDetailViewResolver ViewResolver
        {
            get;
            set;
        }

        public void Start()
        {
            this.Data.Children.Add(
                new PlayersTreeViewItem()
                {
                    Id = this.Data.GetNextChildrenId()
                });

            // Creates Entity View for Users
            this.ViewResolver.Add(
                (x) => x is PlayersTreeViewItem,
                (x) => new EntityView(
                    new EntityViewConfig(
                        new EntityViewListTable<PlayersTreeViewItem>(
                            "Games",
                            EntityViewElementProperty.Create().Labelled("Id").For("Id").AsInteger(),
                            EntityViewElementProperty.Create().Labelled("Game").For("GameId").AsInteger(),
                            EntityViewElementProperty.Create().Labelled("Playername").For("Playername").AsString(),
                            EntityViewElementProperty.Create().Labelled("Empirename").For("Empirename").AsString())
                        .SetSelector((a, y) => y.GetChildren(a).Select(z => z.Entity)))));

            this.ViewResolver.Add(
                (x) => x is PlayerTreeViewItem,
                (x) => new EntityView(
                    new EntityViewConfig(
                        new EntityViewDetailTable(
                            "GetColumn",
                            EntityViewElementProperty.Create().Labelled("Id").For("Id").AsReadOnly().AsInteger(),
                            EntityViewElementProperty.Create().Labelled("Playername").For("Playername").AsString(),
                            EntityViewElementProperty.Create().Labelled("Empirename").For("Empirename").AsString(),
                            EntityViewElementProperty.Create().Labelled("GameId").For("GameId").AsReadOnly().AsInteger()),

                        new EntityViewDetailTable(
                            "Drop Table",
                            EntityViewElementProperty.Create().Labelled("Id").For("Id").AsReadOnly().AsInteger())
                        .SetButtonText("Drop Player")
                        .WithOverrideUrl("players/DropPlayer"))));
        }

        public void Shutdown()
        {
        }
    }
}
