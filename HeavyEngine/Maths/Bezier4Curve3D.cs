using System;
using System.Collections.Generic;

using OpenTK.Mathematics;

// This class was made with help of this video by Freya Holmer: https://www.youtube.com/watch?v=aVwxzDHniEw
namespace HeavyEngine {
    /// <summary>
    /// Represents a curve through 3D space
    /// </summary>
    public class Bezier4Curve3D {
        /// <summary>
        /// A 3D bounding box with a position and a size
        /// </summary>
        public struct BoundingBox3D {
            /// <summary>
            /// The position of the <see cref="BoundingBox3D"/>
            /// </summary>
            public Vector3 position;
            /// <summary>
            /// The size of the <see cref="BoundingBox3D"/>
            /// </summary>
            public Vector3 size;

            /// <summary>
            /// Instantiates a new <see cref="BoundingBox3D"/>
            /// </summary>
            /// <param name="position">The position of the <see cref="BoundingBox3D"/></param>
            /// <param name="size">The size of the <see cref="BoundingBox3D"/></param>
            public BoundingBox3D(Vector3 position, Vector3 size) {
                this.position = position;
                this.size = size;
            }
        }
        
        /// <summary>
        /// The number of points this curve is made up of
        /// </summary>
        public const int PointCount = 4;

        private readonly Vector3[] points;

        /// <summary>
        /// Gets or sets the point at the specified index
        /// </summary>
        /// <param name="index">The index of the point to get or set</param>
        /// <returns>The point at the specified index</returns>
        public Vector3 this[int index] {
            get => points[index];
            set {
                points[index] = value;
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="Bezier4Curve3D"/>
        /// </summary>
        public Bezier4Curve3D() {
            points = new Vector3[PointCount];

            for (int i = 0; i < PointCount; i++)
                points[i] = Vector3.Zero;
        }

        /// <summary>
        /// Instantiates a new <see cref="Bezier4Curve3D"/> with the specified points
        /// </summary>
        /// <param name="points">The points this curve should follow</param>
        /// <exception cref="ArgumentException">Thrown if the length of points isn't equal to <see cref="PointCount"/></exception>
        public Bezier4Curve3D(Vector3[] points) {
            if (points.Length != PointCount)
                throw new ArgumentException($"{nameof(Bezier4Curve3D)} requires 4 points, please pass an array of at least that many points");

            this.points = points;
        }

        /// <summary>
        /// Instantiates a new <see cref="Bezier4Curve3D"/> with the specified points
        /// </summary>
        /// <param name="p0">The first point</param>
        /// <param name="p1">The second point</param>
        /// <param name="p2">The third point</param>
        /// <param name="p3">The fourth point</param>
        public Bezier4Curve3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
            points[0] = p0;
            points[1] = p1;
            points[2] = p2;
            points[3] = p3;
        }

        /// <summary>
        /// Calculates the position <see cref="Vector3"/> for the specified T-value
        /// </summary>
        /// <param name="t">How far along the line to calculate the position <see cref="Vector3"/> for</param>
        /// <returns>A <see cref="Vector3"/> position at the specified T-value</returns>
        public Vector3 GetPosition(float t) {
            return (points[0] * (-(t * t * t) + (3 * t * t) - (3 * t) + 1)) +
                (points[1] * ((3 * t * t * t) - (6 * t * t) + (3 * t))) +
                (points[2] * (-(3 * t * t * t) + (3 * t * t))) +
                (points[3] * (t * t * t));
        }

        /// <summary>
        /// Calculates the velocity <see cref="Vector3"/> for the specified T-value
        /// </summary>
        /// <param name="t">How far along the line to calculate the velocity <see cref="Vector3"/> for</param>
        /// <returns>A <see cref="Vector3"/> containing the velocity at the specified T-value</returns>
        public Vector3 GetVelocity(float t) {
            return (points[0] * (-(3 * t * t) + (6 * t) - 3)) +
                (points[1] * ((9 * t * t) - (12 * t) + 3)) +
                (points[2] * (-(9 * t * t) + (6 * t))) +
                (points[3] * (3 * t * t));
        }

        /// <summary>
        /// Calculates the acceleration <see cref="Vector3"/> for the specified T-value
        /// </summary>
        /// <param name="t">How far along the line to calculate the acceleration <see cref="Vector3"/> for</param>
        /// <returns>A <see cref="Vector3"/> containing the acceleration at the specified T-value</returns>
        public Vector3 GetAcceleration(float t) {
            return (points[0] * (-(6 * t) + 6)) +
                (points[1] * ((18 * t) - 12)) +
                (points[2] * ((-18 * t) + 6)) +
                (points[3] * (6 * t));
        }

        /// <summary>
        /// Calculates the rate of change in acceleration per unit for the provided points of the curve
        /// <para>This value is considered constant and doesn't depend on a T-value. The only way to change this value is to change the points of the curve</para>
        /// </summary>
        /// <returns>A <see cref="Vector3"/> containing the rate of change of the acceleration vector</returns>
        public Vector3 GetJolt() => (points[0] * -6) + (points[1] * 18) + (points[2] * -18) + (points[3] * 6);

        /// <summary>
        /// Calculates the direction <see cref="Vector3"/> at the specified T-value
        /// <para>This is simply the result of <see cref="GetVelocity(float)"/> normalized.</para>
        /// </summary>
        /// <param name="t">How far along the line to calculate the direction <see cref="Vector3"/> for</param>
        /// <returns>A <see cref="Vector3"/> containing the direction of the curve at the specified T-value</returns>
        public Vector3 GetDirection(float t) => GetVelocity(t).Normalized();

        /// <summary>
        /// Calculates a normal <see cref="Vector3"/> at the specified T-value
        /// </summary>
        /// <param name="t">How far along the line to calculate the normal <see cref="Vector3"/> for</param>
        /// <returns>A <see cref="Vector3"/> containing the direction of the curve at the specified T-value</returns>
        public Vector3 GetNormal(float t) {
            var direction = GetDirection(t);
            return new Vector3(-direction.Z, 0.0f, direction.X);
        }

        /// <summary>
        /// Calculates the curvature of the curve in radians per unit at the specified T-value
        /// </summary>
        /// <param name="t">How far along the line to calculate the curvature for</param>
        /// <returns>The curvature of the curve at the specified T-value</returns>
        public float GetCurvature(float t) {
            var velocity = GetVelocity(t);
            var acceleration = GetAcceleration(t);

            var determinant = velocity.X * acceleration.Y - velocity.Y * acceleration.X;
            return determinant / (velocity.LengthSquared * velocity.Length);
        }

        /// <summary>
        /// Returns a bounding box that completely surrounds the curve
        /// </summary>
        /// <returns>A bounding box around the curve</returns>
        public BoundingBox3D GetBoundingBox() {
            var xRoots = CalculateRoots(points[0].X, points[1].X, points[2].X, points[3].X);
            var yRoots = CalculateRoots(points[0].Y, points[1].Y, points[2].Y, points[3].Y);
            var zRoots = CalculateRoots(points[0].Z, points[1].Z, points[2].Z, points[3].Z);

            Vector3 minimum = Vector3.PositiveInfinity, maximum = Vector3.NegativeInfinity;

            var potentialPoints = new List<Vector3>() {
                points[0],
                points[1]
            };

            if (GetPositionIfValid(xRoots.X, out Vector3 position))
                potentialPoints.Add(position);

            if (GetPositionIfValid(xRoots.Y, out position))
                potentialPoints.Add(position);
            
            if (GetPositionIfValid(yRoots.X, out position))
                potentialPoints.Add(position);

            if (GetPositionIfValid(yRoots.Y, out position))
                potentialPoints.Add(position);
            
            if (GetPositionIfValid(zRoots.X, out position))
                potentialPoints.Add(position);

            if (GetPositionIfValid(zRoots.Y, out position))
                potentialPoints.Add(position);

            foreach(var point in potentialPoints) {
                minimum.X = MathF.Min(minimum.X, point.X);
                minimum.Y = MathF.Min(minimum.Y, point.Y);
                minimum.Z = MathF.Min(minimum.Z, point.Z);

                maximum.X = MathF.Max(maximum.X, point.X);
                maximum.Y = MathF.Max(maximum.Y, point.Y);
                maximum.Z = MathF.Max(maximum.Z, point.Z);
            }

            return new BoundingBox3D(minimum, maximum - minimum);
        }

        /// <summary>
        /// Calculates the radius if the circle with the same curvature as the curve has at the specified T-value
        /// </summary>
        /// <param name="t">How far along the line to calculate the oscillating circle's radius for</param>
        /// <returns>The radius of the circle at the specified T-value</returns>
        public float GetOscillatingCircleRadius(float t) => 1 / GetCurvature(t);

        private bool GetPositionIfValid(float t, out Vector3 position) {
            if (!float.IsNaN(t) && t >= 0 && t <= 1) {
                position = GetPosition(t);
                return true;
            } else {
                position = Vector3.Zero;
                return false;
            }
        }

        private Vector2 CalculateRoots(float p0, float p1, float p2, float p3) {
            var a = -(3 * p0) + (9 * p1) - (9 * p2) + (3 * p3);
            var b = (6 * p0) - (12 * p1) + (6 * p2);
            var c = -(3 * p0) + (3 * p1);

            var r0 = (-b + MathF.Sqrt(b * b - (4 * a * c))) / (2 * a);
            var r1 = (-b + MathF.Sqrt(b * b + (4 * a * c))) / (2 * a);

            return new Vector2(r0, r1);
        }
    }
}
