using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
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
        /// Performs te default iniialization
        /// </summary>
        /// <param name="pool">The pool to be used for initialization</param>
        /// <returns>Returns the method itself</returns>
        public static Init DoDefault(IPool pool)
        {
            var init = Injection.Application.Get<Init>();
            init.Do(pool);
            return init;
        }

        /// <summary>
        /// Performs a decoupled initialization
        /// </summary>
        /// <returns>Returns the class</returns>
        public static Init DoDecoupled()
        {
            var genericExtent = new GenericExtent("datenmeister:///types/FileSystems");

            var init = Injection.Application.Get<Init>();
            init.Do(genericExtent);
            return init;
        }


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
            var typeExtent = pool.GetExtents(ExtentType.Type).First();
            Do(typeExtent);
        }

        /// <summary>
        /// Initializes the files and stores the resulting type instances into the given typeExtent
        /// </summary>
        /// <param name="typeExtent">Extent being used for the types</param>
        private void Do(IURIExtent typeExtent)
        {
            // Checks, if the File is already in database, if yes, then the initialization is skipped
            var elements = typeExtent.Elements();
            if (!elements.Any(x => x.AsIObject().get("name").AsSingle().ToString() == "DatenMeister.AddOns.Data.FileSystem.File"))
            {
                AsObject.Types.Init(typeExtent);
            }
            else
            {
                AsObject.Types.File = elements.Where(x =>
                    x.AsIObject().get("name").AsSingle().ToString() == "DatenMeister.AddOns.Data.FileSystem.File").First().AsIObject();
                AsObject.Types.Directory = elements.Where(x =>
                    x.AsIObject().get("name").AsSingle().ToString() == "DatenMeister.AddOns.Data.FileSystem.Directory").First().AsIObject();
            }

            // Performs the type mapping
            var mapping = Injection.Application.Get<IMapsMetaClassFromDotNet>();
            AsObject.Types.AssignTypeMapping(mapping);

            Ensure.That(AsObject.Types.File != null && AsObject.Types.Directory != null,
                "File or Directory types could not be found. Reinitialize the type extent");
        }
    }
}
