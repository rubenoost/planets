﻿using System;
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
        /// Maakt een nieuwe <c>Sprite</c> aan.
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
                Images = CutupImage(bm, Rows, Columns);
            }
            else
            {
                // No cutup
                Columns = -1;
                Rows = 1;
            }
        }

        public Sprite()
        {
        }

        /// <summary>
        ///     Returns the given frame.
        /// </summary>
        /// <param name="index">-1.0f for static, between 0.0f (inclusive) and 1.0f (exclusive) (or higher if cyclic) for dynamic.</param>
        /// <returns></returns>
        public Bitmap this[float index]
        {
            get
            {
                // Als index lager is dan -1 => Ongeldig
                if (index < -1.0f)
                {
                    throw new ArgumentOutOfRangeException("Cannot get frame " + index);
                }
                // Index == -1, geef interne plaatje
                if (Math.Abs(Frames - (-1.0f)) < 0.01f)
                {
                    return Image;
                }
                // Als niet cyclisch...
                if (!Cyclic)
                {
                    // En index te groot, geef empty
                    if (index >= 1.0f)
                        return Empty;
                    // Anders het correcte frame
                    if (index < 0.0f) index = 0.0f;
                    return Images[(int)(index * Images.Count)];
                }
                // Is cyclisch, dus fix index
                index = index - (int)index;
                // Geef correct frame terug
                return Images[(int)(index * Images.Count)];
            }
        }

        /// <summary>
        ///     Helper method to cut up images.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
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
                    using (Graphics g = Graphics.FromImage(subImage))
                        g.DrawImage(bitmap, targetRectangle, new Rectangle(new Point(j * s.Width, i * s.Height), s),
                            GraphicsUnit.Pixel);
                    // Add to result
                    result.Add(subImage);
                }
            }
            return result;
        }

        /// <summary>
        ///     Gets whether this sprite is cyclic.
        /// </summary>
        public bool Cyclic { get; private set; }

        /// <summary>
        ///     Gets the images of this sprite, please use sprite[index] instead of this list.
        /// </summary>
        private List<Bitmap> Images { get; set; }

        /// <summary>
        ///     Create non-cyclic sprite, cut in pieces.
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="countX"></param>
        /// <param name="countY"></param>
        public Sprite(Bitmap bm, int countX = -1, int countY = -1)
            : this(bm, countX, countY, false)
        {
        }


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
