using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Planets.Model;
using Planets.Properties;

namespace Planets.View.Imaging
{
    class Animation
    {
        public List<Animation> animations; 
        private bool cyclic;

        public Animation(string name, bool Cyclic)
        {
            animations.Add(this);
            Cyclic = cyclic;
        }


        public void PlayAnimation(Animation anim, Graphics g)
        {
            
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

        public Bitmap PickFrame(Bitmap bmp, int columns, int rows, int frame)
        {
            List<Bitmap> result = CutupImage(bmp, columns, rows);
            return result[frame];
        }
    }
}
