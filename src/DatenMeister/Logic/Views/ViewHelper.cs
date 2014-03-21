using DatenMeister.Logic.SourceFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.Views
{
    /// <summary>
    /// Contains some helper methods to create view definitions for 
    /// </summary>
    public static class ViewHelper
    {
        /// <summary>
        /// Performs the autogeneration of the view definition, in dependence of the
        /// only type in <c>provider</c>.
        /// </summary>
        /// <param name="viewDefinition">View Definition that shall be filled out</param>
        /// <param name="typeInfo">Type, which shall be included</param>
        public static void AutoGenerateViewDefinition(IObject viewDefinition, Type typeInfo)
        {
            AutoGenerateViewDefinition(viewDefinition, new DotNetTypeProvider(new[] { typeInfo }));
        }

        /// <summary>
        /// Performs the autogeneration of the view definition, in dependence of the
        /// only type in <c>provider</c>.
        /// </summary>
        /// <param name="viewDefinition">View Definition that shall be filled out</param>
        /// <param name="provider">Provider to be used</param>
        public static void AutoGenerateViewDefinition(IObject viewDefinition, ITypeInfoProvider provider)
        {
        }
    }
}
