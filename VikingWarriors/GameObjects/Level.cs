using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace VikingWarriors.GameObjects;

public class Level
{



    

   
    private TileMap _tileMap;
    private Texture2D _tileset;
    private Rectangle _playableBounds;

    public Rectangle PlayableBounds => _playableBounds;
    public void LoadContent(ContentManager content)
    {
        List<Rectangle> tileRects = new List<Rectangle>
        {
            new Rectangle(0, 0, 64, 64),     // 0: Top left
            new Rectangle(64, 0, 64, 64),    // 1: Top mid
            new Rectangle(128, 0, 64, 64),   // 2: Top right
            new Rectangle(0, 64, 64, 64),    // 3: Mid left
            new Rectangle(64, 64, 64, 64),   // 4: Mid mid
            new Rectangle(128, 64, 64, 64),  // 5: Mid right
            new Rectangle(0, 128, 64, 64),   // 6: Bot left
            new Rectangle(64, 128, 64, 64),  // 7: Bot mid
            new Rectangle(128, 128, 64, 64), // 8: Bot right
            new Rectangle(512, 0, 64, 64),   // 9: Dirt top left
            new Rectangle(576, 0, 64, 64),   // 10: Dirt top right
            new Rectangle(512, 64, 64, 64),  // 11: Dirt bot left
            new Rectangle(576, 64, 64, 64),  // 12: Dirt bot right
            new Rectangle(0, 644, 63, 123),  // 13: Green tree
            new Rectangle(383, 793, 128, 38) // 14: Fence
        };

        int[,] mapData = new int[,]
        {
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
            {3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5},
            {6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8}
        };

        int[,] decorationData = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 13, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 13, 0, 0, 0, 14, 0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        _tileset = content.Load<Texture2D>("images/RPGpack_sheet");
        _tileMap = new TileMap(_tileset, 64, 64, mapData, tileRects, decorationData);

        // Calculate playable bounds (excluding walls)
        // mapData.GetLength(1) gives width (number of columns)
        // mapData.GetLength(0) gives height (number of rows)
        _playableBounds = new Rectangle(
            64,  // One tile in from left for walls
            64,  // One tile down from top for walls
            (mapData.GetLength(1) - 2) * 64,   // Width minus two tiles for walls
            (mapData.GetLength(0) - 2) * 64    // Height minus two tiles for walls
        );
    }

    public void DrawBackground(SpriteBatch spriteBatch, float playerBotY)
    {
        _tileMap.DrawBase(spriteBatch);
        _tileMap.DrawDecorationsBackground(spriteBatch, playerBotY);
    }

    public void DrawForeground(SpriteBatch spriteBatch, float playerBotY)
    {
        _tileMap.DrawDecorationsForeground(spriteBatch, playerBotY);
    }

    public void Draw(SpriteBatch spriteBatch, float playerBotY)
    {
        _tileMap.DrawBase(spriteBatch);
    }

    public int TileHeight => 64;

    // Returnera true om positionen är på en "blockerad" tile (endast träd)
    // Kollar en enstaka punkt
    private bool IsPointBlocked(float x, float y)
    {
        int tileX = (int)(x / 64);
        int tileY = (int)(y / 64);
        
        int mapHeight = _tileMap.DecorationData.GetLength(0);
        int mapWidth = _tileMap.DecorationData.GetLength(1);
        
        if (tileX < 0 || tileY < 0 || tileY >= mapHeight || tileX >= mapWidth)
            return true;

        int decoration = _tileMap.DecorationData[tileY, tileX];
        return decoration == 13; // Endast träd, inte staket
    }

    // Kolla om någon del av spelaren/fienden är blockerad
    // Vi kollar centrum och några punkter runt objektet
    public bool IsBlocked(Vector2 position, float width = 32, float height = 32)
    {
        // Kolla centrum
        if (IsPointBlocked(position.X + width / 2, position.Y + height / 2))
            return true;
        
        // Kolla fyra hörn
        if (IsPointBlocked(position.X, position.Y)) // Topp vänster
            return true;
        if (IsPointBlocked(position.X + width, position.Y)) // Topp höger
            return true;
        if (IsPointBlocked(position.X, position.Y + height)) // Botten vänster
            return true;
        if (IsPointBlocked(position.X + width, position.Y + height)) // Botten höger
            return true;

        return false;
    }
}
