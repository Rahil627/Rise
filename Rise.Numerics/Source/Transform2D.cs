﻿using Rise;
using System;

namespace Leaf
{
    public class Transform2D
    {
        public event Action OnChanged;

        private Transform2D parent;
        private Vector2 position;
        private Vector2 scale = Vector2.One;
        private float rotation;
        private Matrix3x2 matrix;
        private bool dirty = true;

        private void MakeDirty()
        {
            dirty = true;
            OnChanged?.Invoke();
        }

        public Transform2D Parent
        {
            get { return parent; }
            set
            {
                if (parent != value)
                {
                    if (parent != null)
                        parent.OnChanged -= MakeDirty;

                    parent = value;

                    if (parent != null)
                        parent.OnChanged += MakeDirty;

                    MakeDirty();
                }

            }
        }

        public float X
        {
            get { return position.X; }
            set
            {
                if (position.X != value)
                {
                    position.X = value;
                    MakeDirty();
                }
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                if (position.Y != value)
                {
                    position.Y = value;
                    MakeDirty();
                }
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    MakeDirty();
                }
            }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                if (scale != value)
                {
                    scale = value;
                    MakeDirty();
                }
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                if (rotation != value)
                {
                    rotation = value;
                    MakeDirty();
                }
            }
        }

        public Matrix3x2 Matrix
        {
            get
            {
                if (dirty)
                {
                    matrix = Matrix3x2.Scale(scale) * Matrix3x2.Rotation(rotation) * Matrix3x2.Translation(position);
                    if (parent != null)
                        matrix = parent.matrix * matrix;
                    dirty = false;
                }

                return matrix;
            }
        }

        public Vector2 GlobalPosition
        {
            get
            {
                if (parent != null)
                    return parent.matrix.TransformPoint(position);
                return position;
            }
        }

    }
}