using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;


public class TileMap
{
    public int[,] MapData; // Each int is a tile index
    public int TileWidth { get; }
    public int TileHeight { get; }
    public Texture2D Tileset { get; }
    private List<Rectangle> _tileRects;

    public TileMap(Texture2D tileset, int tileWidth, int tileHeight, int[,] mapData, List<Rectangle> tileRects)
    {
        Tileset = tileset;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        MapData = mapData;
        _tileRects = tileRects;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int rows = MapData.GetLength(0);
        int cols = MapData.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int tileIndex = MapData[y, x];
                Rectangle sourceRect = _tileRects[tileIndex];
                Vector2 position = new Vector2(x * TileWidth, y * TileHeight);

                spriteBatch.Draw(Tileset, position, sourceRect, Color.White);
            }
        }
    }
}