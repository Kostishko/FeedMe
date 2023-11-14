using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FeedMe
{
    internal class Background : Object2D
    {

        public Background(Texture2D tex, Vector2 pos, int width, int height) : base(tex, pos)
        {
            mainRec.Width = width;
            mainRec.Height = height;
        }

        
    }
}
