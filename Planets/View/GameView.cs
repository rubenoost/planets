using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.Model.GameObjects;
using Planets.Properties;
using Planets.View.Imaging;

namespace Planets.View
{
    public partial class GameView : UserControl
    {
        #region Properties

        private float _propZoom = 2.0f;
        public float Zoom
        {
            get { return _propZoom; }
            set
            {
                if (value >= 1.0f)
                    _propZoom = value;
            }
        }

        #endregion

        Playfield field;

        private SpritePool sp = new SpritePool();

        private static readonly double MaxArrowSize = 150;
        private static readonly double MinArrowSize = 50;

        // Aiming Settings
        /// <summary>
        /// If true, a vector will be drawn to show the current trajectory
        /// </summary>
        public bool IsAiming;
        public Vector AimPoint;

        private SolidBrush ScorePlayerBrush = new SolidBrush(Color.White);
        private Font ScoreFont = new Font(FontFamily.GenericSansSerif, 60.0f, FontStyle.Bold, GraphicsUnit.Pixel);

        // Aiming pen buffer
        private Pen CurVecPen = new Pen(Color.Red, 5);
        private Pen NextVecPen = new Pen(Color.Green, 5);
        private Pen AimVecPen = new Pen(Color.White, 5);
        private Pen BorderPen = new Pen(new TextureBrush(Resources.Texture), 10.0f);

        // Wordt gebruikt voor bewegende achtergrond
        private int _blackHoleAngle;

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

            DrawScores(g);
            DrawHud(g);

            // Debugging
            _blackHoleAngle++;
        }

        #region Draw Functions

        private void DrawBackLayers(Graphics g)
        {
            // Draw background
            Rectangle target;

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.25f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Background1, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.55f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.85f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars, target.Width, target.Height), target);

        }

        private void DrawBorder(Graphics g)
        {
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
			DrawAnimations(g, obj);

            // Get sprite
            int objAngle = 0;

            // Check for BH
            if (obj is BlackHole)
                objAngle = _blackHoleAngle;

            // Draw object
            Rectangle target = GameToScreen(obj.BoundingBox);
            Sprite s = sp.GetSprite(obj.GetType(), target.Width, target.Height, objAngle);
            g.DrawImageUnscaled(s, target);
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

        private void DrawScores(Graphics g)
        {
            lock (field.sb.Scores)
            {
                for(int i = 0; i < field.sb.Scores.Count; i++ ) {
                    ScorePlayerBrush.Color = field.sb.Scores[i].Color;
                    if(field.sb.Scores[i].Value > 0)
                        g.DrawString(String.Format("+{0}", field.sb.Scores[i].Value), ScoreFont, ScorePlayerBrush, (Point)GameToScreen(field.sb.Scores[i].Location));
                    else
                        g.DrawString(String.Format("{0}", field.sb.Scores[i].Value), ScoreFont, ScorePlayerBrush, (Point)GameToScreen(field.sb.Scores[i].Location));
                    field.sb.Scores[i].UpdateLocation();
                }
            }
        }

        // Draw Score Arc Buff
        private Brush HudBackgroundBrush = new SolidBrush(Color.FromArgb(230, 88, 88, 88));
        private Pen HudArcAccentPen = new Pen(Color.White, 4.0f);
        private Pen HudArcAccentPen2 = new Pen(Color.White, 30.0f);
        private Pen HudArcAccentPen3 = new Pen(Color.White, 22.0f);
        private Font HudScoreFont = new Font(FontFamily.GenericMonospace, 18.0f, FontStyle.Bold, GraphicsUnit.Pixel);
        private Size hudSize = new Size(500, 300);

        // Draw WhatEverMeter buff
        private Pen WhitePen = new Pen(Color.White, 2);

        private void DrawHud(Graphics g)
        {
            // Draw hud background
            Point hudLocation = new Point(ClientSize.Width - hudSize.Width, ClientSize.Height - hudSize.Height);

            // Draw hud
            int featherSize = 50;
            Rectangle target = new Rectangle(hudLocation, hudSize);
            g.FillPie(HudBackgroundBrush, new Rectangle(target.Location, new Size(featherSize * 2, featherSize * 2)), -90.0f, -90.0f);
            g.FillRectangle(HudBackgroundBrush, new Rectangle(target.Left + featherSize - 1, target.Top, target.Width - featherSize, target.Height));
            g.FillRectangle(HudBackgroundBrush, new Rectangle(target.Left, target.Top + featherSize - 1, featherSize, target.Height - featherSize));

            // Draw score arc
            float progress = Math.Min((float) (field.CurrentPlayer.Radius / 250.0f), 1.0f);
            float barSize = 90.0f;

            RectangleF arcRectangle = new RectangleF(
                hudLocation.X,
                (float) (hudLocation.Y + hudSize.Height * 0.2),
                hudSize.Width,
                hudSize.Height * 1.2f);

            Pen HudArcPen = new Pen(new LinearGradientBrush(new PointF(arcRectangle.Left, arcRectangle.Top), new PointF(arcRectangle.Left + arcRectangle.Width, arcRectangle.Top), Color.GreenYellow, Color.DarkOrange), 20.0f);

            RectangleF arcAccentRect = new RectangleF(
                arcRectangle.Left + HudArcPen.Width / 2,
                arcRectangle.Top + HudArcPen.Width / 2,
                arcRectangle.Width - HudArcPen.Width,
                arcRectangle.Height - HudArcPen.Width);

            float diff2 = HudArcAccentPen2.Width - HudArcPen.Width;
            RectangleF arcAccentRect2 = new RectangleF(
                arcRectangle.Left - diff2 / 2,
                arcRectangle.Top - diff2 / 2,
                arcRectangle.Width + diff2,
                arcRectangle.Height + diff2
                );

            float diff3 = HudArcAccentPen3.Width - HudArcPen.Width;
            RectangleF arcAccentRect3 = new RectangleF(
                arcRectangle.Left - diff3 / 2,
                arcRectangle.Top - diff3 / 2,
                arcRectangle.Width + diff3,
                arcRectangle.Height + diff3
                );
            
            float barStart = 270.0f - barSize/2;

            // Draw progress
            g.DrawArc(HudArcPen, arcRectangle, barStart, progress * barSize);

            // Draw meter
            float[] meterPoints = {0.7f, 0.5f, 0.35f, 0.25f, 0.17f, 0.1f, 0.05f};
            g.DrawArc(HudArcAccentPen, arcAccentRect, barStart - 1.0f, barSize + 2.0f);
            g.DrawArc(HudArcAccentPen2, arcAccentRect2, barStart, 1.0f);
            g.DrawArc(HudArcAccentPen2, arcAccentRect2, barStart + barSize - 1.0f, 1.0f);
            foreach (var f in meterPoints)
                g.DrawArc(HudArcAccentPen3, arcAccentRect3, barStart + barSize * f - 0.25f, 0.5f);

            // Draw score text
            

            // Draw Mass-o-meter
            Point MassMeterPoint = new Point(hudLocation.X + 20, hudLocation.Y + 60);

            int MassDrawY = (int)(MassMeterPoint.Y + (230 - field.CurrentPlayer.Radius));

            Point MassDrawPoint = new Point(MassMeterPoint.X, (MassDrawY > MassMeterPoint.Y) ? MassDrawY : MassMeterPoint.Y);
            Brush gradientBrush = new LinearGradientBrush(MassMeterPoint, new Point(MassMeterPoint.X, MassMeterPoint.Y + 230), Color.YellowGreen, Color.DarkOrange);
            g.FillRectangle(gradientBrush, new Rectangle(MassDrawPoint, new Size(15, (int)field.CurrentPlayer.Radius)));
            g.DrawRectangle(WhitePen, new Rectangle(MassMeterPoint, new Size(15, 230)));

            // Draw Whatever-o-meter
            int AmountObjects = (field.BOT.Count - 6) * 4;

            Point WhatEverMeterPoint = new Point(ClientSize.Width - 35, hudLocation.Y + 60);

            int WhatEverDrawY = (int)(WhatEverMeterPoint.Y + (230 - AmountObjects));

            Point WhatEverDrawPoint = new Point(WhatEverMeterPoint.X, (WhatEverDrawY > WhatEverMeterPoint.Y) ? WhatEverDrawY : WhatEverMeterPoint.Y);

            g.FillRectangle(gradientBrush, new Rectangle(WhatEverDrawPoint, new Size(15, (int)AmountObjects)));
            g.DrawRectangle(WhitePen, new Rectangle(WhatEverMeterPoint, new Size(15, 230)));

            // Draw something else

        }

        private void DrawAnimations(Graphics g, GameObject obj)
        {
			Rectangle target = GameToScreen(obj.BoundingBox);
			Sprite s = sp.GetSprite(obj.GetType(), target.Width, target.Height);

			if (s.Cyclic) {
				s.animate();
			}

			g.DrawImageUnscaled(s, target);

            // if there are animations queued by a gamerule
            // get the frame from the spritepool list
            // play the frames

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

        public Vector GameToScreen(Vector v, float ParallaxDepth = 1.0f)
        {
            // The game size associated with each layer
            Vector layerGameSize = new Vector(field.Size.Width, field.Size.Height);

            //=================================== [ Game Center ] =================================

            // The center of the game, if no parallax is present
            Vector noPrlxViewCenter = layerGameSize / 2;

            // The center of the game if parallax is 1.0
            Vector onePrlxViewCenter = field.CurrentPlayer.Location;

            // The corrected center of the game, with any parallaxdepth
            Vector viewCenterGame = onePrlxViewCenter * ParallaxDepth + (1.0f - ParallaxDepth) * noPrlxViewCenter;

            //=================================== [ Game View Size ] ==============================

            // View size with no parallax present
            Vector noPrlxViewSize = layerGameSize;

            // View size if parallax is 1.0
            Vector onePrlxViewSize = layerGameSize / Zoom;

            // Corrected view size with any parallaxdepth
            Vector viewSizeGame = onePrlxViewSize * ParallaxDepth + (1.0f - ParallaxDepth) * noPrlxViewSize;

            //=================================== [ Correct viewing rectangle ] ====================

            viewCenterGame = new Vector(Math.Max(viewSizeGame.X / 2, viewCenterGame.X), Math.Max(viewSizeGame.Y / 2, viewCenterGame.Y));
            viewCenterGame = new Vector(Math.Min(viewCenterGame.X, layerGameSize.X - viewSizeGame.X / 2), Math.Min(viewCenterGame.Y, layerGameSize.Y - viewSizeGame.Y / 2));

            //=================================== [ Scale to pixels ] =============================

            double scaleX = ClientSize.Width / viewSizeGame.X;
            double scaleY = ClientSize.Height / viewSizeGame.Y;

            Vector viewCenterPixel = new Vector(ClientSize.Width / 2, ClientSize.Height / 2);

            Vector pointRelativeToViewCenterGame = v - viewCenterGame;
            Vector pointRelativeToViewCenterPixel = new Vector(pointRelativeToViewCenterGame.X * scaleX, pointRelativeToViewCenterGame.Y * scaleY);

            return pointRelativeToViewCenterPixel + viewCenterPixel;
        }

        public Vector ScreenToGame(Vector v, float ParallaxDepth = 1.0f)
        {
            // The game size associated with each layer
            Vector layerGameSize = new Vector(field.Size.Width, field.Size.Height);

            //=================================== [ Game Center ] =================================

            // The center of the game, if no parallax is present
            Vector noPrlxViewCenter = layerGameSize / 2;

            // The center of the game if parallax is 1.0
            Vector onePrlxViewCenter = field.CurrentPlayer.Location;

            // The corrected center of the game, with any parallaxdepth
            Vector viewCenterGame = onePrlxViewCenter * ParallaxDepth + (1.0f - ParallaxDepth) * noPrlxViewCenter;

            //=================================== [ Game View Size ] ==============================

            // View size with no parallax present
            Vector noPrlxViewSize = layerGameSize;

            // View size if parallax is 1.0
            Vector onePrlxViewSize = layerGameSize / Zoom;

            // Corrected view size with any parallaxdepth
            Vector viewSizeGame = onePrlxViewSize * ParallaxDepth + (1.0f - ParallaxDepth) * noPrlxViewSize;

            //=================================== [ Correct viewing rectangle ] ====================

            viewCenterGame = new Vector(Math.Max(viewSizeGame.X / 2, viewCenterGame.X), Math.Max(viewSizeGame.Y / 2, viewCenterGame.Y));
            viewCenterGame = new Vector(Math.Min(viewCenterGame.X, layerGameSize.X - viewSizeGame.X / 2), Math.Min(viewCenterGame.Y, layerGameSize.Y - viewSizeGame.Y / 2));

            //=================================== [ Scale to pixels ] =============================
            Vector viewCenterPixel = new Vector(ClientSize.Width / 2, ClientSize.Height / 2);
            Vector pointRelativeToViewCenterPixel = v - viewCenterPixel;

            double scaleX = ClientSize.Width / viewSizeGame.X;
            double scaleY = ClientSize.Height / viewSizeGame.Y;

            Vector pointRelativeToViewCenterGame = new Vector(pointRelativeToViewCenterPixel.X / scaleX, pointRelativeToViewCenterPixel.Y / scaleY);

            Vector pointGame = pointRelativeToViewCenterGame + viewCenterGame;

            return pointGame;
        }

        public Rectangle GameToScreen(Rectangle gameRectangle, float ParallaxDepth = 1.0f)
        {
            // The game size associated with each layer
            Vector layerGameSize = new Vector(field.Size.Width, field.Size.Height);

            //=================================== [ Game Center ] =================================

            // The center of the game, if no parallax is present
            Vector noPrlxViewCenter = layerGameSize / 2;

            // The center of the game if parallax is 1.0
            Vector onePrlxViewCenter = field.CurrentPlayer.Location;

            // The corrected center of the game, with any parallaxdepth
            Vector viewCenterGame = onePrlxViewCenter * ParallaxDepth + (1.0f - ParallaxDepth) * noPrlxViewCenter;

            //=================================== [ Game View Size ] ==============================

            // View size with no parallax present
            Vector noPrlxViewSize = layerGameSize;

            // View size if parallax is 1.0
            Vector onePrlxViewSize = layerGameSize / Zoom;

            // Corrected view size with any parallaxdepth
            Vector viewSizeGame = onePrlxViewSize * ParallaxDepth + (1.0f - ParallaxDepth) * noPrlxViewSize;

            //=================================== [ Correct viewing rectangle ] ====================

            viewCenterGame = new Vector(Math.Max(viewSizeGame.X / 2, viewCenterGame.X), Math.Max(viewSizeGame.Y / 2, viewCenterGame.Y));
            viewCenterGame = new Vector(Math.Min(viewCenterGame.X, layerGameSize.X - viewSizeGame.X / 2), Math.Min(viewCenterGame.Y, layerGameSize.Y - viewSizeGame.Y / 2));

            //=================================== [ Scale to pixels ] =============================

            double scaleX = ClientSize.Width / viewSizeGame.X;
            double scaleY = ClientSize.Height / viewSizeGame.Y;

            Vector viewCenterPixel = new Vector(ClientSize.Width / 2, ClientSize.Height / 2);

            Vector rectangleSizeGame = new Vector(gameRectangle.Width, gameRectangle.Height);
            Vector rectangleCenterGame = gameRectangle.Location + rectangleSizeGame / 2;

            Vector rectangleCenterGameRelativeToCenter = rectangleCenterGame - viewCenterGame;
            Vector rectangleCenterPixelRelativeToCenter = new Vector(rectangleCenterGameRelativeToCenter.X * scaleX, rectangleCenterGameRelativeToCenter.Y * scaleY);
            Vector rectangleCenterPixel = viewCenterPixel + rectangleCenterPixelRelativeToCenter;

            Vector rectangleSizePixel = new Vector(rectangleSizeGame.X * scaleX, rectangleSizeGame.Y * scaleY);

            return new Rectangle(rectangleCenterPixel - rectangleSizePixel / 2, new Size((int)rectangleSizePixel.X, (int)rectangleSizePixel.Y));
        }

        #endregion
    }
}