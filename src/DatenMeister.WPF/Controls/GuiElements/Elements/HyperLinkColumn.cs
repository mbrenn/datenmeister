using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DatenMeister.WPF.Controls.GuiElements.Elements
{
    public class HyperLinkColumn : GenericColumn
    {
        public HyperLinkColumn(IObject associatedViewColumn, string propertyName)
            : base(associatedViewColumn, propertyName)
        {
        }

        protected override System.Windows.FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return this.GenerateElement(cell, dataItem);
        }

        protected override System.Windows.FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var result = new TextBlock();
            result.SetBinding(TextBlock.TextProperty, this.Binding);

            result.MouseEnter += (x, y) =>
                {
                    result.TextDecorations = TextDecorations.Underline;
                    result.Cursor = Cursors.Hand;
                };

            result.MouseLeave += (x, y) =>
            {
                result.TextDecorations = null;
                result.Cursor = null;
            };

            result.MouseDown += (x, y) =>
            {
                this.StartHyperLinkAction(result.Text);
            };

            return result;
        }

        private void StartHyperLinkAction(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                // Nothing to do here
                return;
            }

            if (result.Contains("@"))
            {
                // Checks, if it is a mail address:
                Process.Start("mailto:" + result);
                return;
            }
            else if (result.StartsWith("http://"))
            {
                // Checks, if it is http address
                Process.Start(result);
                return;
            }
            else if (File.Exists(result))
            {
                // Checks, if it is a file. For security reasons, only the directory is opened
                var directory = Path.GetDirectoryName(result);
                if (!File.Exists(directory) && Directory.Exists(directory))
                {
                    Process.Start(directory);
                }
            }
            else if (!File.Exists(result) && Directory.Exists(result))
            {
                // Checks, if it is a directory
                Process.Start(result);
            }
            else
            {
                MessageBox.Show("Possible unsafe action: " + result + "\r\nFile might not be existing anymore. The action was canceled");
            }
        }
    }
}
