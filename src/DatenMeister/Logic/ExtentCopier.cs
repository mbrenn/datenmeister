using BurnSystems.Test;
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
        private IURIExtent Source
        {
            get;
            set;
        }

        private IURIExtent Target
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
            this.Source = source;
            this.Target = target;

            Ensure.That(this.Source != null);
            Ensure.That(this.Target != null);
            Ensure.That(this.Source != this.Target);
        }

        /// <summary>
        /// Executes the copying
        /// </summary>
        public void Copy()
        {
            this.mapping.Clear();

            // Ok... How to do... First. Get all elements
            var elements = this.Source.Elements();
            var deferredActions = new List<Action>();

            foreach (var element in elements)
            {
                IObject type = null;
                if (element is IElement)
                {
                    type = (element as IElement).getMetaClass();
                }

                var createdObject = this.Target.CreateObject(type);
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
