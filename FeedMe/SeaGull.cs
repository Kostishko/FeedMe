using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace FeedMe
{
    internal class SeaGull : Object2D
    {

        private Vector2 velocity;
        private int maxX, maxY;
        private SpriteEffects effectFlip;
        private int foodCatched; 
        public int FoodCaughtCounter
        { get => foodCatched; }

        public SeaGull (Texture2D tex, Vector2 pos, int maxX, int maxY): base (tex, pos) 
        {
            //vieport bounds
            this.maxX = maxX;
            this.maxY=maxY;

            //Start position (anywhere on the top part of screen
            Position = new Vector2(Game1.rng.Next(0+mainRec.Width,maxX), Game1.rng.Next(0+ mainRec.Height,maxY/2));
            //start velocity
            velocity = new Vector2(Game1.rng.Next(-2, 2), 0);
            //Changes velocity till it not become not 0 (just in case)
            while (velocity.X==0)
            {
                velocity = new Vector2(Game1.rng.Next(-2, 2), 0);
            }

            //Fliping according to direction of movement  
            if (velocity.X > 0)
                effectFlip = SpriteEffects.FlipHorizontally;
            else
                effectFlip = SpriteEffects.None;

            //caught food counter
            foodCatched = 0;

        }

        public new void UpdateMe()
        {
            base.UpdateMe();
            //movement
            Position += velocity;
            
            if(Position.X+mainRec.Width<0||Position.X-mainRec.Width>maxX)
            {
                Position.Y = Game1.rng.Next(0+mainRec.Height,maxY/2); 
                velocity.X*=-1.05f; //increasing difficulty
                

                if (velocity.X > 0)
                    effectFlip = SpriteEffects.FlipHorizontally;
                else
                    effectFlip = SpriteEffects.None;
            }

        }
        /// <summary>
        /// Return true if it caught and adjust size of particular seagull
        /// </summary>
        /// <param name="food">check if this gull catch the food </param>
        /// <returns></returns>
        public bool catchFood(Food food)
        {
            if (food.GetRectangle().Intersects(collisionRec))
            {
                //Adjust rec for drawning
                mainRec.Size = new Vector2(mainRec.Size.X * 1.2f, mainRec.Size.Y * 1.2f).ToPoint();

                //Adjusting for collision rec
                collisionRec.Size = new Vector2(mainRec.Size.X*0.8f, mainRec.Size.Y*0.8f).ToPoint();
                collisionRec.Location = new Vector2(mainRec.Location.X+mainRec.Width/2-collisionRec.Width/2, 
                    mainRec.Location.Y+mainRec.Height/2-collisionRec.Height/2).ToPoint();

                foodCatched++; // counter that we caught the food
                return true; 
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Drawning with fliping
        /// </summary>        
        public new void DrawMe(SpriteBatch sp)
        {
            sp.Draw(Texture, mainRec, null, Color.White, 0, Vector2.Zero, effectFlip,0);
        }

    }
}
