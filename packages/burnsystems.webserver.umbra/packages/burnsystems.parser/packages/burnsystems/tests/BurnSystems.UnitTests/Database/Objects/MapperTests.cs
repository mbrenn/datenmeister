using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using BurnSystems.AdoNet.Queries;
using BurnSystems.Database.Objects;
using NUnit.Framework;
using System.Reflection;

namespace BurnSystems.UnitTests.Database.Objects
{
    [TestFixture]
    public class MapperTests
    {
        /// <summary>
        /// Stores a value indicating that mysql shall be used
        /// </summary>
        private static bool useMySql = true;

        public void ExecuteDatabaseAction(Action<DbConnection> action)
        {
            DbConnection dbConnection = null;

            // Gets the database connection
            if (EnvironmentHelper.IsMono || useMySql)
            {
				Assembly assembly = null;
				try
				{
                	assembly = Assembly.Load("MySql.Data, Version=6.3.6.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d");
				}
				catch { }
				
                if (assembly == null)
                {
					try
					{
						assembly = Assembly.Load("MySql.Data, Version=6.1.3.0, Culture=neutral, PublicKeyToken=20449f9ba87f7ae2");
					}
					catch { }
					
					if (assembly == null)
					{
                    	Assert.Inconclusive("No MySql.Data.dll installed");
					}
                }

                var mysqlType = assembly.GetType("MySql.Data.MySqlClient.MySqlConnection");
                if (mysqlType == null)
                {
                    Assert.Inconclusive("MySql-Type not found");
                }

                var mySqlConstructor = mysqlType.GetConstructor(new Type[] { typeof(string) });
                dbConnection = mySqlConstructor.Invoke(new object[] { "Server=127.0.0.1;Database=unittest;Uid=unittest;Pwd=unittest" })
                    as DbConnection;

                dbConnection.Open();
            }
            else
            {
                dbConnection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=mb_test;Trusted_Connection=True;");
                dbConnection.Open();
            }

            Assert.That(dbConnection, Is.Not.Null);

            var query = Resources_UnitTest.install;
            var createQuery = new FreeQuery(query);
            dbConnection.ExecuteNonQuery(createQuery);

            try
            {
                // Do what has to be done
                action(dbConnection);
            }
            finally
            {
                // Cleans the database 
                var deleteQuery = new FreeQuery("DROP TABLE persons");
                dbConnection.ExecuteNonQuery(deleteQuery);
            }
        }

        [Test]
        public void TestDatabaseConnection()
        {
            this.ExecuteDatabaseAction((dbConnection) => {});
        }

        [Test]
        public void TestInsertPerson()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);

                Person person;
                Person person2;
                Person person3;
                InsertPersons(mapper, out person, out person2, out person3);

                Assert.That(person.Id, Is.Not.EqualTo(0));
                Assert.That(person2.Id, Is.Not.EqualTo(0));
                Assert.That(person3.Id, Is.Not.EqualTo(0));
                Assert.That(person.Id, Is.Not.EqualTo(person2.Id));
                Assert.That(person2.Id, Is.Not.EqualTo(person3.Id));
                Assert.That(person.Id, Is.Not.EqualTo(person3.Id));
            });
        }

        [Test]
        public void TestInsertPersonWithNullValues()
        {
            this.ExecuteDatabaseAction((sqlConnection) => 
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);

                var person = new Person();
                person.Prename = "Karl";
                person.Name = null;
                person.Age_Temp = 12;
                person.Weight = 12.34;
                person.Sex = Sex.Male;
                person.Obsolete = "ABC";
                person.Marriage = new DateTime(1998, 9, 11, 3, 4, 5);

                mapper.Add(person);

                var persons = mapper.GetAll();
                Assert.That(persons.Length, Is.EqualTo(1));
                Assert.That(persons[0].Name, Is.Null);
            });
        }

        [Test]
        public void TestSelectPerson()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                Assert.That(persons.Length, Is.EqualTo(3));
                Assert.That(persons[0].Id, Is.Not.EqualTo(0));
                Assert.That(persons[0].Id, Is.Not.EqualTo(persons[1].Id));
                Assert.That(persons[1].Id, Is.Not.EqualTo(persons[2].Id));
                Assert.That(persons[0].Id, Is.Not.EqualTo(persons[2].Id));

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                Assert.That(karl.Name, Is.EqualTo("Heinz"));
                Assert.That(karl.Age_Temp, Is.EqualTo(12));
                Assert.That(karl.Weight, Is.EqualTo(12.34).Within(0.005));
                Assert.That(karl.Sex, Is.EqualTo(Sex.Male));
                Assert.That(karl.Marriage, Is.EqualTo(new DateTime(1998, 9, 11, 3, 4, 5)));
            });
        }

        [Test]
        public void TestSelectPersonByWhere()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var where = new Dictionary<string, object>();
                where["Prename"] = "Karl";
                var persons = mapper.Get(where);

                Assert.That(persons.Length, Is.EqualTo(1));

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                Assert.That(karl.Name, Is.EqualTo("Heinz"));
                Assert.That(karl.Age_Temp, Is.EqualTo(12));
                Assert.That(karl.Weight, Is.EqualTo(12.34).Within(0.005));
                Assert.That(karl.Sex, Is.EqualTo(Sex.Male));
            });
        }

        [Test]
        public void TestSelectPersonById()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                var karl2 = mapper.Get(karl.Id);

                Assert.That(karl2.Name, Is.EqualTo(karl.Name));
                Assert.That(karl2.Age_Temp, Is.EqualTo(karl.Age_Temp));
                Assert.That(karl2.Weight, Is.EqualTo(karl.Weight));
                Assert.That(karl2.Sex, Is.EqualTo(karl.Sex));
            });
        }

        [Test]
        public void TestSelectPersonByWrongId()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                Person p1;
                Person p2;
                Person p3;
                InsertPersons(mapper, out p1, out p2, out p3);

                var karl = mapper.Get(p1.Id + p2.Id + p3.Id);
                Assert.That(karl, Is.Null);
            });
        }

        [Test]
        public void TestDeletePersonByInstance()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                mapper.Delete(karl);

                persons = mapper.GetAll();
                karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Null);
            });
        }

        [Test]
        public void TestDeletePersonById()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                mapper.Delete(karl.Id);

                persons = mapper.GetAll();
                karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Null);
            });
        }

        [Test]
        public void TestUpdatePerson()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);
                Assert.That(karl.Name, Is.EqualTo("Heinz"));

                karl.Name = "Mommenschatz";
                mapper.Update(karl);

                persons = mapper.GetAll();
                karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);
                Assert.That(karl.Name, Is.EqualTo("Mommenschatz"));
            });
        }

        [Test]
        public void TestUpdatePersonWithNullValues()
        {
            this.ExecuteDatabaseAction((sqlConnection) =>
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);
                Assert.That(karl.Name, Is.EqualTo("Heinz"));

                karl.Name = null;
                mapper.Update(karl);

                persons = mapper.GetAll();
                karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);
                Assert.That(karl.Name, Is.Null);
            });
        }

        /// <summary>
        /// Inserts three persons into database
        /// </summary>
        /// <param name="mapper">Mapper to be used</param>
        private static void InsertPersons(Mapper<Person> mapper)
        {
            Person p1;
            Person p2;
            Person p3;
            InsertPersons(mapper, out p1, out p2, out p3);
        }

        /// <summary>
        /// Inserts three persons into database
        /// </summary>
        /// <param name="mapper">Mapper to be used</param>
        /// <param name="person">First person that has been added</param>
        /// <param name="person2">Second person that has been added</param>
        /// <param name="person3">Third person that has been added</param>
        private static void InsertPersons(Mapper<Person> mapper, out Person person, out Person person2, out Person person3)
        {
            person = new Person();
            person.Prename = "Karl";
            person.Name = "Heinz";
            person.Age_Temp = 12;
            person.Weight = 12.34;
            person.Sex = Sex.Male;
            person.Obsolete = "ABC";
            person.Marriage = new DateTime(1998, 9, 11, 3, 4, 5);

            mapper.Add(person);

            person2 = new Person();
            person2.Prename = "Otto";
            person2.Name = "Meier";
            person2.Age_Temp = 34;
            person2.Weight = 56.78;
            person2.Sex = Sex.Male;
            person2.Obsolete = "DEF";
            person2.Marriage = new DateTime(1999, 10, 12, 2, 3, 4);

            mapper.Add(person2);

            person3 = new Person();
            person3.Prename = "Gertrud";
            person3.Name = "Müller";
            person3.Age_Temp = 56;
            person3.Weight = 90.12;
            person3.Sex = Sex.Female;
            person3.Obsolete = "ABC";
            person3.Marriage = new DateTime(2000, 11, 13, 1, 2, 3);

            mapper.Add(person3);
        }
    }
}
