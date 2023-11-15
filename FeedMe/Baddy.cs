using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FeedMe
{
    internal class Baddy : Object2D
    {

        public int hungry;
        public Mouth mouth;


        public Baddy (Texture2D tex, Vector2 pos, Texture2D mouthTex) : base (tex, pos) 
        {

            hungry = 50;
            mouth = new Mouth(mouthTex, new Vector2(pos.X + tex.Width / 2 - mouthTex.Width / 4, pos.Y + 30),2);
        }

        public void UpdateMe(List<Food> foods)
        {
            base.UpdateMe();
            mouth.UpdateMe(foods);
        }

        public new void DrawMe(SpriteBatch sp)
        {
            base.DrawMe(sp);
            mouth.DrawMe(sp);
        }


        
    }

    internal class Mouth : Object2D
    {

        private Rectangle[] sourceRec; //what frame we looking now
        

        //opening
        private float timer;
        private const float TIMER=2f;


        public Mouth (Texture2D tex, Vector2 pos, int frameAmount) : base (tex,pos)
        {
            sourceRec = new Rectangle[frameAmount];
            sourceRec[0] = new Rectangle(0, 0, tex.Width / frameAmount, tex.Height);
            collisionRec = sourceRec[0];

            //oppening
            timer = 0;

            sourceRec[1] = new Rectangle(sourceRec[0].Location.X+tex.Width/2, sourceRec[0].Location.Y, sourceRec[0].Width, sourceRec[0].Height);
            
        }

        public void CatchFood(List<Food> foods)
        {
            for (int i = 0; i < foods.Count; i++) 
            {
                if(foods[i].GetRectangle().Intersects(collisionRec))
                {

                    foods.RemoveAt(i);
                    timer = TIMER;
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
