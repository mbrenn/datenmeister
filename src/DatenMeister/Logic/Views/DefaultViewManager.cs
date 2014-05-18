using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.Views
{
    /// <summary>
    /// Defines the default view manager
    /// </summary>
    public class DefaultViewManager : IViewManager
    {
        /// <summary>
        /// Stores the list of entries and views and their predicates
        /// </summary>
        private List<ViewEntry> entries = new List<ViewEntry>();

        /// <summary>
        /// Adds a predicate to find out the correct 
        /// </summary>
        /// <param name="predicate">Predicate being used to check, if view is applicable</param>
        /// <param name="view">View being associated</param>
        /// <param name="isDefault">true, if this view should be seen as default. 
        /// If there are more than one default views, the first view will be returned</param>
        public void Add(Func<IObject, ViewType, bool> predicate, IObject view, bool isDefault)
        {
            Ensure.That(predicate != null);
            Ensure.That(view != null);

            this.entries.Add(new ViewEntry()
                {
                    Predicate = predicate,
                    View = view,
                    IsDefault = isDefault
                });
        }

        /// <summary>
        /// Adds a mapping between a certain type and a view
        /// </summary>
        /// <param name="type">Type being associated</param>
        /// <param name="view">View being associated</param>
        /// <param name="isDefault">true, if default</param>
        public void Add(IObject type, IObject view, bool isDefault)
        {
            this.Add(
                (obj, viewType) =>
                {
                    var asElement = obj as IElement;
                    if (asElement == null)
                    {
                        return false;
                    }

                    return asElement.getMetaClass() == type && viewType == ViewType.FormView;
                },
                view,
                isDefault);
        }

        /// <summary>
        /// Gets default view
        /// </summary>
        /// <param name="obj">Object to be evaluated</param>
        /// <param name="type">Type of the view, which is required</param>
        /// <returns>The default view or nothing</returns>
        public IObject GetDefaultView(IObject obj, ViewType type)
        {
            var result = this.entries.Where(x => x.Predicate(obj, type) && x.IsDefault).FirstOrDefault();
            if (result == null)
            {
                return null;
            }

            return result.View;
        }

        /// <summary>
        /// Gets the views, which are applicable for the item 
        /// </summary>
        /// <param name="obj">Object to be evaulated</param>
        /// <param name="type">Type of the view, which is required</param>
        /// <returns>An enumeration of all applicable view definitions</returns>
        public IEnumerable<IObject> GetViews(IObject obj, ViewType type)
        {
            return this.entries.Where(x => x.Predicate(obj, type) && x.IsDefault).Select(x => x.View);
        }

        /// <summary>
        /// Defines the class, containing the view entries and their information
        /// </summary>
        private class ViewEntry
        {
            public Func<IObject, ViewType, bool> Predicate
            {
                get;
                set;
            }

            public IObject View
            {
                get;
                set;
            }

            public bool IsDefault
            {
                get;
                set;
            }
        }
    }
}
