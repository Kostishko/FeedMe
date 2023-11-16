using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FeedMe
{
    internal class Baddy : Object2D
    {

        private int hungry;
        private float hungryTimer;
        private const float HUNGRYTIME = 8f;

        public Mouth mouth;


        public Baddy (Texture2D tex, Vector2 pos, Texture2D mouthTex) : base (tex, pos) 
        {

            hungry = 50;
            mouth = new Mouth(mouthTex, new Vector2(pos.X + tex.Width / 2 - mouthTex.Width / 4, pos.Y + 30),2, this);
        }

        public void UpdateMe(List<Food> foods)
        {
            base.UpdateMe();
            mouth.UpdateMe(foods);


            //hungryness
            if(hungryTimer>0)
            {
                hungryTimer-=0.1f;
            }
            else
            {
                hungryTimer = HUNGRYTIME;
                hungry--;
            }
            
        }

        public new void DrawMe(SpriteBatch sp)
        {
            base.DrawMe(sp);
            mouth.DrawMe(sp);
            
            //DebugManager.DebugString("Hungry: " + hungry, Vector2.Zero);
        }

        public int GetHungry()
        {
            return hungry;
        }
        public void AddHungry(int value) // it can be simplified, I thought that I wil call it from Game1
        {
            hungry += value;
        }


        
    }

    //Mouth class - collider to food catching and animations
    internal class Mouth : Object2D
    {

        private Rectangle[] sourceRec; //what frame we looking now
        

        //opening
        private float timer;
        private const float TIMER=2f;
        private Baddy bad;


        public Mouth (Texture2D tex, Vector2 pos, int frameAmount, Baddy bad) : base (tex,pos)
        {
            sourceRec = new Rectangle[frameAmount]; // frameRate exist for no reason in this case (I decided to rework it and simplify but not so scalebale)
            sourceRec[0] = new Rectangle(0, 0, tex.Width / frameAmount, tex.Height); //closed mouth rectangle
            collisionRec = sourceRec[0]; // collision rec shold has a size just like a source, and it goes on a right place with a first update
            this.bad = bad;

            //oppening
            timer = 0;

            sourceRec[1] = new Rectangle(sourceRec[0].Location.X+tex.Width/2, sourceRec[0].Location.Y, sourceRec[0].Width, sourceRec[0].Height); //oppened mouth
            
        }

        //checking if we cauth some food
        public void CatchFood(List<Food> foods)
        {
            for (int i = 0; i < foods.Count; i++) 
            {
                if(foods[i].GetRectangle().Intersects(collisionRec))
                {

                    foods.RemoveAt(i);
                    timer = TIMER; //timer for animation of oppened mouth
                    bad.AddHungry(1);
                    Game1.statistic.baddyAte++;
                    break;
                    
                }
            }

        }

        public void UpdateMe(List<Food> foods)
        {
            base.UpdateMe();
            CatchFood(foods);
            if (timer>=0) 
            {
                timer -= 0.1f;
            }
        }

        public new void DrawMe(SpriteBatch sp)
        {
            //drawing a particular frame according to timer (timer back to const if we caught food)
            if(timer >0)
            {
                sp.Draw(this.Texture, collisionRec, sourceRec[1] , Color.White);
            }
            else
            {
                sp.Draw(this.Texture, collisionRec, sourceRec[0], Color.White);
            }
        }




    }
}
