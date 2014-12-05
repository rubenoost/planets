using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Planets.View.Imaging
{
    public class Sprite
    {
        public const int Cursor = 2;
        public const int CometTail = 4;
        public const int Sprity = 7;
        public const int Stars = 10;

        public const int Background1 = 405011;
        public const int Background2 = 405012;
        public const int Background3 = 405013;


        /// <summary>
        ///     Bitmap die wordt teruggegeven als er geen logisch alternatief is.
        /// </summary>
        public static readonly Bitmap Empty = new Bitmap(1, 1);

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        ///     Gets the amount of columns in this sprite.
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        ///     Gets the amount of rows in this sprite.
        /// </summary>
        public int Rows { get; set; }

        public int Frames
        {
            get { return Rows * Columns; }
        }

        public Bitmap Image;

        public List<Bitmap> FrameList;

        public static implicit operator Sprite(Bitmap bm)
        {
            return new Sprite() { Columns = 1, Rows = 1, Image = bm };
        }

        public static implicit operator Bitmap(Sprite s)
        {
            return s.Image;
        }
    }
}
