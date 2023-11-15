using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System;

namespace FeedMe
{
    internal class HungryUI : Object2D 
    {

        private Rectangle activeRectangle;
        private Baddy bad;

        private Vector2 scale;
        private float coefX;
        private float coefY;

        public HungryUI(Texture2D tex, Vector2 pos, Baddy baddy) : base(tex, pos) 
        {
            bad = baddy;
            activeRectangle = mainRec;
            coefX = (float)Texture.Width /100;
            coefY = (float)Texture.Height /100;
            scale = Vector2.Zero;

        }

        public new void UpdateMe()
        {
            scale.X = bad.GetHungry() * coefX;
            scale.Y = bad.GetHungry() * coefY;
            activeRectangle.Size = new Vector2(scale.X, scale.Y).ToPoint();
            activeRectangle.Location  =new Vector2( mainRec.Location.X + (mainRec.Width / 2 - activeRectangle.Width/2),
                mainRec.Location.Y+(mainRec.Height/2 - activeRectangle.Height/2)).ToPoint();
        }

        public new void DrawMe(SpriteBatch sp)
        {
            
            sp.Draw(Texture, mainRec, Color.DarkGreen);
            sp.Draw(Texture, activeRectangle, null, Color.White, 0 , Vector2.Zero, SpriteEffects.None,0);
            //DebugManager.DebugRectangle(activeRectangle);
                
        }




    }
}
