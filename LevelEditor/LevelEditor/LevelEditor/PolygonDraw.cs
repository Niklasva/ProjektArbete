using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public static class PolygonDraw
    {
        private static Texture2D _blankTexture;

        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            _blankTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _blankTexture.SetData(new[] { Color.White });
        }

        public static void LoadContent(Texture2D blankTexture)
        {
            _blankTexture = blankTexture;
        }

        public static void DrawLineSegment(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(_blankTexture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, lineWidth),
                       SpriteEffects.None, 0);
        }
    }
}
