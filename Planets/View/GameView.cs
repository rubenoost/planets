using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.Properties;
using Planets.View.Imaging;

namespace Planets.View
{
    public partial class GameView : UserControl
    {
        public static readonly bool EnableScaling = true;

        public new float Scale = 0.8f;

        public bool DiscoMode = false;

        Playfield field;

        private SpritePool sp = new SpritePool();

        private static readonly double MaxArrowSize = 150;
        private static readonly double MinArrowSize = 50;

        private float ParallaxDepth = 0.0f;

        // Aiming Settings
        /// <summary>
        /// If true, a vector will be drawn to show the current trajectory
        /// </summary>
        public bool IsAiming;
        public Vector AimPoint;

        // Aiming pen buffer
        private Pen CurVecPen = new Pen(Color.Red, 5);
        private Pen NextVecPen = new Pen(Color.Green, 5);
        private Pen AimVecPen = new Pen(Color.White, 5);
        private Pen BorderPen = new Pen(new TextureBrush(Resources.Texture), 10.0f);

        // Wordt gebruikt voor bewegende achtergrond
        private int _blackHoleAngle = 0;

        public GameView(Playfield field)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.field = field;
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
            CurVecPen.CustomEndCap = bigArrow;
            NextVecPen.CustomEndCap = bigArrow;
            AimVecPen.DashPattern = new float[] { 10 };
            AimVecPen.DashStyle = DashStyle.Dash;
            AimVecPen.CustomEndCap = bigArrow;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Draw static back layer
            DrawBackLayers(g);

            // Draw top layer
            DrawBorder(g);
            lock (field.BOT)
            {
                field.BOT.Iterate(obj => DrawGameObject(g, obj));
                DrawAimVectors(g);
                DrawDemo(g);
                DrawDebug(g);
            }
        }

        #region Draw Functions

        private void DrawBackLayers(Graphics g)
        {
            // Draw background
            Rectangle target;

            ParallaxDepth = 0.0f;
            target = GameToScreen(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Background, ClientSize.Width, ClientSize.Height), target);

            ParallaxDepth = 0.25f;
            target = GameToScreen(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars, ClientSize.Width, ClientSize.Height), target);

            ParallaxDepth = 0.5f;
            target = GameToScreen(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars, ClientSize.Width, ClientSize.Height), target);

            ParallaxDepth = 0.75f;
            target = GameToScreen(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars, ClientSize.Width, ClientSize.Height), target);
        }

        private void DrawBorder(Graphics g)
        {
            ParallaxDepth = 1.0f;
            Rectangle rp = GameToScreen(new Rectangle(new Point(), field.Size));
            g.DrawRectangle(BorderPen, rp.X - BorderPen.Width / 2, rp.Y - BorderPen.Width / 2, rp.Width + BorderPen.Width, rp.Height + BorderPen.Width);
        }

        private void DrawAimVectors(Graphics g)
        {
            GameObject obj = field.CurrentPlayer;
            if (IsAiming)
            {
                Vector CursorPosition = Cursor.Position;
                AimPoint = obj.Location - CursorPosition;

                Vector CurVec = obj.Location + obj.DV.ScaleToLength(obj.Radius + Math.Min(MaxArrowSize, Math.Max(obj.DV.Length(), MinArrowSize)));
                // Draw current direction vector
                g.DrawLine(CurVecPen, GameToScreen(obj.Location + obj.DV.ScaleToLength(obj.Radius + 1)), GameToScreen(CurVec));

                // Draw aim direction vector
                Vector AimVec = obj.Location + AimPoint.ScaleToLength(obj.Radius + Math.Min(MaxArrowSize, Math.Max(obj.DV.Length(), MinArrowSize)));
                g.DrawLine(AimVecPen, GameToScreen(obj.Location + AimPoint.ScaleToLength(obj.Radius + 1)), GameToScreen(AimVec));

                // Draw next direction vector
                Vector NextVec = ShootProjectileController.CalcNewDV(obj, new GameObject(new Vector(0, 0), new Vector(0, 0), 0.05 * obj.Mass), Cursor.Position);
                g.DrawLine(NextVecPen, GameToScreen(obj.Location + NextVec.ScaleToLength(obj.Radius + 1.0)), GameToScreen(obj.Location + NextVec.ScaleToLength(obj.Radius + Math.Min(MaxArrowSize, Math.Max(obj.DV.Length(), MinArrowSize)))));
            }
        }

        private void DrawGameObject(Graphics g, GameObject obj)
        {
            float radius = (float)obj.Radius;
            int length = (int)(radius * 2);

            // Calculate player
            /*if (obj.DV.Length() > 1.0)
            {
                int angleO = 0;
                angleO = (int)(Math.Atan2(obj.DV.X, obj.DV.Y) / Math.PI * 180.0);
                // Retrieve sprites
                Sprite cometSprite = sp.GetSprite(Sprite.CometTail, length * 4, length * 4, angleO + 180);
                g.DrawImageUnscaled(cometSprite, (int)(obj.Location.X - cometSprite.Width / 2), (int)(obj.Location.Y - cometSprite.Height / 2));
            }*/

            // Get sprite
            int spriteID;
            int objAngle = 0;

            if (obj == field.CurrentPlayer)
            {
                spriteID = Sprite.Player;
            }
            else if (obj is BlackHole)
            {
                spriteID = Sprite.BlackHole;
                objAngle = _blackHoleAngle;
                _blackHoleAngle++;
            } 
			else if (obj is Antigravity) 
			{
				spriteID = Sprite.BlackHole;
			}
            else if (obj is AntiMatter)
            {
                spriteID = Sprite.BlackHole;
            }
            else
            {
                spriteID = Sprite.Player;
            }

            // Draw object
            Rectangle target = GameToScreen(obj.BoundingBox);

            if (!DiscoMode)
            {
                Sprite s = sp.GetSprite(spriteID, target.Width, target.Height, objAngle);
                g.DrawImageUnscaled(s, target);
            }

            if (DiscoMode)
            {
                Sprite s1 = sp.GetSprite(Sprite.Sprity, 8, 8, 0, true);
                g.DrawImage(s1, target);
            }
        }

        private void DrawDemo(Graphics g)
        {
            // Drawing the autodemo
            double f = (DateTime.Now - field.LastAutoClickMoment).TotalMilliseconds;
            if (f < 1000)
            {
                int r = 20 + (int)(f / 10);
                Rectangle autoDemoEffectTarget = new Rectangle(field.LastAutoClickGameLocation.X - r / 2, field.LastAutoClickGameLocation.Y - r / 2, r, r);
                g.FillEllipse(new SolidBrush(Color.FromArgb((int)(255 - f / 1000 * 255), 255, 0, 0)), autoDemoEffectTarget);
                Point cursorPixelPoint = field.LastAutoClickGameLocation;
                g.DrawImageUnscaled(sp.GetSprite(Sprite.Cursor, 100, 100), cursorPixelPoint.X - 4, cursorPixelPoint.Y - 10);
            }
        }

        private void DrawDebug(Graphics g)
        {
            if (Debug.Enabled)
            {
                using (Pen p = new Pen(Color.OrangeRed, 2.0f))
                {
                    field.BOT.DoCollisions((go1, go2, ms) => g.DrawLine(p, go1.Location, go2.Location), 0);
                }

                int d = field.BOT.Count;
                int d2 = (d - 1) * d / 2;

                using (Brush b = new SolidBrush(Color.Magenta))
                {
                    Font f = new Font(FontFamily.GenericSansSerif, 16.0f, FontStyle.Bold);
                    g.DrawString("Regular Collision Detection: " + d2, f, b, 100, 300);
                    g.DrawString("Binary Tree Collision Detection: " + (field.BOT.ColCount), f, b, 100, 320);
                    g.DrawString("Collision Detection improvement: " + (d2 - field.BOT.ColCount) * 100 / d2 + "%", f, b, 100, 340);
                }
            }
        }

        #endregion

        #region Game / Screen conversions

        public Vector ViewCenter()
        {
            Vector pixelCenter = new Vector(ClientSize.Width / 2, ClientSize.Height / 2);
            Vector gameCenter = field.CurrentPlayer.Location;
            return gameCenter * ParallaxDepth + pixelCenter * (1.0 - ParallaxDepth);
        }

        public Vector ViewSize()
        {
            var pixelSize = new Vector(ClientSize.Width, ClientSize.Height);
            var gameSize = pixelSize / Scale;
            var corrSize = gameSize * ParallaxDepth + pixelSize * (1.0 - ParallaxDepth);
            return corrSize;
        }

        public Rectangle ViewRectangle()
        {
            Point p = ViewCenter() - ViewSize() / 2;
            Size s = new Size((int)ViewSize().X, (int)ViewSize().Y);
            return new Rectangle(p, s);
        }

        public Rectangle GameToScreen(Rectangle gameRectangle)
        {
            Rectangle view = ViewRectangle();

            double scaleX = (double)view.Width / ClientSize.Width;
            double scaleY = (double)view.Height / ClientSize.Height;

            Vector viewCenter = ViewCenter();
            Vector rectangleGameCenter = new Vector(gameRectangle.X + gameRectangle.Width / 2, gameRectangle.Y + gameRectangle.Height / 2);
            Vector relativeRectangleGameCenter = rectangleGameCenter - viewCenter;
            Vector relativeRectanglePixelCenter = new Vector(relativeRectangleGameCenter.X * scaleX, relativeRectangleGameCenter.Y * scaleY);
            Vector rectanglePixelCenter = relativeRectanglePixelCenter + new Vector(ClientSize.Width/2, ClientSize.Height/2);

            var pixelSize = new Size((int)(gameRectangle.Width * scaleX), (int)(gameRectangle.Height * scaleY));
            var pixelLocation = new Point((int) (rectanglePixelCenter.X - pixelSize.Width / 2), (int) (rectanglePixelCenter.Y - pixelSize.Height / 2));
            return new Rectangle(pixelLocation, pixelSize);
        }

        public Vector GameToScreen(Vector v)
        {
            Rectangle view = ViewRectangle();
            double scaleX = (double)view.Width / ClientSize.Width;
            double scaleY = (double)view.Height / ClientSize.Height;

            Vector viewCenter = ViewCenter();
            Vector relativeGamePoint = v - viewCenter;
            Vector relativePixelPoint = new Vector(relativeGamePoint.X * scaleX, relativeGamePoint.Y * scaleY);
            Vector pixelPoint = new Vector(ClientSize.Width / 2, ClientSize.Height / 2) + relativePixelPoint;
            return pixelPoint;
        }

        public Vector ScreenToGame(Vector v)
        {
            Rectangle view = ViewRectangle();
            double scaleX = (double)view.Width / ClientSize.Width;
            double scaleY = (double)view.Height / ClientSize.Height;

            var relativePixelPoint = v - new Vector(ClientSize.Width/2, ClientSize.Height/2);
            var relativeGamePoint = new Vector(relativePixelPoint.X/scaleX, relativePixelPoint.Y/scaleY);

            return relativeGamePoint + ViewCenter();
        }
        #endregion
    }
}
