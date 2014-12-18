using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Planets.Controller;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.Model.GameObjects;
using Planets.View.Imaging;
using System.Drawing.Text;

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

        private GameEngine ge;

        private Playfield field
        {
            get { return ge.field; }
        }

        private SpritePool sp = new SpritePool();

        private static readonly double MaxArrowSize = 150;
        private static readonly double MinArrowSize = 50;

        // Custom Font!
        private static PrivateFontCollection pfc = new PrivateFontCollection();
        private Font EndGameFont;
        private Font CustomNameFont;

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

        // Wordt gebruikt voor bewegende achtergrond
        private int _blackHoleAngle;

        public GameView(GameEngine ge)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.ge = ge;
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
            CurVecPen.CustomEndCap = bigArrow;
            NextVecPen.CustomEndCap = bigArrow;
            AimVecPen.DashPattern = new float[] { 10 };
            AimVecPen.DashStyle = DashStyle.Dash;
            AimVecPen.CustomEndCap = bigArrow;

            // Custom font
            pfc.AddFontFile(@"Data\Fonts\Prototype.ttf");
            pfc.AddFontFile(@"Data\Fonts\MicroExtend.ttf");
            Font = new Font(pfc.Families[1], 28, FontStyle.Regular);
            EndGameFont = new Font(pfc.Families[1], 40, FontStyle.Regular);
            CustomNameFont = new Font(pfc.Families[0], 20, FontStyle.Italic);
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


            lock (field.BOT)
            {
                field.BOT.Iterate(obj => DrawGameObject(g, obj));
                DrawAimVectors(g);
                DrawDemo(g);
            }

            DrawScores(g);
            DrawHud(g);

            if (field.CurrentPlayer.GameOver || field.CurrentPlayer.GameWon)
                DrawEndGame(g);

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

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.35f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars1, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.45f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars2, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.55f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars3, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.65f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars4, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.75f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars5, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.9f);
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Stars6, target.Width, target.Height), target);
        }

        private Brush EndGameBrush = new SolidBrush(Color.FromArgb(230, 88, 88, 88));

        private void DrawEndGame(Graphics g)
        {
            g.FillRectangle(EndGameBrush, new Rectangle(new Point(0, 0), new Size(1920, 1080)));

            g.DrawString("Highscore: ", EndGameFont, new SolidBrush(Color.White), new Point(200, 200));

            int Highscore = ScoreBoard.GetHighScore();

            // write your score -> ScoreBoard.WriteScore();
            g.DrawString(Highscore.ToString(), EndGameFont, new SolidBrush(Color.White), new Point(200, 210));

            g.DrawString("Your score: ", EndGameFont, new SolidBrush(Color.Yellow), new Point(176, 300));

            g.DrawString("Ruben Oost\nRobert Oost\nRick Vaarkamp\nBart Willemsen\nMartijn Rondeel\nStan Swanborn", CustomNameFont, new SolidBrush(Color.WhiteSmoke), new Point(1640, 880));
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

            if (obj is AnimatedGameObject)
            {
                DateTime nu = DateTime.Now;
                DateTime begin = ((AnimatedGameObject)obj).Begin;
                TimeSpan duration = ((AnimatedGameObject)obj).Duration;

                float p = (float)(nu - begin).TotalMilliseconds / (float)duration.TotalMilliseconds;

                int frames = s.Frames;

                int currentFrame = (int)(p * frames);
                g.DrawImageUnscaled(s.GetFrame(currentFrame), target);

                if (nu - begin >= duration)
                {
                    field.BOT.Remove(obj);
                }
            }
            else
            {
                g.DrawImageUnscaled(s, target);
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

        private void DrawScores(Graphics g)
        {
            lock (field.sb)
            {
                for (int i = 0; i < field.sb.Scores.Count; i++)
                {
                    ScorePlayerBrush.Color = field.sb.Scores[i].Color;
                    if (field.sb.Scores[i].Value > 0)
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
        private Brush LabelBrush = new SolidBrush(Color.White);
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
            float progress = Math.Min(field.sb.Total / 5000.0f, 1.0f);
            float barSize = 90.0f;

            RectangleF arcRectangle = new RectangleF(
                hudLocation.X,
                (float)(hudLocation.Y + hudSize.Height * 0.2),
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

            float barStart = 270.0f - barSize / 2;

            // Draw progress
            g.DrawArc(HudArcPen, arcRectangle, barStart, progress * barSize);

            // Draw meter
            float[] meterPoints = { 0.7f, 0.5f, 0.35f, 0.25f, 0.17f, 0.1f, 0.05f };
            g.DrawArc(HudArcAccentPen, arcAccentRect, barStart - 1.0f, barSize + 2.0f);
            g.DrawArc(HudArcAccentPen2, arcAccentRect2, barStart, 1.0f);
            g.DrawArc(HudArcAccentPen2, arcAccentRect2, barStart + barSize - 1.0f, 1.0f);
            foreach (var f in meterPoints)
                g.DrawArc(HudArcAccentPen3, arcAccentRect3, barStart + barSize * f - 0.25f, 0.5f);

            // Draw score text
            g.DrawString(field.sb.Total.ToString(), HudScoreFont, LabelBrush, arcRectangle.Left + arcRectangle.Width / 2, arcRectangle.Top + arcRectangle.Height / 6);

            // Draw Mass-o-meter
            Point MassMeterPoint = new Point(hudLocation.X + 20, hudLocation.Y + 60);

            int MassDrawY = (int)(MassMeterPoint.Y + (230 - field.CurrentPlayer.Radius));

            Point MassDrawPoint = new Point(MassMeterPoint.X, (MassDrawY > MassMeterPoint.Y) ? MassDrawY : MassMeterPoint.Y);
            Brush gradientBrush = new LinearGradientBrush(MassMeterPoint, new Point(MassMeterPoint.X, MassMeterPoint.Y + 230), Color.YellowGreen, Color.DarkOrange);
            g.FillRectangle(gradientBrush, new Rectangle(MassDrawPoint, new Size(15, (int)field.CurrentPlayer.Radius)));
            g.DrawRectangle(WhitePen, new Rectangle(MassMeterPoint, new Size(15, 230)));
            g.DrawString("Mass", HudScoreFont, LabelBrush, MassMeterPoint.X - 10, MassMeterPoint.Y - 30);

            // Draw Objects-o-meter
            int AmountObjects = (field.BOT.Count - 6) * 4;

            Point ObjectMeter = new Point(ClientSize.Width - 35, hudLocation.Y + 60);

            int ObjectDraw = ObjectMeter.Y + (230 - AmountObjects);

            Point ObjectDrawPoint = new Point(ObjectMeter.X, (ObjectDraw > ObjectMeter.Y) ? ObjectDraw : ObjectMeter.Y);

            g.FillRectangle(gradientBrush, new Rectangle(ObjectDrawPoint, new Size(15, AmountObjects)));
            g.DrawRectangle(WhitePen, new Rectangle(ObjectMeter, new Size(15, 230)));
            g.DrawString("Objects", HudScoreFont, LabelBrush, ObjectMeter.X - 50, ObjectMeter.Y - 30);

            // Draw Radar
            int RadiusRadar = 130;
            Size s = new Size(RadiusRadar, RadiusRadar);
            Point RadarPoint = new Point((hudLocation.X + ((hudSize.Width / 2) - (RadiusRadar / 2))), (hudLocation.Y + ((hudSize.Height / 2) - (RadiusRadar / 2))) + 60);

            g.FillEllipse(Brushes.Red, new Rectangle(RadarPoint, s));
            field.BOT.Iterate(go1 =>
            {
                double xField = go1.Location.X / field.Size.Width;
                double yField = go1.Location.Y / field.Size.Height;

                double xRadar = s.Width * xField;
                double yRadar = s.Height * yField;

                Point blip = new Point(Convert.ToInt32(xRadar), Convert.ToInt32(yRadar));
                blip.X += RadarPoint.X;
                blip.Y += RadarPoint.Y;

                if (go1 is Player && !(go1 is Antagonist))
                {
                    Point pPlayer = new Point(field.Size.Width - (hudSize.Width / 2) - 5, (field.Size.Height - (hudSize.Height / 2)) + 55);
                    g.FillEllipse(Brushes.Yellow, new Rectangle(pPlayer, new Size(10, 10)));
                }
            });
        }

        private void DrawAnimations(Graphics g, GameObject obj)
        {
            Rectangle target = GameToScreen(obj.BoundingBox);
            Sprite s = sp.GetSprite(obj.GetType(), target.Width, target.Height);

            g.DrawImageUnscaled(s, target);

            // if there are animations queued by a gamerule
            // get the frame from the spritepool list
            // play the frames

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