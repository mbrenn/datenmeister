using DatenMeister;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Entities.FieldInfos;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.Tests
{
    /// <summary>
    /// Stores the datenmeister instance and all its properties
    /// </summary>
    public class TestDatabase
    {
        public const int TotalElements = 3;
        public const int TotalPersons = 2;
        public const int TotalTasks = 1;

        /// <summary>
        /// Stores the uri for new instances
        /// </summary>
        public const string uri = "datenmeister:///projektmeister/data";

        /// <summary>
        /// Stores the uri for the types and other general information
        /// </summary>
        private const string typeUri = "datenmeister:///projektmeister/types";

        /// <summary>
        /// Stores the uri for the views
        /// </summary>
        private const string viewUri = "datenmeister:///projektmeister/views";

        /// <summary>
        /// Stores the datenmeister pool
        /// </summary>
        private DatenMeisterPool pool;

        /// <summary>
        /// Stores the xmlsettings being used for 
        /// </summary>
        private XmlSettings xmlSettings;

        /// <summary>
        /// The extent, which is used by the ProjektMeister
        /// </summary>
        private IURIExtent projectExtent;

        /// <summary>
        /// The extent, which is used by the ProjektMeister
        /// </summary>
        private IURIExtent typeExtent;

        public IURIExtent ProjectExtent
        {
            get { return this.projectExtent; }
        }

        /// <summary>
        /// Gets the settings of the xml file
        /// </summary>
        public XmlSettings Settings
        {
            get { return this.xmlSettings; }
        }

        /// <summary>
        /// Initializes a new instance of the the database
        /// </summary>
        public DatenMeisterPool Init()
        {
            ApplicationCore.PerformBinding();
            this.pool = DatenMeisterPool.Create();

            this.InitTypes();
            this.InitDatabase();
            this.InitInstances();

            return this.pool;
        }

        private void InitDatabase()
        {
            var dataDocument = new XDocument(new XElement("data"));
            var xmlProjectExtent = new XmlExtent(dataDocument, uri);
            this.xmlSettings = new XmlSettings();
            this.xmlSettings.OnlyUseAssignedNodes = true;
            this.xmlSettings.Mapping.Add("person", TestDatabase.Types.Person, (x) => x.Elements("data").Elements("persons").First());
            this.xmlSettings.Mapping.Add("task", TestDatabase.Types.Task, (x) => x.Elements("data").Elements("tasks").First());

            xmlProjectExtent.Settings = xmlSettings;

            this.pool.Add(xmlProjectExtent, null, "ProjektMeister", Logic.ExtentType.Data);

            var xmlPersons = new XElement("persons");
            var xmlTasks = new XElement("tasks");

            dataDocument.Root.Add(xmlPersons);
            dataDocument.Root.Add(xmlTasks);

            this.projectExtent = xmlProjectExtent;
        }

        private void InitTypes()
        {
            var typeDocument = new XDocument(new XElement("types"));
            this.typeExtent = new XmlExtent(typeDocument, typeUri);
            this.pool.Add(this.typeExtent, null, "ProjektMeister Types", Logic.ExtentType.Type);

            // Creates the types
            Types.Person = Factory.GetFor(this.typeExtent).CreateInExtent(this.typeExtent);
            var person = new DatenMeister.Entities.AsObject.Uml.Type(TestDatabase.Types.Person);
            person.setName("Person");

            Types.Task = Factory.GetFor(this.typeExtent).CreateInExtent(this.typeExtent);
            var task = new DatenMeister.Entities.AsObject.Uml.Type(TestDatabase.Types.Task);
            task.setName("Task");
        }

        private void InitInstances()
        {           
            // Create some persons
            var person = Factory.GetFor(this.ProjectExtent).CreateInExtent(this.ProjectExtent, TestDatabase.Types.Person);
            person.set("name", "Martin Brenn");
            person.set("email", "brenn@depon.net");
            person.set("phone", "0151/560");
            person.set("title", "Project Lead");
            person.set("isFemale", false);

            person = Factory.GetFor(this.ProjectExtent).CreateInExtent(this.ProjectExtent, TestDatabase.Types.Person);
            person.set("name", "Martina Brenn");
            person.set("email", "brenna@depon.net");
            person.set("phone", "0151/650");
            person.set("title", "Project Support");
            person.set("isFemale", true);

            var task = Factory.GetFor(this.ProjectExtent).CreateInExtent(this.ProjectExtent, TestDatabase.Types.Task);
            task.set("name", "My First Task");
            task.set("startdate", DateTime.Now);
            task.set("enddate", DateTime.Now.AddYears(1));
            task.set("finished", false);
        }

        /// <summary>
        /// Stores the types for persons and tasks
        /// </summary>
        public static class Types
        {
            public static IObject Person
            {
                get;
                internal set;
            }

            public static IObject Task
            {
                get;
                internal set;
            }
        }

        public static class Views
        {
            public static IObject PersonTable
            {
                get;
                internal set;
            }

            public static IObject TaskTable
            {
                get;
                internal set;
            }

            public static IObject PersonDetail
            {
                get;
                internal set;
            }

            public static IObject TaskDetail
            {
                get;
                internal set;
            }
        }

        public class Person
        {
            public string FirstName
            {
                get;
                set;
            }

            public string LastName
            {
                get;
                set;
            }

            public int Age
            {
                get;
                set;
            }

            private int PrivateVariable
            {
                get;
                set;
            }

            public static int StaticVariable
            {
                get;
                set;
            }
        }
    }
}
