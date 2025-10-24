using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Tile
{
    public Texture2D Texture { get; set; }
    public Rectangle SourceRect { get; set; }
    public Vector2 Position { get; set; }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, SourceRect, Color.White);
    }
}