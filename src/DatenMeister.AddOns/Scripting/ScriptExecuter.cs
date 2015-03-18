using DatenMeister.AddOns.Scripting.ScriptObjects;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Scripting
{
    public class ScriptExecuter : IScriptExecuter
    {
        /// <summary>
        /// Stores the engine
        /// </summary>
        private ScriptEngine engine;
        
        /// <summary>
        /// Initializes a new instance of the ScriptExecuter class
        /// </summary>
        public ScriptExecuter()
        {
            this.engine = Python.CreateEngine();
        }

        public object Execute(string script)
        {
            var scope = this.engine.CreateScope();
            scope.SetVariable("Message", new Message());

            var source = this.engine.CreateScriptSourceFromString(script, Microsoft.Scripting.SourceCodeKind.SingleStatement);
            var result = source.Execute(scope);

            return result;
        }
    }
}
