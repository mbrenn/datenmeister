//-----------------------------------------------------------------------
// <copyright file="Result.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Stores the result of the testcase
    /// </summary>
    [Serializable()]
    public class Result
    {
        /// <summary>
        /// Gets or sets a value indicating whether the test was successful.
        /// </summary>
        public bool Failed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the duration of test
        /// </summary>
        public TimeSpan Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exception of the testcase
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }
    }
}
