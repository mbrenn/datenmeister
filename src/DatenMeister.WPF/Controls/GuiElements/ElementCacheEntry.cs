using DatenMeister.Entities.AsObject.FieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// Being used to cache the active wpf elements and field infos
    /// </summary>
    public class ElementCacheEntry
    {
        private List<AdditionalCheckBox> additionalColumns = new List<AdditionalCheckBox>();

        public IWpfElementGenerator WPFElementCreator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the created WPF element
        /// </summary>
        public UIElement WPFElement
        {
            get;
            set;
        }

        public IObject FieldInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of additional columns
        /// </summary>
        public List<AdditionalCheckBox> AdditionalColumns
        {
            get { return this.additionalColumns; }
        }

        public ElementCacheEntry(IObject fieldInfo)
        {
            this.FieldInfo = fieldInfo;
        }

        public ElementCacheEntry(IWpfElementGenerator wpfElementCreator, UIElement wpfElement, IObject fieldInfo)
        {
            this.WPFElement = wpfElement;
            this.WPFElementCreator = wpfElementCreator;
            this.FieldInfo = fieldInfo;
        }

        public abstract class AdditionalCheckBox
        {
            /// <summary>
            /// Defines a function, which is used to retrieve the checklist status
            /// </summary>
            public Func<bool> GetCheckBoxStatus
            {
                get;
                set;
            }

            /// <summary>
            /// The function, which assigns a new value from the checkbox
            /// to the specific object
            /// </summary>
            /// <param name="detailObject">Detail object to be used</param>
            /// <param name="fieldInfo">The field information</param>
            /// <returns>true, if an assignment occured</returns>
            public abstract bool Assign(IObject detailObject, IObject fieldInfo);
        }

        /// <summary>
        /// This object sets a specific value to the property, if the checkbox is set.
        /// The value of the object will be retrieved via the ValueFunction Functor
        /// </summary>
        public class SetValueCheckBox : AdditionalCheckBox
        {
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

            /// <summary>
            /// Initializes a new instance of the DefaultColumnInfo class
            /// </summary>
            /// <param name="valueFunction">Gets or sets the value function</param>
            public SetValueCheckBox(Func<bool, object> valueFunction)
            {
                this.ValueFunction = valueFunction;
            }

            /// <summary>
            /// The function, which assigns a new value from the checkbox
            /// to the specific object
            /// </summary>
            /// <param name="detailObject">Detail object to be used</param>
            /// <param name="fieldInfo">The field information</param>
            /// <returns>true, if an assignment occured</returns>
            public override bool Assign(IObject detailObject, IObject fieldInfo)
            {
                if (this.GetCheckBoxStatus())
                {
                    var newValue = this.ValueFunction(this.GetCheckBoxStatus());
                    detailObject.set(
                        General.getBinding(fieldInfo),
                        newValue);

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// When the user has checked the checkbox, the value will not be updated. 
        /// </summary>
        public class IgnoreChangeCheckBox : AdditionalCheckBox
        {
            public override bool Assign(IObject detailObject, IObject fieldInfo)
            {
                return this.GetCheckBoxStatus();
            }
        }
    }
}
