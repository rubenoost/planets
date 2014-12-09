
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model.GameObjects
{
	class Animation : GameObject
    {
		public Bitmap spriteSheet { get; set; }

		public List<Bitmap> spriteChunks = new List<Bitmap>();

		public int spriteWidth;

		private int spriteIndex = 0;

		public int duration { get; set; }

		public bool continueus { get; set; }

		public Animation(Vector location, Vector velocity, double mass, int d)
			: base(location, velocity, mass, Rule.NONE)
		{
			//this.spritesheet = spritesheet;
			//this.spritewidth = spritewidth;

			this.cutImages();
		}

		private void cutImages()
		{
			for (int i = 0; i < spriteSheet.Width / spriteWidth; i++) {
				Rectangle currentSprite = new Rectangle(i * spriteWidth, 0, spriteWidth, spriteSheet.Height);

				spriteChunks.Add(spriteSheet.Clone(currentSprite, spriteSheet.PixelFormat));
			}
		}

		public void animate()
		{
			spriteIndex = (spriteIndex < spriteChunks.Count - 1) ? spriteIndex + 1 : 0;
		}
    }
}
