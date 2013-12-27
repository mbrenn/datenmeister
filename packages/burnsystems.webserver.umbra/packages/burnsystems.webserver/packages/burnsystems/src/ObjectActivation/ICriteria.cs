using System;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Description of ICriteria.
    /// </summary>
    public interface ICriteria
    {
        /// <summary>
        /// Shall return true, if enabler matches to this criteria. 
        /// </summary>
        /// <param name="enabler"></param>
        /// <returns></returns>
        bool DoesMatch(IEnabler enabler);
    }
}
