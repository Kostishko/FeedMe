using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FeedMe
{
    internal class Object2D
    {
        protected Vector2 Position;
        protected Texture2D Texture;
        protected Color Color;


        protected Rectangle mainRec; // rectangle for drawning
        protected Rectangle collisionRec; // rectangle for collisions
        
        

        /// <summary>
        /// Simple object with no animation. Collision rectangle equal to main
        /// </summary>
        /// <param name="tex">Main texture - used as a size for Rectangle</param>
        /// <param name="pos">Position of left upper corner</param>
        public Object2D(Texture2D tex, Vector2 pos)
        {
            Texture = tex;
            Position = pos;
            mainRec = new Rectangle((int)Math.Round(pos.X), (int)Math.Round(pos.Y), tex.Width, tex.Height);
            collisionRec = mainRec;

            //default colour
            Color = Color.White;

        }


        public void UpdateMe()
        {
            mainRec.Location = Position.ToPoint();
            collisionRec.Location = Position.ToPoint();
        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(Texture, mainRec,Color);
        }

        public Vector2 GetPos()
        {
            return Position;
        }





    }
}
