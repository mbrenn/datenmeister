using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.WebServer.Modules.UserManagement;

namespace BurnSystems.WebServer.UnitTests.UserManagement
{
    [TestFixture]
    public class TokenTests
    {
        /// <summary>
        /// Checks, if the subset function is working
        /// </summary>
        [Test]
        public void CheckSubset()
        {
            var tokens = new List<Token>();

            for (var n = 0; n < 10; n++)
            {
                var token = new Token()
                {
                    Id = Guid.NewGuid(),
                    Name = "Token " + n
                };

                tokens.Add(token);
            }

            var tokenSet1 = new TokenSet();
            tokenSet1.Add(tokens[0]);
            tokenSet1.Add(tokens[1]);
            tokenSet1.Add(tokens[2]);

            var tokenSet2 = new TokenSet();
            tokenSet2.Add(tokens[0]);
            tokenSet2.Add(tokens[1]);
            tokenSet2.Add(tokens[2]);
            tokenSet2.Add(tokens[3]);
            tokenSet2.Add(tokens[4]);

            var tokenSet3 = new TokenSet();
            tokenSet3.Add(tokens[0]);
            tokenSet3.Add(tokens[1]);
            tokenSet3.Add(tokens[3]);

            var tokenSet4 = new TokenSet();
            tokenSet4.Add(tokens[1]);
            tokenSet4.Add(tokens[2]);
            tokenSet4.Add(tokens[3]);

            Assert.That(TokenSet.IsSubsetOf(tokenSet1, tokenSet2), Is.True);
            Assert.That(TokenSet.IsSubsetOf(tokenSet2, tokenSet1), Is.False);
            Assert.That(TokenSet.IsSubsetOf(tokenSet3, tokenSet2), Is.True);
            Assert.That(TokenSet.IsSubsetOf(tokenSet4, tokenSet2), Is.True);

            Assert.That(TokenSet.IsSubsetOf(tokenSet1, tokenSet3), Is.False);
            Assert.That(TokenSet.IsSubsetOf(tokenSet3, tokenSet1), Is.False);
            
            Assert.That(TokenSet.IsSubsetOf(tokenSet1, tokenSet4), Is.False);
            Assert.That(TokenSet.IsSubsetOf(tokenSet4, tokenSet1), Is.False);

            Assert.That(TokenSet.IsSubsetOf(tokenSet3, tokenSet4), Is.False);
            Assert.That(TokenSet.IsSubsetOf(tokenSet4, tokenSet3), Is.False);
        }
    }
}
