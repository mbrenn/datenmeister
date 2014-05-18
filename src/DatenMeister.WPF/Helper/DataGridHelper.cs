using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DatenMeister.WPF.Helper
{
    /// <summary>
    /// Contains several helper methods for the DataGrid
    /// </summary>
    public static class DataGridHelper
    {
        #region Some Helper for DataGrid

        /// <sumemary>
        /// Gives the focus to the content, so user can easily navigate between cells by using the cursor.
        /// Is quite complicated, due to some aweful constraints of DataGrid
        /// </summary>
        /// <param name="grid">Grid being used</param>
        public static void GiveFocusToContent(DataGrid grid)
        {
            var n = 0;
            if (grid.SelectedIndex != -1)
            {
                n = grid.SelectedIndex;
            }

            var row = grid.ItemContainerGenerator.ContainerFromIndex(n) as DataGridRow;

            if (row != null)
            {
                var cell = DataGridHelper.GetCell(grid, row, 0);
                if (cell != null)
                {
                    cell.Focus();
                }
            }
        }

        /// <summary>
        /// Thanks to: http://blog.magnusmontin.net/2013/11/08/how-to-programmatically-select-and-focus-a-row-or-cell-in-a-datagrid-in-wpf/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// http://blog.magnusmontin.net/2013/11/08/how-to-programmatically-select-and-focus-a-row-or-cell-in-a-datagrid-in-wpf/
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="rowContainer"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                var presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method 
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    var cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }

        #endregion
    }
}
