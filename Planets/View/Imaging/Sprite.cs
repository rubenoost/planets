using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Planets.View.Imaging
{
    public class Sprite
    {
        public const int Cursor = 2;
        public const int CometTail = 4;
        public const int Stars1 = 10;
        public const int Stars2 = 20;
        public const int Stars3 = 40;
        public const int Stars4 = 80;
        public const int Stars5 = 160;
        public const int Stars6 = 320;

        public const int Background1 = 405011;

        /// <summary>
        ///     Bitmap die wordt teruggegeven als er geen logisch alternatief is.
        /// </summary>
        private static readonly Bitmap Empty = new Bitmap(1, 1);

        public int Frames
        {
            get { return Columns * Rows; }
        }

        public int Columns;
        public int Rows;
        public List<Bitmap> Images;
        public bool Cyclic;


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
            if (frame >= Frames)
                return Empty;
            return Images[frame];
        }
    }
}
