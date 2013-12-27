using BurnSystems.FlexBG.Helper.ProviderByIdM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.BaseProviderByIdM
{
    [TestFixture]
    public class TestIndexProvider
    {
        [Test]
        public void TestAddAndRetrieval()
        {
            var provider = new IndexProviderById<UnitType>();
            provider.Add(
                new UnitType()
                {
                    Id = 1,
                    Token = "Unit 1"
                });
            provider.Add(
                new UnitType()
                {
                    Id = 2,
                    Token = "Unit 2"
                });

            var unitType1 = provider.Get(1);
            Assert.That(unitType1, Is.Not.Null);
            Assert.That(unitType1.Token, Is.EqualTo("Unit 1"));

            var unitType2 = provider.Get(2);
            Assert.That(unitType2, Is.Not.Null);
            Assert.That(unitType2.Token, Is.EqualTo("Unit 2"));

            var unitType3 = provider.Get(3);
            Assert.That(unitType3, Is.Null);
        }

        [Test]
        public void TestDouble()
        {
            var provider = new IndexProviderById<UnitType>();
            provider.Add(
                new UnitType()
                {
                    Id = 1,
                    Token = "Unit 1"
                });

            Assert.Throws<InvalidOperationException>(() =>
                provider.Add(
                    new UnitType()
                    {
                        Id = 1,
                        Token = "Unit 2"
                    }));
        }
    }
}
