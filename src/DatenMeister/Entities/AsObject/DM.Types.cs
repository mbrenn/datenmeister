namespace DatenMeister.Entities.AsObject.DM
{
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/datenmeister";

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
            if(Types.ExtentInfo == null || true)
            {
                Types.ExtentInfo = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ExtentInfo, "ExtentInfo");
                extent.Elements().add(Types.ExtentInfo);
            }

            if(Types.RecentProject == null || true)
            {
                Types.RecentProject = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.RecentProject, "RecentProject");
                extent.Elements().add(Types.RecentProject);
            }

            if(Types.Workbench == null || true)
            {
                Types.Workbench = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Workbench, "Workbench");
                extent.Elements().add(Types.Workbench);
            }


            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }

            OnInitCompleted();

            {
                // ExtentInfo.uri
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "uri");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInfo, property);
            }

            {
                // ExtentInfo.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInfo, property);
            }

            {
                // ExtentInfo.storagePath
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "storagePath");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInfo, property);
            }

            {
                // ExtentInfo.extentType
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentType");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInfo, property);
            }

            {
                // ExtentInfo.isPrepopulated
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isPrepopulated");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInfo, property);
            }

            {
                // ExtentInfo.dataProviderSettings
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "dataProviderSettings");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInfo, property);
            }

            {
                // ExtentInfo.extentClass
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentClass");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInfo, property);
            }

            {
                // RecentProject.filePath
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "filePath");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.RecentProject, property);
            }

            {
                // RecentProject.created
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "created");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.RecentProject, property);
            }

            {
                // RecentProject.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.RecentProject, property);
            }

            {
                // Workbench.path
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "path");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Workbench, property);
            }

            {
                // Workbench.type
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "type");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Workbench, property);
            }

            {
                // Workbench.instances
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "instances");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Workbench, property);
            }

        }

        public static DatenMeister.IObject ExtentInfo;

        public static DatenMeister.IObject RecentProject;

        public static DatenMeister.IObject Workbench;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.ExtentInfo), Types.ExtentInfo);
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.RecentProject), Types.RecentProject);
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.Workbench), Types.Workbench);
        }

        static partial void OnInitCompleted();
    }
}
