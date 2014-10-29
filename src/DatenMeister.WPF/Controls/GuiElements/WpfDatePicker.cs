using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls.GuiElements
{
    class WpfDatePicker : IWpfElementGenerator
    {
        private DatePicker fieldInfo;

        /// <summary>
        /// Stores the wpf grid
        /// </summary>
        private System.Windows.Controls.Grid wpfGrid;

        /// <summary>
        /// Stores the datepicker
        /// </summary>
        private System.Windows.Controls.DatePicker datePicker;

        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            this.wpfGrid = new System.Windows.Controls.Grid();
            this.wpfGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition()
                {
                    Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star)
                });
            this.wpfGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition()
                {
                    Width = new System.Windows.GridLength(0, System.Windows.GridUnitType.Auto)
                });

            this.fieldInfo = new DatePicker(fieldInfo);

            this.datePicker = new System.Windows.Controls.DatePicker();
            this.datePicker.FontSize = 16;
            this.datePicker.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            
            if ((state.EditMode == EditMode.Edit || state.EditMode == EditMode.Read) && detailObject != null)
            {
                var fieldName = this.fieldInfo.getBinding().ToString();
                var propertyValue = detailObject.get(fieldName);
                if (propertyValue != null && propertyValue != ObjectHelper.NotSet)
                {
                    var date = ObjectConversion.ToDateTime(
                        propertyValue.AsSingle());

                    if (date == null)
                    {
                        this.datePicker.SelectedDate = null;
                    }
                    else
                    {
                        this.datePicker.SelectedDate = date;
                    }
                }

                // Do we have a read-only flag
                if (state.EditMode == EditMode.Read || this.fieldInfo.isReadOnly())
                {
                    // Cannot be done...
                }
            }

            var todayButton = new System.Windows.Controls.Button();
            todayButton.Content = "Today";
            todayButton.Padding = new System.Windows.Thickness(10, 0, 10, 0);
            todayButton.FontSize = 14;
            todayButton.Click += (x, y) => this.datePicker.SelectedDate = DateTime.Today;

            System.Windows.Controls.Grid.SetColumn(this.datePicker, 0);
            System.Windows.Controls.Grid.SetRow(this.datePicker, 0);
            System.Windows.Controls.Grid.SetColumn(todayButton, 1);
            System.Windows.Controls.Grid.SetRow(todayButton, 0);
            this.wpfGrid.Children.Add(this.datePicker);
            this.wpfGrid.Children.Add(todayButton);


            return this.wpfGrid;
        }

        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            if (!this.fieldInfo.isReadOnly())
            {
                var selected = datePicker.SelectedDate;
                if (selected == null)
                {
                    detailObject.set(this.fieldInfo.getBinding().ToString(), ObjectHelper.NotSet);
                }
                else
                {
                    detailObject.set(this.fieldInfo.getBinding().ToString(), datePicker.SelectedDate);
                }
            }
        }

        /// <summary>
        /// Sets a focus on the given element
        /// </summary>
        /// <param name="element">Element, where focus is put on </param>
        public void Focus(System.Windows.UIElement element)
        {
            this.datePicker.Focus();
        }
    }
}
