namespace DatenMeister.AddOns.Data.FileSystem.AsObject
{
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/dmaddons/filesystem";

        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(DefaultExtentUri);
            DatenMeister.Entities.AsObject.Uml.Types.AssignTypeMapping(extent);
            Init(extent);
            return extent;
        }

        public static void Init(DatenMeister.IURIExtent extent)
        {
            var factory = DatenMeister.DataProvider.Factory.GetFor(extent);
            if(Types.File == null || true)
            {
                Types.File = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.File, "File");
                extent.Elements().add(Types.File);
            }

            if(Types.Directory == null || true)
            {
                Types.Directory = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Directory, "Directory");
                extent.Elements().add(Types.Directory);
            }


            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }

            OnInitCompleted();

            {
                // File.relativePath
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "relativePath");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.File, property);
            }

            {
                // Directory.relativePath
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "relativePath");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Directory, property);
            }

        }

        public static DatenMeister.IObject File;

        public static DatenMeister.IObject Directory;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.AddOns.Data.FileSystem.File), Types.File);
            extent.Mapping.Add(typeof(DatenMeister.AddOns.Data.FileSystem.Directory), Types.Directory);
        }

        static partial void OnInitCompleted();
    }
}
