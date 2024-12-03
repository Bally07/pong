using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Collections.Generic;


namespace pong
{
    public class Game1 : Game
    {
        bool cheats = false;
        bool first = true;
        List<string> scores = new List<string>();
        string highscorefile = @"C:\Users\oscar\Documents\GitHub\pong\highscore_pong.txt";
        StreamReader SR;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        const int plrSpeed = 10;
        int BallSpeed = 5;
        bool Nstarted = true;
        int highscore;
        int exitcount = 0;
        int timer;
        bool aiup;
        SpriteFont playfont,endfont;
        Texture2D PLR1, PLR2, Ball, startscreen, Board, gameoverscreen;
        Rectangle P1HitBox, P2HitBox, BallHitBox, board, background;
        bool gameover = false;
        bool gamePlaying = false;
        bool bounce = false;
        SoundEffect playbounce;
        bool Left = true;
        bool Top = true;
        int P1pointCounter, P2pointCounter;
        Random rand = new Random();
        public Game1()
        {
           
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
           
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            P1HitBox = new Rectangle(0, (_graphics.GraphicsDevice.Viewport.Height) / 2, 25, 150);
            P2HitBox = new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 25, (_graphics.GraphicsDevice.Viewport.Height) / 2, 25, 150);
            background = new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
            BallHitBox = new Rectangle((_graphics.GraphicsDevice.Viewport.Width) / 2, (_graphics.GraphicsDevice.Viewport.Height) / 2, 50, 50);
            board = new Rectangle((_graphics.GraphicsDevice.Viewport.Width) / 2, 0, 5, _graphics.GraphicsDevice.Viewport.Height);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Board = Content.Load<Texture2D>("pongBoard");
            PLR1 = Content.Load<Texture2D>("padle");
            PLR2 = Content.Load<Texture2D>("padle");
            Ball = Content.Load<Texture2D>("ball");
            playfont = Content.Load<SpriteFont>("play");
            endfont = Content.Load<SpriteFont>("over");
            startscreen = Content.Load<Texture2D>("splash screen");
            gameoverscreen = Content.Load<Texture2D>("Game over splash");
            playbounce = Content.Load<SoundEffect>("ballBounce");
            //loadhighscor(highscorefile,SR,highscore);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                exitcount++;
                if (exitcount == 2)
                {
                    Exit();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                Nstarted = false;
                gameover = false;
                P1pointCounter = 0;
                P2pointCounter = 0;
                BallSpeed = 5;
                
            }
            pointssystem();
            //P2HitBox =  AI(plrSpeed,P2HitBox,BallHitBox);
          level(P1pointCounter,P2HitBox.Height);
            movePlr(plrSpeed);
            moveball(BallSpeed);
            ishighscore(P1pointCounter, highscore);
            clock(P2pointCounter, timer);
            base.Update(gameTime);  
        }
        protected override void Draw(GameTime gameTime)
        {
            Vector2 textPos = new Vector2(0, 0);
            Vector2 scorepos = new Vector2(367, 136),
                    highscorepos=new Vector2(496, 204),
                   
                    clockpos = new Vector2(554, 272);

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            if (Nstarted == true && gameover == true)
            {
                _spriteBatch.Draw(gameoverscreen, background, Color.White);
                _spriteBatch.DrawString(endfont,$"{P1pointCounter}",scorepos, Color.White);
                _spriteBatch.DrawString(endfont, $"{highscore}", highscorepos, Color.White);
                _spriteBatch.DrawString(endfont, $"{timer / 60} seconds", clockpos, Color.White);

            }
            else if (Nstarted == false && gameover == false)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.DrawString(playfont, $"Player 1 points:{P1pointCounter}\nCPU points:{P2pointCounter}", textPos, Color.White);
                _spriteBatch.Draw(Board, board, Color.White);
                _spriteBatch.Draw(PLR1, P1HitBox, Color.FloralWhite);
                _spriteBatch.Draw(PLR2, P2HitBox, Color.FloralWhite);
                _spriteBatch.Draw(Ball, BallHitBox, Color.White);
            }
            else if (Nstarted == true && gameover == false)
            {
                _spriteBatch.Draw(startscreen, background, Color.White);
            }

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
            void pointssystem()
        {
            if (cheats == false)
            {
                if (BallHitBox.X < 0 || BallHitBox.X > _graphics.GraphicsDevice.Viewport.Width)
                {
                    if (BallHitBox.X > _graphics.GraphicsDevice.Viewport.Width / 2)
                    {
                        P1pointCounter++;
                    }
                    else
                    {
                        P2pointCounter++;
                    }
                    BallHitBox.X = _graphics.GraphicsDevice.Viewport.Width / 2;
                    BallHitBox.Y = _graphics.GraphicsDevice.Viewport.Height / 2;
                    gamePlaying = false;
                }
            }
            else
            {
                if (BallHitBox.X < 0 || BallHitBox.X > _graphics.GraphicsDevice.Viewport.Width)
                {
                    if (BallHitBox.X > _graphics.GraphicsDevice.Viewport.Width / 2)
                    {
                        P1pointCounter+= 10;
                    }
                    else
                    {
                        P2pointCounter++;
                    }
                    BallHitBox.X = _graphics.GraphicsDevice.Viewport.Width / 2;
                    BallHitBox.Y = _graphics.GraphicsDevice.Viewport.Height / 2;
                    gamePlaying = false;
                }
            }
           
            return;

        }
            void level(int level,int Height)
        {

            //if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed ||Keyboard.GetState().IsKeyDown(Keys.C))
            //{
            //    cheats = true;
            //}
            //if (cheats == false)
            //{
            //    if (level < 10)
            //    {

            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 1;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 1;
            //        }


            //    }
            //    else if (level >= 10 && level < 20)
            //    {

            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 2;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 2;
            //        }

            //    }
            //    else if (level >= 20 && level < 30)
            //    {


            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 3;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 3;
            //        }

            //    }
            //    else if (level >= 30 && level < 40)
            //    {

            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 4;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 4;
            //        }

            //    }
            //    else if (level >= 40 && level < 50)
            //    {


            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 5;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 5;
            //        }

            //    }
            //    else if (level >= 50 && level < 60)
            //    {


            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 6;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 6;
            //        }

            //    }
            //    else if (level >= 60 && level < 70)
            //    {

            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 7;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 7;
            //        }

            //    }
            //    else if (level >= 70 && level < 80)
            //    {

            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 8;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 8;
            //        }

            //    }
            //    else if (level >= 80 && level < 90)
            //    {

            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 9;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 9;
            //        }

            //    }
            //    else if (level >= 90 && level < 100)
            //    {


            if (P2HitBox.Y > BallHitBox.Y)
            {
                P2HitBox.Y -= 10;
            }
            else if (P2HitBox.Y < BallHitBox.Y)
            {
                P2HitBox.Y += 10;
            }

            //    }
            //    else if (level >= 100)
            //    {


            //        if (P2HitBox.Y > BallHitBox.Y)
            //        {
            //            P2HitBox.Y -= 11;
            //        }
            //        else if (P2HitBox.Y < BallHitBox.Y)
            //        {
            //            P2HitBox.Y += 11;
            //        }

            //    }
            //    if (P2HitBox.Y < 0)
            //    {
            //        P2HitBox.Y = 0;
            //    }
            if (P2HitBox.Y > _graphics.GraphicsDevice.Viewport.Height - Height)
                P2HitBox.Y = _graphics.GraphicsDevice.Viewport.Height - Height;
            return;
            //}
            //else
            //{
            //    P2HitBox.Y = 0;
            //    P1pointCounter++;
            //}

        }//done
            void moveball(int Speed)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) || gamePlaying == true)
                {
                    gamePlaying = true;

                    if (BallHitBox.Intersects(P1HitBox) && BallHitBox.Left <= P1HitBox.Right)
                    {
                        Left = true;
                        BallSpeed++;
                        playbounce.Play();
                    }
                    else if (BallHitBox.Intersects(P2HitBox) && BallHitBox.X <= P2HitBox.Left)
                    {
                        Left = false;
                        BallSpeed++;
                        playbounce.Play();
                    }//bounce back from paddle
                    if (Left == true)
                    {
                        BallHitBox.X += Speed;
                        bounce = false;
                    }//moves ball 
                    else
                    {
                        BallHitBox.X -= Speed;
                        bounce = false;
                    }
                    if (Top == true)
                    {
                        BallHitBox.Y += Speed;
                        bounce = false;
                    }
                    else
                    {
                        BallHitBox.Y -= Speed;
                        bounce = false;
                    }
                    if (BallHitBox.Y < 0)
                    {
                        Top = true;
                        playbounce.Play();
                    }
                    else if (BallHitBox.Y + BallHitBox.Height > _graphics.GraphicsDevice.Viewport.Height)
                    {
                        Top = false;
                        playbounce.Play();
                    }

                    //starts game
                }
               
            }
            void movePlr(int Speed)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                {
                    P1HitBox.Y -= Speed;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                {
                    P1HitBox.Y += Speed;
                }
                if (P1HitBox.Y < 0)
                {
                    P1HitBox.Y = 0;
                }//moves players
                if (P1HitBox.Y > _graphics.GraphicsDevice.Viewport.Height - P1HitBox.Height)
                    P1HitBox.Y = _graphics.GraphicsDevice.Viewport.Height - P1HitBox.Height;
                return;
            }
            int clock(int points, int clock)
            {
                if (points < 10)
                {
                    timer++;
                }
                else if (points == 10)
                {
                    gameover = true;
                    Nstarted = true;
                }
            return clock;
            }
            void ishighscore(int points, int score)
            {
                if (points >= score)
                {
                    score = points;
                }
                return;
            }
       
        Rectangle AI(int Speed,Rectangle AIbody,Rectangle ballbody)
        {
            //Vector2 pos = new Vector2(P2HitBox.X, P2HitBox.Y);

            //Vector2 ballpos = new Vector2(BallHitBox.X, BallHitBox.Y);
            //float c = ballpos.Y / -ballpos.X;
            //float y = (-_graphics.GraphicsDevice.Viewport.Width) + c;
            //pos.Y = y;
            
            if (AIbody.Y < ballbody.Y)
            {
                AIbody.Y += 10;
            }
            else if (AIbody.Y > ballbody.Y)
            {
                AIbody.Y -= 10;
            }
            if (AIbody.Y == 0) 
            {
                AIbody.Y = 0;
            }
            if (AIbody.Y == _graphics.GraphicsDevice.Viewport.Height - AIbody.Height)
            {
                AIbody.Y = _graphics.GraphicsDevice.Viewport.Height - AIbody.Height;
            }


            return AIbody;
        }
        void storehighscore(int highscor,bool gamovr,StreamWriter _streamWriter) 
        {
            if (gamovr == true)
            {
                _streamWriter.Write(highscor);
            }
            _streamWriter.Close();
        }
        int loadhighscor(string filePath,StreamReader _streamreader,int highscor)
        {
            
            for (int i = 0; i < highscorefile.Length; i++)
            {
               highscor = int.Parse(_streamreader.ReadLine());
            }
            _streamreader.Close();
            return highscor;
        }
    }
}
