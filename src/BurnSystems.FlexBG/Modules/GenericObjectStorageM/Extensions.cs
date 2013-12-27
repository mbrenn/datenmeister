using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.GenericObjectStorageM
{
    public static class Extensions
    {
        public static void Set<T>(this GenericObjectStorage storage, T value) where T : class
        {
            storage.Set<T>("/", value);
        }

        public static T Get<T>(this GenericObjectStorage storage) where T : class
        {
            return storage.Get<T>("/");
        }
    }
}
