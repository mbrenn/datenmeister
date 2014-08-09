namespace DatenMeister.Entities.AsObject.Uml
{
    public static partial class Types
    {
        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent("datenmeister:///types/uml");
            Init(extent);
            return extent;
        }

        public static void Init(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            if(Types.NamedElement == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "NamedElement";
                Types.NamedElement = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.NamedElement);
            }

            if(Types.Type == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "Type";
                Types.Type = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.Type);
            }

            extent.AddDefaultMappings();
        }

        public static DatenMeister.IObject NamedElement;

        public static DatenMeister.IObject Type;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.Entities.UML.NamedElement), Types.NamedElement);
            extent.Mapping.Add(typeof(DatenMeister.Entities.UML.Type), Types.Type);
        }

    }
}
