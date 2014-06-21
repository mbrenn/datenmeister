using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic
{
    /// <summary>
    /// This class is capable to copy the content of one extent to another extent. 
    /// All types are presevered and the copying is performed recursively. 
    /// The copying is performed in a way that no connection between old and new extent is existing. 
    /// </summary>
    public class ObjectCopier
    {
        /// <summary>
        /// Stores the target extent
        /// </summary>
        public IURIExtent targetExtent
        {
            get;
            private set;
        }

        public ObjectCopier(IURIExtent targetExtent)
        {
            this.targetExtent = targetExtent;
        }

        /// <summary>
        /// Maps the objects from source to target
        /// </summary>
        private Dictionary<string, IObject> mapping = new Dictionary<string, IObject>();
        
        /// <summary>
        /// Copies the element
        /// </summary>
        /// <param name="sourceElement">Source element being used to copy the element</param>
        /// <param name="deferredActions">List of deferred actions to complete the copying</param>
        /// <returns>The copied element</returns>
        public IObject CopyElement(IObject sourceElement, List<Action> deferredActions = null)
        {
            IObject type = null;
            if (sourceElement is IElement)
            {
                type = (sourceElement as IElement).getMetaClass();
            }

            var factory = Factory.GetFor(this.targetExtent);
            var targetElement = factory.create(type);

            if (sourceElement.Id != null)
            {
                this.mapping[sourceElement.Id] = targetElement;
            }

            var pairs = sourceElement.getAll();
            foreach (var pair in pairs)
            {
                var currentValueAsSingle = pair.Value.AsSingle(false);

                if (Extensions.IsNative(currentValueAsSingle))
                {
                    targetElement.set(pair.PropertyName, currentValueAsSingle);
                }
                else if (currentValueAsSingle is ResolvableByPath)
                {
                    // If the given object is another object, we will do the tracing as a deferred action
                    // after the complete file has been copied
                    var pairValue = currentValueAsSingle.AsIObject() as IObject;

                    if (deferredActions != null)
                    {
                        var deferredAction = new Action(() =>
                        {
                            targetElement.set(pair.PropertyName, this.mapping[pairValue.Id]);
                        });

                        deferredActions.Add(deferredAction);
                    }
                }
            }
            return targetElement;
        }
    }
}
