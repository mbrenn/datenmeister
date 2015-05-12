namespace DatenMeister.AddOns.Data.FileSystem.AsObject
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpTypeDefinitionFactory", "1.1.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/dmaddons/filesystem";

        public static DatenMeister.IURIExtent Init(bool forceRecreate = false)
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(DefaultExtentUri);
            DatenMeister.Entities.AsObject.Uml.Types.AssignTypeMapping(extent);
            Init(extent, forceRecreate);
            return extent;
        }

        public static void Init(DatenMeister.IURIExtent extent, bool forceRecreate = false)
        {
            var factory = DatenMeister.DataProvider.Factory.GetFor(extent);
            Init(extent, factory, forceRecreate);
        }

        public static void Init(DatenMeister.IURIExtent extent, DatenMeister.IFactory factory, bool forceRecreate = false)
        {
            if(Types.File == null || forceRecreate)
            {
                Types.File = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.File, "File");
                extent.Elements().add(Types.File);

                {
                    // File.relativePath
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "relativePath");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.File, property);
                }
            }

            if(Types.Directory == null || forceRecreate)
            {
                Types.Directory = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Directory, "Directory");
                extent.Elements().add(Types.Directory);

                {
                    // Directory.relativePath
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "relativePath");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Directory, property);
                }
            }


            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }

            OnInitCompleted();

        }

        public static DatenMeister.IObject File;

        public static DatenMeister.IObject Directory;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            AssignTypeMapping(extent.Mapping);
        }

        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.IMapsMetaClassFromDotNet mapping)
        {
            mapping.Add(typeof(DatenMeister.AddOns.Data.FileSystem.File), Types.File);
            mapping.Add(typeof(DatenMeister.AddOns.Data.FileSystem.Directory), Types.Directory);
        }

        public static void Reset()
        {
            Types.File = null;
            Types.Directory = null;
        }

        static partial void OnInitCompleted();
    }
}
