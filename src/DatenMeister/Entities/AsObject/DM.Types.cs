namespace DatenMeister.Entities.AsObject.DM
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpTypeDefinitionFactory", "1.0.8.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
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
            if(Types.RecentProject == null || true)
            {
                Types.RecentProject = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.RecentProject, "RecentProject");
                extent.Elements().add(Types.RecentProject);
            }


            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }

            OnInitCompleted();

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

        public static DatenMeister.IObject RecentProject;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            AssignTypeMapping(extent.Mapping);
        }

        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.IMapsMetaClassFromDotNet mapping)
        {
            mapping.Add(typeof(DatenMeister.Entities.DM.RecentProject), Types.RecentProject);
        }

        static partial void OnInitCompleted();
    }
}
