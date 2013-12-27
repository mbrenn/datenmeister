using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.WebServer;
using BurnSystems.ObjectActivation;
using BurnSystems.Logging;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Responses.Tests;
using BurnSystems.WebServer.Dispatcher.Test;
using BurnSystems.WebServer.UnitTests.Controller;
using BurnSystems.WebServer.Modules.MVC;
using BurnSystems.WebServer.Responses;
using BurnSystems.WebServer.Modules.UserManagement.InMemory;
using BurnSystems.WebServer.Modules.UserManagement;

namespace SimpleTestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.TheLog.FilterLevel = LogLevel.Verbose;
            Log.TheLog.AddLogProvider(new ConsoleProvider());

            // Container storing all the plugins, activities and filters
            var activationContainer = new ActivationContainer("Server");

            // In Memory-UserManagement
            var userStorage = new UserStorage();
            var karl = new User(1, "Karl", "Heinz");
            userStorage.Users.Add(karl);
            userStorage.Users.Add(new User(2, "Wilhelm", "Otto"));
            activationContainer.Bind<UserStorage>().ToConstant(userStorage);
            activationContainer.Bind<IWebUserManagement>().To<UserManagement>();
            activationContainer.Bind<IAuthentication>().To<Authentication>();

            // Creates server
            var server = Server.CreateDefaultServer(activationContainer);
            server.AddPrefix("http://127.0.0.1:8081/");
            server.AddPrefix("http://localhost:8081/");

            server.Add(new ControllerDispatcher<CalcController>(DispatchFilter.ByUrl("/controller/Calculator/"), "/controller/Calculator/"));
            server.Add(new ControllerDispatcher<UserManagementController>(DispatchFilter.ByUrl("/controller/Users/"), "/controller/Users/"));
            server.Add(new ControllerDispatcher<JsonPostController>(DispatchFilter.ByUrl("/controller/jsontest/"), "/controller/jsontest/"));
            server.Add(new ControllerDispatcher<TestController>(DispatchFilter.ByUrl("/controller/"), "/controller/"));
            server.Add(new ControllerDispatcher<PostController>(DispatchFilter.ByUrl("/postcontroller/"), "/postcontroller/"));
            server.Add(new RelocationDispatcher("/", "/test.html"));
            //server.Add(new StaticContentResponse(DispatchFilter.ByExactUrl("/js/jquery.js"), "text/javascript", Encoding.UTF8.GetBytes(Files.JQuery)));
 
            server.Add(new ExceptionDispatcher(DispatchFilter.ByUrl("/exception/")));
            server.Add(new FileSystemDispatcher(DispatchFilter.All, "htdocs\\"));

            // Web Auth
            var webAuth = new WebAuthorisation();
            webAuth.RestrictTo(DispatchFilter.ByUrl("/controller/Calculator/"), karl.Token);
            activationContainer.Bind<IRequestFilter>().ToConstant(webAuth);

            server.Start();            
            
            Console.WriteLine("Press key to stop server");
            Console.ReadKey();
            server.Stop();

            Console.WriteLine("Server stopped");
        }
    }
}
