using DatenMeister.Logic;
using DatenMeister.Logic.TypeConverter;
using DatenMeister.Pool;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    /// <summary>
    /// Initializes all necessary instances and classes for the filesystem
    /// </summary>
    public class Init
    {
        /// <summary>
        /// Gets or sets the DotNetTypeConverter
        /// </summary>
        public IDotNetTypeConverter DotNetTypeConverter
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the Init class. 
        /// </summary>
        /// <param name="typeConverter">Type Converter to be used</param>
        [Inject]
        public Init(IDotNetTypeConverter typeConverter)
        {
            this.DotNetTypeConverter = typeConverter;
        }
    
        /// <summary>
        /// Performs the initialization and adds all necessary items into the pool
        /// </summary>
        /// <param name="pool">Pool to be used</param>
        public void Do(IPool pool)
        {
            var typeExtent = pool.GetExtent(ExtentType.Type).First();

            // Checks, if the File is already in database, if yes, then the initialization is skipped
            if (!typeExtent.Elements().Any(x => x.AsIObject().get("name").AsSingle().ToString() == "DatenMeister.AddOns.Data.FileSystem.File"))
            {
                this.DotNetTypeConverter.Convert(typeExtent, typeof(File));
                this.DotNetTypeConverter.Convert(typeExtent, typeof(Directory));
            }
        }
    }
}
