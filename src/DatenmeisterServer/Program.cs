using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer;
using BurnSystems.WebServer.Filters;
using BurnSystems.WebServer.Modules.MVC;
using DatenMeister;
using DatenMeister.DataProvider.CSV;
using DatenMeister.DataProvider.Views;
using DatenMeister.Logic;
using DatenMeister.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.DataProvider.Xml;

namespace DatenmeisterServer
{
    public class Program
    {
        public static void Main (string[] args)
		{
			Log.TheLog.AddLogProvider (new ConsoleProvider (false));
			Log.TheLog.AddLogProvider (new FileProvider ("log.txt"));
			Log.TheLog.FilterLevel = LogLevel.Verbose;

			// Starting up
			Log.TheLog.LogEntry (LogLevel.Message, "Datenmeister is starting up");
			
			// Checks, if data directory exists
			if (!Directory.Exists ("data")) {
				Directory.CreateDirectory ("data");
			}

			var activationContainer = new ActivationContainer ("Website");

			// Initialize DatenMeisterPool
			var pool = new DatenMeisterPool ();

			// Adds pool
			var poolExtent = new DatenMeisterPoolExtent (pool);
			pool.Add (poolExtent, null);

			// Add view pool
			var viewExtent = new ViewsExtent ("datenmeister:///defaultviews/");
			viewExtent.Fill ();
			pool.Add (viewExtent, null);
			
			var poolProvider = new DatenMeisterPoolDataProvider ();
			poolProvider.Load (pool, "data/pools.xml");

			// Adds the csv-extent
			/*
			var provider = new CSVDataProvider ();
			var csvSettings = new CSVSettings ()
                {
                    HasHeader = true
                };
			IURIExtent extent;
			if (File.Exists ("data/test_save.csv")) {
				extent = provider.Load ("data/test_save.csv", csvSettings);
			} else {
				extent = provider.Load ("data/test.csv", csvSettings);
			}

			pool.Add (extent, "data/test_save.csv");*/
			activationContainer.Bind<DatenMeisterPool> ().ToConstant (pool);
			activationContainer.Bind<XmlDataProvider> ().To<XmlDataProvider> ();

			using (var server = Server.CreateDefaultServer(activationContainer)) {
				activationContainer.Bind<IRequestFilter> ().ToConstant (new AddCorsHeaderFilter ());

				server.Add (new ControllerDispatcher<ExtentController> (
					BurnSystems.WebServer.Dispatcher.DispatchFilter.ByUrl ("/extent/"),
					"/extent/"
				)
				);

				server.AddPrefix ("http://127.0.0.1:8081/");
				server.AddPrefix ("http://*:8081/");

				server.Start ();
				Console.WriteLine ("Press Key to stop server");
				Console.ReadKey ();

				server.Stop ();
			}

			// Storing the data
			Log.TheLog.Message ("Storing data");
			poolProvider.Save (pool, "pool.xml");
		}
	}
}