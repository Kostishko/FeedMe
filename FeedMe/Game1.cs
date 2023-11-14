using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
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


        //Food list
        private List<Food> foods;
       // private Queue<Food> foodQueue;
        private Texture2D poffin;


        //background
        private Background background;

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

            //food load
            foods = new List<Food>();
            //foodQueue = new Queue<Food>();
            poffin = Content.Load<Texture2D>("poffin");
            //foodQueue.Enqueue(new Food(poffin, new Vector2(GraphicsDevice.Viewport.Width / 2 - poffin.Width / 2, 0)));
            foods.Add(new Food(poffin, new Vector2(GraphicsDevice.Viewport.Width / 2 - poffin.Width/2, 0)));

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
                Food curFodd = foods[0];
                curFodd.Dropped();
                foods.Add(curFodd);
                
                foods.Add(new Food(poffin, new Vector2(GraphicsDevice.Viewport.Width / 2 - poffin.Width / 2, 0)));
                foods.RemoveAt(0);
            }
          

            foreach(Food item in foods)
            {
                item.UpdateMe();
              //  DebugManager.DebugString(foodQueue.Peek().GetPos().ToString(), Vector2.Zero);
            }


            


            oldMouse = currentMouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();



            //Backgroound drawning
            background.DrawMe(_spriteBatch);

            foreach (Food item in foods)
            {
                item.DrawMe(_spriteBatch);
            }
            foreach (Food item in foods)
            {
                item.DrawMe(_spriteBatch);
            }

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        
    }
}