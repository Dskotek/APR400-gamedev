using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingWarriors.GameObjects;

public class Score
{
    private int _score;
    private int _coinsCollected;
    private SpriteFont _font;
    private Vector2 _position;
    private Vector2 _origin;

    public int CurrentScore => _score;
    public int CoinsCollected => _coinsCollected;

    public Score(SpriteFont font, Vector2 position)
    {
        _font = font;
        _position = position;
        _score = 0;
        _coinsCollected = 0;

        // Calculate text origin for vertical centering
        if (_font != null)
        {
            float textYOrigin = _font.MeasureString("Score").Y * 0.5f;
            _origin = new Vector2(0, textYOrigin);
        }
        else
        {
            _origin = Vector2.Zero;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
    }

    public void IncrementCoinsCollected()
    {
        _coinsCollected++;
    }

    public void Reset()
    {
        _score = 0;
        _coinsCollected = 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(
            _font,              // spriteFont
            $"Score: {_score}", // text
            _position,          // position
            Color.White,        // color
            0.0f,               // rotation
            _origin,            // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );
    }
}
