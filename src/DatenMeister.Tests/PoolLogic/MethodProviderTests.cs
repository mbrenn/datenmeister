using DatenMeister.DataProvider;
using DatenMeister.Logic;
using DatenMeister.Logic.MethodProvider;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.PoolLogic
{
    [TestFixture]
    public class MethodProviderTests
    {
        [Test]
        public void TestStaticMethod()
        {
            ApplicationCore.PerformBinding();

            var database = new TestDatabase();
            var pool = database.Init();

            var methodProvider = Injection.Application.Get<IMethodProvider>();
            Assert.That(methodProvider, Is.Not.Null);

            var function = methodProvider.AddStaticMethod(TestDatabase.Types.Person, "Sum",
                new Func<int, int, int>((x, y) => x + y));
            Assert.That(function, Is.Not.Null);

            var result = function.Invoke(null, 2, 3);
            Assert.That(result, Is.TypeOf<int>());
            Assert.That((int)result, Is.EqualTo(5));
        }

        [Test]
        public void TestInstanceMethod()
        {
            ApplicationCore.PerformBinding();

            var database = new TestDatabase();
            var pool = database.Init();

            var factory = Factory.GetFor(database.ProjectExtent);
            var item = factory.create(null);

            var methodProvider = Injection.Application.Get<IMethodProvider>();
            Assert.That(methodProvider, Is.Not.Null);

            var function = methodProvider.AddInstanceMethod(item, "Sum",
                new Func<int, int, int>((x, y) => x + y));
            Assert.That(function, Is.Not.Null);

            var result = function.Invoke(null, 2, 3);
            Assert.That(result, Is.TypeOf<int>());
            Assert.That((int)result, Is.EqualTo(5));
        }

        [Test]
        public void TestTypeMethod()
        {
            ApplicationCore.PerformBinding();

            var database = new TestDatabase();
            var pool = database.Init();

            var factory = Factory.GetFor(database.ProjectExtent);
            var item = factory.create(null);

            var methodProvider = Injection.Application.Get<IMethodProvider>();
            Assert.That(methodProvider, Is.Not.Null);

            var calc = new X();
            calc.Summand = 10;

            var function = methodProvider.AddTypeMethod(item, "Sum",
                new Func<X, int, int, int>((t, x, y) => t.Summand + x + y));
            Assert.That(function, Is.Not.Null);

            var result = function.Invoke(calc, 2, 3);
            Assert.That(result, Is.TypeOf<int>());
            Assert.That((int)result, Is.EqualTo(15));
        }

        [Test]
        public void TestFunctionAssignment()
        {
            ApplicationCore.PerformBinding();

            var database = new TestDatabase();
            var pool = database.Init();

            var factory = Factory.GetFor(database.ProjectExtent);
            var item = factory.create(null);

            var methodProvider = Injection.Application.Get<IMethodProvider>();
            Assert.That(methodProvider, Is.Not.Null);

            // Person to be used
            var person = factory.create(TestDatabase.Types.Person);
            person.set("age", 10);
            var otherPerson = factory.create(TestDatabase.Types.Person);
            otherPerson.set("age", 15);

            // Add functions
            var staticSumFunction = methodProvider.AddStaticMethod(TestDatabase.Types.Person,
                new Func<int, int, int>((x, y) => x + y));

            var typeSumFunction = methodProvider.AddTypeMethod(TestDatabase.Types.Person,
                "Sum",
                new Func<IObject, int, int, int>((x, y, z) =>
                    {
                        var v = ObjectConversion.ToInt32(x.get("age"));
                        return v + y + z;
                    }));

            var instanceMulFunction = methodProvider.AddInstanceMethod(person,
                new Func<int, int, int>((x, y) => x * y));

            // Now check and test
            Assert.That(
                (int)staticSumFunction.Invoke(null, 5, 6),
                Is.EqualTo(11));
            Assert.That(
                (int)typeSumFunction.Invoke(person, 5, 6),
                Is.EqualTo(21));
            Assert.That(
                (int)typeSumFunction.Invoke(otherPerson, 5, 6),
                Is.EqualTo(26));
            Assert.That(
                (int)instanceMulFunction.Invoke(null, 5, 6),
                Is.EqualTo(30));

            // Gets the type functions
            var typeFunctions = methodProvider.GetFunctionsOnType(TestDatabase.Types.Person).ToList();
            var instanceFunctions = methodProvider.GetFunctionsOnInstance(person).ToList();
            var instanceOtherFunctions = methodProvider.GetFunctionsOnInstance(otherPerson).ToList();

            Assert.That(typeFunctions.Count, Is.EqualTo(2));
            Assert.That(instanceFunctions.Count, Is.EqualTo(2));
            Assert.That(instanceOtherFunctions.Count, Is.EqualTo(1));

            Assert.That(typeFunctions.Any(x => x.MethodType == MethodType.StaticMethod));
            Assert.That(typeFunctions.Any(x => x.MethodType == MethodType.TypeMethod));

            Assert.That(instanceFunctions.Any(x => x.MethodType == MethodType.TypeMethod));
            Assert.That(instanceFunctions.Any(x => x.MethodType == MethodType.InstanceMethod));

            Assert.That(instanceOtherFunctions.Any(x => x.MethodType == MethodType.TypeMethod));

            Assert.That(typeFunctions.Any(x => x.Name == staticSumFunction.Name));
            Assert.That(typeFunctions.Any(x => x.Name == typeSumFunction.Name));
            
            Assert.That(instanceFunctions.Any(x => x.Name == typeSumFunction.Name));
            Assert.That(instanceFunctions.Any(x => x.Name == instanceMulFunction.Name));

            Assert.That(instanceOtherFunctions.Any(x => x.Name == typeSumFunction.Name));

            var foundMethod1 = methodProvider.GetMethodOfTypeByName(TestDatabase.Types.Person, "Sum");
            var foundMethod2 = methodProvider.GetMethodOfInstanceByName(person, "Sum");

            Assert.That(foundMethod1, Is.EqualTo(typeSumFunction));
            Assert.That(foundMethod2, Is.EqualTo(typeSumFunction));
        }

        private class X
        {
            public int Summand
            {
                get;
                set;
            }
        }
    }
}
