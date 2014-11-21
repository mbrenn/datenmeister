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
            if(Types.ExtentInstance == null || true)
            {
                Types.ExtentInstance = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ExtentInstance, "ExtentInstance");
                extent.Elements().add(Types.ExtentInstance);
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
                // ExtentInstance.url
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "url");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInstance, property);
            }

            {
                // ExtentInstance.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInstance, property);
            }

            {
                // ExtentInstance.extentType
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentType");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInstance, property);
            }

            {
                // ExtentInstance.isPrepopulated
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isPrepopulated");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInstance, property);
            }

            {
                // ExtentInstance.loadConfiguration
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "loadConfiguration");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentInstance, property);
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
                // Workbench.Instances
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "Instances");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Workbench, property);
            }

        }

        public static DatenMeister.IObject ExtentInstance;

        public static DatenMeister.IObject RecentProject;

        public static DatenMeister.IObject Workbench;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.ExtentInWorkbench), Types.ExtentInstance);
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.RecentProject), Types.RecentProject);
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.Workbench), Types.Workbench);
        }

        static partial void OnInitCompleted();
    }
}
