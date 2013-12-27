//-----------------------------------------------------------------------
// <copyright file="TestMethodAttribute.cs" company="Martin Brenn">
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
    /// This attribute declares all methods containing tests. 
    /// The class of the method has to be delared by the <c>TestClassAttribute</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestMethodAttribute : Attribute
    {
    }
}
