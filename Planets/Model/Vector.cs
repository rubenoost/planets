using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Planets.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        /// <summary>
        /// X component of this Vector.
        /// </summary>
        public readonly double X;

        /// <summary>
        /// Y component of this Vector.
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Creates a new Vector with the given X and Y component.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Scales this vector so it becomes the given length.
        /// <code>this.ScaleToLength(newLength).Length() == newLength</code>
        /// </summary>
        /// <param name="newLength">The new length of this Vector.</param>
        /// <returns>A new Vector.</returns>
        public Vector ScaleToLength(double newLength)
        {
            return Normal() * newLength;
        }

        /// <summary>
        /// Returns the normal of this Vector.
        /// </summary>
        /// <returns>A new Vector.</returns>
        public Vector Normal()
        {
            double lenth = (Length() > 0) ? Length() : 1;
            return this / lenth;
        }

        /// <summary>
        /// Returns the length of this Vector.
        /// </summary>
        /// <returns><code>Math.sqrt(X * X + Y * Y)</code></returns>
        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// Adds two Vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>A new Vector</returns>
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// Subtracts two Vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>A new Vector.</returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// Multiplies Vector with scalar (double).
        /// </summary>
        /// <param name="v1">Vector to be multiplied.</param>
        /// <param name="scalar">Scalar to be multiplied with.</param>
        /// <returns>A new Vector.</returns>
        public static Vector operator *(Vector v1, double scalar)
        {
            return new Vector(v1.X * scalar, v1.Y * scalar);
        }
        public static Vector operator *(double scalar, Vector v1)
        {
            return v1 * scalar;
        }

        /// <summary>
        /// Inner product of two vectors.
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns></returns>
        public double InnerProduct(Vector v)
        {
            return (X * v.X + Y * v.Y);
        }

        /// <summary>
        /// Divides Vector by scalar (double).
        /// </summary>
        /// <param name="v1">Vector to be divided.</param>
        /// <param name="scalar">Scalar to be divided by.</param>
        /// <returns>A new Vector.</returns>
        public static Vector operator /(Vector v1, double scalar)
        {
            return new Vector(v1.X / scalar, v1.Y / scalar);
        }

        /// <summary>
        /// Converts point to Vector.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator Vector(Point p)
        {
            return new Vector(p.X, p.Y);
        }

        /// <summary>
        /// Converts point to Vector.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator Point(Vector v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", X.ToString("0.000", new CultureInfo("en-US", false)), Y.ToString("0.000", new CultureInfo("en-US", false)));
        }
    }

}
