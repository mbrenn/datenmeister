namespace DatenMeister.Entities.AsObject.DM
{
    public static class Types
    {
        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent("datenmeister:///types");
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "RecentProject";
                BurnSystems.Test.Ensure.That(Types.RecentProject == null);
                Types.RecentProject = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.RecentProject);
            }

            extent.AddDefaultMappings();
            return extent;
        }

        public static DatenMeister.IObject RecentProject;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.Entities.DM.RecentProject), Types.RecentProject);
        }

    }
}
