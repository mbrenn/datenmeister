using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Description of EnablerChecker.
    /// </summary>
    internal class CriteriaCatalogue
    {
        /// <summary>
        /// Stores the list of included criteria
        /// </summary>
        private List<ICriteria> criterias = new List<ICriteria>();

        /// <summary>
        /// Stores the unique id of the criteria catalogue
        /// </summary>
        private Guid guid = Guid.NewGuid();

        /// <summary>
        /// Gets the unique id of the criteria catalogue
        /// </summary>
        public Guid Guid
        {
            get { return this.guid; }
        }

        /// <summary>
        /// Initializes a new instance of the CriteriaCatalogie class
        /// </summary>
        public CriteriaCatalogue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the CriteriaCatalogie class
        /// </summary>
        /// <param name="criterias">Criteria to be added</param>
        public CriteriaCatalogue(params ICriteria[] criterias)
        {
            foreach (var item in criterias)
            {
                this.Add(item);
            }
        }
        /// <summary>
        /// Adds a criteria to this object
        /// </summary>
        /// <param name="criteria">Criteria to be added</param>
        public void Add(ICriteria criteria)
        {
            this.criterias.Add(criteria);
        }

        /// <summary>
        /// Checks if the list of enablers matches
        /// to all included criteria
        /// </summary>
        /// <param name="enablers">List of enablers</param>
        /// <returns>true, if all objects match as expected</returns>
        public bool DoesMatch(IEnumerable<IEnabler> enablers)
        {
            // All criteria have to match to at least one enabler, 
            // Otherwise we have a problem.
            return this.criterias.All(
                x => enablers.Any(y => x.DoesMatch(y)));
        }

        /// <summary>
        /// Converts the instance to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var criteriaText = new StringBuilder();
            foreach (var criteria in this.criterias)
            {
                criteriaText.Append(criteria.ToString() + ", ");
            }

            return string.Format("{1}Guid={0}", guid, criteriaText.ToString());
        }

    }
}