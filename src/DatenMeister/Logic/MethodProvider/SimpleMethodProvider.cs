using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    public class SimpleMethodProvider : IMethodProvider
    {
        /// <summary>
        /// Defines the function map between id of the function and the function itself
        /// </summary>
        private Dictionary<string, IFunction> functionMap = new Dictionary<string, IFunction>();

        /// <summary>
        /// Adds a static method to the type
        /// </summary>
        /// <param name="type">Type of the object, where function shall be added</param>
        /// <param name="id">Name of the function</param>
        /// <param name="functionMethod"></param>
        /// <returns>The created function</returns>
        public IFunction AddStaticMethod(IObject type, string id, Delegate functionMethod)
        {
            var function = new StaticFunctionImpl(
                functionMethod);

            this.functionMap[id] = function;

            return function;
        }
    }
}
