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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        const int plrSpeed = 10;
        const int BallSpeed = 8;
        bool Nstarted = true;
        int highscore = 1972;
        int exitcount = 0;
        int timer;
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
            }
            pointssystem(P1pointCounter,P2pointCounter, BallHitBox.X, BallHitBox.Y, gamePlaying);
            level(P1pointCounter,cheats,BallHitBox.Y,P2HitBox.Y,P2HitBox.Height);
            movePlr(plrSpeed);
            moveball(BallSpeed);
            ishighscore(P1pointCounter, highscore);
            clock(P2pointCounter, timer, gameover, Nstarted);
            base.Update(gameTime);  
        }
        protected override void Draw(GameTime gameTime)
        {
            Vector2 textPos = new Vector2(0, 0);

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            if (Nstarted == true && gameover == true)
            {
                _spriteBatch.Draw(gameoverscreen, background, Color.White);
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
            void pointssystem(int Point, int Point2, int X,int Y,bool inplay)
        {
            

                if (X > _graphics.GraphicsDevice.Viewport.Width / 2)
                {
                    Point++;
                }
                else
                {
                    Point2++;
                }
                X = _graphics.GraphicsDevice.Viewport.Width / 2;
                Y = _graphics.GraphicsDevice.Viewport.Height / 2;
               inplay = false;


            

            return;
        }
            void level(int level,bool CHEATS,int BALL_Y,int PLR_Y,int Height)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed ||Keyboard.GetState().IsKeyDown(Keys.C))
            {
                cheats = true;
            }
            if (cheats == false)
            {
                if (level < 10)
                {
                    
                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 1;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 1;
                    }
                  

                }
                else if (level >= 10 && level < 20)
                {
                   
                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 2;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 2;
                    }
                    
                }
                else if (level >= 20 && level < 30)
                {
                  

                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 3;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 3;
                    }
                   
                }
                else if (level >= 30 && level < 40)
                {
                   
                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 4;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 4;
                    }
                   
                }
                else if (level >= 40 && level < 50)
                {
                    

                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 5;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 5;
                    }
                   
                }
                else if (level >= 50 && level < 60)
                {
                   

                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 6;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 6;
                    }
                   
                }
                else if (level >= 60 && level < 70)
                {

                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 7;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 7;
                    }
                    
                }
                else if (level >= 70 && level < 80)
                {
                   
                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 8;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 8;
                    }
                   
                }
                else if (level >= 80 && level < 90)
                {
                   
                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 9;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 9;
                    }
                   
                }
                else if (level >= 90 && level < 100)
                {
                    

                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 10;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 10;
                    }
                   
                }
                else if (level >= 100)
                {
                    

                    if (PLR_Y > BALL_Y)
                    {
                        PLR_Y -= 11;
                    }
                    else if (PLR_Y < BALL_Y)
                    {
                        PLR_Y += 11;
                    }
                   
                }
                if (PLR_Y < 0)
                {
                    PLR_Y = 0;
                }
                if (PLR_Y > _graphics.GraphicsDevice.Viewport.Height -Height)
                    PLR_Y = _graphics.GraphicsDevice.Viewport.Height - Height;
                return;
            }
        }//broken speeds FIX!!!!!!
            void moveball(int Speed)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) || gamePlaying == true)
                {
                    gamePlaying = true;

                    if (BallHitBox.Intersects(P1HitBox) && BallHitBox.X <= P1HitBox.Right)
                    {
                        Left = true;
                        playbounce.Play();
                    }
                    else if (BallHitBox.Intersects(P2HitBox) && BallHitBox.X <= P2HitBox.Left)
                    {
                        Left = false;
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
                return;
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
            void clock(int points, int clock,bool GO,bool NS)
            {
                if (points < 10)
                {
                    clock++;
                }
                else if (points == 10)
                {
                    GO = true;
                    NS = true;
                }
            return;
            }
            void ishighscore(int points, int score)
            {
                if (points >= score)
                {
                    score = points;
                }
                return;
            }
       
        void AI()
        {
            Vector2 pos = new Vector2(50,400);
            Vector2 ballpos = new Vector2(50, 200);
            pos.Y = MathHelper.Lerp(pos.Y, ballpos.Y, 1);
            
        }
        void storehighscore(int highscore) 
        {
            string highscorefile = @"C:\Users\oscar\Documents\GitHub\pong\highscore_pong.txt";
        }
    }
}
