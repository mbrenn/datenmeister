using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.Collections;

namespace BurnSystems.UnitTests.Collections
{
    /// <summary>
    /// Implements some tests for the typed container
    /// </summary>
    [TestFixture]
    public class TypedContainerTests
    {
        [Test]
        public void AddAndRetrieveAvailableObject()
        {
            var a = new A();
            var b = new B();

            var typedContainer = new TypedContainer();
            typedContainer.Add(a);
            typedContainer.Add(b);

            var aFound = typedContainer.Get<A>();
            Assert.That(aFound, Is.SameAs(a));

            var bFound = typedContainer.Get<B>();
            Assert.That(bFound, Is.SameAs(b));

            var cFound = typedContainer.Get<C_B>();
            Assert.That(cFound, Is.Null);
        }

        [Test]
        public void AddAndRetrieveNonAvailableObject()
        {
            var a = new A();
            var b = new B();

            var typedContainer = new TypedContainer();
            typedContainer.Add(a);
            typedContainer.Add(b);

            var cFound = typedContainer.Get<C_B>();
            Assert.That(cFound, Is.Null);
        }

        [Test]
        public void AddAndRetrieveWithInheritence()
        {
            var a = new A();
            var b = new B();
            var c = new C_B();

            var typedContainer = new TypedContainer();
            typedContainer.Add(a);
            typedContainer.Add(b);
            typedContainer.Add(c);

            var cFound = typedContainer.Get<C_B>();
            Assert.That(cFound, Is.SameAs(c));
        }

        [Test]
        public void AddAndRetrieveInheritedClass()
        {
            var c = new C_B();

            var typedContainer = new TypedContainer();
            typedContainer.Add(c);

            var bFound = typedContainer.Get<B>();
            Assert.That(bFound, Is.SameAs(c));
        }

        [Test]
        public void AddAndRetrieveInheritedClassWithException()
        {
            var b = new B();
            var c = new C_B();

            var typedContainer = new TypedContainer();
            typedContainer.Add(b);
            typedContainer.Add(c);

            Assert.Throws<InvalidOperationException>(
                () => typedContainer.Get<B>());
        }

        public class A
        {
        }

        public class B
        {
        }

        public class C_B : B
        {
        }
    }
}
