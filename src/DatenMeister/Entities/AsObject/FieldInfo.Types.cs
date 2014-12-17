namespace DatenMeister.Entities.AsObject.FieldInfo
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpTypeDefinitionFactory", "1.0.8.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public static partial class Types
    {
        public const string DefaultExtentUri="datenmeister:///types/fieldinfo";

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
            if(Types.Comment == null || true)
            {
                Types.Comment = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Comment, "Comment");
                extent.Elements().add(Types.Comment);
            }

            if(Types.General == null || true)
            {
                Types.General = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.General, "General");
                extent.Elements().add(Types.General);
            }

            if(Types.Checkbox == null || true)
            {
                Types.Checkbox = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.Checkbox, "Checkbox");
                extent.Elements().add(Types.Checkbox);
            }

            if(Types.TextField == null || true)
            {
                Types.TextField = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.TextField, "TextField");
                extent.Elements().add(Types.TextField);
            }

            if(Types.HyperLinkColumn == null || true)
            {
                Types.HyperLinkColumn = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.HyperLinkColumn, "HyperLinkColumn");
                extent.Elements().add(Types.HyperLinkColumn);
            }

            if(Types.DatePicker == null || true)
            {
                Types.DatePicker = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.DatePicker, "DatePicker");
                extent.Elements().add(Types.DatePicker);
            }

            if(Types.ActionButton == null || true)
            {
                Types.ActionButton = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ActionButton, "ActionButton");
                extent.Elements().add(Types.ActionButton);
            }

            if(Types.ReferenceBase == null || true)
            {
                Types.ReferenceBase = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ReferenceBase, "ReferenceBase");
                extent.Elements().add(Types.ReferenceBase);
            }

            if(Types.ReferenceByConstant == null || true)
            {
                Types.ReferenceByConstant = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ReferenceByConstant, "ReferenceByConstant");
                extent.Elements().add(Types.ReferenceByConstant);
            }

            if(Types.ReferenceByRef == null || true)
            {
                Types.ReferenceByRef = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ReferenceByRef, "ReferenceByRef");
                extent.Elements().add(Types.ReferenceByRef);
            }

            if(Types.ReferenceByValue == null || true)
            {
                Types.ReferenceByValue = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.ReferenceByValue, "ReferenceByValue");
                extent.Elements().add(Types.ReferenceByValue);
            }

            if(Types.MultiReferenceField == null || true)
            {
                Types.MultiReferenceField = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.MultiReferenceField, "MultiReferenceField");
                extent.Elements().add(Types.MultiReferenceField);
            }

            if(Types.SubElementList == null || true)
            {
                Types.SubElementList = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.SubElementList, "SubElementList");
                extent.Elements().add(Types.SubElementList);
            }

            if(Types.View == null || true)
            {
                Types.View = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.View, "View");
                extent.Elements().add(Types.View);
            }

            if(Types.FormView == null || true)
            {
                Types.FormView = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.FormView, "FormView");
                extent.Elements().add(Types.FormView);
            }

            if(Types.TableView == null || true)
            {
                Types.TableView = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.TableView, "TableView");
                extent.Elements().add(Types.TableView);
            }

            if(Types.TreeView == null || true)
            {
                Types.TreeView = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
                DatenMeister.Entities.AsObject.Uml.Type.setName(Types.TreeView, "TreeView");
                extent.Elements().add(Types.TreeView);
            }


            if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)
            {
                (extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();
            }

            OnInitCompleted();

            {
                // Comment.comment
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "comment");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Comment, property);
            }

            {
                // Comment.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Comment, property);
            }

            {
                // Comment.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Comment, property);
            }

            {
                // Comment.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Comment, property);
            }

            {
                // Comment.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Comment, property);
            }

            {
                // Comment.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Comment, property);
            }

            {
                // General.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.General, property);
            }

            {
                // General.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.General, property);
            }

            {
                // General.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.General, property);
            }

            {
                // General.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.General, property);
            }

            {
                // General.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.General, property);
            }

            {
                // Checkbox.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Checkbox, property);
            }

            {
                // Checkbox.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Checkbox, property);
            }

            {
                // Checkbox.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Checkbox, property);
            }

            {
                // Checkbox.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Checkbox, property);
            }

            {
                // Checkbox.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.Checkbox, property);
            }

            {
                // TextField.width
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "width");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // TextField.isMultiline
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isMultiline");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // TextField.isDateTime
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isDateTime");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // TextField.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // TextField.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // TextField.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // TextField.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // TextField.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TextField, property);
            }

            {
                // HyperLinkColumn.width
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "width");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // HyperLinkColumn.isMultiline
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isMultiline");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // HyperLinkColumn.isDateTime
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isDateTime");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // HyperLinkColumn.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // HyperLinkColumn.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // HyperLinkColumn.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // HyperLinkColumn.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // HyperLinkColumn.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.HyperLinkColumn, property);
            }

            {
                // DatePicker.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.DatePicker, property);
            }

            {
                // DatePicker.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.DatePicker, property);
            }

            {
                // DatePicker.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.DatePicker, property);
            }

            {
                // DatePicker.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.DatePicker, property);
            }

            {
                // DatePicker.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.DatePicker, property);
            }

            {
                // ActionButton.text
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "text");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ActionButton, property);
            }

            {
                // ActionButton.clickUrl
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "clickUrl");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ActionButton, property);
            }

            {
                // ReferenceBase.propertyValue
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "propertyValue");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceBase, property);
            }

            {
                // ReferenceBase.referenceUrl
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "referenceUrl");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceBase, property);
            }

            {
                // ReferenceBase.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceBase, property);
            }

            {
                // ReferenceBase.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceBase, property);
            }

            {
                // ReferenceBase.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceBase, property);
            }

            {
                // ReferenceBase.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceBase, property);
            }

            {
                // ReferenceBase.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceBase, property);
            }

            {
                // ReferenceByConstant.values
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "values");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByConstant, property);
            }

            {
                // ReferenceByConstant.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByConstant, property);
            }

            {
                // ReferenceByConstant.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByConstant, property);
            }

            {
                // ReferenceByConstant.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByConstant, property);
            }

            {
                // ReferenceByConstant.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByConstant, property);
            }

            {
                // ReferenceByConstant.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByConstant, property);
            }

            {
                // ReferenceByRef.propertyValue
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "propertyValue");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByRef, property);
            }

            {
                // ReferenceByRef.referenceUrl
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "referenceUrl");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByRef, property);
            }

            {
                // ReferenceByRef.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByRef, property);
            }

            {
                // ReferenceByRef.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByRef, property);
            }

            {
                // ReferenceByRef.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByRef, property);
            }

            {
                // ReferenceByRef.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByRef, property);
            }

            {
                // ReferenceByRef.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByRef, property);
            }

            {
                // ReferenceByValue.propertyValue
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "propertyValue");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByValue, property);
            }

            {
                // ReferenceByValue.referenceUrl
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "referenceUrl");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByValue, property);
            }

            {
                // ReferenceByValue.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByValue, property);
            }

            {
                // ReferenceByValue.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByValue, property);
            }

            {
                // ReferenceByValue.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByValue, property);
            }

            {
                // ReferenceByValue.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByValue, property);
            }

            {
                // ReferenceByValue.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.ReferenceByValue, property);
            }

            {
                // MultiReferenceField.propertyValue
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "propertyValue");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // MultiReferenceField.referenceUrl
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "referenceUrl");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // MultiReferenceField.tableViewInfo
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "tableViewInfo");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // MultiReferenceField.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // MultiReferenceField.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // MultiReferenceField.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // MultiReferenceField.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // MultiReferenceField.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.MultiReferenceField, property);
            }

            {
                // SubElementList.typeForNew
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "typeForNew");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.SubElementList, property);
            }

            {
                // SubElementList.listTableView
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "listTableView");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.SubElementList, property);
            }

            {
                // SubElementList.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.SubElementList, property);
            }

            {
                // SubElementList.binding
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "binding");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.SubElementList, property);
            }

            {
                // SubElementList.isReadOnly
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "isReadOnly");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.SubElementList, property);
            }

            {
                // SubElementList.columnWidth
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "columnWidth");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.SubElementList, property);
            }

            {
                // SubElementList.height
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "height");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.SubElementList, property);
            }

            {
                // View.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.View, property);
            }

            {
                // View.fieldInfos
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "fieldInfos");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.View, property);
            }

            {
                // View.startInEditMode
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "startInEditMode");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.View, property);
            }

            {
                // View.doAutoGenerateByProperties
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "doAutoGenerateByProperties");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.View, property);
            }

            {
                // FormView.allowEdit
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "allowEdit");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.allowDelete
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "allowDelete");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.allowNew
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "allowNew");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.showColumnHeaders
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "showColumnHeaders");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.allowNewProperty
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "allowNewProperty");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.fieldInfos
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "fieldInfos");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.startInEditMode
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "startInEditMode");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // FormView.doAutoGenerateByProperties
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "doAutoGenerateByProperties");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.FormView, property);
            }

            {
                // TableView.extentUri
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentUri");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.mainType
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "mainType");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.typesForCreation
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "typesForCreation");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.allowEdit
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "allowEdit");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.allowDelete
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "allowDelete");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.allowNew
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "allowNew");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.fieldInfos
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "fieldInfos");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.startInEditMode
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "startInEditMode");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TableView.doAutoGenerateByProperties
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "doAutoGenerateByProperties");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TableView, property);
            }

            {
                // TreeView.extentUri
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "extentUri");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TreeView, property);
            }

            {
                // TreeView.name
                var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                DatenMeister.Entities.AsObject.Uml.Property.setName(property, "name");
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(Types.TreeView, property);
            }

        }

        public static DatenMeister.IObject Comment;

        public static DatenMeister.IObject General;

        public static DatenMeister.IObject Checkbox;

        public static DatenMeister.IObject TextField;

        public static DatenMeister.IObject HyperLinkColumn;

        public static DatenMeister.IObject DatePicker;

        public static DatenMeister.IObject ActionButton;

        public static DatenMeister.IObject ReferenceBase;

        public static DatenMeister.IObject ReferenceByConstant;

        public static DatenMeister.IObject ReferenceByRef;

        public static DatenMeister.IObject ReferenceByValue;

        public static DatenMeister.IObject MultiReferenceField;

        public static DatenMeister.IObject SubElementList;

        public static DatenMeister.IObject View;

        public static DatenMeister.IObject FormView;

        public static DatenMeister.IObject TableView;

        public static DatenMeister.IObject TreeView;


        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)
        {
            AssignTypeMapping(extent.Mapping);
        }

        public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.IMapsMetaClassFromDotNet mapping)
        {
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.Comment), Types.Comment);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.General), Types.General);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.Checkbox), Types.Checkbox);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.TextField), Types.TextField);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.HyperLinkColumn), Types.HyperLinkColumn);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.DatePicker), Types.DatePicker);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ActionButton), Types.ActionButton);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ReferenceBase), Types.ReferenceBase);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ReferenceByConstant), Types.ReferenceByConstant);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ReferenceByRef), Types.ReferenceByRef);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.ReferenceByValue), Types.ReferenceByValue);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.MultiReferenceField), Types.MultiReferenceField);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.SubElementList), Types.SubElementList);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.View), Types.View);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.FormView), Types.FormView);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.TableView), Types.TableView);
            mapping.Add(typeof(DatenMeister.Entities.FieldInfos.TreeView), Types.TreeView);
        }

        static partial void OnInitCompleted();
    }
}
