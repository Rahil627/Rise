﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rise.OpenGL;
namespace Rise
{
    public class Mesh3D : Mesh
    {
        const int vertexSize = 24;

        Vertex3D[] vertices;
        int vertexCount;

        public override int VertexCount { get { return vertexCount; } }

        public Mesh3D(int vertexCapacity, int indexCapacity) : base(indexCapacity)
        {
            vertices = new Vertex3D[vertexCapacity];
        }
        public Mesh3D() : this(4, 6)
        {

        }

        internal override void AssignAttributes()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(0, 2, VertexType.Float, false, vertexSize, new IntPtr(0));
            GL.VertexAttribPointer(1, 2, VertexType.Float, false, vertexSize, new IntPtr(8));
            GL.VertexAttribPointer(2, 4, VertexType.UnsignedByte, true, vertexSize, new IntPtr(16));
            GL.VertexAttribPointer(3, 4, VertexType.UnsignedByte, true, vertexSize, new IntPtr(20));
        }

        public override void UpdateVertices()
        {
            GL.BindBuffer(BufferTarget.Array, arrayID);
            GL.BufferData(BufferTarget.Array, vertexSize * vertexCount, vertices, BufferUsage.DynamicDraw);
            GL.BindBuffer(BufferTarget.Array, 0);
            uploadedVertexCount = vertexCount;
        }

        internal override void ClearVertices()
        {
            vertexCount = 0;
        }

        public void GetVertex(int i, out Vertex3D result)
        {
            vertices[i].CopyTo(out result);
        }
        public Vertex3D GetVertex(int i)
        {
            return vertices[i];
        }

        public void SetVertex(int i, ref Vertex3D vert)
        {
            vert.CopyTo(out vertices[i]);
        }

        public void SetVertices(Vertex3D[] verts, bool copy)
        {
            vertexCount = verts.Length;
            if (copy)
            {
                if (vertices.Length < vertexCount)
                    Array.Resize(ref vertices, vertexCount);
                Array.Copy(verts, vertices, vertexCount);
            }
            else
                vertices = verts;
        }

        public void AddVertex(ref Vertex3D vert)
        {
            if (vertexCount == vertices.Length)
                Array.Resize(ref vertices, vertexCount * 2);
            vert.CopyTo(out vertices[vertexCount++]);
        }
        public void AddVertex(Vertex3D vert)
        {
            AddVertex(ref vert);
        }

        public void AddVertices(ref Vertex3D a, ref Vertex3D b)
        {
            while (vertexCount + 2 > vertices.Length)
                Array.Resize(ref vertices, vertexCount * 2);
            a.CopyTo(out vertices[vertexCount++]);
            b.CopyTo(out vertices[vertexCount++]);
        }
        public void AddVertices(ref Vertex3D a, ref Vertex3D b, ref Vertex3D c)
        {
            while (vertexCount + 3 > vertices.Length)
                Array.Resize(ref vertices, vertexCount * 2);
            a.CopyTo(out vertices[vertexCount++]);
            b.CopyTo(out vertices[vertexCount++]);
            c.CopyTo(out vertices[vertexCount++]);
        }
        public void AddVertices(ref Vertex3D a, ref Vertex3D b, ref Vertex3D c, ref Vertex3D d)
        {
            while (vertexCount + 4 > vertices.Length)
                Array.Resize(ref vertices, vertexCount * 2);
            a.CopyTo(out vertices[vertexCount++]);
            b.CopyTo(out vertices[vertexCount++]);
            c.CopyTo(out vertices[vertexCount++]);
            d.CopyTo(out vertices[vertexCount++]);
        }
        public void AddVertices(Vertex3D[] verts)
        {
            while (vertexCount + verts.Length > vertices.Length)
                Array.Resize(ref vertices, vertexCount * 2);
            Array.Copy(verts, 0, vertices, vertexCount, verts.Length);
        }

        public void AddTriangle(ref Vertex3D a, ref Vertex3D b, ref Vertex3D c)
        {
            while (indexCount + 3 > indices.Length)
                Array.Resize(ref indices, indexCount * 2);
            int i = (int)vertexCount;
            indices[indexCount++] = i;
            indices[indexCount++] = i + 1;
            indices[indexCount++] = i + 2;

            AddVertices(ref a, ref b, ref c);
        }
        public void AddTriangle(Vertex3D a, Vertex3D b, Vertex3D c)
        {
            AddTriangle(ref a, ref b, ref c);
        }

        public void AddQuad(ref Vertex3D a, ref Vertex3D b, ref Vertex3D c, ref Vertex3D d)
        {
            while (indexCount + 6 > indices.Length)
                Array.Resize(ref indices, indexCount * 2);
            int i = (int)vertexCount;
            indices[indexCount++] = i;
            indices[indexCount++] = i + 1;
            indices[indexCount++] = i + 2;
            indices[indexCount++] = i;
            indices[indexCount++] = i + 2;
            indices[indexCount++] = i + 3;

            AddVertices(ref a, ref b, ref c, ref d);
        }
        public void AddQuad(Vertex3D a, Vertex3D b, Vertex3D c, Vertex3D d)
        {
            AddQuad(ref a, ref b, ref c, ref d);
        }
    }
}