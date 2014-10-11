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
    }
}
