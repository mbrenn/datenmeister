using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    internal class InstanceFunctionImpl : StaticFunctionImpl, IMethod
    {
        /// <summary>
        /// Initializes a new instance of the StaticFunctionImpl class. 
        /// </summary>
        /// <param name="del"></param>
        public InstanceFunctionImpl(string id, Delegate del)
            :base(id, del)
        {
        }

        /// <summary>
        /// Gets the method type
        /// </summary>
        public new MethodType MethodType
        {
            get { return MethodType.InstanceMethod; }
        }
    }
}
