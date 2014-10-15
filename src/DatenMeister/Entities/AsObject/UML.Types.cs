namespace DatenMeister.Entities.AsObject.Uml
{
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/uml";

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
            if(Types.NamedElement == null || true)
            {
                Types.NamedElement = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Type);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.NamedElement, "NamedElement");
                extent.Elements().add(Types.NamedElement);
            }

            if(Types.Type == null || true)
            {
                Types.Type = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Type);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Type, "Type");
                extent.Elements().add(Types.Type);
            }

            if(Types.Property == null || true)
            {
                Types.Property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Type);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Property, "Property");
                extent.Elements().add(Types.Property);
            }

            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }

            OnInitCompleted();
        }

        public static DatenMeister.IObject NamedElement;

        public static DatenMeister.IObject Type;

        public static DatenMeister.IObject Property;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.Entities.UML.NamedElement), Types.NamedElement);
            extent.Mapping.Add(typeof(DatenMeister.Entities.UML.Type), Types.Type);
            extent.Mapping.Add(typeof(DatenMeister.Entities.UML.Property), Types.Property);
        }

        static partial void OnInitCompleted();
    }
}
