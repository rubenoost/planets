using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Planets.Properties;

namespace Planets.View.Imaging
{
    public struct ImageRequest
    {
        public readonly int no;
        public readonly int w;
        public readonly int h;
        public readonly int r;
        public readonly bool a;

        public ImageRequest(int index, int width, int height, int rotation, bool animated)
        {
            no = index;
            w = width;
            h = height;
            r = rotation;
            a = animated;
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
            _imageSource.Add(Sprite.Background, Resources.space_wallpaper);
            _imageSource.Add(Sprite.CometTail, Resources.KomeetStaartje);
            _imageSource.Add(Sprite.Cursor, Resources.Cursors_Red);
            _imageSource.Add(Sprite.Stars, Resources.smallStars);
            _imageSource.Add(Sprite.Sprity, Resources.spritety);
            _imageSource.Add(Sprite.Stasis, Resources.pooface);
        }

        public Sprite GetSprite(int imageId, int width, int height, int rotation = 0, bool animated = false)
        {
            // Check for drawing size 0
            if (width == 0 || height == 0) return new Bitmap(1, 1);

            // Normalize rotation
            rotation = rotation % 360;

            ImageRequest i = new ImageRequest(imageId, width, height, rotation, animated);
            Sprite s;
            _imageBuffer.TryGetValue(i, out s);
            if (s != null && i.a != true)
                return s;

            s = CreateImage(i);

            // Als die geanimeerd is niet bufferen
            // Moet wel maar werkt niet
            if (i.a != true)
            {
                _imageBuffer.Add(i, s);
            }
            return s;
        }

        private Sprite CreateImage(ImageRequest i)
        {
            // Check for rotation
            if (i.r > 1)
            {
                // Create result image
                Bitmap b = GetSprite(i.no, i.w, i.h);
                return RotateImg(b, i.r);
            }
            // Check if image is animated
            if (i.a)
            {
                // Dirty code
                Bitmap b = GetSprite(i.no, i.w, i.h);

                // Image met maar 2 bij 2 sprites
                List<Bitmap> frameList = CutupImage(b, 2, 2);

                // Wanneer counter lager is dan beschikbare frames
                // Geef frame terug en doe counter+1
                // Sprite blijft animated
                if (counter < frameList.Count)
                {
                    counter += 1;
                }
                if (counter == frameList.Count)
                {
                    counter = 0;
                }
                return frameList[counter];
            }
            else
            {
                // Create result image
                var result = new Bitmap(i.w, i.h, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(result);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Bitmap b = _imageSource[i.no];
                g.DrawImage(b, new Rectangle(0, 0, i.w, i.h), new Rectangle(0, 0, b.Width, b.Height),
                    GraphicsUnit.Pixel);
                return result;
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

        private static List<Bitmap> CutupImage(Image bitmap, int rows, int columns)
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
