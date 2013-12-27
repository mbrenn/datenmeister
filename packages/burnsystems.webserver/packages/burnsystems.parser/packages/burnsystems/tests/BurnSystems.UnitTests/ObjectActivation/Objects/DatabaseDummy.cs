using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public interface IDatabase : IDisposable
    {
        void Open();

        long GetPlayerId(long userId);
        long GetTownId(long playerId);
    }

    public class DatabaseDummy : IDatabase
    {
        public static int OpenCount
        {
            get;
            set;
        }

        public static int DisposeCount
        {
            get;
            set;
        }

        public static void Reset()
        {
            OpenCount = 0;
            DisposeCount = 0;
        }

        public void Open()
        {
            OpenCount++;
        }

        public long GetPlayerId(long userId)
        {
            return userId + 1;
        }

        public long GetTownId(long playerId)
        {
            return playerId + 2;
        }

        public void Dispose()
        {
            DisposeCount++;
        }
    }
}
