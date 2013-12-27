
namespace BurnSystems.Parser.UnitTests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ExpressionParserTests
    {
        [Test]
        public void TestCompareObjectsWithNull()
        {
            Assert.That(
                ExpressionParser.CompareObjects(null, null),
                Is.EqualTo(0));
        }
        
        [Test]
        public void TestCompareObjectsWithLeftNull()
        {
            Assert.That(
                ExpressionParser.CompareObjects(null, "T"),
                Is.GreaterThan(0));
        }
        
        [Test]
        public void TestCompareObjectsWithRightNull()
        {
            Assert.That(
                ExpressionParser.CompareObjects("T", null),
                Is.LessThan(0));
        }
        
        [Test]
        public void TestCompareObjectsWithDifferentIntegers()
        {
            Assert.That(
                ExpressionParser.CompareObjects(1, 2),
                Is.LessThan(0));
        }
    }
}
