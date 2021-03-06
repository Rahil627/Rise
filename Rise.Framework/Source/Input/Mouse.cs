﻿using System;
namespace Rise
{
    public delegate void MouseMoveEvent();
    public delegate void MouseScrollEvent();
    public delegate void MouseButtonEvent(MouseButton button);

    public static class Mouse
    {
        public static event MouseMoveEvent OnMove;
        public static event MouseScrollEvent OnScroll;
        public static event MouseButtonEvent OnPress;
        public static event MouseButtonEvent OnRelease;

        public static Point2 LastPosition { get; private set; }
        public static Point2 Position { get; private set; }
        public static Point2 Scroll { get; private set; }

        static bool[] down = new bool[16];
        static bool[] pressed = new bool[16];
        static bool[] released = new bool[16];

        internal static void Init()
        {
            PreUpdate();
            LastPosition = Position;

            App.platform.OnMouseButtonDown += id =>
            {
                down[id] = true;
                pressed[id] = true;
                OnPress?.Invoke((MouseButton)id);
            };

            App.platform.OnMouseButtonUp += id =>
            {
                down[id] = false;
                released[id] = true;
                OnRelease?.Invoke((MouseButton)id);
            };

            App.platform.OnMouseScroll += (x, y) =>
            {
                Scroll += new Point2((int)(x / Screen.PixelSize), (int)(y / Screen.PixelSize));
                OnScroll?.Invoke();
            };
        }

        internal static void PreUpdate()
        {
            LastPosition = Position;

            int mx, my;
            App.platform.GetMousePosition(out mx, out my);
            mx = (int)((mx - Screen.X) / Screen.PixelSize);
            my = (int)((my - Screen.Y) / Screen.PixelSize);
            Position = new Point2(mx, my);

            if (LastPosition != Position)
                OnMove?.Invoke();
        }

        internal static void PostUpdate()
        {
            Scroll = Point2.Zero;

            for (int i = 0; i < 5; ++i)
                pressed[i] = released[i] = false;
        }

        public static bool Down(MouseButton button)
        {
            return down[(int)button];
        }

        public static bool Pressed(MouseButton button)
        {
            return pressed[(int)button];
        }

        public static bool Released(MouseButton button)
        {
            return released[(int)button];
        }

        public static bool LeftDown
        {
            get { return Down(MouseButton.Left); }
        }

        public static bool LeftPressed
        {
            get { return Pressed(MouseButton.Left); }
        }

        public static bool LeftReleased
        {
            get { return Released(MouseButton.Left); }
        }

        public static bool RightDown
        {
            get { return Down(MouseButton.Right); }
        }

        public static bool RightPressed
        {
            get { return Pressed(MouseButton.Right); }
        }

        public static bool RightReleased
        {
            get { return Released(MouseButton.Right); }
        }

        public static bool MiddleDown
        {
            get { return Down(MouseButton.Middle); }
        }

        public static bool MiddlePressed
        {
            get { return Pressed(MouseButton.Middle); }
        }

        public static bool MiddleReleased
        {
            get { return Released(MouseButton.Middle); }
        }

        public static int X
        {
            get { return Position.X; }
        }

        public static int Y
        {
            get { return Position.Y; }
        }

        public static int ScrollX
        {
            get { return Scroll.X; }
        }

        public static int ScrollY
        {
            get { return Scroll.Y; }
        }
    }

    public enum MouseButton
    {
        Left = 1,
        Middle = 2,
        Right = 3,
        X1 = 4,
        X2 = 5
    }
}
