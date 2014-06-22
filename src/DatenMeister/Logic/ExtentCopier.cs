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
    public class ExtentCopier : ObjectCopier
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
            : base(target)
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
        /// Initializes a new instance of the ExtentCopier class
        /// </summary>
        /// <param name="source">Source to be copied</param>
        /// <param name="target">Target, which shall receive the copy</param>
        public ExtentCopier(IReflectiveSequence source, IReflectiveSequence target)
            : base(target.Extent)
        {
            Ensure.That(source != null);
            Ensure.That(target != null);
            Ensure.That(source != target);

            this.source = source;
            this.target = target;

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
            var sourceElements = this.source;
            var deferredActions = new List<Action>();

            foreach (var sourceElement in sourceElements.Select(x=> x.AsIObject()))
            {
                var targetElement = this.CopyElement(sourceElement, deferredActions);

                this.target.add(targetElement);
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
