namespace DatenMeister.Entities.AsObject.FieldInfo
{
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/fieldinfo";

        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(DefaultExtentUri);
            Init(extent);
            return extent;
        }

        public static void Init(DatenMeister.IURIExtent extent)
        {
            var factory = DatenMeister.DataProvider.Factory.GetFor(extent);
            if(Types.Comment == null || true)
            {
                Types.Comment = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Comment, "Comment");
                extent.Elements().add(Types.Comment);
            }

            if(Types.General == null || true)
            {
                Types.General = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.General, "General");
                extent.Elements().add(Types.General);
            }

            if(Types.Checkbox == null || true)
            {
                Types.Checkbox = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Checkbox, "Checkbox");
                extent.Elements().add(Types.Checkbox);
            }

            if(Types.TextField == null || true)
            {
                Types.TextField = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.TextField, "TextField");
                extent.Elements().add(Types.TextField);
            }

            if(Types.DatePicker == null || true)
            {
                Types.DatePicker = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.DatePicker, "DatePicker");
                extent.Elements().add(Types.DatePicker);
            }

            if(Types.ActionButton == null || true)
            {
                Types.ActionButton = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ActionButton, "ActionButton");
                extent.Elements().add(Types.ActionButton);
            }

            if(Types.ReferenceBase == null || true)
            {
                Types.ReferenceBase = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ReferenceBase, "ReferenceBase");
                extent.Elements().add(Types.ReferenceBase);
            }

            if(Types.ReferenceByValue == null || true)
            {
                Types.ReferenceByValue = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ReferenceByValue, "ReferenceByValue");
                extent.Elements().add(Types.ReferenceByValue);
            }

            if(Types.ReferenceByRef == null || true)
            {
                Types.ReferenceByRef = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ReferenceByRef, "ReferenceByRef");
                extent.Elements().add(Types.ReferenceByRef);
            }

            if(Types.MultiReferenceField == null || true)
            {
                Types.MultiReferenceField = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.MultiReferenceField, "MultiReferenceField");
                extent.Elements().add(Types.MultiReferenceField);
            }

            if(Types.View == null || true)
            {
                Types.View = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.View, "View");
                extent.Elements().add(Types.View);
            }

            if(Types.FormView == null || true)
            {
                Types.FormView = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.FormView, "FormView");
                extent.Elements().add(Types.FormView);
            }

            if(Types.TableView == null || true)
            {
                Types.TableView = factory.create(null);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.TableView, "TableView");
                extent.Elements().add(Types.TableView);
            }

            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }
        }

        public static DatenMeister.IObject Comment;

        public static DatenMeister.IObject General;

        public static DatenMeister.IObject Checkbox;

        public static DatenMeister.IObject TextField;

        public static DatenMeister.IObject DatePicker;

        public static DatenMeister.IObject ActionButton;

        public static DatenMeister.IObject ReferenceBase;

        public static DatenMeister.IObject ReferenceByValue;

        public static DatenMeister.IObject ReferenceByRef;

        public static DatenMeister.IObject MultiReferenceField;

        public static DatenMeister.IObject View;

        public static DatenMeister.IObject FormView;

        public static DatenMeister.IObject TableView;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.Comment), Types.Comment);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.General), Types.General);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.Checkbox), Types.Checkbox);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.TextField), Types.TextField);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.DatePicker), Types.DatePicker);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ActionButton), Types.ActionButton);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ReferenceBase), Types.ReferenceBase);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ReferenceByValue), Types.ReferenceByValue);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ReferenceByRef), Types.ReferenceByRef);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.MultiReferenceField), Types.MultiReferenceField);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.View), Types.View);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.FormView), Types.FormView);
            extent.Mapping.Add(typeof(DatenMeister.Entities.FieldInfos.TableView), Types.TableView);
        }

    }
}
