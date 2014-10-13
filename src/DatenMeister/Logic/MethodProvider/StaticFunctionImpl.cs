using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    internal class StaticFunctionImpl : IMethod
    {
        /// <summary>
        /// Gets the id
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the method type
        /// </summary>
        public MethodType MethodType
        {
            get { return MethodType.StaticMethod; }
        }

        /// <summary>
        /// Defines the delegate, that will be called for the instance
        /// </summary>
        private Delegate del;

        /// <summary>
        /// Initializes a new instance of the StaticFunctionImpl class. 
        /// </summary>
        /// <param name="del"></param>
        public StaticFunctionImpl (string name, Delegate del)
        {
            this.Name = name;
            Ensure.That(del != null);
            this.del = del;
        }

        /// <summary>
        /// Invokes the method
        /// </summary>
        /// <param name="context">Context being ignored for static types</param>
        /// <param name="parameters">The parameters being sent to the object</param>
        /// <returns>The invoked object</returns>
        public object Invoke(object context, params object[] parameters)
        {
            // Try to get Func
            var returnType = del.Method.ReturnType;
            var parameterTypes = del.Method.GetParameters();

            // Context will be ignored and the parameters need to be converted
            if (parameters.Length != parameterTypes.Length)
            {
                throw new InvalidOperationException("Number of parameters does not match to delegate");
            }

            // It seems to match, now convert the parameters
            var targetParameters = new object[parameters.Length];
            for (var n = 0; n < parameters.Length; n++)
            {
                targetParameters[n] =
                    ObjectConversion.ConvertTo(parameters[n], parameterTypes[n].ParameterType);
            }

            return del.DynamicInvoke(targetParameters);
        }
    }
}
