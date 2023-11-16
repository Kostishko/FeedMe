using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
//using System.Windows.Forms;

namespace FeedMe
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Control
        private MouseState currentMouse;
        private MouseState oldMouse;
        private KeyboardState currentKeybouard;
        private KeyboardState oldKeyboard;

        //Baddy
        private Baddy baddy;



        //Food list
        private List<Food> foods;
        private Food currentFood;
       // private Queue<Food> foodQueue;
        private Texture2D poffin;

        //Seaguls
        private List<SeaGull> seagulls;

        //UI
        private HungryUI hungryUI;
        static public SpriteFont uiFont;
        private String loseMessage;


        //background
        private Background background;

        

        //Random
        public static Random rng;

        //statistic
        public struct Statistic
        {
            public float timer;
            public int droppedPoff;
            public int baddyAte;
            public int gullsAte;
            public int gullsFeeded;
            public int mostFeededGull;
            public float dropRate;
            public float feedRate;
        }

        static public Statistic statistic;
        string statString;

        // Game stats
        private enum GameStates
        {
            Start,
            Lose,
            Win,
            Game,
            Statistic
        }
        private GameStates state;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            rng = new Random();

            uiFont = Content.Load<SpriteFont>("mainFont");

            StartStateInitialise();
            state = GameStates.Start;

            //debug initialise
            DebugManager.debugFont = Content.Load<SpriteFont>("debugFont");
            DebugManager.debugRecTex = Content.Load<Texture2D>("DebugBounds");
            DebugManager.sp = _spriteBatch;
            DebugManager.HideDebug();

        }

        protected override void Update(GameTime gameTime)
        {
            //currentState of buttons and keys
            currentMouse = Mouse.GetState();
            currentKeybouard =  Keyboard.GetState();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch(state)
            {
                case GameStates.Start:
                    StartStateUpdate();
                    break;
                case GameStates.Game:
                    GameStateUpdate(gameTime);
                    break;
                case GameStates.Lose:
                    LoseStateUpdate();
                    break;
                case GameStates.Win:
                    WinStateUpdate();
                    break;
                case GameStates.Statistic:
                    StatisticStateUpdate();
                    break;

            }

            

            


            //keys state frame ago
            oldKeyboard = currentKeybouard;
            oldMouse = currentMouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();



            //Backgroound drawning
            background.DrawMe(_spriteBatch);

            //Drawning
            switch (state)
            {
                case GameStates.Start:
                    StartStateDraw();
                    break;
                case GameStates.Game:
                    GameStateDrawMe();
                    break;
                case GameStates.Lose:
                    LoseStateDraw();
                    break;
                case GameStates.Win:
                    WinStateDraw();
                    break;
                case GameStates.Statistic:
                    StatisticStateDraw();
                    break;
            }


            DebugManager.DebugString("statistic timer: " + statistic.timer + "\nGameTime Second: " + gameTime.ElapsedGameTime.TotalSeconds, Vector2.Zero);
                    _spriteBatch.End();

            // TODO: Add your drawing code here
            

            base.Draw(gameTime);
        }

        
        public void StartStateInitialise()
        {
           
            //food load
            foods = new List<Food>();
            poffin = Content.Load<Texture2D>("poffin");
            currentFood = new Food(poffin, new Vector2(GraphicsDevice.Viewport.Width / 2 - poffin.Width / 2, 0));

            //Baddy
            var baddyTex = Content.Load<Texture2D>("143");
            baddy = new Baddy(baddyTex, new Vector2(GraphicsDevice.Viewport.Width / 2 - baddyTex.Width / 2, GraphicsDevice.Viewport.Height - baddyTex.Height), Content.Load<Texture2D>("mouth"));
            //Baddy UI
            hungryUI = new HungryUI(Content.Load<Texture2D>("face"), new Vector2(GraphicsDevice.Viewport.Width * 8 / 10, GraphicsDevice.Viewport.Height * 4 / 6), baddy);

            //back load
            background = new Background(Content.Load<Texture2D>("back"), Vector2.Zero, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //seagulls
            seagulls = new List<SeaGull>();
            for (int i = 0; i < 7; i++)
            {
                seagulls.Add(new SeaGull(Content.Load<Texture2D>("gull"), Vector2.Zero, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            }

            //Statistics
            statistic = new Statistic();

        }

        public void StartStateUpdate()
        {
            if (currentKeybouard.IsKeyUp(Keys.Space) && oldKeyboard.IsKeyDown(Keys.Space))
            {
                StartStateInitialise();
                state = GameStates.Game;
            }
        }
        public void StartStateDraw()
        {
            _spriteBatch.DrawString(uiFont, "Press SPACE to start", new Vector2(GraphicsDevice.Viewport.Width/2 - 150, 
                GraphicsDevice.Viewport.Height/2 - 11), Color.Yellow);
        }

        public void GameStateUpdate(GameTime gameTime)
        {
            //Dropping foods
            if (currentMouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
            {
                currentFood.Dropped();
                foods.Add(currentFood);
                currentFood = new Food(poffin, new Vector2(GraphicsDevice.Viewport.Width / 2 - poffin.Width / 2, 0));
                statistic.droppedPoff++;
            }

            // foods updating
            if (foods.Count > 0)
            {
                for (int i = 0; i < foods.Count; i++)
                {
                    foods[i].UpdateMe();
                    //  DebugManager.DebugString(foodQueue.Peek().GetPos().ToString(), Vector2.Zero);
                }
            }


            //updating baddy
            baddy.UpdateMe(foods);

            if (baddy.GetHungry()>=100)
            {
                background.SetTexture(Content.Load<Texture2D>("win"));
                state = GameStates.Win;
            }

            if (baddy.GetHungry()<=0)
            {
                background.SetTexture(Content.Load<Texture2D>("lose"));
                loseMessage = "Your pokemon\nare starving!\nPress SPACE to start";
                state = GameStates.Lose;
            }

            //Seaguls updating
            for (int i = 0; i < seagulls.Count; i++)
            {
                seagulls[i].UpdateMe();

            }

            //seaguls are catching food
            for (int i = 0; i < seagulls.Count; i++)
            {
                for (int j = 0; j < foods.Count; j++)
                {
                    if (seagulls[i].catchFood(foods[j]))
                    {
                        foods.RemoveAt(j);
                        if (seagulls[i].FoodCaughtCounter>6)
                        {
                            background.SetTexture(Content.Load<Texture2D>("lose"));
                            loseMessage = "Some seagull have\nate too much food!\nPress SPACE to start";
                            state = GameStates.Lose;
                        }
                        break;
                    }
                }

            }

            //Pictures of Face updating
            hungryUI.UpdateMe();

            //Statistic
            statistic.timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            


        }

        public void GameStateDrawMe()
        {
            //baddy drawing
            baddy.DrawMe(_spriteBatch);

            //food
            currentFood.DrawMe(_spriteBatch); //not dropped
            //dropped food
            if (foods.Count > 0)
            {
                for (int i = 0; i < foods.Count; i++)
                {
                    foods[i].DrawMe(_spriteBatch);
                }
            }

            for (int i = 0; i < seagulls.Count; i++)
            {
                seagulls[i].DrawMe(_spriteBatch);
            }

            //ui hungry
            hungryUI.DrawMe(_spriteBatch);
        }

        public void LoseStateUpdate()
        {
            if (currentKeybouard.IsKeyUp(Keys.Space)&&oldKeyboard.IsKeyDown(Keys.Space))
            {
                StartStateInitialise();
                state = GameStates.Start;
            }
        }
        public void LoseStateDraw()
        {
            _spriteBatch.DrawString(uiFont, loseMessage, new Vector2(GraphicsDevice.Viewport.Width / 3+50, GraphicsDevice.Viewport.Height / 2), Color.Chocolate);
        }

        public void WinStateUpdate()
        {
            if (currentKeybouard.IsKeyUp(Keys.Space)&&oldKeyboard.IsKeyDown(Keys.Space))
            {
                statString = StatisticCalculation();
                background.SetTexture(Content.Load<Texture2D>("back"));
                state = GameStates.Statistic;
            }
        }
        public void WinStateDraw()
        {
            _spriteBatch.DrawString(uiFont, "YOU WIN!\nPress SPACE to watch\nyour statistic.", new Vector2(GraphicsDevice.Viewport.Width/2-150, GraphicsDevice.Viewport.Height*2/3), Color.Chocolate);
            
        }

        public String StatisticCalculation()
        {
            for (int i = 0;i<seagulls.Count;i++) 
            {
                statistic.gullsAte += seagulls[i].FoodCaughtCounter; //calculate how many ate all gulls
                if (seagulls[i].FoodCaughtCounter > 0) //calculate how many gulls ate our food
                    statistic.gullsFeeded++;
                if (seagulls[i].FoodCaughtCounter > statistic.mostFeededGull) //seagull that ate most part of food
                    statistic.mostFeededGull = seagulls[i].FoodCaughtCounter;
            }
            statistic.dropRate = statistic.timer / statistic.droppedPoff;
            statistic.feedRate = statistic.timer / statistic.baddyAte;

            return "Your game took " + Math.Round(statistic.timer,2) + " seconds.\nYou dropped " + statistic.droppedPoff + " poffins.\n" + statistic.baddyAte + " of them eaten by snorlax\n"
                + "and " + statistic.gullsAte + " of them by seagulls.\nYou fed " + statistic.gullsFeeded + " seagulls\nand the largest seagull ate " + statistic.mostFeededGull +
                "poffins\nYour drope rate was " + Math.Round(statistic.dropRate,2) + " poffin/sec\nand your feed rate was: " + Math.Round(statistic.feedRate,2) + " poffins/sec\n\n Press SPACE to start again";
        }

        public void StatisticStateUpdate()
        {
            if (currentKeybouard.IsKeyUp(Keys.Space) && oldKeyboard.IsKeyDown(Keys.Space))
            {
                StartStateInitialise();
                state = GameStates.Start;               
            }
        }
        public void StatisticStateDraw()
        {
            _spriteBatch.DrawString(uiFont, statString, Vector2.One, Color.Black);
        }





    }
}