using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// The activation container stores the information how objects
    /// shall be created, how they shall be disposed and if an object
    /// shall be created for each request.
    /// An activation container can be embedded within another activation container. 
    /// The containers requires a name.
    /// </summary>
    public class ActivationContainer : IActivates
    {
        /// <summary>
        /// Gets or sets the name of the activationcontainer
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Stores the instance of the inner container
        /// </summary>
        private ActivationContainer outerContainer;

        /// <summary>
        /// Stores the list of activation infos
        /// </summary>
        private List<ActivationInfo> activationInfos = new List<ActivationInfo>();

        /// <summary>
        /// Stores the list of all active instances within this collection
        /// </summary>
        private ActiveInstanceCollection activeInstances =
            new ActiveInstanceCollection();

        /// <summary>
        /// Gets the list of activation infos
        /// </summary>
        internal List<ActivationInfo> ActivationInfos
        {
            get { return this.activationInfos; }
        }

        /// <summary>
        /// Gets the outer container or null, if no outer container
        /// has been defined.
        /// </summary>
        public ActivationContainer OuterContainer
        {
            get { return this.outerContainer; }
        }

        /// <summary>
        /// Gets the active instances within the container
        /// </summary>
        internal ActiveInstanceCollection ActiveInstances
        {
            get { return this.activeInstances; }
        }

        /// <summary>
        /// Throws the event that binding has changed
        /// </summary>
        public event EventHandler BindingChanged;

        /// <summary>
        /// Initializes a new instance of the ActivationContainer class.
        /// </summary>
        /// <param name="name">Name of the Container</param>
        public ActivationContainer(string name)
        {
            this.Name = name;

            // Add this
            this.Bind<IActivates>().ToConstant(this);
        }

        /// <summary>
        /// Initializes a new instance of the ActivationContainer class. 
        /// </summary>
        /// <param name="name">Name of the container class</param>
        /// <param name="outerContainer">The outercontainer that shall be 
        /// used as fallback, if this container does not have a resolution for this 
        /// instance. </param>
        public ActivationContainer(string name, ActivationContainer outerContainer)
            : this(name)
        {
            Ensure.IsNotNull(outerContainer);
            this.outerContainer = outerContainer;
            this.outerContainer.BindingChanged += (x, y) => this.OnBindingChanged();
        }

        /// <summary>
        /// Calls the OnBindingChanged event
        /// </summary>
        protected void OnBindingChanged()
        {
            var e = this.BindingChanged;
            if (e != null)
            {
                e(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Adds an activation info to this block. 
        /// It is necessary to add the factory methods for 
        /// creation within ActivationBlock and ActivationContainer
        /// </summary>
        /// <param name="catalogue">Criteria Catalogue to be used</param>
        /// <returns>The activation info</returns>
        internal ActivationInfo Add(CriteriaCatalogue catalogue)
        {
            var info = new ActivationInfo(catalogue);
            this.activationInfos.Add(info);
            return info;
        }

        /// <summary>
        /// Activates an object by a list of enablers
        /// </summary>
        /// <param name="enablers">Enabler to be used</param>
        /// <returns>Created object</returns>
        public IEnumerable<object> GetAll(IEnumerable<IEnabler> enablers)
        {
            return this.GetAllInternal(this, enablers);
        }

        /// <summary>
        /// Activates an object by a list of enablers
        /// </summary>
        /// <param name="enablers">Enabler to be used</param>
        /// <param name="innerMost">Most inner thing</param>
        /// <returns>Created object</returns>
        private IEnumerable<object> GetAllInternal(IActivates innerMost, IEnumerable<IEnabler> enablers)
        {
            foreach (var item in this.ActivationInfos)
            {
                if (item.CriteriaCatalogues.Any(y => y.DoesMatch(enablers)))
                {
                    var value = item.FactoryActivationContainer(this, innerMost, enablers);
                    yield return value;
                }
            }

            // No match...
            // Asking parent object, if existing
            if (this.OuterContainer != null)
            {
                foreach (var item in this.OuterContainer.GetAllInternal(innerMost, enablers))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Checks, if the container knows a binding to the specific enablers
        /// </summary>
        /// <param name="enablers">Enablers to be tested</param>
        /// <returns>true, if container or block knows how to activate an object by the enablers</returns>
        public bool Has(IEnumerable<IEnabler> enablers)
        {
            foreach (var item in this.ActivationInfos)
            {
                if (item.CriteriaCatalogues.Any(y => y.DoesMatch(enablers)))
                {
                    return true;
                }
            }

            // No match...
            // Asking parent object, if existing
            if (this.OuterContainer != null)
            {
                return this.OuterContainer.Has(enablers);
            }

            return false;
        }

        /// <summary>
        /// Converts activation block to string
        /// </summary>
        /// <returns>String containing the name</returns>
        public override string ToString()
        {
            return string.Format(
                "ActivationContainer: {0}",
                this.Name);
        }
    }
}
    