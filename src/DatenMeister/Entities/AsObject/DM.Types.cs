namespace DatenMeister.Entities.AsObject.DM
{
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/datenmeister";

        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(DefaultExtentUri);
            Init(extent);
            return extent;
        }

        public static void Init(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            if(Types.RecentProject == null || true)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "RecentProject";
                Types.RecentProject = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.RecentProject);
            }

            extent.AddDefaultMappings();
        }

        public static DatenMeister.IObject RecentProject;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.RecentProject), Types.RecentProject);
        }

    }
}
