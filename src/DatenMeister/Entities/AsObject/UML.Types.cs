namespace DatenMeister.Entities.AsObject.Uml
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpTypeDefinitionFactory", "1.1.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
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
            if(Types.NamedElement == null /*|| true*/)
            {
                Types.NamedElement = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.NamedElement, "NamedElement");
                extent.Elements().add(Types.NamedElement);

                {
                    // NamedElement.name
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.NamedElement, property);
                }
            }

            if(Types.Type == null /*|| true*/)
            {
                Types.Type = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Type, "Type");
                extent.Elements().add(Types.Type);

                {
                    // Type.name
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Type, property);
                }
            }

            if(Types.Property == null /*|| true*/)
            {
                Types.Property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Property, "Property");
                extent.Elements().add(Types.Property);

                {
                    // Property.name
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Property, property);
                }
            }

            if(Types.Class == null /*|| true*/)
            {
                Types.Class = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Class, "Class");
                extent.Elements().add(Types.Class);

                {
                    // Class.isAbstract
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isAbstract");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Class, property);
                }

                {
                    // Class.ownedAttribute
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "ownedAttribute");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Class, property);
                }

                {
                    // Class.name
                    var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                    DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Class, property);
                }
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

        public static DatenMeister.IObject Class;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            AssignTypeMapping(extent.Mapping);
        }

        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.IMapsMetaClassFromDotNet mapping)
        {
            mapping.Add(typeof(DatenMeister.Entities.UML.NamedElement), Types.NamedElement);
            mapping.Add(typeof(DatenMeister.Entities.UML.Type), Types.Type);
            mapping.Add(typeof(DatenMeister.Entities.UML.Property), Types.Property);
            mapping.Add(typeof(DatenMeister.Entities.UML.Class), Types.Class);
        }

        public static void Reset()
        {
            Types.NamedElement = null;
            Types.Type = null;
            Types.Property = null;
            Types.Class = null;
        }

        static partial void OnInitCompleted();
    }
}
