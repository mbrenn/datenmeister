namespace DatenMeister.Entities.AsObject.Uml
{
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/uml";

        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(DefaultExtentUri);
            Init(extent);
            return extent;
        }

        public static void Init(DatenMeister.IURIExtent extent)
        {
            var factory = DatenMeister.DataProvider.Factory.GetFor(extent);
            if(Types.NamedElement == null || true)
            {
                Types.NamedElement = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.NamedElement, "NamedElement");
                extent.Elements().add(Types.NamedElement);
            }

            if(Types.Type == null || true)
            {
                Types.Type = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Type, "Type");
                extent.Elements().add(Types.Type);
            }

            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }
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
