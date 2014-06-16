using BurnSystems.Test;
using DatenMeister.DataProvider;
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
    public class ExtentCopier
    {
        private IReflectiveSequence source
        {
            get;
            set;
        }

        private IReflectiveSequence target
        {
            get;
            set;
        }

        /// <summary>
        /// Maps the objects from source to target
        /// </summary>
        private Dictionary<string, IObject> mapping = new Dictionary<string, IObject>();

        /// <summary>
        /// Initializes a new instance of the ExtentCopier class
        /// </summary>
        /// <param name="source">Source to be copied</param>
        /// <param name="target">Target, which shall receive the copy</param>
        public ExtentCopier(IURIExtent source, IURIExtent target)
        {
            Ensure.That(source != null);
            Ensure.That(target != null);
            Ensure.That(source != target);

            this.source = source.Elements();
            this.target = target.Elements();

            Ensure.That(this.source != null);
            Ensure.That(this.target != null);
            Ensure.That(this.source != this.target);
        }

        /// <summary>
        /// Executes the copying
        /// </summary>
        public void Copy()
        {
            this.mapping.Clear();

            // Ok... How to do... First. Get all elements
            var elements = this.source;
            var deferredActions = new List<Action>();

            foreach (var element in elements.Select(x=> x.AsIObject()))
            {
                IObject type = null;
                if (element is IElement)
                {
                    type = (element as IElement).getMetaClass();
                }

                var factory = Factory.GetFor(this.target.Extent); 
                var createdObject = factory.create(type);
                this.target.add(createdObject);
                this.mapping[element.Id] = createdObject;

                var pairs = element.getAll();
                foreach (var pair in pairs)
                {
                    var currentValue = pair.Value.AsSingle();

                    if (Extensions.IsNative(currentValue))
                    {
                        createdObject.set(pair.PropertyName, currentValue);
                    }
                    else if (currentValue is IObject)
                    {
                        // If the given object is another object, we will do the tracing as a deferred action
                        // after the complete file has been copied
                        var pairValue = currentValue as IObject;

                        var deferredAction = new Action(() =>
                            {
                                createdObject.set(pair.PropertyName, this.mapping[pairValue.Id]);
                            });

                        deferredActions.Add(deferredAction);
                    }
                }
            }

            // Now execute the deferred action
            foreach (var action in deferredActions)
            {
                action();
            }
        }

        public static void Copy(IURIExtent source, IURIExtent target)
        {
            var i = new ExtentCopier(source, target);
            i.Copy();
        }
    }
}
