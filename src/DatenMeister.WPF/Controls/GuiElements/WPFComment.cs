using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    class WpfComment : IWpfElementGenerator
    {
        /// <summary>
        /// Generates the element
        /// </summary>
        /// <param name="detailInfo">Detailed information of the object</param>
        /// <param name="fieldInfo">Information of the field</param>
        /// <param name="state">Status of the entity</param>
        /// <returns>The created UI Element</returns>
        public UIElement GenerateElement(IObject detailInfo, IObject fieldInfo, IDataPresentationState state, ElementCacheEntry entry)
        {
            var comment = new DatenMeister.Entities.AsObject.FieldInfo.Comment(fieldInfo);
            var commentText = comment.getComment();

            var label = new Label();
            label.Content = commentText;

            return label;
        }

        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            // Nothing to do here
            return;
        }
    }
}
