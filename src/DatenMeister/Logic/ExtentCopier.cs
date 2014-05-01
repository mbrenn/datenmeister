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
            // Ok... How to do... First. Get all elements
            var elements = this.Source.Elements();
            foreach (var element in elements)
            {
                IObject type = null;
                if ( element is IElement)
                {
                    type = (element as IElement).getMetaClass();
                }

                var createdObject = this.Target.CreateObject(type);
                
                var pairs = element.getAll();
                foreach (var pair in pairs)
                {
                    createdObject.set(pair.PropertyName, pair.Value);
                }
            }
        }

        public static void Copy(IURIExtent source, IURIExtent target)
        {
            var i = new ExtentCopier(source, target);
            i.Copy();
        }
    }
}
