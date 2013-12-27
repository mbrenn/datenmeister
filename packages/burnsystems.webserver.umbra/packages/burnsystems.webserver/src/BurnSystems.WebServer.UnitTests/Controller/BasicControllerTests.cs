using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net;

namespace BurnSystems.WebServer.UnitTests.Controller
{
    [TestFixture]
    public class BasicControllerTests
    {
        [Test]
        public void TestControllerWithoutArguments()
        {
            using (var server = ServerTests.CreateServer())
            {
                var webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var data = webClient.DownloadString("http://localhost:8081/controller/Test");

                // Shall not get reached
                Assert.That(data.Trim(), Is.EqualTo("Test"));
            }
        }

        [Test]
        public void TestControllerWithOneStringArgument()
        {
            using (var server = ServerTests.CreateServer())
            {
                var webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var data = webClient.DownloadString("http://localhost:8081/controller/Greet?name=Dummy");

                // Shall not get reached
                Assert.That(data.Trim(), Is.EqualTo("Hello Dummy"));
            }
        }

        [Test]
        public void TestControllerWithTwoIntArguments()
        {
            using (var server = ServerTests.CreateServer())
            {
                var webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var data = webClient.DownloadString("http://localhost:8081/controller/Add?a=4&b=10");

                // Shall not get reached
                Assert.That(data.Trim(), Is.EqualTo("4 + 10 = 14"));
            }
        }

        [Test]
        public void TestControllerWithTwoIntOneNegativeArguments()
        {
            using (var server = ServerTests.CreateServer())
            {
                var webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var data = webClient.DownloadString("http://localhost:8081/controller/Add?a=-4&b=10");

                // Shall not get reached
                Assert.That(data.Trim(), Is.EqualTo("-4 + 10 = 6"));
            }
        }

        [Test]
        public void TestControllerWithNamedController()
        {
            using (var server = ServerTests.CreateServer())
            {
                var webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var data = webClient.DownloadString("http://localhost:8081/controller/Subtract?a=20&b=10");

                // Shall not get reached
                Assert.That(data.Trim(), Is.EqualTo("20 - 10 = 10"));
            }
        }
    }
}
