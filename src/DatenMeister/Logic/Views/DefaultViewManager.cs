﻿using BurnSystems.Test;
using DatenMeister.DataProvider;
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
        /// Gets or sets a value indicating wheter to autogenerate the detail form, 
        /// if no applicable detail form has been found
        /// </summary>
        public bool DoAutogenerateForm
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the list of entries and views and their predicates
        /// </summary>
        private List<ViewEntry> entries = new List<ViewEntry>();

        /// <summary>
        /// Stores the extent, which shall be used for factory creation
        /// </summary>
        private IURIExtent extent;

        public DefaultViewManager(IURIExtent extent)
        {
            Ensure.That(extent != null);
            this.extent = extent;
        }

        /// <summary>
        /// Adds a mapping between a certain type and a view
        /// </summary>
        /// <param name="metaClass">Type being associated</param>
        /// <param name="view">View being associated</param>
        /// <param name="isDefault">true, if default</param>
        public void Add(IObject metaClass, IObject view, bool isDefault)
        {
            Ensure.That(metaClass != null);
            Ensure.That(view != null);
            Ensure.That(view != null);

            this.entries.Add(new ViewEntry()
            {
                MetaClass = metaClass,
                View = view,
                IsDefault = isDefault
            });
                
        }

        private IEnumerable<ViewEntry> FindEntries(IObject obj, ViewType viewType)
        {
            return this.entries.Where(viewEntry =>
                {
                    var asElement = obj as IElement;
                    if (asElement == null)
                    {
                        return false;
                    }

                    var metaClassOfElement = asElement.getMetaClass();
                    if (metaClassOfElement == null)
                    {
                        return false;
                    }

                    return metaClassOfElement.Equals(viewEntry.MetaClass) && viewType == ViewType.FormView;
                });
        }

        /// <summary>
        /// Gets default view
        /// </summary>
        /// <param name="obj">Object to be evaluated</param>
        /// <param name="type">Type of the view, which is required</param>
        /// <returns>The default view or nothing</returns>
        public IObject GetDefaultView(IObject obj, ViewType type)
        {
            var result = FindEntries(obj, type).Where(x => x.IsDefault).FirstOrDefault();
            if (result == null)
            {
                if (this.DoAutogenerateForm && type == ViewType.FormView)
                {
                    return this.CreateAutogeneratedForm(obj);
                }

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
            return FindEntries(obj, type).Where(x => x.IsDefault).Select(x => x.View);
        }

        /// <summary>
        /// Creates an autogenerated form by the object
        /// </summary>
        /// <param name="obj">Object to be used</param>
        /// <returns>Returns the auto generated form</returns>
        private IObject CreateAutogeneratedForm(IObject obj)
        {
            var factory = Factory.GetFor(this.extent);

            var view = factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.FormView);
            var viewObj = new DatenMeister.Entities.AsObject.FieldInfo.FormView(view);
            viewObj.setAllowDelete(false);
            viewObj.setAllowEdit(false);
            viewObj.setAllowNew(false);

            if (obj.Extent != null) 
            {
                // Object has an extent
                ViewHelper.AutoGenerateViewDefinition(obj.Extent, view, true);
            }
            else
            {
                // Don't know what to do... throw exception
                ViewHelper.AutoGenerateViewDefinition(obj, view, true);
            }

            return view;
        }

        /// <summary>
        /// Defines the class, containing the view entries and their information
        /// </summary>
        private class ViewEntry
        {
            public IObject MetaClass
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
