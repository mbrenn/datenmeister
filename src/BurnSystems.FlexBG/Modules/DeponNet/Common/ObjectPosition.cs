using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Common
{
    /// <summary>
    /// Stores the position of an object
    /// </summary>
    [Serializable]
    public class ObjectPosition
    {
        private Vector3D Position = new Vector3D();

        public ObjectPosition()
        {
        }

        public ObjectPosition(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X
        {
            get { return this.Position.X; }
            set { this.Position.X = value; }
        }

        public double Y
        {
            get { return this.Position.Y; }
            set { this.Position.Y = value; }
        }

        public double Z
        {
            get { return this.Position.Z; }
            set { this.Position.Z = value; }
        }

        public static ObjectPosition operator +(ObjectPosition obj1, ObjectPosition obj2)
        {
            var result = new ObjectPosition();
            result.Position = obj1.Position + obj2.Position;
            return result;
        }

        public static ObjectPosition operator -(ObjectPosition obj1, ObjectPosition obj2)
        {
            var result = new ObjectPosition();
            result.Position = obj1.Position - obj2.Position;
            return result;
        }

        public static ObjectPosition operator *(ObjectPosition obj1, double value)
        {
            var result = new ObjectPosition();
            result.Position = obj1.Position * value;
            return result;
        }

        public double Length
        {
            get
            {
                return this.Position.Length;
            }
        }

        public void Normalize()
        {
            this.Position.Normalize();
        }

        public object AsJson()
        {
            return new
            {
                x = this.X,
                y = this.Y,
                z = this.Z
            };
        }

        public static double Distance(ObjectPosition o1, ObjectPosition o2)
        {
            var dx = o1.X - o2.X;
            var dy = o1.Y - o2.Y;
            var dz = o1.Z - o2.Z;

            return
                Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

    }
}
