//-----------------------------------------------------------------------
// <copyright file="TestClassAttribute.cs" company="Martin Brenn">
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
    /// This attribute declares all classes containing tests
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TestClassAttribute : Attribute
    {
    }
}
