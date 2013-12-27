using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.Treeview
{
    /// <summary>
    /// Some extension methods for treeview stuff
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Resolves the tree item by path parts
        /// </summary>
        /// <param name="item">Item to be resolved</param>
        /// <param name="activates">Activationcontainer to be used</param>
        /// <param name="path">Path to be used</param>
        /// <returns>Found item</returns>
        public static ITreeViewItem ResolveByPath(this ITreeViewItem item, IActivates activates, string path)
        {
            var pathParts = path.Split(new[] { '/' })
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Convert.ToInt64(x));

            return ResolveByPath(item, activates, pathParts);
        }

        /// <summary>
        /// Resolves the tree item by path parts
        /// </summary>
        /// <param name="item">Item to be resolved</param>
        /// /// <param name="activates">Activationcontainer to be used</param>
        /// <param name="pathParts"></param>
        /// <returns></returns>
        private static ITreeViewItem ResolveByPath(this ITreeViewItem item, IActivates activates, IEnumerable<long> pathParts)
        {
            var current = item;

            foreach (var part in pathParts)
            {
                current = current.GetChildren(activates).Where(x => x.Id == part).FirstOrDefault();
                if (current == null)
                {
                    // Nothing found
                    return null;
                }
            }

            return current;
        }
    }
}
