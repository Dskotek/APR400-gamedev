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

    public void Draw(SpriteBatch spriteBatch)
    {
        _tileMap.Draw(spriteBatch);
    }

    public int TileHeight => 64;
}
