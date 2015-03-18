using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Scripting
{
    public interface IScriptExecuter
    {
        /// <summary>
        /// Executes a certain script
        /// </summary>
        /// <param name="script"></param>
        object Execute(string script);
    }
}
