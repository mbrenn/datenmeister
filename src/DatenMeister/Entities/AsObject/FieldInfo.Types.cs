namespace DatenMeister.Entities.AsObject.FieldInfo
{
    public static class Types
    {
        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent("datenmeister:///types/fieldinfo");
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "Comment";
                BurnSystems.Test.Ensure.That(Types.Comment == null);
                Types.Comment = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.Comment);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "General";
                BurnSystems.Test.Ensure.That(Types.General == null);
                Types.General = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.General);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "Checkbox";
                BurnSystems.Test.Ensure.That(Types.Checkbox == null);
                Types.Checkbox = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.Checkbox);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "TextField";
                BurnSystems.Test.Ensure.That(Types.TextField == null);
                Types.TextField = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.TextField);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "DatePicker";
                BurnSystems.Test.Ensure.That(Types.DatePicker == null);
                Types.DatePicker = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.DatePicker);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ActionButton";
                BurnSystems.Test.Ensure.That(Types.ActionButton == null);
                Types.ActionButton = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ActionButton);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ReferenceBase";
                BurnSystems.Test.Ensure.That(Types.ReferenceBase == null);
                Types.ReferenceBase = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ReferenceBase);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ReferenceByValue";
                BurnSystems.Test.Ensure.That(Types.ReferenceByValue == null);
                Types.ReferenceByValue = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ReferenceByValue);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ReferenceByRef";
                BurnSystems.Test.Ensure.That(Types.ReferenceByRef == null);
                Types.ReferenceByRef = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.ReferenceByRef);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "MultiReferenceField";
                BurnSystems.Test.Ensure.That(Types.MultiReferenceField == null);
                Types.MultiReferenceField = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.MultiReferenceField);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "View";
                BurnSystems.Test.Ensure.That(Types.View == null);
                Types.View = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.View);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "FormView";
                BurnSystems.Test.Ensure.That(Types.FormView == null);
                Types.FormView = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Elements().add(Types.FormView);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "TableView";
                BurnSystems.Test.Ensure.That(Types.TableView == null);
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
