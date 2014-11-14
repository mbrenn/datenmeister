using DatenMeister.AddOns.Data.FileSystem;
using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Modules
{
    /// <summary>
    /// Performs the tests for the filesystem
    /// </summary>
    [TestFixture]
    public class FileSystemTests
    {
        /// <summary>
        /// Performs some Testing
        /// </summary>
        [Test]
        public void TestExtent()
        {
            ApplicationCore.PerformBinding();
            DatenMeister.Entities.AsObject.Uml.Types.InitDecoupled();
            Init.DoDecoupled();
            var extent =
                new FileSystemExtent("datenmeister:///test", Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles));
            var files = extent.Elements();
        }
    }
}
