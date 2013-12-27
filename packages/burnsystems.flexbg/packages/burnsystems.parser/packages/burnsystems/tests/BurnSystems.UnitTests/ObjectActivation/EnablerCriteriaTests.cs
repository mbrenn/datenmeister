
using System;
using BurnSystems.ObjectActivation.Criteria;
using BurnSystems.ObjectActivation.Enabler;
using NUnit.Framework;

namespace BurnSystems.UnitTests.ObjectActivation
{
	[TestFixture]
	public class EnablerCriteriaTests
	{
		[Test]
		public void TestByName()
		{
			var byName = new ByNameCriteria("abc");
			
			Assert.That(byName.DoesMatch(new ByNameEnabler("def")), Is.False);
			Assert.That(byName.DoesMatch(new ByNameEnabler("abc")), Is.True);
			Assert.That(byName.DoesMatch(new ByTypeEnabler(typeof(string))), Is.False);
		}
		
		[Test]
		public void TestByType()
		{
			var byType = new ByTypeCriteria(typeof(string));
			
			Assert.That(byType.DoesMatch(new ByNameEnabler("def")), Is.False);
			Assert.That(byType.DoesMatch(new ByNameEnabler("abc")), Is.False);
			Assert.That(byType.DoesMatch(new ByTypeEnabler(typeof(string))), Is.True);
			Assert.That(byType.DoesMatch(new ByTypeEnabler(typeof(long))), Is.False);
		}
	}
}
