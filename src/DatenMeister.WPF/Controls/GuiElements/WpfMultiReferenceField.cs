using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.Pool;
using DatenMeister.WPF.Controls.GuiElements.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// Shows the interface for the MultiReferenceField
    /// </summary>
    public class WpfMultiReferenceField : IWPFElementGenerator
    {
        /// <summary>
        /// Stores the detailobject
        /// </summary>
        public IObject DetailObject
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the fiel information
        /// </summary>
        public DatenMeister.Entities.AsObject.FieldInfo.MultiReferenceField FieldInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the context information
        /// </summary>
        public IDataPresentationState State
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the sequence being used to add and remove items from the detail object
        /// </summary>
        public IReflectiveSequence Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// Generates the wpf element being included into the form
        /// </summary>
        /// <param name="detailObject">Defines the object, which shall receive the selected values</param>
        /// <param name="fieldInfo">The field information being used to design the field</param>
        /// <param name="state">Gives the context</param>
        /// <returns>Creates the element</returns>
        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            this.DetailObject = detailObject;
            this.FieldInfo = new Entities.AsObject.FieldInfo.MultiReferenceField(fieldInfo);
            this.State = state;

            if ((state.EditMode == EditMode.Edit || state.EditMode == EditMode.Read) && detailObject != null)
            {
                this.Sequence = detailObject.get(this.FieldInfo.getBinding()).AsReflectiveSequence();
            }

            var element = new WpfMultiReferenceFieldElement(this);
            element.RefreshData();
            return element;
        }
        
        /// <summary>
        /// Stores the data back into the object
        /// </summary>
        /// <param name="detailObject">Detailobject to be used</param>
        /// <param name="entry">Stores the cached informaton</param>
        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
        }
    }
}
