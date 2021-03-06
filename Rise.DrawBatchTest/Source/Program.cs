﻿using System;
using System.Collections.Generic;
namespace Rise.DrawBatchTest
{
    static class Program
    {
        static Atlas atlas;
        static AtlasTiles icons;
        static DrawBatch2D batch;

        public static void Main(string[] args)
        {
            App.OnInit += Init;
            App.OnUpdate += Update;
            App.OnRender += Render;
            App.Init<PlatformSDL.PlatformSDL2>();
            App.Run("DrawBatch Test", 960, 540, null);
        }

        static void Init()
        {
            Screen.ClearColor = 0x1e4e50ff;

            Texture.DefaultMinFilter = TextureFilter.Nearest;
            Texture.DefaultMagFilter = TextureFilter.Nearest;

            var builder = new AtlasBuilder(1024);

            //Pack a font
            var font = new Font("Assets/NotoSans-Regular.ttf", FontCharSet.BasicLatin);
            var size32 = new FontSize(font, 32);
            builder.AddFont("font", size32, true);

            //Pack a bunch of icons
            builder.AddTiles("Assets/icons.png", 16, 16, true, true);

            atlas = builder.Build(1);
            if (atlas == null)
                throw new Exception("Failed to build the atlas.");

            icons = atlas.GetTiles("icons");

            batch = new DrawBatch2D();
        }

        static int fpsCount;
        static double fpsAverage;

        static void Update()
        {
            if (fpsCount < 10)
            {
                fpsAverage += App.FPS;
                ++fpsCount;
            }
            else
            {
                Console.WriteLine("FPS: " + Math.Round(fpsAverage / fpsCount));
                fpsCount = 0;
                fpsAverage = 0;
            }
        }

        static void Render()
        {
            //Create a camera matrix (0,0 is the center of the camera, and we'll zoom in 5x)
            var camera = Matrix4x4.CreateTransform2D(Screen.Center, Vector2.Zero, Vector2.One, 0f);

            //The inverse of our camera matrix is how we convert screen to world coordinates
            var screenToWorld = camera.Inverse;

            //This is the cutoff region that the camera can see (accounting for rotation)
            var cameraBounds = screenToWorld.TransformRect(new Rectangle(Screen.Width, Screen.Height));

            //Now let's draw a bunch of sprites, batched
            batch.Begin(null, null, camera, BlendMode.Premultiplied);
            {
                Rand.SetSeed(123);
                for (int i = 0; i < 15000; ++i)
                {
                    var icon = icons.GetTile(i % 5);
                    var pos = Rand.PointInRect(ref cameraBounds);
                    pos.X -= icon.Width / 2f;
                    pos.Y -= icon.Height / 2f;
                    batch.DrawImage(icon, pos, Color4.White);
                }
            }
            batch.End();
        }
    }
}
