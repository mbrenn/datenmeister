using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Interfaces;
using System.Xml.Linq;
using System.Globalization;
using NUnit.Framework;
using BurnSystems.Collections;

namespace BurnSystems.UnitTests.Collections
{
    /// <summary>
    /// Stores all tests that have been created for the xmllist
    /// </summary>
    [TestFixture]
    public class XmlListTests
    {
        /// <summary>
        /// Stores the testentity
        /// </summary>
        public class Entity
        {
            public string Name
            {
                get;
                set;
            }

            public string Prename
            {
                get;
                set;
            }

            public int Age
            {
                get;
                set;
            }

            public override bool Equals(object obj)
            {
                var entity = obj as Entity;
                if (entity == null)
                {
                    return false;
                }

                return entity.Name == this.Name
                    && entity.Prename == this.Prename
                    && entity.Age == this.Age;
            }

            public override int GetHashCode()
            {
                return
                    this.Name.GetHashCode() ^
                    this.Prename.GetHashCode() ^
                    this.Age.GetHashCode();
            }
        }

        public class EntityConverter : IXElementConverter<Entity>
        {
            public Entity Convert(System.Xml.Linq.XElement element)
            {
                var entity = new Entity()
                {
                    Name = element.Element("name").Value,
                    Prename = element.Element("prename").Value,
                    Age = System.Convert.ToInt32(element.Element("age").Value, CultureInfo.InvariantCulture)
                };

                return entity;
            }

            public System.Xml.Linq.XElement Convert(Entity entity)
            {
                return
                    new XElement(
                        "entity",
                        new XElement("name", entity.Name),
                        new XElement("prename", entity.Prename),
                        new XElement("age", entity.Age.ToString(CultureInfo.InvariantCulture)));
            }
        }

        public static XmlList<Entity> EmptyRootElement
        {
            get
            {
                var document = new XDocument(
                    new XElement("elements"));

                return new XmlList<Entity>(
                    document.Element("elements"),
                    new EntityConverter());
            }
        }

        public static XElement AttributeListDocument
        {
            get
            {
                var document = new XDocument(
                    new XElement("elements",
                        new XElement(
                            "item",
                            new XAttribute("value", 12)),
                        new XElement(
                            "item",
                            new XAttribute("value", 25)),
                        new XElement(
                            "item",
                            new XAttribute("value", 68))));

                return document.Element("elements");
            }
        }

        public static XElement ElementListDocument
        {
            get
            {
                var document = new XDocument(
                    new XElement("elements",
                        new XElement(
                            "item",
                            12),
                        new XElement(
                            "item",
                            25),
                        new XElement(
                            "item",
                            68)));

                return document.Element("elements");
            }
        }

        public Entity Person1
        {
            get
            {
                return new Entity()
                {
                    Name = "John",
                    Prename = "Miller",
                    Age = 23
                };
            }
        }

        public Entity Person2
        {
            get
            {
                return new Entity()
                {
                    Name = "Mike",
                    Prename = "Wayne",
                    Age = 25
                };
            }
        }

        public Entity Person3
        {
            get
            {
                return new Entity()
                {
                    Name = "Barack",
                    Prename = "Bush",
                    Age = 50
                };
            }
        }

        [Test]
        public void CreateEmptyList()
        {
            var list = EmptyRootElement;

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddAndRetrieveItems()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo(Person1));
            Assert.That(list[1], Is.EqualTo(Person2));

            list.Add(this.Person3);

            Assert.That(list.Count, Is.EqualTo(3));
        }

        [Test]
        public void AddAndRemoveItems()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);
            list.Add(this.Person3);

            Assert.That(list.Count, Is.EqualTo(3));

            list.RemoveAt(1);

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo(Person1));
            Assert.That(list[1], Is.EqualTo(Person3));
        }

        [Test]
        public void InsertItems()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person3);
            list.Insert(1, this.Person2);

            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(Person1));
            Assert.That(list[1], Is.EqualTo(Person2));
            Assert.That(list[2], Is.EqualTo(Person3));
        }

        [Test]
        public void Remove()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);
            list.Add(this.Person3);

            Assert.That(list.Remove(this.Person2), Is.True);
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo(Person1));
            Assert.That(list[1], Is.EqualTo(Person3));

            list.Remove(this.Person1);
            list.Remove(this.Person3);

            Assert.That(list.Count, Is.EqualTo(0));

            Assert.That(list.Remove(this.Person2), Is.False);
        }

        [Test]
        public void IndexOf()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);
            list.Add(this.Person3);

            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list.IndexOf(this.Person1), Is.EqualTo(0));
            Assert.That(list.IndexOf(this.Person2), Is.EqualTo(1));
            Assert.That(list.IndexOf(this.Person3), Is.EqualTo(2));

            list.Remove(this.Person2);
            Assert.That(list.IndexOf(this.Person2), Is.EqualTo(-1));
        }

        [Test]
        public void Setter()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list.IndexOf(this.Person1), Is.EqualTo(0));
            Assert.That(list.IndexOf(this.Person2), Is.EqualTo(1));

            list[1] = this.Person3;

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[1], Is.EqualTo(Person3));
        }

        [Test]
        public void Clear()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);

            Assert.That(list.Count, Is.EqualTo(2));

            list.Clear();

            Assert.That(list.Count, Is.EqualTo(0));
            Assert.That(list.IndexOf(this.Person2), Is.EqualTo(-1));
        }

        [Test]
        public void IsReadOnly()
        {
            var list = EmptyRootElement;

            Assert.That(list.IsReadOnly, Is.False);
        }

        [Test]
        public void Enumerator()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);

            var count = 0;
            foreach (var element in list)
            {
                count++;

                Assert.That(element.Equals(this.Person1) || element.Equals(this.Person2), Is.True);
                Assert.That(element.Equals(this.Person3), Is.False);
            }

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void LinqTests()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);
            list.Add(this.Person3);

            Assert.That(list.Any(x => x.Age == 25), Is.True);
            Assert.That(list.Any(x => x.Age == 99), Is.False);

            Assert.That(list.All(x => x.Age > 10), Is.True);

            Assert.That(list.Select(x => x.Name + " " + x.Prename).Count(), Is.EqualTo(3));
        }

        [Test]
        public void CopyToTests()
        {
            var list = EmptyRootElement;

            list.Add(this.Person1);
            list.Add(this.Person2);
            list.Add(this.Person3);

            var array = new Entity[10];

            list.CopyTo(array, 1);

            Assert.That(array[1], Is.EqualTo(this.Person1));
            Assert.That(array[2], Is.EqualTo(this.Person2));
            Assert.That(array[3], Is.EqualTo(this.Person3));

            list.CopyTo(array, 3);
            Assert.That(array[3], Is.EqualTo(this.Person1));
            Assert.That(array[4], Is.EqualTo(this.Person2));
            Assert.That(array[5], Is.EqualTo(this.Person3));
        }

        /// <summary>
        /// Tests whether the list for attributes is working
        /// </summary>
        [Test]
        public void TestListForAttributes()
        {
            var element = AttributeListDocument;
            var values = XmlList<int>.GetListForAttributes(element, "item", "value");

            Assert.That(values.Sum(), Is.EqualTo(105));
            Assert.That(values.Count, Is.EqualTo(3));

            values.Add(10);
            Assert.That(values.Sum(), Is.EqualTo(115));
            Assert.That(values.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Tests whether the list for attributes is working
        /// </summary>
        [Test]
        public void TestListForElements()
        {
            var element = ElementListDocument;
            var values = XmlList<int>.GetListForElements(element, "item");

            Assert.That(values.Sum(), Is.EqualTo(105));
            Assert.That(values.Count, Is.EqualTo(3));

            values.Add(10);
            Assert.That(values.Sum(), Is.EqualTo(115));
            Assert.That(values.Count, Is.EqualTo(4));
        }
    }
}
