using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Planets.Properties;

namespace Planets.View.Imaging
{
    public struct ImageRequest
    {
        public readonly int no;
        public readonly int w;
        public readonly int h;
        public readonly int r;

        public ImageRequest(int index, int width, int height, int rotation)
        {
            no = index;
            w = width;
            h = height;
            r = rotation;
        }

        public override int GetHashCode()
        {
            int i = 23;
            i = i * 486187739 + no;
            i = i * 486187739 + w;
            i = i * 486187739 + h;
            i = i * 486187739 + r;
            return i;
        }

        public override bool Equals(object o)
        {
            if (!(o is ImageRequest))
                return false;

            var i = (ImageRequest)o;

            return no == i.no && w == i.w && h == i.h && r == i.r;
        }
    }

    public class SpritePool
    {
        private readonly Dictionary<int, Sprite> _imageSource = new Dictionary<int, Sprite>();

        private readonly Dictionary<ImageRequest, Sprite> _imageBuffer = new Dictionary<ImageRequest, Sprite>();

        private int counter = 0;

        public SpritePool()
        {
            _imageSource.Add(Sprite.Player, Resources.Pluto);
            _imageSource.Add(Sprite.BlackHole, Resources.Hole1);
            _imageSource.Add(Sprite.Background1, Resources.space_wallpaper);
            _imageSource.Add(Sprite.Background2, Resources.Para1);
            _imageSource.Add(Sprite.Background3, Resources.Para2);
            _imageSource.Add(Sprite.CometTail, Resources.KomeetStaartje);
            _imageSource.Add(Sprite.Cursor, Resources.Cursors_Red);
            _imageSource.Add(Sprite.Stars, Resources.smallStars);
            _imageSource.Add(Sprite.Sprity, Resources.spritety);
            _imageSource.Add(Sprite.Stasis, Resources.stasis);
            _imageSource.Add(Sprite.Tardis, Resources.Tardis);
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
                return new Sprite { Columns = 1, Rows = 1, Image = ResizeImg(s.Image, width, height) };

            Sprite result = new Sprite { Columns = s.Columns, Rows = s.Rows, Image = s.Image, FrameList = new List<Bitmap>() };
            foreach (Bitmap bm in s.FrameList)
                result.FrameList.Add(ResizeImg(bm, width, height));
            return result;
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
                return new Sprite { Columns = 1, Rows = 1, Image = RotateImg(s.Image, angle) };

            Sprite result = new Sprite { Columns = s.Columns, Rows = s.Rows, Image = s.Image, FrameList = new List<Bitmap>() };
            foreach (Bitmap bm in s.FrameList)
                result.FrameList.Add(RotateImg(bm, angle));
            return result;
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


        private static Bitmap PickFrame(Bitmap bmp, int columns, int rows, int frame)
        {
            List<Bitmap> result = CutupImage(bmp, columns, rows);
            return result[frame];
        }

        private static List<Bitmap> CutupImage(Image bitmap, int columns, int rows)
        {
            // Determine target
            var s = new Size(bitmap.Width / columns, bitmap.Height / rows);
            var targetRectangle = new Rectangle(new Point(0, 0), s);

            // Create result
            var result = new List<Bitmap>(s.Height * s.Width);

            // Cut up image
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Create new Bitmap
                    var subImage = new Bitmap(s.Width, s.Height);

                    // Draw scaled image
                    Graphics g = Graphics.FromImage(subImage);
                    g.DrawImage(bitmap, targetRectangle, new Rectangle(new Point(j * s.Width, i * s.Height), s),
                        GraphicsUnit.Pixel);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // Add to result
                    result.Add(subImage);
                }
            }
            return result;
        }


    }
}
