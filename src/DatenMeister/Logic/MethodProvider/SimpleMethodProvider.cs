using BurnSystems.Logging;
using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    /// <summary>
    /// Implements the IMethodProvider interface in a very simple, and perhabs slow 
    /// way.
    /// </summary>
    public class SimpleMethodProvider : IMethodProvider
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(SimpleMethodProvider));
        
        /// <summary>
        /// Stores the mapping between types and methods
        /// </summary>
        private MultiValueDictionary<IObject, IMethod> typeMapping
            = new MultiValueDictionary<IObject, IMethod>();

        /// <summary>
        /// Stores the mapping between instances and methods
        /// </summary>
        private MultiValueDictionary<IObject, IMethod> instanceMapping
            = new MultiValueDictionary<IObject, IMethod>();

        /// <summary>
        /// Adds a static method to the type
        /// </summary>
        /// <param name="type">Type of the object, where function shall be added</param>
        /// <param name="id">Name of the function</param>
        /// <param name="functionMethod"></param>
        /// <returns>The created function</returns>
        public IMethod AddStaticMethod(IObject type, string id, Delegate functionMethod)
        {
            var function = new StaticFunctionImpl(
                id,
                functionMethod);

            this.typeMapping.Add(type, function);

            return function;
        }

        /// <summary>
        /// Adds an instance method to the type
        /// </summary>
        /// <param name="type">Instance of the , where function shall be added</param>
        /// <param name="id">Name of the function</param>
        /// <param name="functionMethod"></param>
        /// <returns>The created function</returns>
        public IMethod AddInstanceMethod(IObject instance, string id, Delegate functionMethod)
        {
            logger.Message("Add instance: " + instance.Id);

            // Check, if the object is a proxy object
            var instanceAsProxy = instance as IProxyObject;
            if (instanceAsProxy != null)
            {
                return AddInstanceMethod(instanceAsProxy.Value, id, functionMethod);
            }
            else
            {
                var function = new InstanceFunctionImpl(
                    id,
                    functionMethod);

                this.instanceMapping.Add(instance, function);

                return function;
            }
        }

        /// <summary>
        /// Adds a type method to the specific type. All instances of the type can now execute
        /// the type
        /// </summary>
        /// <param name="type">Type which shall have the function</param>
        /// <param name="id">Id of the function</param>
        /// <param name="functionMethod">Method of the function</param>
        /// <returns>The created function</returns>
        public IMethod AddTypeMethod(IObject type, string id, Delegate functionMethod)
        {
            var function = new TypeFunctionImpl(
                id,
                functionMethod);

            this.typeMapping.Add(type, function);

            return function;
        }

        /// <summary>
        /// Gets all function on a certain type
        /// </summary>
        /// <param name="type">Type of the function being queried</param>
        /// <returns>Returns the functions</returns>
        public IEnumerable<IMethod> GetFunctionsOnType(IObject type)
        {
            IReadOnlyCollection<IMethod> result;
            this.typeMapping.TryGetValue(type, out result);

            foreach (var method in result)
            {
                yield return method;
            }
        }

        /// <summary>
        /// Gets all function on a certain type
        /// </summary>
        /// <param name="instance">Instance of the function being queried</param>
        /// <returns>Returns the functions on the instance</returns>
        public IEnumerable<IMethod> GetFunctionsOnInstance(IObject instance)
        {
            var instanceAsProxy = instance as IProxyObject;
            if (instanceAsProxy != null)
            {
                foreach (var result in GetFunctionsOnInstance(instanceAsProxy.Value))
                {
                    yield return result;
                }
            }
            else
            {
                IReadOnlyCollection<IMethod> instanceMethods;
                this.instanceMapping.TryGetValue(instance, out instanceMethods);
                if (instanceMethods != null)
                {
                    foreach (var instanceMethod in instanceMethods)
                    {
                        yield return instanceMethod;
                    }
                }

                var element = instance as IElement;
                if (element != null)
                {
                    var metaClass = element.getMetaClass();

                    if (metaClass != null)
                    {
                        IReadOnlyCollection<IMethod> typeFunctions;
                        this.typeMapping.TryGetValue(metaClass, out typeFunctions);

                        if (typeFunctions != null)
                        {
                            foreach (var typeFunction in typeFunctions
                                .Where(x => x.MethodType == MethodType.TypeMethod))
                            {
                                yield return typeFunction;
                            }
                        }
                    }
                }
            }
        }
    }
}
