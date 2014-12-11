using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using Planets.Model;
using Planets.Model.GameObjects;
using Planets.Properties;

namespace Planets.View.Imaging
{
    public class SpritePool
    {
        private readonly Dictionary<int, Sprite> _imageSource = new Dictionary<int, Sprite>();

        private readonly Dictionary<ImageRequest, Sprite> _imageBuffer = new Dictionary<ImageRequest, Sprite>();

        private int counter = 0;

        public SpritePool()
        {
            RegisterImage(typeof(Player), Resources.Pluto);
            RegisterImage(typeof(BlackHole), Resources.Hole1);
            RegisterImage(typeof(Stasis), Resources.stasis);
            RegisterImage(typeof(Tardis), Resources.Tardis);
            RegisterImage(typeof(Mine), Resources.Pluto_Red);
            RegisterImage(typeof(AntiMatter), Resources.Pluto_Blue);
            RegisterImage(typeof(Antigravity), Resources.Pluto_Green);
            RegisterImage(typeof(Antagonist), Resources.antagonist);
            RegisterImage(typeof(GameObject), Resources.Pluto);
            RegisterImage(typeof(Explosion), Resources.explosion2);

            RegisterImage(Sprite.Background1, Resources.space_wallpaper);
            RegisterImage(Sprite.Background2, Resources.Para1);
            RegisterImage(Sprite.Background3, Resources.Para2);
            RegisterImage(Sprite.CometTail, Resources.KomeetStaartje);
            RegisterImage(Sprite.Cursor, Resources.Cursors_Red);
            RegisterImage(Sprite.Stars, Resources.smallStars);
        }

        private void RegisterImage(Type t, Sprite s)
        {
            RegisterImage(t.GetHashCode(), s);
        }

        private void RegisterImage(int id, Sprite s)
        {
            _imageSource.Add(id, s);
        }

        public Sprite GetSprite(Type t, int width, int height, int rotation = 0)
        {
            return GetSprite(t.GetHashCode(), width, height, rotation);
        }

        public Sprite GetSprite(int imageId, int width, int height, int rotation = 0)
        {
            // Check for drawing size 0
            if (width == 0 || height == 0) return new Bitmap(1, 1);

            // Normalize rotation
            rotation = rotation % 360;

            ImageRequest i = new ImageRequest(imageId, width, height, rotation);
            Sprite s;
            _imageBuffer.TryGetValue(i, out s);
            if (s != null)
                return s;

            s = CreateImage(i);
            _imageBuffer.Add(i, s);

            return s;
        }

        private Sprite CreateImage(ImageRequest i)
        {
            if (i.r != 0)
            {
                return RotateImg(CreateImage(new ImageRequest(i.no, i.w, i.h, 0)), i.r);
            }
            else
            {
                Sprite sourceSprite = _imageSource[i.no];
                return ResizeImg(sourceSprite, i.w, i.h);
            }
        }

        private static Sprite ResizeImg(Sprite s, int width, int height)
        {
            if (s.Frames == 1)
            {
                return ResizeImg((Bitmap)s, width, height);
            }
            else
            {
                List<Bitmap> resized = new List<Bitmap>();

                foreach (Bitmap bm in s.Images)
                {
                    resized.Add(ResizeImg(bm, width, height));
                }

                return new Sprite(resized,s.Columns,s.Rows,s.Cyclic);
            }
        }

        private static Sprite ResizeImg(Bitmap s, int width, int height)
        {
            // Create result image
            var result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(result);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(s, new Rectangle(0, 0, width, height), new Rectangle(0, 0, s.Width, s.Height),
                GraphicsUnit.Pixel);
            return result;
        }

        private static Sprite RotateImg(Sprite s, int angle)
        {
            if (s.Frames == 1)
            {
                return RotateImg((Bitmap)s, angle);
            }
            else
            {
                List<Bitmap> resized = new List<Bitmap>();

                foreach (Bitmap bm in s.Images)
                {
                    resized.Add(RotateImg(bm, angle));
                }

                return new Sprite(resized, s.Columns, s.Rows, s.Cyclic);
            }
        }

        private static Bitmap RotateImg(Bitmap bmp, int angle)
        {
            double size = Math.Max(bmp.Width, bmp.Height);
            Bitmap result = new Bitmap((int)size, (int)size, PixelFormat.Format32bppPArgb);
            Graphics g = Graphics.FromImage(result);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.TranslateTransform((float)(size / 2), (float)(size / 2));
            g.RotateTransform(-angle);
            g.TranslateTransform((float)(-size / 2), (float)(-size / 2));
            g.DrawImageUnscaled(bmp, 0, 0);
            return result;
        }


    }
}
