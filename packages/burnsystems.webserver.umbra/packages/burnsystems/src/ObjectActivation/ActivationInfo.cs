using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// This class contains the information how the object shall be
    /// created, when the object shall be created and which additional actions shall
    /// be performed.
    /// </summary>
    internal class ActivationInfo
    {
        /// <summary>
        /// Stores the criteria catalogue being used for activation
        /// </summary>
        private List<CriteriaCatalogue> criteriaCatalogues = new List<CriteriaCatalogue>();

        /// <summary>
        /// Gets or sets a value, indicating whether autobinding with
        /// BindAlsoToAttribute shall be executed. Per default, autobinding is enabled.
        /// </summary>
        public bool NoAutoBinding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the criteriacatalogue
        /// </summary>
        public List<CriteriaCatalogue> CriteriaCatalogues
        {
            get { return criteriaCatalogues; }
        }

        /// <summary>
        /// Stores the factory method for the object within an activationblock
        /// </summary>
        public Func<ActivationContainer, IActivates, IEnumerable<IEnabler>, object> FactoryActivationContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression for activation. 
        /// This is just used to ease debugging
        /// </summary>
        public Expression ExpressionActivationContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the factory method for the object within an activationblock
        /// </summary>
        /// <remarks>
        /// 1. Parameter: Current ActivationBlock
        /// 2. Parameter: InnerMost ActivationBlock, which is used to find the correct block where to store the data and to restart searching
        /// 3. Parameter: List of enablers
        /// </remarks>
        public Func<ActivationBlock, ActivationBlock, IEnumerable<IEnabler>, object> FactoryActivationBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression for activation. 
        /// This is just used to ease debugging
        /// </summary>
        public Expression ExpressionActivationBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ActivationInfo class.
        /// </summary>
        /// <param name="criteriaCatalogue">CriteriaCatalogue to be used</param>
        public ActivationInfo(CriteriaCatalogue criteriaCatalogue)
        {
            if (criteriaCatalogue == null)
                throw new ArgumentNullException("criteriaCatalogue");

            this.criteriaCatalogues.Add(criteriaCatalogue);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var catalogue in this.criteriaCatalogues)
            {
                result.AppendLine(catalogue.ToString());
            }

            return result.ToString();
        }
    }
}
