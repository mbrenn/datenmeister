using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Wrapper.EventOnChange;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic.Sources;
using DatenMeister.Logic.TypeResolver;
using DatenMeister.Pool;
using DatenMeister.Transformations;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Stores the data that is used for the application specific data
    /// </summary>
    public partial class ApplicationCore
    {
        /// <summary>
        /// Adds the default views
        /// </summary>
        public void AddDefaultViews()
        {
            var names = Enum.GetNames(typeof(ExtentType));

            foreach (var name in names)
            {
                var pool = Injection.Application.Get<IPool>();

                var enumValue = (ExtentType) Enum.Parse(typeof(ExtentType), name);
                var uri = string.Format ( 
                    "datenmeister://datenmeister/all/extenttype/{0}",
                    enumValue.ToString());

                var extent = new AllElementsExtent(uri, enumValue);
                pool.Add(extent, null, "All Extents of Type " + name, ExtentType.Query);
            }
        }
    }
}
