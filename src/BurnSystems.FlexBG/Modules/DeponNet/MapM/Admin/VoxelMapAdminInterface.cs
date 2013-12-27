using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Admin;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.DetailView;
using BurnSystems.WebServer.Umbra.Views.DetailView.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapM.Admin
{
    /// <summary>
    /// Defines the map admin interface
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class VoxelMapAdminInterface : IFlexBgRuntimeModule
    {
        /// <summary>
        /// Gets or sets the view resolver
        /// </summary>
        [Inject(IsMandatory = true)]
        public BasicDetailViewResolver ViewResolver
        {
            get;
            set;
        }

        /// <summary>
        /// Starts the interface
        /// </summary>
        public void Start()
        {
            this.ViewResolver.Add(
                (x) => x is MapTreeViewItem,
                (x) => new MapTreeView());


        }

        /// <summary>
        /// Shutdowns the interface
        /// </summary>
        public void Shutdown()
        {
        }
    }
}
