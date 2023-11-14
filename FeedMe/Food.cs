using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FeedMe
{
    internal class Food : Object2D
    {

        private Vector2 velocity;
        private bool isDropped;

        public Food (Texture2D tex, Vector2 pos) : base (tex, pos) 
        {

            velocity = new Vector2(0, 5);
            isDropped = false;

        }


        public new void UpdateMe()
        {
            if (isDropped) 
            {
                Position += velocity;
                collisionRec.Location += velocity.ToPoint();
            }

            base.UpdateMe();
        }

        public void Dropped()
        {
            isDropped = true;
        }


    }
}
