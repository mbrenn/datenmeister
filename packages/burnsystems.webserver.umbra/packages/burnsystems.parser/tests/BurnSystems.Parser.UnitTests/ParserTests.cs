
namespace BurnSystems.Parser.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using NUnit.Framework;

    /// <summary>
    /// This class tests the capability of the parser against a certain set of tests
    /// </summary>
    [TestFixture]
    public class ParserTests
    {
        /// <summary>
        /// Gets the default parser
        /// </summary>
        /// <returns></returns>
        public TemplateParser GetParser()
        {
            var parser = new TemplateParser();
            parser.AddVariable("Vorname", "Martin");
            parser.AddVariable("Name", "Brenn");
            parser.AddVariable("Alter", 18);
            parser.AddVariable("Gewicht", 62);
            parser.AddVariable("Primzahlen", new int[] { 2, 3, 5, 7, 11, 13 });

            return parser;
        }

        [Test]
        public void TestParser()
        {
            XDocument document = XDocument.Parse(Test_Resources.ParserTest_Case);

            foreach (var test in document.Elements("tests").Elements("case"))
            {
                var input = test.Element("input").Value;
                var shallOutput = test.Element("output").Value;

                var isOutput = GetParser().Parse(input);

                Assert.That(isOutput, Is.EqualTo(shallOutput));
            }
        }
    }
}
