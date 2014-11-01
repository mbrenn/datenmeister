using BurnSystems.Logging;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Common;
using DatenMeister.DataProvider.DotNet;
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
        /// Defines the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(ObjectCopier));

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
            foreach (var pair in pairs.ToList())
            {
                // Checks, if the object is an IReflective Collection
                if (ObjectHelper.IsReflectiveCollection(pair.Value))
                {
                    var currentValueAsReflectiveCollection = pair.Value.AsReflectiveCollection();
                    var targetCollection = targetElement.get(pair.PropertyName).AsReflectiveCollection();
                    foreach (var element in currentValueAsReflectiveCollection)
                    {
                        // TODO: Refactor this method in a way, that sourceElement may also be
                        // a simple object
                        if (ObjectConversion.IsNative(element))
                        {
                            targetCollection.add(element);
                            continue;
                        }

                        var elementAsIObject = element as IObject;
                        if (elementAsIObject != null)
                        {
                            targetCollection.add(this.CopyElement(elementAsIObject, deferredActions));
                            continue;
                        }

                        throw new NotImplementedException("Unknown enumeration type");
                    }
                }
                else
                {
                    var currentValueAsSingle = pair.Value.AsSingle(false);

                    if (ObjectConversion.IsNative(currentValueAsSingle))
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
                    else if (currentValueAsSingle is IObject)
                    {
                        // We got a self-contained object
                        // Perform a temporary copy to a DotNetExtent and store these elements into the object
                        var tempDotNetExtent = new GenericExtent("TEMP");
                        var tempCopier = new ObjectCopier(tempDotNetExtent);
                        var createdCopy = tempCopier.CopyElement(currentValueAsSingle as IObject);

                        // And now store the element back
                        targetElement.set(pair.PropertyName, createdCopy);
                    }
                    else if (currentValueAsSingle == null
                        || currentValueAsSingle == ObjectHelper.Null
                        || currentValueAsSingle == ObjectHelper.NotSet)
                    {
                        // We skip null objects
                    }
                    else
                    {
                        logger.Message("Object cannot be copied: " + currentValueAsSingle.GetType().ToString());
                    }
                }
            }
            return targetElement;
        }
    }
}
