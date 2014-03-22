namespace DatenMeister.Entities.AsObject.FieldInfo
{
    public static class Types
    {
        public static DatenMeister.IURIExtent Init()
        {
            var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent("datenmeister:///types");
            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "Comment";
                Types.Comment = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.Comment);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "General";
                Types.General = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.General);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "TextField";
                Types.TextField = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.TextField);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "ActionButton";
                Types.ActionButton = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.ActionButton);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "View";
                Types.View = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.View);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "FormView";
                Types.FormView = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.FormView);
            }

            {
                var type = new DatenMeister.Entities.UML.Type();
                type.name = "TableView";
                Types.TableView = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);
                extent.Add(Types.TableView);
            }

            return extent;
        }

        public static DatenMeister.IObject Comment;

        public static DatenMeister.IObject General;

        public static DatenMeister.IObject TextField;

        public static DatenMeister.IObject ActionButton;

        public static DatenMeister.IObject View;

        public static DatenMeister.IObject FormView;

        public static DatenMeister.IObject TableView;


    }
}
