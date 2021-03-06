﻿using System;
namespace Rise
{
	public struct BoundingSphere : IEquatable<BoundingSphere>
	{
		public Vector3 Center;
		public float Radius;

		public BoundingSphere(Vector3 center, float radius)
		{
			Center = center;
			Radius = radius;
		}
		public BoundingSphere(float radius)
		{
			Center = Vector3.Zero;
			Radius = radius;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public bool Equals(ref BoundingSphere other)
		{
			return Center.Equals(ref other.Center) && Radius == other.Radius;
		}
		public bool Equals(BoundingSphere other)
		{
			return Equals(ref other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + Center.X.GetHashCode();
				hash = hash * 23 + Center.Y.GetHashCode();
				hash = hash * 23 + Center.Z.GetHashCode();
				hash = hash * 23 + Radius.GetHashCode();
				return hash;
			}
		}

		public void Project(ref Vector3 point, out Vector3 result)
		{
			Geom3D.Project(ref this, ref point, out result);
		}
		public Vector3 Project(ref Vector3 point)
		{
			Vector3 result;
			Geom3D.Project(ref this, ref point, out result);
			return result;
		}

		public bool Intersects(ref BoundingSphere sphere)
		{
			return Geom3D.Intersects(ref this, ref sphere);
		}
		public bool Intersects(BoundingSphere sphere)
		{
			return Geom3D.Intersects(ref this, ref sphere);
		}
		public bool Intersects(ref BoundingBox box)
		{
			return Geom3D.Intersects(ref box, ref this);
		}
		public bool Intersects(BoundingBox box)
		{
			return Geom3D.Intersects(ref box, ref this);
		}
		public bool Intersects(ref Plane plane)
		{
			return Geom3D.Intersects(ref this, ref plane);
		}
		public bool Intersects(Plane plane)
		{
			return Geom3D.Intersects(ref this, ref plane);
		}

		public bool Contains(ref Vector3 point)
		{
			return Geom3D.Contains(ref this, ref point);
		}
		public bool Contains(Vector3 point)
		{
			return Geom3D.Contains(ref this, ref point);
		}
		public bool Contains(ref BoundingSphere sphere)
		{
			return Geom3D.Contains(ref this, ref sphere);
		}
		public bool Contains(BoundingSphere sphere)
		{
			return Geom3D.Contains(ref this, ref sphere);
		}
		public bool Contains(ref BoundingBox box)
		{
			return Geom3D.Contains(ref this, ref box);
		}
		public bool Contains(BoundingBox box)
		{
			return Geom3D.Contains(ref this, ref box);
		}

		public bool Raycast(ref Ray3D ray, out float dist)
		{
			return Geom3D.Raycast(ref this, ref ray, out dist);
		}
		public bool Raycast(Ray3D ray, out float dist)
		{
			return Geom3D.Raycast(ref this, ref ray, out dist);
		}
		public bool Raycast(ref Ray3D ray, out Vector3 point)
		{
			float dist;
			if (Geom3D.Raycast(ref this, ref ray, out dist))
			{
				point = ray.GetPoint(dist);
				return true;
			}
			point = ray.Point;
			return false;
		}
		public bool Raycast(Ray3D ray, out Vector3 point)
		{
			return Raycast(ref ray, out point);
		}

		public static BoundingSphere Conflate(ref BoundingSphere a, ref BoundingSphere b)
		{
			Vector3 center;
			Vector3.Lerp(ref a.Center, ref b.Center, 0.5f, out center);
			var radius = (Vector3.Distance(ref a.Center, ref b.Center) + a.Radius + b.Radius) * 0.5f;
			return new BoundingSphere(center, radius);
		}
		public static BoundingSphere Conflate(BoundingSphere a, BoundingSphere b)
		{
			return Conflate(ref a, ref b);
		}

		public static bool operator ==(BoundingSphere a, BoundingSphere b)
		{
			return a.Equals(ref b);
		}
		public static bool operator !=(BoundingSphere a, BoundingSphere b)
		{
			return !a.Equals(ref b);
		}
	}
}
