using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;

namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    /// <summary>
    /// Implements several tests for columns
    /// </summary>
    [TestFixture]
    public class ColumnTests
    {
        /// <summary>
        /// Tests the initialization of a column
        /// </summary>
        [Test]
        public void TestInit()
        {
            var column = new VoxelMapColumn();
            column.InitFields();

            Assert.That(column.Changes.Count, Is.EqualTo(1));
            Assert.That(column.Changes[0].ChangeHeight, Is.EqualTo(float.MaxValue));
            Assert.That(column.Changes[0].FieldType, Is.EqualTo(0));
        }

        [Test]
        public void TestGet()
        {
            var column = new VoxelMapColumn();
            column.InitFields();

            Assert.That(column.GetFieldType(0), Is.EqualTo(0));
            Assert.That(column.GetFieldType(1000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(-1000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(float.MaxValue), Is.EqualTo(0));
            Assert.That(column.GetFieldType(float.MinValue), Is.EqualTo(0));
        }

        [Test]
        public void TestSingleSet()
        {
            var column = new VoxelMapColumn();
            column.InitFields();
            column.SetFieldType(1, 0, -1000);

            Assert.That(column.GetFieldType(1000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(1), Is.EqualTo(0));
            Assert.That(column.GetFieldType(0), Is.EqualTo(1));
            Assert.That(column.GetFieldType(-500), Is.EqualTo(1));
            Assert.That(column.GetFieldType(-1000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(float.MaxValue), Is.EqualTo(0));
            Assert.That(column.GetFieldType(float.MinValue), Is.EqualTo(0));
        }

        [Test]
        public void TestMultipleSet()
        {
            var column = new VoxelMapColumn();
            column.InitFields();
            column.SetFieldType(1, 0, -1000);
            column.SetFieldType(2, 3000, 1000);
            column.SetFieldType(3, 5000, 4000);

            Assert.That(column.GetFieldType(4500), Is.EqualTo(3));
            Assert.That(column.GetFieldType(3500), Is.EqualTo(0));
            Assert.That(column.GetFieldType(2000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(1000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(1), Is.EqualTo(0));
            Assert.That(column.GetFieldType(0), Is.EqualTo(1));
            Assert.That(column.GetFieldType(-500), Is.EqualTo(1));
            Assert.That(column.GetFieldType(-1000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(float.MaxValue), Is.EqualTo(0));
            Assert.That(column.GetFieldType(float.MinValue), Is.EqualTo(0));
        }

        [Test]
        public void TestInterlappingSet()
        {
            var column = new VoxelMapColumn();
            column.InitFields();
            column.SetFieldType(1, 1000, -1000);
            column.SetFieldType(2, -500, -2000);
            column.SetFieldType(3, 3000, 500);

            Assert.That(column.GetFieldType(4000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(3000), Is.EqualTo(3));
            Assert.That(column.GetFieldType(2000), Is.EqualTo(3));
            Assert.That(column.GetFieldType(1500), Is.EqualTo(3));
            Assert.That(column.GetFieldType(1000), Is.EqualTo(3));
            Assert.That(column.GetFieldType(500), Is.EqualTo(1));
            Assert.That(column.GetFieldType(0), Is.EqualTo(1));
            Assert.That(column.GetFieldType(-500), Is.EqualTo(2));
            Assert.That(column.GetFieldType(-1000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(-1000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(-1500), Is.EqualTo(2));
            Assert.That(column.GetFieldType(-2000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(-3000), Is.EqualTo(0));
        }

        [Test]
        public void TestRandomSettings()
        {
            var column = new VoxelMapColumn();
            column.InitFields();

            var random = new Random();

            for (var n = 0; n < 10000; n++)
            {
                var startHeight = (float)(random.NextDouble() - 0.5) * 1000;
                var endHeight = (float)(random.NextDouble() - 0.5) * 1000;

                var fieldType = (byte)random.Next(255);

                column.SetFieldType(fieldType, startHeight, endHeight);
            }

            column.SetFieldType(0, float.MaxValue, float.MinValue);

            for (var f = 3000.0f; f > -3000.0f; f -= 100)
            {
                Assert.That(column.GetFieldType(f), Is.EqualTo(0));
            }
        }

        [Test]
        public void TestInnerSetting()
        {
            var column = new VoxelMapColumn();
            column.InitFields();
            column.SetFieldType(1, 1000, 500);
            column.SetFieldType(1, 900, 800);
            column.SetFieldType(2, 700, 600);

            Assert.That(column.GetFieldType(1200), Is.EqualTo(0));
            Assert.That(column.GetFieldType(1000), Is.EqualTo(1));
            Assert.That(column.GetFieldType(950), Is.EqualTo(1));
            Assert.That(column.GetFieldType(900), Is.EqualTo(1));
            Assert.That(column.GetFieldType(850), Is.EqualTo(1));
            Assert.That(column.GetFieldType(800), Is.EqualTo(1));
            Assert.That(column.GetFieldType(750), Is.EqualTo(1));
            Assert.That(column.GetFieldType(700), Is.EqualTo(2));
            Assert.That(column.GetFieldType(650), Is.EqualTo(2));
            Assert.That(column.GetFieldType(600), Is.EqualTo(1));
            Assert.That(column.GetFieldType(550), Is.EqualTo(1));
            Assert.That(column.GetFieldType(500), Is.EqualTo(0));
            Assert.That(column.GetFieldType(450), Is.EqualTo(0));
        }

        [Test]
        public void TestOuterSetting()
        {
            var column = new VoxelMapColumn();
            column.InitFields();
            column.SetFieldType(1, 1000, 500);
            column.SetFieldType(1, 1500, 400);

            Assert.That(column.GetFieldType(2000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(1500), Is.EqualTo(1));
            Assert.That(column.GetFieldType(1000), Is.EqualTo(1));
            Assert.That(column.GetFieldType(500), Is.EqualTo(1));
            Assert.That(column.GetFieldType(400), Is.EqualTo(0));
            Assert.That(column.GetFieldType(200), Is.EqualTo(0));
            Assert.That(column.GetFieldType(0), Is.EqualTo(0));

            column.SetFieldType(2, 2000, 300);

            Assert.That(column.GetFieldType(2300), Is.EqualTo(0));
            Assert.That(column.GetFieldType(2000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(1500), Is.EqualTo(2));
            Assert.That(column.GetFieldType(1000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(500), Is.EqualTo(2));
            Assert.That(column.GetFieldType(400), Is.EqualTo(2));
            Assert.That(column.GetFieldType(200), Is.EqualTo(0));
            Assert.That(column.GetFieldType(0), Is.EqualTo(0));
        }

        [Test]
        public void TestRepeatingSetting()
        {
            var column = new VoxelMapColumn();
            column.InitFields();

            column.SetFieldType(2, 1000, 500);
            column.SetFieldType(2, 1000, 500);

            Assert.That(column.GetFieldType(2000), Is.EqualTo(0));
            Assert.That(column.GetFieldType(1500), Is.EqualTo(0));
            Assert.That(column.GetFieldType(1000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(800), Is.EqualTo(2));
            Assert.That(column.GetFieldType(600), Is.EqualTo(2));
            Assert.That(column.GetFieldType(500), Is.EqualTo(0));
            Assert.That(column.GetFieldType(400), Is.EqualTo(0));
        }

        [Test]
        public void TestAttachingOfSameFieldTypes()
        {
            var column = new VoxelMapColumn();
            column.InitFields();

            column.SetFieldType(2, 1000, 500);
            column.SetFieldType(2, 500, 100);

            Assert.That(column.GetFieldType(1000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(800), Is.EqualTo(2));
            Assert.That(column.GetFieldType(600), Is.EqualTo(2));
            Assert.That(column.GetFieldType(500), Is.EqualTo(2));
            Assert.That(column.GetFieldType(400), Is.EqualTo(2));
            Assert.That(column.GetFieldType(0), Is.EqualTo(0));

            // Air -> 2 -> Air
            Assert.That(column.Changes.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestAttachingOfDifferentFieldTypes()
        {
            var column = new VoxelMapColumn();
            column.InitFields();

            column.SetFieldType(2, 1000, 500);
            column.SetFieldType(1, 500, 100);

            Assert.That(column.GetFieldType(1000), Is.EqualTo(2));
            Assert.That(column.GetFieldType(800), Is.EqualTo(2));
            Assert.That(column.GetFieldType(600), Is.EqualTo(2));
            Assert.That(column.GetFieldType(500), Is.EqualTo(1));
            Assert.That(column.GetFieldType(400), Is.EqualTo(1));
            Assert.That(column.GetFieldType(0), Is.EqualTo(0));

            // Air -> 2 -> 1 -> Air
            Assert.That(column.Changes.Count, Is.EqualTo(4));
        }
    }
}
