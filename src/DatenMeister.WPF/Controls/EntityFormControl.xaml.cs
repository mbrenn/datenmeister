using BurnSystems.Logger;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic;
using DatenMeister.WPF.Controls.GuiElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Interaktionslogik für ObjectForm.xaml
    /// </summary>
    public partial class EntityFormControl : UserControl, ILayoutHostState
    {
        /// <summary>
        /// Defines the logger to be used
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(EntityFormControl));

        /// <summary>
        /// Stores the information whether the list ist configured 
        /// </summary>
        private bool isConfigured = false;

        /// <summary>
        /// Stores the configuration for the formlayout
        /// </summary>
        private FormLayoutConfiguration configuration;

        /// <summary>
        /// Stores the list of wpf elements
        /// </summary>
        private List<ElementCacheEntry> wpfElements = new List<ElementCacheEntry>();

        #region Event handlers

        public event EventHandler Cancelled;
        public event EventHandler Accepted;

        #endregion

        /// <summary>
        /// Initializes a new instance of the EntityFormControl class
        /// </summary>
        public EntityFormControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the EntityFormControl class
        /// </summary>
        public EntityFormControl(FormLayoutConfiguration configuration)
            : this()
        {
            this.Configure(configuration);
        }

        /// <summary>
        /// Configures the table view
        /// </summary>
        /// <param name="configuration"></param>
        public void Configure(FormLayoutConfiguration configuration)
        {
            Ensure.That(configuration != null, "No Configuration is given");
            this.configuration = configuration;
            this.isConfigured = true;
            this.Relayout();
        }

        /// <summary>
        /// Called, when user control had been initialized
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Arguments of event</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.isConfigured)
            {
                this.Relayout();
            }
        }

        /// <summary>
        /// Does the relayout
        /// </summary>
        public void Relayout()
        {
            if (!this.isConfigured)
            {
                throw new InvalidOperationException("The window is not properly configured");
            }

            this.wpfElements.Clear();
            var additionalColumns = this.GetAdditionalColumns();
            var hasAdditionalColumns = this.AddFirstRowForAdditionalColumns(additionalColumns);

            if (this.configuration.FormViewInfo != null)
            {
                // Creates the form 
                var fieldInfos = this.configuration.GetFormViewInfoAsFormView().getFieldInfos();
                this.formGrid.RowDefinitions.Clear();
                var currentRow = this.AddAdditionalColumnAtHeadline(additionalColumns);

                // When one height is a Star-Height (auto height), this value 
                // will be set to true. If still false at end, the last row will be set to autoheight
                var hadOneStarHeight = false;

                // Goes through each element
                foreach (var fieldInfo in fieldInfos.Cast<IObject>())
                {
                    // Gets fieldinformation as object
                    var fieldInfoObj = new DatenMeister.Entities.AsObject.FieldInfo.General(fieldInfo);

                    /////////////////////////////////////////////////
                    // Creates the key element for the form
                    var name = (fieldInfoObj.getName() ?? string.Empty).ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        name = "Unknown";
                    }

                    var nameLabel = new Label();
                    nameLabel.Content = string.Format("{0}: ", name);
                    nameLabel.Margin = new Thickness(10, 5, 10, 5);
                    if (ObjectDictionaryForView.IsSpecialBinding(General.getBinding(fieldInfo)))
                    {
                        nameLabel.FontStyle = FontStyles.Italic;
                    }

                    Grid.SetRow(nameLabel, currentRow);

                    /////////////////////////////////////////////////
                    // Creates the value element for the form
                    var fieldInfoAsElement = fieldInfo as IElement;
                    var wpfElementCreator = WpfElementMapping.MapForForm(fieldInfoAsElement);
                    UIElement wpfElement = null;
                    var elementCacheEntry = new ElementCacheEntry(fieldInfo);

                    // Only, if we show a single object or the field info supports
                    // multiple values
                    if (!this.configuration.HasMultipleObjects)
                    {
                        wpfElement = wpfElementCreator.GenerateElement(
                            this.configuration.DetailObject, 
                            fieldInfo, 
                            this,
                            elementCacheEntry);
                    }
                    else
                    {
                        var multiCreator = wpfElementCreator as IPropertyToMultipleValues;
                        if (multiCreator != null)
                        {
                            wpfElement = multiCreator.GenerateElement(
                                this.configuration.DetailObjects, 
                                fieldInfo, 
                                this, 
                                elementCacheEntry);
                        }
                    }

                    // Checks, if we have an element, otherwise we skip this row
                    if (wpfElement != null)
                    {
                        if (wpfElement != null)
                        {
                            var border = new Border()
                            {
                                Child = wpfElement,
                                Margin = new Thickness(10, 5, 10, 5)
                            };

                            Grid.SetRow(border, currentRow);
                            Grid.SetColumn(border, 1);
                            formGrid.Children.Add(border);

                            // Adds the information into the storage
                            elementCacheEntry.WPFElement = wpfElement;
                            elementCacheEntry.WPFElementCreator = wpfElementCreator;
                        }

                        /////////////////////////////////////////////////
                        // Defines the height
                        var height = fieldInfoObj.getHeight();
                        if (height == 0)
                        {
                            this.formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                        }
                        else if (height < 0)
                        {
                            this.formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(-height, GridUnitType.Star) });
                            hadOneStarHeight = true;
                        }
                        else
                        {
                            this.formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(height, GridUnitType.Pixel) });
                        }

                        formGrid.Children.Add(nameLabel);

                        this.AddAdditionalColumnForRow(
                            additionalColumns, 
                            currentRow, 
                            fieldInfoObj, 
                            elementCacheEntry);

                        this.wpfElements.Add(elementCacheEntry);

                        currentRow++;
                    }
                }

                if (!hadOneStarHeight)
                {
                    // Add last row to make the scrolling ok
                    formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Star) });
                }
            }

            // Focuses first element
            if (this.wpfElements.Count > 0)
            {
                FocusCacheEntry(this.wpfElements.First());
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // Creates Detailobject if necessary
            if (this.configuration.EditMode == Controls.EditMode.New)
            {
                this.configuration.DetailObject = Factory.GetFor(this.configuration.StorageCollection.Extent).create(
                    this.configuration.TypeToCreate);
                this.configuration.StorageCollection.add(this.configuration.DetailObject);
                Ensure.That(this.configuration.DetailObject != null, "Element Factory has not returned a value");
            }

            // Store values into object, if the view is not in read-only mode
            if (this.configuration.EditMode != Controls.EditMode.Read)
            {
                if (!this.configuration.HasMultipleObjects)
                {
                    this.SetPropertiesOnObject(this.configuration.DetailObject);
                }
                else
                {
                    foreach (var detailObject in this.configuration.DetailObjects)
                    {
                        this.SetPropertiesOnObject(detailObject);
                    }
                }
            }

            // And now throw the event for the window
            var ev = this.Accepted;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the properties on a detail object by using the additional columns
        /// or the UIElement of the row
        /// </summary>
        /// <param name="detailObject">Detail object, whose values shall be set</param>
        private void SetPropertiesOnObject(IObject detailObject)
        {
            foreach (var cacheEntry in this.wpfElements)
            {
                // Checks, if one of the additional columns set the content
                var wasSet = this.CheckForValueByAdditionalContent(detailObject, cacheEntry);

                if (!wasSet)
                {
                    // Check for additional columns
                    cacheEntry.WPFElementCreator.SetData(detailObject, cacheEntry);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Do nothing...

            // And throw the event for the window
            var ev = this.Cancelled;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Selects the field with the given name. The "name" of the fieldinfo will be used
        /// </summary>
        /// <param name="name">Name of the field, that should receive the focus</param>
        public void FocusFieldWithName(string name)
        {
            foreach (var element in this.wpfElements)
            {
                var elementName = element.FieldInfo.get("name").AsSingle().ToString();
                if (elementName == name)
                {
                    FocusCacheEntry(element);
                }
            }
        }

        /// <summary>
        /// Called, when the user asks to copy the information into the clipboard
        /// </summary>
        /// <param name="sender">Sender being used</param>
        /// <param name="e">Event arguments</param>
        private void btnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var tempObj = new GenericObject();

            // Store values into object
            foreach (var cacheEntry in this.wpfElements)
            {
                cacheEntry.WPFElementCreator.SetData(tempObj, cacheEntry);
            }

            // Now create a view obj
            var viewObj = new ObjectDictionaryForView(tempObj);

            var builder = new StringBuilder();
            foreach (var pair in viewObj)
            {
                builder.AppendFormat("{0}: {1}\r\n", pair.Key, pair.Value);
            }

            Clipboard.SetText(builder.ToString());

            MessageBox.Show("The content has been copied to the clipboard");
        }

        /// <summary>
        /// Focuses a the field information behind a certain cache entry
        /// </summary>
        /// <param name="element">Element to be focused</param>
        private static void FocusCacheEntry(ElementCacheEntry element)
        {
            var asFocusable = element.WPFElementCreator as IFocusable;
            if (asFocusable != null)
            {
                asFocusable.Focus(element.WPFElement);
            }
            else
            {
                element.WPFElement.Focus();
            }
        }

        EditMode ILayoutHostState.EditMode
        {
            get { return this.configuration.EditMode; }
        }

        DisplayMode ILayoutHostState.DisplayMode
        {
            get { return Controls.DisplayMode.Form; }
        }

        #region Functions for the additional columns

        /// <summary>
        /// Gets the additional columns
        /// </summary>
        /// <returns></returns>
        public virtual AdditionalColumn[] GetAdditionalColumns()
        {
            if (!this.configuration.HasMultipleObjects)
            {
                return new AdditionalColumn[] {   
                    new SetValueColumnInfo(
                        "Not Set",
                         (x) => x ? ObjectHelper.NotSet : null)
                };
            }
            else
            {
                return new AdditionalColumn[] {
                    new IgnoreChangeColumnInfo(
                        "No change", true),                        
                    new SetValueColumnInfo(
                        "Not Set",
                         (x) => x ? ObjectHelper.NotSet : null)
                };
            }
        }

        /// <summary>
        /// Adds the additional columns at the header line
        /// </summary>
        /// <param name="additionalColumns">Additional columns being used</param>
        /// <returns>triue, if we have additional columns</returns>
        private bool AddFirstRowForAdditionalColumns(AdditionalColumn[] additionalColumns)
        {
            var hasAdditionalColumns = additionalColumns.Length > 0;
            while (additionalColumns.Length + 2 > this.formGrid.ColumnDefinitions.Count)
            {
                var definition = new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Auto)
                };

                this.formGrid.ColumnDefinitions.Add(definition);
            }
            return hasAdditionalColumns;
        }

        /// <summary>
        /// Adds an additional column for a row
        /// </summary>
        /// <param name="additionalColumns"></param>
        /// <returns>The number of the row after the headlines</returns>
        private int AddAdditionalColumnAtHeadline(AdditionalColumn[] additionalColumns)
        {
            var hasAdditionalColumns = additionalColumns.Length > 0;
            var currentRow = 0;

            // Checks, if we have an additional column, if yes, we need to create an 
            // extra row
            if (hasAdditionalColumns)
            {
                this.formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                var currentColumn = 2;

                foreach (var column in additionalColumns)
                {
                    var text = new TextBlock()
                    {
                        Text = column.Name,
                        Style = this.Resources["TitleAdditionalColumn"] as Style
                    };

                    Grid.SetColumn(text, currentColumn);
                    Grid.SetRow(text, currentRow);
                    currentColumn++;

                    this.formGrid.Children.Add(text);
                }

                currentRow++;
            }
            return currentRow;
        }

        private void AddAdditionalColumnForRow(AdditionalColumn[] additionalColumns, int currentRow, General fieldInfoObj, ElementCacheEntry elementCacheEntry)
        {
            // Add the additional columns
            var currentColumn = 2;
            var propertyName = fieldInfoObj.getBinding();

            foreach (var column in additionalColumns)
            {
                var objectValue = ObjectHelper.GetCommonValue(this.configuration.DetailObjects, propertyName);
                var checkBox = new CheckBox();
                if (this.configuration.HasMultipleObjects)
                {
                    // As long, as we need the "No change thing", we cannot use the three states
                    // checkBox.IsThreeState = true;
                }

                checkBox.IsChecked = column.IsChecked(objectValue);

                Grid.SetRow(checkBox, currentRow);
                Grid.SetColumn(checkBox, currentColumn);
                checkBox.Style = this.Resources["CheckBoxAdditionalColumn"] as Style;
                this.formGrid.Children.Add(checkBox);

                // Creates the instance for the ElementCacheEntry, depending on the type of the column info
                ElementCacheEntry.AdditionalCheckBox newCheckBox = null;
                if (column is EntityFormControl.SetValueColumnInfo)
                {
                    var copyColumn = column as EntityFormControl.SetValueColumnInfo;
                    newCheckBox = new ElementCacheEntry.SetValueCheckBox(
                            copyColumn.ValueFunction);
                }

                if (column is EntityFormControl.IgnoreChangeColumnInfo)
                {
                    newCheckBox = new ElementCacheEntry.IgnoreChangeCheckBox();
                }

                if (newCheckBox != null)
                {
                    newCheckBox.Checkbox = checkBox;
                    elementCacheEntry.ChangeContent += (x, y) =>
                        {
                            newCheckBox.OnContentChange();
                        };
                    elementCacheEntry.AdditionalColumns.Add(newCheckBox);
                }

                currentColumn++;
            }
        }

        /// <summary>
        /// Sets the value for a property by an additional content
        /// </summary>
        /// <param name="cacheEntry">The cache entry to be used to retrive the information
        /// fast</param>
        /// <returns>true, if the element has been set by the additional content</returns>
        private bool CheckForValueByAdditionalContent(IObject detailObject, ElementCacheEntry cacheEntry)
        {
            var wasSet = false;

            // Checks, if one of the additional columns sets the property
            foreach (var column in cacheEntry.AdditionalColumns)
            {
                wasSet = column.Assign(
                    detailObject,
                    cacheEntry.FieldInfo);

                if (wasSet)
                {
                    break;
                }
            }
            return wasSet;
        }

        /// <summary>
        /// Defines the information that shall be given in the additional column
        /// </summary>
        public abstract class AdditionalColumn
        {
            public AdditionalColumn(string name)
            {
                this.Name = name;
            }

            /// <summary>
            /// Gets or sets the name of the additional column
            /// </summary>
            public string Name
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the function, which determines whether a checkbox is checked.
            /// </summary>
            public abstract bool? IsChecked(object value);
        }

        /// <summary>
        /// Offers a checkbox which is checked, when the property has a certain value. 
        /// This is determined by IsCheckedFunction
        /// When the checkbox is still checked on submission, the property will receive 
        /// a value given by ValueFunction. 
        /// </summary>
        public class SetValueColumnInfo : AdditionalColumn
        {
            public SetValueColumnInfo(
                string name,
                Func<bool, object> valueFunction)
                : base(name)
            {
                this.ValueFunction = valueFunction;
            }

            /// <summary>
            /// Gets the object, which is determined whether the checkbox is still checked.
            /// If the return value is null, the value is not set and the next checkbox will be
            /// used as a trigger
            /// </summary>
            public Func<bool, object> ValueFunction
            {
                get;
                set;
            }

            public override bool? IsChecked(object value)
            {
                if (value == ObjectHelper.Different)
                {
                    return null;
                }

                return value.AsSingle() == ObjectHelper.NotSet;
            }
        }

        /// <summary>
        /// Creates a checkbox, at which the user can decide whether to change the 
        /// value. If the user clicks the checkbox, the content is not changed, independent
        /// on the value at the column
        /// </summary>
        public class IgnoreChangeColumnInfo : AdditionalColumn
        {
            /// <summary>
            /// Stores the default check status for the columns
            /// </summary>
            private bool defaultCheckStatus;

            public IgnoreChangeColumnInfo(string name, bool defaultCheckStatus = false)
                : base(name)
            {
                this.defaultCheckStatus = defaultCheckStatus;
            }

            public override bool? IsChecked(object value)
            {
                return this.defaultCheckStatus;
            }

            
        }

        #endregion
    }
}