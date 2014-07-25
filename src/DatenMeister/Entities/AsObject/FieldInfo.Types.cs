namespace DatenMeister.Entities.AsObject.FieldInfo
{
    public static class Types
    {
        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent("datenmeister:///types/fieldinfo");
            if(Types.Comment == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "Comment";
                Types.Comment = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.Comment);
            }

            if(Types.General == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "General";
                Types.General = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.General);
            }

            if(Types.Checkbox == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "Checkbox";
                Types.Checkbox = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.Checkbox);
            }

            if(Types.TextField == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "TextField";
                Types.TextField = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.TextField);
            }

            if(Types.DatePicker == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "DatePicker";
                Types.DatePicker = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.DatePicker);
            }

            if(Types.ActionButton == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ActionButton";
                Types.ActionButton = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ActionButton);
            }

            if(Types.ReferenceBase == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ReferenceBase";
                Types.ReferenceBase = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ReferenceBase);
            }

            if(Types.ReferenceByValue == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ReferenceByValue";
                Types.ReferenceByValue = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ReferenceByValue);
            }

            if(Types.ReferenceByRef == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ReferenceByRef";
                Types.ReferenceByRef = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ReferenceByRef);
            }

            if(Types.MultiReferenceField == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "MultiReferenceField";
                Types.MultiReferenceField = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.MultiReferenceField);
            }

            if(Types.View == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "View";
                Types.View = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.View);
            }

            if(Types.FormView == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "FormView";
                Types.FormView = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.FormView);
            }

            if(Types.TableView == null)
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "TableView";
                Types.TableView = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.TableView);
            }

            extent.AddDefaultMappings();
            return extent;
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
