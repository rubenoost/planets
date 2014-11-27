using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using Planets.Properties;

namespace Planets.View.Imaging
{
    public struct ImageRequest
    {
        public readonly int w;
        public readonly int h;
        public readonly int r;
        public ImageRequest(int width, int height, int rotation)
        {
            w = width;
            h = height;
            r = rotation;
        }

        public override int GetHashCode()
        {
            int i = 23;
            i = i * 17 + w;
            i = i * 17 + h;
            i = i * 17 + r;
            return i;
        }

        public override bool Equals(object o)
        {
            if (o == null)
                return false;
            if (!(o is ImageRequest))
                return false;

            var i = (ImageRequest)o;

            return w == i.w && h == i.h && r == i.r;
        }
    }

    public class SpritePool
    {
        private Dictionary<int, Sprite> _imageSource = new Dictionary<int, Sprite>();

        private Dictionary<ImageRequest, Sprite> _imageBuffer = new Dictionary<ImageRequest, Sprite>();

        public SpritePool()
        {
            _imageSource.Add(Sprite.Player, Resources.Pluto);
            _imageSource.Add(Sprite.BlackHole, Resources.Hole1);
            _imageSource.Add(Sprite.Background, Resources.LogoFinal);
        }

        public Sprite GetSprite(int imageId, int width, int height, int rotation = 0)
        {
            // Check for drawing size 0
            if (width == 0 || height == 0) return new Bitmap(1, 1);

            // Normalize rotation
            rotation = rotation % 360;

            ImageRequest i = new ImageRequest(width, height, rotation);
            Sprite s;
            _imageBuffer.TryGetValue(i, out s);
            if (s != null)
                return s;

            s = CreateImage(_imageSource[imageId], i);
            _imageBuffer.Add(i, s);
            return s;
        }

        private Sprite CreateImage(Bitmap bm, ImageRequest i)
        {
            // Create result image
            var result = new Bitmap(i.w, i.h, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(result);
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(bm, new Rectangle(0, 0, i.w, i.h), new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

            if (i.r == 0)
                return result;
            return RotateImg(result, i.r);
        }

        public static Bitmap RotateImg(Bitmap bmp, int angle)
        {
            double scale = Math.PI/180.0;
            double r = angle*scale;

            double c = Math.Cos(r);
            double s = Math.Sin(r);

            double x = bmp.Width / 2;
            double y = bmp.Height/2;

            double size = Math.Max(bmp.Width, bmp.Height);

            Bitmap result = new Bitmap((int) size, (int)size);
            Graphics g = Graphics.FromImage(result);

            g.TranslateTransform((float) (size / 2), (float) (size / 2));
            g.RotateTransform(angle);
            g.TranslateTransform((float)(-size / 2), (float)(-size / 2));
            g.DrawImageUnscaled(bmp, 0, 0);
            return result;
        }
    }
}
