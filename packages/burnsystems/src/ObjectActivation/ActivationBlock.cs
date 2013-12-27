using BurnSystems.ObjectActivation.Enabler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// The activation block contains the objects that shall
    /// get disposed, the activation block is disposed. 
    /// The disposal of an included object shall only occur, if
    /// the object has been created by this instance.
    /// </summary>
    public class ActivationBlock : IDisposable, IActivates
    {
        /// <summary>
        /// Gets or sets the name of the block
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the container that contains the information how, when and
        /// in which way to create the object
        /// </summary>
        private ActivationContainer container;

        /// <summary>
        /// Contains an inner block that may already contain the required object.
        /// </summary>
        private ActivationBlock outerBlock;

        /// <summary>
        /// Stores the list of all active instances within this collection
        /// </summary>
        private ActiveInstanceCollection activeInstances =
            new ActiveInstanceCollection();

        /// <summary>
        /// Gets the active instances
        /// </summary>
        internal ActiveInstanceCollection ActiveInstances
        {
            get { return this.activeInstances; }
        }

        /// <summary>
        /// Gets the container that contains the information how, when and
        /// in which way to create the object
        /// </summary>
        internal ActivationContainer Container
        {
            get { return this.container; }
        }

        /// <summary>
        /// Gets an inner block that may already contain the required object.
        /// </summary>
        internal ActivationBlock OuterBlock
        {
            get { return this.outerBlock; }
        }

        /// <summary>
        /// Throws the event that binding has changed
        /// </summary>
        public event EventHandler BindingChanged;

        /// <summary>
        /// Initializes a new instance of the ActivationBlock class.
        /// </summary>
        /// <param name="name">Name of the activation block</param>
        /// <param name="container">The inner container</param>
        public ActivationBlock(string name, ActivationContainer container)
        {
            this.Name = name;
            this.container = container;

            this.container.BindingChanged += (x, y) => this.OnBindingChanged();
        }

        /// <summary>
        /// Initializes a new instance of the ActivationBlock class.
        /// </summary>
        /// <param name="name">Name of the object to be created</param>
        /// <param name="container">The container to be used</param>
        /// <param name="outerBlock">The inner block containing the necessary
        /// information</param>
        public ActivationBlock(
            string name,
            ActivationContainer container,
            ActivationBlock outerBlock)
            : this(name, container)
        {
            this.outerBlock = outerBlock;

            this.outerBlock.BindingChanged += (x, y) => this.OnBindingChanged();
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
        /// Disposes all included object
        /// </summary>
        public void Dispose()
        {
            lock (this.activeInstances)
            {
                foreach (var activeInstance in this.activeInstances)
                {
                    var disposable = activeInstance.Value as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }

                this.activeInstances.Clear();
            }
        }

        /// <summary>
        /// Adds an instance to the block
        /// </summary>
        /// <param name="activeInstance">Instance to be added</param>
        internal void Add(ActiveInstance activeInstance)
        {
            lock (this.activeInstances)
            {
                this.activeInstances.Add(activeInstance);
            }
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
        /// Gets all elements matching to enabler in the current Activation block. 
        /// If an activationblock has been found, the most inner actvation block will be used 
        /// to instantiate the object
        /// </summary>
        /// <param name="enablers">Enumeration of enablers</param>
        /// <param name="mostInner">Most inner block being sent to constructor/factory method</param>
        /// <returns>Enumeration of found objects</returns>
        private IEnumerable<object> GetAllInternal(ActivationBlock mostInner, IEnumerable<IEnabler> enablers)
        {
            var enablerList = enablers.ToList();

            // If enabler is just an IActivates or ActivationBlock, return this
            if (enablerList.Count == 1
                && enablerList[0] is ByTypeEnabler
                && (
                    ((ByTypeEnabler)enablerList[0]).Type == typeof(IActivates)
                    || ((ByTypeEnabler)enablerList[0]).Type == typeof(ActivationBlock)))
            {
                yield return mostInner;
            }

            var currentContainer = this.container;

            while (currentContainer != null)
            {
                foreach (var item in currentContainer.ActivationInfos)
                {
                    if (item.CriteriaCatalogues.Any(y => y.DoesMatch(enablerList)))
                    {
                        yield return item.FactoryActivationBlock(this, mostInner, enablers);
                    }
                }

                currentContainer = currentContainer.OuterContainer;
            }

            if (this.outerBlock != null)
            {
                // Calls outer Activation Block, but references to the mostInner one to make as much
                // objects available as possible
                var result = this.outerBlock.GetAllInternal(mostInner, enablers);
                foreach (var item in result)
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
            var currentContainer = this.container;

            while (currentContainer != null)
            {
                foreach (var item in currentContainer.ActivationInfos)
                {
                    if (item.CriteriaCatalogues.Any (y=> y.DoesMatch(enablers)))
                    {
                        return true;
                    }
                }

                currentContainer = currentContainer.OuterContainer;
            }

            if (this.outerBlock != null)
            {
                var result = this.outerBlock.GetAll(enablers);
                if (result != null)
                {
                    return true;
                }
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
                "ActivationBlock: {0}",
                this.Name);
        }

        /// <summary>
        /// Finds the activation block, matching to the given function
        /// </summary>
        /// <param name="where">Condition of the function</param>
        /// <returns>Found Activationblock</returns>
        public ActivationBlock FindActivationBlockInChain(Func<ActivationBlock, bool> where)
        {
            var currentActivationBlock = this;

            while (currentActivationBlock != null)
            {
                if (where(currentActivationBlock))
                {
                    return currentActivationBlock;
                }

                // Try to add to the next outer scope
                currentActivationBlock = currentActivationBlock.OuterBlock;
            }

            return null;

        }
    }
}