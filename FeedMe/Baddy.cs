using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FeedMe
{
    internal class Baddy : Object2D
    {


        public Baddy (Texture2D tex, Vector2 pos) : base (tex, pos) 
        {
            
        }


        
    }

    internal class Mouth : Object2D
    {

        private Rectangle sourceRec; //what frame we looking now
        private Vector2[] sourcePos;

        //opening
        private float timer;
        private const float TIMER=2f;


        public Mouth (Texture2D tex, Vector2 pos, int frameAmount) : base (tex,pos)
        {

            sourceRec = new Rectangle((int)Math.Round(pos.X), (int)Math.Round(pos.Y), tex.Width / frameAmount, tex.Height);

            //oppening
            timer= TIMER;
            sourcePos = new Vector2[frameAmount];
            sourcePos[0] = Vector2.Zero;
            for (int i = 1; i < sourcePos.Length-1; i++)
            {
                sourcePos[i] = sourcePos[i - 1] + new Vector2(tex.Width / frameAmount, 0);
            }
        }

        public void CatchFood()
        {

        }




    }
}
