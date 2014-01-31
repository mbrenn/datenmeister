using System;
using System.Linq;
using NUnit.Framework;
using DatenMeister.DataProvider.Xml;
using System.IO;
using DatenMeister.Logic;

namespace DatenMeister.Tests.PoolLogic
{
	[TestFixtureAttribute]
	public class PoolProviderTests
	{
		public static void PrepareDirectory ()
		{
			if (!Directory.Exists ("data")) {
				Directory.Delete ("data", true);

			}
			
			Directory.CreateDirectory ("data");
		}
		
		public PoolProviderTests ()
		{
		}
		
		[Test]
		public void DoStoreAndLoad ()
		{
			PrepareDirectory ();
			
			var pool = new DatenMeisterPool ();
			
			var xmlDataProvider = new XmlDataProvider ();
			var extent1 = xmlDataProvider.CreateEmpty (
				"data/empty1.xml",
				"http://test",
				"MyName"
			);
			var extent2 = xmlDataProvider.CreateEmpty (
				"data/empty2.xml",
				"http://test2",
				"MyName2"
			);
			
			pool.Add (extent1);
			pool.Add (extent2);
			
			var poolProvider = new DatenMeisterPoolProvider ();
			
			// Saving is now done
			poolProvider.Save (pool, "data/pools.xml");
			
			// Try to read
			var poolProviderLoad = new DatenMeisterPoolProvider ();
			var loadPool = new DatenMeisterPool ();
			poolProviderLoad.Load (loadPool, "data/pools.xml");
			
			var first = loadPool.Instances.Where (x => x.Name == "MyName").FirstOrDefault ();
			var second = loadPool.Instances.Where (x => x.Name == "MyName2").FirstOrDefault ();
			
			Assert.That (first, Is.Not.Null);
			Assert.That (second, Is.Not.Null);
			
			Assert.That (first.Path == "data/empty1.xml");
			Assert.That (second.Path == "data/empty2.xml");
			
			Assert.That (first.Extent, Is.Not.Null);
			Assert.That (second.Extent, Is.Not.Null);
			
			Assert.That (first.Extent, Is.TypeOf (typeof(XmlExtent)));
		}
	}
}

