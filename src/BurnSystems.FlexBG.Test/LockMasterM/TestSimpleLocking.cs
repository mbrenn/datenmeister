using BurnSystems.FlexBG.Modules.DeponNet;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.FlexBG.Modules.LockMasterM.Simple;
using BurnSystems.ObjectActivation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.LockMasterM
{
    [TestFixture]
    public class TestSimpleLocking
    {
        public ILockMaster Init()
        {
            var container = new ActivationContainer("Test");
            container.Bind<ILockMaster>().To<SimpleLockMaster>().AsSingleton();

            var lockMaster = container.Get<ILockMaster>();
            lockMaster.AddRelationShip(EntityType.Server, EntityType.Game);
            lockMaster.AddRelationShip(EntityType.Game, EntityType.Player);
            lockMaster.AddRelationShip(EntityType.Player, EntityType.Town);
            lockMaster.AddRelationShip(EntityType.Player, EntityType.Unit);
            return lockMaster;
        }

        [Test]
        public void TestStupidReadLocking()
        {
            var lockMaster = this.Init(); 
            using (lockMaster.AcquireReadLock(EntityType.Game, 1))
            {
            }

            using (lockMaster.AcquireReadLock(EntityType.Player, 1))
            {
            }

            using (lockMaster.AcquireReadLock(EntityType.Town, 1))
            {
            }

            using (lockMaster.AcquireReadLock(EntityType.Player, 1))
            {
                using (lockMaster.AcquireReadLock(EntityType.Town, 1))
                {
                }
            }

            using (lockMaster.AcquireReadLock(EntityType.Game, 1))
            {
            }
        }

        [Test]
        public void TestStupidWriteLocking()
        {
            var lockMaster = this.Init();
            using (lockMaster.AcquireWriteLock(EntityType.Game, 1))
            {
            }

            using (lockMaster.AcquireWriteLock(EntityType.Player, 1))
            {
            }

            using (lockMaster.AcquireWriteLock(EntityType.Town, 1))
            {
            }

            using (lockMaster.AcquireWriteLock(EntityType.Player, 1))
            {
                using (lockMaster.AcquireWriteLock(EntityType.Town, 1))
                {
                }
            }

            using (lockMaster.AcquireWriteLock(EntityType.Game, 1))
            {
            }
        }

        [Test]
        public void TestSameInstanceLocking()
        {
            var lockMaster = this.Init(); 
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    using (lockMaster.AcquireReadLock(EntityType.Town, 1))
                    {
                        using (lockMaster.AcquireReadLock(EntityType.Town, 2))
                        {
                        }
                    }
                });
        }

        [Test]
        public void TestParentalLocking()
        {
            var lockMaster = this.Init(); 
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    using (lockMaster.AcquireReadLock(EntityType.Town, 1))
                    {
                        using (lockMaster.AcquireReadLock(EntityType.Game, 2))
                        {
                        }
                    }
                });
        }

        [Test]
        public void TestBlocking()
        {
            var lockMaster = this.Init();

            var y = 0;

            Parallel.For(0, 100, (x) =>
                {
                    using (lockMaster.AcquireWriteLock(EntityType.Server, 0))
                    {
                        var temp = y;
                        Thread.Sleep(10);
                        y = temp + 1;
                    }
                });
        }

        [Test]
        public void TestMultithreadedLocking()
        {
            var lockMaster = this.Init();

            Parallel.For(0, 100, (x) =>
            {
                using (lockMaster.AcquireWriteLock(EntityType.Town, x))
                {
                    Thread.Sleep(10);
                }
            });
        }
    }
}
