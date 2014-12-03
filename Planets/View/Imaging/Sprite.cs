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
