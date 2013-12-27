//-----------------------------------------------------------------------
// <copyright file="FloatRectangle.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Graphics
{
    /// <summary>
    /// Eine Rechteckstruktur, bei dem Breite und Höhe in einer Fließkommazahl
    /// mit doppelter Genauigkeit gespeichert wird. 
    /// </summary>
    public class FloatRectangle
    {
        /// <summary>
        /// Initializes a new instance of the FloatRectangle class.
        /// </summary>
        public FloatRectangle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the FloatRectangle class.
        /// </summary>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        public FloatRectangle(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the FloatRectangle class.
        /// </summary>
        /// <param name="left">Left border</param>
        /// <param name="top">Top border</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        public FloatRectangle(double left, double top, double width, double height)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Gets or sets the x-value of the left border
        /// </summary>
        public double Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y-value of the top border
        /// </summary>
        public double Top
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the x-value of the right border
        /// </summary>
        public double Right
        {
            get { return this.Left + this.Width; }
            set { this.Width = value - this.Left; }
        }

        /// <summary>
        /// Gets or sets the y-value of the bottom border
        /// </summary>
        public double Bottom
        {
            get { return this.Top + this.Height; }
            set { this.Height = value - this.Top; }
        }

        /// <summary>
        /// Gets or sets the width of rectangle
        /// </summary>
        public double Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of rectangle
        /// </summary>
        public double Height
        {
            get;
            set;
        }
    }
}
