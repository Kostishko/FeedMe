using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace FeedMe
{
    static class DebugManager
    {
        static public SpriteFont debugFont;
        static public SpriteBatch sp;
        static public Texture2D debugRecTex;

        static private bool isActive = false;

        /// <summary>
        /// It works ONLY in Draw method
        /// </summary>
        /// <param name="message">Your string message</param>
        /// <param name="pos">Where to draw</param>
        public static void DebugString(string message, Vector2 pos)
        {
            if (isActive)
            {                
                sp.DrawString(debugFont, message, pos, Color.White);
                
            }

        }

        /// <summary>
        /// It works ONLY in Draw method
        /// </summary>
        /// <param name="rec">place your rectangle here</param>
        public static void DebugRectangle(Rectangle rec)
        {
            if (isActive)
            {
                sp.Draw(debugRecTex, rec, Color.White);
            }
        }

        public static void ShowDebug()
        {
            isActive = true;
        }

        public static void HideDebug() 
        {
            isActive = false;
        }


    }
}
