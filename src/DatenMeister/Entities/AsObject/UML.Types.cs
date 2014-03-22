namespace DatenMeister.Entities.AsObject.Uml
{
    public static class Types
    {
        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent("datenmeister:///types");
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "NamedElement";
                Types.NamedElement = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.NamedElement);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "Type";
                Types.Type = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.Type);
            }

            return extent;
        }

        public static DatenMeister.IObject NamedElement;

        public static DatenMeister.IObject Type;


    }
}
