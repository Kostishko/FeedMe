using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FeedMe
{
    internal class Food : Object2D
    {

        private Vector2 velocity;
        private bool isDropped;

        private Color[] colours;

        public Food(Texture2D tex, Vector2 pos) : base(tex, pos)
        {

            velocity = new Vector2(0, 5);
            isDropped = false;

            colours = new Color[5];
            colours[0] = Color.Purple;
            colours[1] = Color.Red;
            colours[2] = Color.Yellow;
            colours[3] = Color.Green;
            colours[4] = Color.Blue;

            Color = colours[Game1.rng.Next(0, 5)];


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
