﻿using DatenMeister.DataProvider;
using DatenMeister.Transformations;
using DatenMeister.DataProvider.Pool;
using DatenMeister.Logic;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Controls
{
    public class SelectTypeOfNewObjectDialog : SelectionListDialog
    {
        /// <summary>
        /// Selects the type of the new object
        /// </summary>
        public SelectTypeOfNewObjectDialog()
        {

        }

        /// <summary>
        /// Shows a list of all available types and the user can select one. 
        /// The selected item will be created.
        /// </summary>
        /// <param name="reflectiveCollection">The reflective collection which will be used
        /// to create the object and to which the recently created object will be added</param>
        /// <param name="settings">The DatenMeisterSettings which are used to create the view</param>
        /// <param name="extentType">The extent type which is searched to find the types that may 
        /// be created</param>
        public static IObject ShowNewOfGenericTypeDialog(
            IReflectiveCollection reflectiveCollection,
            IPublicDatenMeisterSettings settings,
            ExtentType extentType = ExtentType.Type)
        {
            var pool = Injection.Application.Get<IPool>();

            var dialog = new SelectionListDialog();
            var allTypes =
                new AllItemsReflectiveCollection(pool)
                .FilterByExtentType(extentType)
                .FilterByType(DatenMeister.Entities.AsObject.Uml.Types.Type);
            dialog.SetReflectiveCollection(allTypes, settings);
            if (dialog.ShowDialog() == true)
            {
                if (dialog.SelectedElements.Count() > 0)
                {
                    // Finds the factory
                    var factory = Factory.GetFor(reflectiveCollection.Extent);

                    // Adds the element to the reflective collection
                    var createdElement = factory.create(dialog.SelectedElements.AsSingle().AsIObject());
                    reflectiveCollection.add(createdElement);

                    // Now, add the item, it might be, that other extent views also need to be updated.
                    return createdElement;
                }
                else
                {
                    MessageBox.Show(Localization_DatenMeister_WPF.NoElementsSelected);
                }
            }

            return null;
        }
    }
}