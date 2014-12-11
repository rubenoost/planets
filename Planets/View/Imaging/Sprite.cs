using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Security.Authentication;
using System.Windows.Forms;

namespace Planets.View.Imaging
{
    public class Sprite
    {
        public const int Cursor = 2;
        public const int CometTail = 4;
        public const int Stars = 10;

        public const int Background1 = 405011;
        public const int Background2 = 405012;
        public const int Background3 = 405013;

        /// <summary>
        ///     Bitmap die wordt teruggegeven als er geen logisch alternatief is.
        /// </summary>
        public static readonly Bitmap Empty = new Bitmap(1, 1);

        public int Frames
        {
            get { return Columns * Rows; }
        }

        public int Columns { get; set; }
        public int Rows { get; set; }
        public List<Bitmap> Images { get; set; }
        public bool Cyclic { get; set; }


        public Sprite(List<Bitmap> bm, int columns = 1, int rows = 1, bool cyclic = false)
        {
            Columns = columns;
            Rows = rows;
            Cyclic = cyclic;

            Images = bm;
        }

        public Sprite(Bitmap bm, int columns = 1, int rows = 1, bool cyclic = false)
            : this(Cutsheet(bm, rows, columns), columns, rows, cyclic)
        {
        }

        public static implicit operator Sprite(Bitmap bm)
        {
            return new Sprite(bm);
        }

        public static implicit operator Bitmap(Sprite s)
        {
            return s.Images[0];
        }

        private static List<Bitmap> Cutsheet(Bitmap bm, int rows, int columns)
        {
            List<Bitmap> result = new List<Bitmap>();

            int widthimg = bm.Width;
            int heightimg = bm.Height;
            int subwidth = widthimg / columns;
            int subheight = heightimg / rows;

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    Bitmap subimg = new Bitmap(subwidth, subheight, PixelFormat.Format32bppPArgb);

                    Graphics g = Graphics.FromImage(subimg);

                    Rectangle srcRectangle = new Rectangle((c * subwidth), (r * subheight), subwidth, subheight);

                    g.DrawImage(bm, 0, 0, srcRectangle, GraphicsUnit.Pixel);

                    result.Add(subimg);
                }
            }

            return result;
        }

        public Bitmap GetFrame(int frame)
        {
            if (frame >= this.Frames)
                return Empty;
            return Images[frame];
        }
    }
}
