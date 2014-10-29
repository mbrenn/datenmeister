using BurnSystems.Logging;
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
    public partial class EntityFormControl : UserControl, IDataPresentationState
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

            if (this.configuration.FormViewInfo != null)
            {
                // Creates the form 
                var fieldInfos = this.configuration.GetFormViewInfoAsFormView().getFieldInfos();
                this.formGrid.RowDefinitions.Clear();

                var currentRow = 0;

                // when one height is a Star-Height (auto height), this value 
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
                    nameLabel.FontSize = 16;
                    if (ObjectDictionaryForView.IsSpecialBinding(General.getBinding(fieldInfo)))
                    {
                        nameLabel.FontStyle = FontStyles.Italic;
                    }

                    Grid.SetRow(nameLabel, currentRow);

                    /////////////////////////////////////////////////
                    // Creates the value element for the form
                    var fieldInfoAsElement = fieldInfo as IElement;
                    var wpfElementCreator = WpfElementMapping.MapForForm(fieldInfoAsElement);
                    var wpfElement = wpfElementCreator.GenerateElement(this.configuration.DetailObject, fieldInfo, this);                    
                    if (wpfElement != null)
                    {
                        var border = new Border()
                        {
                            Child = wpfElement,
                            //BorderBrush = Brushes.Black,
                            //BorderThickness = new Thickness(2),
                            Margin = new Thickness(10,5,10,5)
                        };

                        Grid.SetRow(border, currentRow);
                        Grid.SetColumn(border, 1);
                        formGrid.Children.Add(border);

                        // Adds the information into the storage
                        this.wpfElements.Add(new ElementCacheEntry(wpfElementCreator, wpfElement, fieldInfo));
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

                    currentRow++;
                }

                if (!hadOneStarHeight)
                {
                    // Add last row to make the scrolling ok
                    formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Star) });
                }

                //formGrid.Background = Brushes.Red;
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
                foreach (var cacheEntry in this.wpfElements)
                {
                    cacheEntry.WPFElementCreator.SetData(this.configuration.DetailObject, cacheEntry);
                }
            }

            // And now throw the event for the window
            var ev = this.Accepted;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
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

        EditMode IDataPresentationState.EditMode
        {
            get { return this.configuration.EditMode; }
        }

        DisplayMode IDataPresentationState.DisplayMode
        {
            get { return Controls.DisplayMode.Form; }
        }
    }
}