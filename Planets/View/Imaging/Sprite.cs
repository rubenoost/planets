using System;
using System.Collections.Generic;
using System.Drawing;

namespace Planets.View.Imaging
{
    public class Sprite
    {
        public const int Player = 0;
        public const int Background = 1;
        public const int Cursor = 2;
        public const int BlackHole = 3;
        public const int CometTail = 4;
        public const int Stasis = 5;
        public const int BlackHoleExplosion = 6;
        public const int Sprity = 7;
        public const int Tardis = 8;
        public const int Stars = 10;


        /// <summary>
        ///     Bitmap die wordt teruggegeven als er geen logisch alternatief is.
        /// </summary>
        public static readonly Bitmap Empty = new Bitmap(1, 1);

        public int Width
        {
            get { return _bm.Width; }
        }

        public int Height
        {
            get { return _bm.Height; }
        }

        private readonly Bitmap _bm;

        public Sprite(Bitmap bm)
        {
            _bm = bm;
        }

        /// <summary>
        ///     Gets the amount of columns in this sprite.
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        ///     Gets the amount of rows in this sprite.
        /// </summary>
        public int Rows { get; private set; }

        public static implicit operator Sprite(Bitmap bm)
        {
            return new Sprite(bm);
        }

        public static implicit operator Bitmap(Sprite s)
        {
            return s._bm;
        }
    }
}
