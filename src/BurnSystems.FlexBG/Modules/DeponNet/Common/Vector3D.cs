using System;
namespace BurnSystems.FlexBG
{
	public class Vector3D
	{
		public double X
		{
			get;
			set;
		}
		
		public double Y
		{
			get;
			set;
		} 
		
		public double Z
		{
			get;
			set;
		}
		
		public double Length
		{
			get{
				return Math.Sqrt(
					this.X * this.X + this.Y * this.Y + this.Z * this.Z );
			}
		}
		
		public Vector3D ()
		{
		}
		
		public void Normalize ()
		{
			var length = this.Length;
			this.X /= length;
			this.Y /= length;
			this.Z /= length;
		}
		
		public static Vector3D operator+ (Vector3D first, Vector3D second)
		{
			var result = new Vector3D ();
			result.X = first.X + second.X;
			result.Y = first.Y + second.Y;
			result.Z = first.Z + second.Z;
			return result;
		}
		
		public static Vector3D operator- (Vector3D first, Vector3D second)
		{
			var result = new Vector3D ();
			result.X = first.X - second.X;
			result.Y = first.Y - second.Y;
			result.Z = first.Z - second.Z;
			return result;
		}
		
		public static Vector3D operator* (Vector3D first, double scalingFactor)
		{
			var result = new Vector3D ();
			result.X = first.X * scalingFactor;
			result.Y = first.Y * scalingFactor;
			result.Z = first.Z * scalingFactor;
			return result;
		}
	}
}

