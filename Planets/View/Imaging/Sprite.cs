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
        ///     Maakt een nieuwe <c>Sprite</c> aan.
        /// </summary>
        /// <param name="bm">De <see cref="Bitmap" /> van deze <c>Sprite</c>.</param>
        /// <param name="countX">Aantal stukjes om de x-as in te verdelen.</param>
        /// <param name="countY">Aantal stukjes om de y-as in te verdelen.</param>
        /// <param name="cyclic">Of deze <c>Sprite</c> cyclisch moet zijn.</param>
        public Sprite(Bitmap bm, int countX, int countY, bool cyclic)
        {
            // Save variables
            Image = bm;
            Cyclic = cyclic;

            // Check for moving image or static image
            if (countX > 0 && countY > 0 && countX * countY > 1)
            {
                // Has to be cut up
                Columns = countX;
                Rows = countY;
                Images = SpritePool.CutupImage(bm, Rows, Columns);
            }
            else
            {
                // No cutup
                Columns = -1;
                Rows = 1;
            }
        }

        public static implicit operator Sprite(Bitmap bm)
        {
            return new Sprite(bm);
        }

        public static implicit operator Bitmap(Sprite s)
        {
            return s._bm;
        }

        /// <summary>
        ///     Gets the amount of frames in this Sprite. -1 if this is a static image.
        /// </summary>
        public int Frames
        {
            get { return Columns * Rows; }
        }

        /// <summary>
        ///     Gets the amount of columns in this sprite.
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        ///     Gets the amount of rows in this sprite.
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        ///     Gets whether this sprite is cyclic.
        /// </summary>
        public bool Cyclic { get; private set; }

        /// <summary>
        ///     Gets the images of this sprite, please use sprite[index] instead of this list.
        /// </summary>
        private List<Bitmap> Images { get; set; }

        /// <summary>
        ///     Gets the internal image of this sprite
        /// </summary>
        public Bitmap Image { get; private set; }
    }
}
