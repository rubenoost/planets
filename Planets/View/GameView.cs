using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Planets.Controller;
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

        private readonly GameEngine _ge;

        private Playfield Field
        {
            get { return _ge.Field; }
        }

        private readonly SpritePool _sp = new SpritePool();

        private static readonly double MaxArrowSize = 150;
        private static readonly double MinArrowSize = 50;

        // Custom Font!
        private static readonly PrivateFontCollection Pfc = new PrivateFontCollection();
        private readonly Font _endGameFont;

        // Aiming Settings
        /// <summary>
        /// If true, a vector will be drawn to show the current trajectory
        /// </summary>
        public bool IsAiming;
        public bool ClickOnNextButton;
        public bool PrevClickNext;
        public Vector AimPoint;

        private readonly SolidBrush _scorePlayerBrush = new SolidBrush(Color.White);
        private readonly Font _scoreFont = new Font(FontFamily.GenericSansSerif, 60.0f, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly Font _planetsFont;
        private readonly Font _nextLevelFont;
        private readonly Font _gameStatusFont;

        // Aiming pen buffer
        private readonly Pen _curVecPen = new Pen(Color.Red, 5);
        private readonly Pen _nextVecPen = new Pen(Color.Green, 5);
        private readonly Pen _aimVecPen = new Pen(Color.White, 5);

        // Wordt gebruikt voor bewegende achtergrond
        private int _blackHoleAngle;

        public GameView(GameEngine ge)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this._ge = ge;
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
            _curVecPen.CustomEndCap = bigArrow;
            _nextVecPen.CustomEndCap = bigArrow;
            _aimVecPen.DashPattern = new float[] { 10 };
            _aimVecPen.DashStyle = DashStyle.Dash;
            _aimVecPen.CustomEndCap = bigArrow;

            ClickOnNextButton = false;
            PrevClickNext = false;

            // Custom font
            /*Pfc.AddFontFile(@"Data\Fonts\Prototype.ttf");
            Pfc.AddFontFile(@"Data\Fonts\MicroExtend.ttf");
            Pfc.AddFontFile(@"Data\Fonts\spacebar.ttf");
            Pfc.AddFontFile(@"Data\Fonts\game_over.ttf");
            Font = new Font(Pfc.Families[1], 28, FontStyle.Italic);
            _endGameFont = new Font(Pfc.Families[2], 40);
            _planetsFont = new Font(Pfc.Families[3], 50);
            _nextLevelFont = new Font(Pfc.Families[3], 35);
            _gameStatusFont = new Font(Pfc.Families[0], 140);*/
            _endGameFont = Font;
            _planetsFont = Font;
            _nextLevelFont = Font;
            _gameStatusFont = Font;
        }

        private void Set(Graphics g, bool highness)
        {
            g.SmoothingMode = highness ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed;
            g.InterpolationMode = InterpolationMode.Low;
            g.CompositingQuality = CompositingQuality.HighSpeed;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Set(g, false);

            // Draw static back layer
            DrawBackLayers(g);
            // Draw top layer


            lock (Field.GameObjects)
            {
                Field.GameObjects.Iterate(obj => DrawGameObject(g, obj));
                DrawDemo(g);
            }

            Set(g, true);
            DrawAimVectors(g);
            DrawScores(g);
            DrawHud(g);

            if (Field.CurrentPlayer.GameOver || Field.CurrentPlayer.GameWon)
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
            g.DrawImageUnscaled(_sp.GetSprite(Sprite.Background1, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.35f);
            g.DrawImageUnscaled(_sp.GetSprite(Sprite.Stars1, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.45f);
            g.DrawImageUnscaled(_sp.GetSprite(Sprite.Stars2, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.55f);
            g.DrawImageUnscaled(_sp.GetSprite(Sprite.Stars3, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.65f);
            g.DrawImageUnscaled(_sp.GetSprite(Sprite.Stars4, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.75f);
            g.DrawImageUnscaled(_sp.GetSprite(Sprite.Stars5, target.Width, target.Height), target);

            target = GameToScreen(new Rectangle(new Point(0, 0), ClientSize), 0.9f);
            g.DrawImageUnscaled(_sp.GetSprite(Sprite.Stars6, target.Width, target.Height), target);
        }

        private readonly Brush _endGameBrush = new SolidBrush(Color.FromArgb(230, 88, 88, 88));
        private readonly Brush _yourScoreBrush = new SolidBrush(Color.Yellow);
        private readonly Brush _highScoreBrush = new SolidBrush(Color.White);

        private void DrawEndGame(Graphics g)
        {
            // Background rectangle
            g.FillRectangle(_endGameBrush, new Rectangle(new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)));

            // Highscore
            g.DrawString("Highscore: ", _endGameFont, _highScoreBrush, new Point(175, 200));
            int highscore = ScoreBoard.GetHighScore();
            g.DrawString(highscore.ToString(), _endGameFont, _highScoreBrush, new Point(640 - TextRenderer.MeasureText(highscore.ToString(), _endGameFont).Width, 200));

            // Win or Lose
            g.DrawString((Field.CurrentPlayer.GameOver) ? "GameOver" : "Level Completed", _gameStatusFont, new SolidBrush((Field.CurrentPlayer.GameOver) ? Color.Red : Color.Green), new Point(180, 60));

            // Score box
            g.DrawLine(new Pen(Color.WhiteSmoke, 2), new Point(150, 370), new Point(630, 370));
            g.DrawLine(new Pen(Color.WhiteSmoke, 2), new Point(630, 370), new Point(630, 170));

            // Next button
            if (ClickOnNextButton)
            {
                g.FillRectangle(new SolidBrush(Color.WhiteSmoke), new Rectangle(new Point(175, 400), new Size(430, 100)));
            }
            else
            {
                g.DrawRectangle(new Pen(Color.WhiteSmoke, 2), new Rectangle(new Point(175, 400), new Size(430, 100)));
            }

            g.DrawString("Next level", _nextLevelFont, new SolidBrush((ClickOnNextButton) ? Color.FromArgb(230, 88, 88, 88) : Color.WhiteSmoke), new Point(185, 420));

            PrevClickNext = ClickOnNextButton;

            if (ClickOnNextButton)
                ClickOnNextButton = false;

            // Your score
            g.DrawString("Gamescore: ", _endGameFont, _yourScoreBrush, new Point(176, 300));
            g.DrawString(Field.ScoreBoard.Total.ToString(), _endGameFont, _yourScoreBrush, new Point(640 - TextRenderer.MeasureText(Field.ScoreBoard.Total.ToString(), _endGameFont).Width, 300));
            ScoreBoard.WriteScore(Field.ScoreBoard.Total);

            // Draw icon
            g.DrawImage(Resources.HighScoreLogo, new Point(Screen.PrimaryScreen.Bounds.Width - 570, 40));
            g.DrawString("Planets", _planetsFont, _highScoreBrush, new Point(Screen.PrimaryScreen.Bounds.Width - 555, 500));
        }

        private void DrawAimVectors(Graphics g)
        {
            GameObject obj = Field.CurrentPlayer;
            if (IsAiming)
            {
                Vector cursorPosition = Cursor.Position;
                AimPoint = obj.Location - cursorPosition;

                Vector curVec = obj.Location + obj.Dv.ScaleToLength(obj.Radius + Math.Min(MaxArrowSize, Math.Max(obj.Dv.Length(), MinArrowSize)));
                // Draw current direction vector
                g.DrawLine(_curVecPen, GameToScreen(obj.Location + obj.Dv.ScaleToLength(obj.Radius + 1)), GameToScreen(curVec));

                // Draw aim direction vector
                Vector aimVec = obj.Location + AimPoint.ScaleToLength(obj.Radius + Math.Min(MaxArrowSize, Math.Max(obj.Dv.Length(), MinArrowSize)));
                g.DrawLine(_aimVecPen, GameToScreen(obj.Location + AimPoint.ScaleToLength(obj.Radius + 1)), GameToScreen(aimVec));

                // Draw next direction vector
                Vector nextVec = ShootProjectileController.CalcNewDv(obj, new GameObject(new Vector(0, 0), new Vector(0, 0), 0.05 * obj.Mass), Cursor.Position);
                g.DrawLine(_nextVecPen, GameToScreen(obj.Location + nextVec.ScaleToLength(obj.Radius + 1.0)), GameToScreen(obj.Location + nextVec.ScaleToLength(obj.Radius + Math.Min(MaxArrowSize, Math.Max(obj.Dv.Length(), MinArrowSize)))));
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
            Sprite s = _sp.GetSprite(obj.GetType(), target.Width, target.Height, objAngle);

            var o = obj as AnimatedGameObject;
            if (o != null)
            {
                DateTime nu = DateTime.Now;
                DateTime begin = o.Begin;
                TimeSpan duration = o.Duration;

                float p = (float)(nu - begin).TotalMilliseconds / (float)duration.TotalMilliseconds;

                int frames = s.Frames;

                int currentFrame = (int)(p * frames);
                g.DrawImageUnscaled(s.GetFrame(currentFrame), target);

                if (nu - begin >= duration)
                {
                    Field.GameObjects.Remove(o);
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
            double f = (DateTime.Now - Field.LastAutoClickMoment).TotalMilliseconds;
            if (f < 1000)
            {
                int r = 20 + (int)(f / 10);
                Rectangle autoDemoEffectTarget = new Rectangle(Field.LastAutoClickGameLocation.X - r / 2, Field.LastAutoClickGameLocation.Y - r / 2, r, r);
                g.FillEllipse(new SolidBrush(Color.FromArgb((int)(255 - f / 1000 * 255), 255, 0, 0)), autoDemoEffectTarget);
                Point cursorPixelPoint = Field.LastAutoClickGameLocation;
                g.DrawImageUnscaled(_sp.GetSprite(Sprite.Cursor, 100, 100), cursorPixelPoint.X - 4, cursorPixelPoint.Y - 10);
            }
        }

        private void DrawScores(Graphics g)
        {
            lock (Field.ScoreBoard)
            {
                for (int i = 0; i < Field.ScoreBoard.Scores.Count; i++)
                {
                    _scorePlayerBrush.Color = Field.ScoreBoard.Scores[i].Color;
                    g.DrawString(
                        Field.ScoreBoard.Scores[i].Value > 0
                            ? String.Format("+{0}", Field.ScoreBoard.Scores[i].Value)
                            : String.Format("{0}", Field.ScoreBoard.Scores[i].Value), _scoreFont, _scorePlayerBrush,
                        (Point)GameToScreen(Field.ScoreBoard.Scores[i].Location));
                    Field.ScoreBoard.Scores[i].UpdateLocation();
                }
            }
        }

        // Draw Score Arc Buff
        private readonly Brush _hudBackgroundBrush = new SolidBrush(Color.FromArgb(230, 88, 88, 88));
        private readonly Pen _hudArcAccentPen = new Pen(Color.White, 4.0f);
        private readonly Pen _hudArcAccentPen2 = new Pen(Color.White, 30.0f);
        private readonly Pen _hudArcAccentPen3 = new Pen(Color.White, 22.0f);
        private readonly Font _hudScoreFont = new Font(FontFamily.GenericMonospace, 18.0f, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly Brush _labelBrush = new SolidBrush(Color.White);
        private Size _hudSize = new Size(500, 300);

        // Draw WhatEverMeter buff
        private readonly Pen _whitePen = new Pen(Color.White, 2);
        private readonly DateTime _start = DateTime.Now;

        private void DrawHud(Graphics g)
        {
            // Draw hud background
            Point hudLocation = new Point(ClientSize.Width - _hudSize.Width, ClientSize.Height - _hudSize.Height);

            // Draw hud
            int featherSize = 50;
            Rectangle target = new Rectangle(hudLocation, _hudSize);
            g.FillPie(_hudBackgroundBrush, new Rectangle(target.Location, new Size(featherSize * 2, featherSize * 2)), -90.0f, -90.0f);
            g.FillRectangle(_hudBackgroundBrush, new Rectangle(target.Left + featherSize - 1, target.Top, target.Width - featherSize, target.Height));
            g.FillRectangle(_hudBackgroundBrush, new Rectangle(target.Left, target.Top + featherSize - 1, featherSize, target.Height - featherSize));

            // Draw score arc
            float progress = Math.Min(Field.ScoreBoard.Total / 5000.0f, 1.0f);
            float barSize = 90.0f;

            RectangleF arcRectangle = new RectangleF(
                hudLocation.X,
                (float)(hudLocation.Y + _hudSize.Height * 0.1),
                _hudSize.Width,
                _hudSize.Height * 1.2f);

            Pen hudArcPen = new Pen(new LinearGradientBrush(new PointF(arcRectangle.Left, arcRectangle.Top), new PointF(arcRectangle.Left + arcRectangle.Width, arcRectangle.Top), Color.GreenYellow, Color.DarkOrange), 20.0f);

            RectangleF arcAccentRect = new RectangleF(
                arcRectangle.Left + hudArcPen.Width / 2,
                arcRectangle.Top + hudArcPen.Width / 2,
                arcRectangle.Width - hudArcPen.Width,
                arcRectangle.Height - hudArcPen.Width);

            float diff2 = _hudArcAccentPen2.Width - hudArcPen.Width;
            RectangleF arcAccentRect2 = new RectangleF(
                arcRectangle.Left - diff2 / 2,
                arcRectangle.Top - diff2 / 2,
                arcRectangle.Width + diff2,
                arcRectangle.Height + diff2
                );

            float diff3 = _hudArcAccentPen3.Width - hudArcPen.Width;
            RectangleF arcAccentRect3 = new RectangleF(
                arcRectangle.Left - diff3 / 2,
                arcRectangle.Top - diff3 / 2,
                arcRectangle.Width + diff3,
                arcRectangle.Height + diff3
                );

            float barStart = 270.0f - barSize / 2;

            // Draw progress
            g.DrawArc(hudArcPen, arcRectangle, barStart, progress * barSize);

            // Draw meter
            float[] meterPoints = { 0.7f, 0.5f, 0.35f, 0.25f, 0.17f, 0.1f, 0.05f };
            g.DrawArc(_hudArcAccentPen, arcAccentRect, barStart - 1.0f, barSize + 2.0f);
            g.DrawArc(_hudArcAccentPen2, arcAccentRect2, barStart, 1.0f);
            g.DrawArc(_hudArcAccentPen2, arcAccentRect2, barStart + barSize - 1.0f, 1.0f);
            foreach (var f in meterPoints)
                g.DrawArc(_hudArcAccentPen3, arcAccentRect3, barStart + barSize * f - 0.25f, 0.5f);

            // Draw score text
            g.DrawString(Field.ScoreBoard.Total.ToString(), _hudScoreFont, _labelBrush, arcRectangle.Left + arcRectangle.Width / 2, arcRectangle.Top + arcRectangle.Height / 11);

            // Draw Mass-o-meter
            Point massMeterPoint = new Point(hudLocation.X + 20, hudLocation.Y + 60);

            int massDrawY = (int)(massMeterPoint.Y + (230 - Field.CurrentPlayer.Radius));

            Point massDrawPoint = new Point(massMeterPoint.X, (massDrawY > massMeterPoint.Y) ? massDrawY : massMeterPoint.Y);
            Brush gradientBrush = new LinearGradientBrush(massMeterPoint, new Point(massMeterPoint.X, massMeterPoint.Y + 230), Color.YellowGreen, Color.DarkOrange);
            g.FillRectangle(gradientBrush, new Rectangle(massDrawPoint, new Size(15, (int)Field.CurrentPlayer.Radius)));
            g.DrawRectangle(_whitePen, new Rectangle(massMeterPoint, new Size(15, 230)));
            g.DrawString("Mass", _hudScoreFont, _labelBrush, massMeterPoint.X - 10, massMeterPoint.Y - 30);

            // Draw Objects-o-meter
            int amountObjects = (Field.GameObjects.Count - 6) * 4;

            Point objectMeter = new Point(ClientSize.Width - 35, hudLocation.Y + 60);

            int objectDraw = objectMeter.Y + (230 - amountObjects);

            Point objectDrawPoint = new Point(objectMeter.X, (objectDraw > objectMeter.Y) ? objectDraw : objectMeter.Y);

            g.FillRectangle(gradientBrush, new Rectangle(objectDrawPoint, new Size(15, amountObjects)));
            g.DrawRectangle(_whitePen, new Rectangle(objectMeter, new Size(15, 230)));
            g.DrawString("Objects", _hudScoreFont, _labelBrush, objectMeter.X - 50, objectMeter.Y - 30);


            #region Radar

            // Draw Radar
            int radiusRadar = 105;
            Vector radarCenter = hudLocation + new Vector(_hudSize.Width, (_hudSize.Height + 80)) / 2;
            Vector radarSize = new Vector(radiusRadar * 2, radiusRadar * 2);
            Rectangle radarRectangle = new Rectangle(radarCenter - radarSize / 2, new Size((int)radarSize.X, (int)radarSize.Y));
            float dotRadius = 5;

            Vector playerLocation = Field.CurrentPlayer.Location;
            float scale = 0.05f;

            //all brushes for the radar
            Brush radarbackgroundbrush = new SolidBrush(Color.FromArgb(230, 23, 23, 23));
            Brush gameobjectbrush = new SolidBrush(Color.FromArgb(255, 0, 198, 0));
            Brush playerBrush = new SolidBrush(Color.Red);
            Brush antagonistbrush = new SolidBrush(Color.Blue);
            Brush bonusbrush = new SolidBrush(Color.Yellow);
            Pen radarPen = new Pen(Color.GreenYellow, 2.0f);

            //The brush that is going to be used to draw the object
            Brush b;

            //Draw the circle for the background of the radar
            g.FillEllipse(radarbackgroundbrush, radarRectangle);

            //Go through the bot and do something with every object
            Field.GameObjects.Iterate(go =>
            {
                //If gameobject is outside of the range of the radar than don't draw it
                if (((go.Location - playerLocation).Length() * scale + dotRadius) > radiusRadar) return;

                //if antagonist then draw a blue circle
                if (go is Antagonist)
                {
                    b = antagonistbrush;
                }

                //If player then draw a red circle
                else if (go is Player)
                {
                    b = playerBrush;
                }

                 //
                else if (go is Bonus)
                {
                    b = bonusbrush;
                }

                //if gameobject then draw green circle 
                else
                {
                    b = gameobjectbrush;
                }

                Vector drawCenter = radarCenter + (go.Location - playerLocation) * scale;
                g.FillEllipse(b, new Rectangle(drawCenter - new Vector(dotRadius, dotRadius), new Size((int)(dotRadius * 2), (int)(dotRadius * 2))));
            });

            // Draw fancy line
            double rotationspeed = 5.0d;
            double phase = (DateTime.Now - _start).TotalSeconds % rotationspeed;
            double angle = phase / rotationspeed * Math.PI * 2.0d;
            Vector dir = new Vector(Math.Cos(angle), Math.Sin(angle));

            g.DrawLine(radarPen, radarCenter, radarCenter + dir.ScaleToLength(radiusRadar));
        }
            #endregion

        private void DrawAnimations(Graphics g, GameObject obj)
        {
            Rectangle target = GameToScreen(obj.BoundingBox);
            Sprite s = _sp.GetSprite(obj.GetType(), target.Width, target.Height);

            g.DrawImageUnscaled(s, target);

            // if there are animations queued by a gamerule
            // get the frame from the spritepool list
            // play the frames

        }

        #endregion

        #region Game / Screen conversions

        public Vector GameToScreen(Vector v, float parallaxDepth = 1.0f)
        {
            // The game size associated with each layer
            Vector layerGameSize = new Vector(Field.Size.Width, Field.Size.Height);

            //=================================== [ Game Center ] =================================

            // The center of the game, if no parallax is present
            Vector noPrlxViewCenter = layerGameSize / 2;

            // The center of the game if parallax is 1.0
            Vector onePrlxViewCenter = Field.CurrentPlayer.Location;

            // The corrected center of the game, with any parallaxdepth
            Vector viewCenterGame = onePrlxViewCenter * parallaxDepth + (1.0f - parallaxDepth) * noPrlxViewCenter;

            //=================================== [ Game View Size ] ==============================

            // View size with no parallax present
            Vector noPrlxViewSize = layerGameSize;

            // View size if parallax is 1.0
            Vector onePrlxViewSize = layerGameSize / Zoom;

            // Corrected view size with any parallaxdepth
            Vector viewSizeGame = onePrlxViewSize * parallaxDepth + (1.0f - parallaxDepth) * noPrlxViewSize;

            //=================================== [ Correct viewing rectangle ] ====================

            viewCenterGame = new Vector(Math.Max(viewSizeGame.X / 2, viewCenterGame.X), Math.Max(viewSizeGame.Y / 2, viewCenterGame.Y));
            viewCenterGame = new Vector(Math.Min(viewCenterGame.X, layerGameSize.X - viewSizeGame.X / 2), Math.Min(viewCenterGame.Y, layerGameSize.Y - viewSizeGame.Y / 2));

            //=================================== [ Scale to pixels ] =============================

            double scaleX = ClientSize.Width / viewSizeGame.X;
            double scaleY = ClientSize.Height / viewSizeGame.Y;

            Vector viewCenterPixel = new Vector((double)ClientSize.Width / 2, (double)ClientSize.Height / 2);

            Vector pointRelativeToViewCenterGame = v - viewCenterGame;
            Vector pointRelativeToViewCenterPixel = new Vector(pointRelativeToViewCenterGame.X * scaleX, pointRelativeToViewCenterGame.Y * scaleY);

            return pointRelativeToViewCenterPixel + viewCenterPixel;
        }

        public Vector ScreenToGame(Vector v, float parallaxDepth = 1.0f)
        {
            // The game size associated with each layer
            Vector layerGameSize = new Vector(Field.Size.Width, Field.Size.Height);

            //=================================== [ Game Center ] =================================

            // The center of the game, if no parallax is present
            Vector noPrlxViewCenter = layerGameSize / 2;

            // The center of the game if parallax is 1.0
            Vector onePrlxViewCenter = Field.CurrentPlayer.Location;

            // The corrected center of the game, with any parallaxdepth
            Vector viewCenterGame = onePrlxViewCenter * parallaxDepth + (1.0f - parallaxDepth) * noPrlxViewCenter;

            //=================================== [ Game View Size ] ==============================

            // View size with no parallax present
            Vector noPrlxViewSize = layerGameSize;

            // View size if parallax is 1.0
            Vector onePrlxViewSize = layerGameSize / Zoom;

            // Corrected view size with any parallaxdepth
            Vector viewSizeGame = onePrlxViewSize * parallaxDepth + (1.0f - parallaxDepth) * noPrlxViewSize;

            //=================================== [ Correct viewing rectangle ] ====================

            viewCenterGame = new Vector(Math.Max(viewSizeGame.X / 2, viewCenterGame.X), Math.Max(viewSizeGame.Y / 2, viewCenterGame.Y));
            viewCenterGame = new Vector(Math.Min(viewCenterGame.X, layerGameSize.X - viewSizeGame.X / 2), Math.Min(viewCenterGame.Y, layerGameSize.Y - viewSizeGame.Y / 2));

            //=================================== [ Scale to pixels ] =============================
            Vector viewCenterPixel = new Vector((double)ClientSize.Width / 2, (double)ClientSize.Height / 2);
            Vector pointRelativeToViewCenterPixel = v - viewCenterPixel;

            double scaleX = ClientSize.Width / viewSizeGame.X;
            double scaleY = ClientSize.Height / viewSizeGame.Y;

            Vector pointRelativeToViewCenterGame = new Vector(pointRelativeToViewCenterPixel.X / scaleX, pointRelativeToViewCenterPixel.Y / scaleY);

            Vector pointGame = pointRelativeToViewCenterGame + viewCenterGame;

            return pointGame;
        }

        public Rectangle GameToScreen(Rectangle gameRectangle, float parallaxDepth = 1.0f)
        {
            // The game size associated with each layer
            Vector layerGameSize = new Vector(Field.Size.Width, Field.Size.Height);

            //=================================== [ Game Center ] =================================

            // The center of the game, if no parallax is present
            Vector noPrlxViewCenter = layerGameSize / 2;

            // The center of the game if parallax is 1.0
            Vector onePrlxViewCenter = Field.CurrentPlayer.Location;

            // The corrected center of the game, with any parallaxdepth
            Vector viewCenterGame = onePrlxViewCenter * parallaxDepth + (1.0f - parallaxDepth) * noPrlxViewCenter;

            //=================================== [ Game View Size ] ==============================

            // View size with no parallax present
            Vector noPrlxViewSize = layerGameSize;

            // View size if parallax is 1.0
            Vector onePrlxViewSize = layerGameSize / Zoom;

            // Corrected view size with any parallaxdepth
            Vector viewSizeGame = onePrlxViewSize * parallaxDepth + (1.0f - parallaxDepth) * noPrlxViewSize;

            //=================================== [ Correct viewing rectangle ] ====================

            viewCenterGame = new Vector(Math.Max(viewSizeGame.X / 2, viewCenterGame.X), Math.Max(viewSizeGame.Y / 2, viewCenterGame.Y));
            viewCenterGame = new Vector(Math.Min(viewCenterGame.X, layerGameSize.X - viewSizeGame.X / 2), Math.Min(viewCenterGame.Y, layerGameSize.Y - viewSizeGame.Y / 2));

            //=================================== [ Scale to pixels ] =============================

            double scaleX = ClientSize.Width / viewSizeGame.X;
            double scaleY = ClientSize.Height / viewSizeGame.Y;

            Vector viewCenterPixel = new Vector((double)ClientSize.Width / 2, (double)ClientSize.Height / 2);

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