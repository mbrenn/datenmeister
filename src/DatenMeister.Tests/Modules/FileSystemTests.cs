using BurnSystems;
using DatenMeister.AddOns.Data.FileSystem;
using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
                new FileSystemExtent(
                    "datenmeister:///test", 
                    Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles));
            var files = extent.Elements();
            Assert.That(files, Is.Not.Null);
        }

        [Test]
        public void TestDirectoriesAndFiles()
        {
            ApplicationCore.PerformBinding();
            DatenMeister.Entities.AsObject.Uml.Types.InitDecoupled();
            Init.DoDecoupled();

            // Create some test directories
            var rootPath = Path.Combine(Path.GetTempPath(), StringManipulation.SecureRandomString(8));
            System.IO.Directory.CreateDirectory(rootPath);

            // Create three directories
            var dir1 = Path.Combine(rootPath, "First");
            var dir2 = Path.Combine(rootPath, "Second");
            var dir3 = Path.Combine(rootPath, "Third");
            System.IO.Directory.CreateDirectory(dir1);
            System.IO.Directory.CreateDirectory(dir2);
            System.IO.Directory.CreateDirectory(dir3);

            // Create three files, two in root, one in second subdirectory
            var file1 = Path.Combine(rootPath, "x.txt");
            System.IO.File.WriteAllText(file1, "Testtext");
            var file2 = Path.Combine(rootPath, "y.txt");
            System.IO.File.WriteAllText(file2, "More Testtext");
            var file3 = Path.Combine(dir2, "y.txt");
            System.IO.File.WriteAllText(file3, "Another Testtext");

            // Perform the test
            var extent =
                new FileSystemExtent(
                    "datenmeister:///test",
                    rootPath);

            var elements = extent.Elements().ToList();
            Assert.That(elements.Count, Is.EqualTo(5)); // 2 files, 3 directories

            var foundFileXTxt = elements
                .Where(x => x.AsIObject().get("name").AsSingle().ToString() == "x.txt")
                .Select ( x=> x.AsIObject() as IElement)
                .FirstOrDefault();
            Assert.That(foundFileXTxt, Is.Not.Null);
            Assert.That(foundFileXTxt.get("name").AsSingle().ToString(), Is.EqualTo("x.txt"));
            Assert.That(foundFileXTxt.get("extension").AsSingle().ToString(), Is.EqualTo(".txt"));
            Assert.That(Convert.ToInt32(foundFileXTxt.get("length").AsSingle().ToString()), Is.EqualTo(8));
            Assert.That(foundFileXTxt.get("relativePath").AsSingle().ToString(), Is.EqualTo("/x.txt"));
            Assert.That(
                foundFileXTxt.getMetaClass(),
                Is.EqualTo(DatenMeister.AddOns.Data.FileSystem.AsObject.Types.File));

            var foundDirectory = elements
                .Where(x => x.AsIObject().get("name").AsSingle().ToString() == "First")
                .Select(x => x.AsIObject() as IElement)
                .FirstOrDefault();
            Assert.That(foundDirectory, Is.Not.Null);
            Assert.That(foundDirectory.get("name").AsSingle().ToString(), Is.EqualTo("First"));
            Assert.That(foundDirectory.get("extension").AsSingle().ToString(), Is.EqualTo(""));
            Assert.That(foundDirectory.get("relativePath").AsSingle().ToString(), Is.EqualTo("/First"));
            Assert.That(foundFileXTxt.getMetaClass()
                    == DatenMeister.AddOns.Data.FileSystem.AsObject.Types.Directory);
            // Everything is done, now remove all the stuff
            System.IO.Directory.Delete(rootPath, true);
        }
    }
}
