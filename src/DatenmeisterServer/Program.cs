﻿using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenmeisterServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.TheLog.AddLogProvider(new ConsoleProvider(false));
            Log.TheLog.AddLogProvider(new FileProvider("log.txt"));
            Log.TheLog.FilterLevel = LogLevel.Verbose;

            // Starting up
            Log.TheLog.LogEntry(LogLevel.Message, "Datenmeister");

            var activationContainer = new ActivationContainer("Website");

            using (var server = Server.CreateDefaultServer(activationContainer))
            {
                server.AddPrefix("http://127.0.0.1:8081/");

                server.Start();
                Console.WriteLine("Press Key to stop server");
                Console.ReadKey();

                server.Stop();
            }
        }
    }
}
