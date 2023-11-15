using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
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

        //Baddy
        private Baddy baddy;



        //Food list
        private List<Food> foods;
        private Food currentFood;
       // private Queue<Food> foodQueue;
        private Texture2D poffin;

        //UI
        private HungryUI hungryUI;


        //background
        private Background background;

        //Random
        public static Random rng;

        //debug


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


            //debug initialise
            DebugManager.debugFont = Content.Load<SpriteFont>("debugFont");
            DebugManager.debugRecTex = Content.Load<Texture2D>("DebugBounds");
            DebugManager.sp = _spriteBatch;
            DebugManager.ShowDebug();
        }

        protected override void Update(GameTime gameTime)
        {

            currentMouse = Mouse.GetState();

            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (currentMouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
            {
                currentFood.Dropped();
                foods.Add(currentFood);
                currentFood = new Food(poffin, new Vector2(GraphicsDevice.Viewport.Width / 2 - poffin.Width / 2, 0));
            }


            if (foods.Count > 0)
            {
                for(int i = 0;i < foods.Count; i++) 
                {
                    foods[i].UpdateMe();
                    //  DebugManager.DebugString(foodQueue.Peek().GetPos().ToString(), Vector2.Zero);
                }
            }

            baddy.UpdateMe(foods);
            hungryUI.UpdateMe();

            


            oldMouse = currentMouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();



            //Backgroound drawning
            background.DrawMe(_spriteBatch);

            baddy.DrawMe(_spriteBatch);
            currentFood.DrawMe(_spriteBatch);
            if (foods.Count > 0)
            {
                for (int i =0; i<foods.Count;i++)
                {
                    foods[i].DrawMe(_spriteBatch);
                }
            }
            hungryUI.DrawMe(_spriteBatch);
            

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        
    }
}