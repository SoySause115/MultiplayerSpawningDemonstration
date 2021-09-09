using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiplayerSpawningDemonstration
{
    class SpawnCircle
    {
        //position
        public Vector2 Center { get; set; }
        //radius of the circle
        public float Radius { get; set; }
        Texture2D texture;

        public SpawnCircle(Vector2 center, float radius, Texture2D texture)
        {
            Center = center;
            Radius = radius;
            this.texture = texture;
        }

        public bool Contains(Vector2 point)
        {
            //true if the vector length is less than the radius
            return ((point - Center).Length() <= Radius * 1.5);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle((int)Center.X, (int)Center.Y, 10, 10), Color.Black);
            spriteBatch.End();
        }
    }
}
