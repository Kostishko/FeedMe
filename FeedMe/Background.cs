using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace FeedMe
{
    internal class Background : Object2D
    {

        public Background(Texture2D tex, Vector2 pos, int width, int height) : base(tex, pos)
        {
            mainRec.Width = width;
            mainRec.Height = height;
        }

        public void SetTexture(Texture2D tex)
        {
            Texture = tex;
            mainRec.Width = Texture.Width;
            mainRec.Height = Texture.Height;
        }

        
    }
}
