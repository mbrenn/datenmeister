namespace DatenMeister.Entities.AsObject.DM
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpTypeDefinitionFactory", "1.1.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/datenmeister";

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
            if(Types.ExtentInfo == null || forceRecreate)
            {
                Types.ExtentInfo = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ExtentInfo, "ExtentInfo");
                extent.Elements().add(Types.ExtentInfo);

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
            }

            if(Types.ExtentLoadInfo == null || forceRecreate)
            {
                Types.ExtentLoadInfo = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ExtentLoadInfo, "ExtentLoadInfo");
                extent.Elements().add(Types.ExtentLoadInfo);

                {
                    // ExtentLoadInfo.extentType
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentType");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentLoadInfo, property);
                }

                {
                    // ExtentLoadInfo.extentUri
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentUri");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ExtentLoadInfo, property);
                }
            }

            if(Types.CSVExtentLoadInfo == null || forceRecreate)
            {
                Types.CSVExtentLoadInfo = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.CSVExtentLoadInfo, "CSVExtentLoadInfo");
                extent.Elements().add(Types.CSVExtentLoadInfo);

                {
                    // CSVExtentLoadInfo.filePath
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "filePath");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.CSVExtentLoadInfo, property);
                }

                {
                    // CSVExtentLoadInfo.extentType
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentType");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.CSVExtentLoadInfo, property);
                }

                {
                    // CSVExtentLoadInfo.extentUri
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentUri");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.CSVExtentLoadInfo, property);
                }
            }

            if(Types.RecentProject == null || forceRecreate)
            {
                Types.RecentProject = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.RecentProject, "RecentProject");
                extent.Elements().add(Types.RecentProject);

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
            }

            if(Types.Workbench == null || forceRecreate)
            {
                Types.Workbench = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Workbench, "Workbench");
                extent.Elements().add(Types.Workbench);

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


            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }

            OnInitCompleted();

        }

        public static DatenMeister.IObject ExtentInfo;

        public static DatenMeister.IObject ExtentLoadInfo;

        public static DatenMeister.IObject CSVExtentLoadInfo;

        public static DatenMeister.IObject RecentProject;

        public static DatenMeister.IObject Workbench;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            AssignTypeMapping(extent.Mapping);
        }

        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.IMapsMetaClassFromDotNet mapping)
        {
            mapping.Add(typeof(DatenMeister.Entities.DM.ExtentInfo), Types.ExtentInfo);
            mapping.Add(typeof(DatenMeister.Entities.DM.ExtentLoadInfo), Types.ExtentLoadInfo);
            mapping.Add(typeof(DatenMeister.Entities.DM.CSVExtentLoadInfo), Types.CSVExtentLoadInfo);
            mapping.Add(typeof(DatenMeister.Entities.DM.RecentProject), Types.RecentProject);
            mapping.Add(typeof(DatenMeister.Entities.DM.Workbench), Types.Workbench);
        }

        public static void Reset()
        {
            Types.ExtentInfo = null;
            Types.ExtentLoadInfo = null;
            Types.CSVExtentLoadInfo = null;
            Types.RecentProject = null;
            Types.Workbench = null;
        }

        static partial void OnInitCompleted();
    }
}
