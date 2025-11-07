using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace VikingWarriors.GameObjects;

public class Player
{
    private AnimatedSprite _sprite;
    private Animation _animDown, _animLeft, _animUp, _animRight;
    private Vector2 _position;

    private const float MOVEMENT_SPEED = 5.0f;

    public Vector2 Position => _position;
    public Rectangle Bounds => new Rectangle(
        (int)_position.X,
        (int)_position.Y,
        (int)_sprite.Width,
        (int)_sprite.Height
    );

    public Player(Animation animDown, Animation animLeft, Animation animUp, Animation animRight, Vector2 startPosition)
    {
        _animDown = animDown;
        _animLeft = animLeft;
        _animUp = animUp;
        _animRight = animRight;
        
        _sprite = new AnimatedSprite(_animDown);
        _sprite.Scale = new Vector2(2.5f, 2.5f);
        _position = startPosition;
    }

    public void Update(GameTime gameTime, Rectangle roomBounds, Level level)
    {
        // Spara tidigare position
        Vector2 previousPosition = _position;
        
        bool isMoving = HandleInput(gameTime);

        // Kolla kollision med staket/träd - om blockerad, återgå till förra positionen
        if (level.IsBlocked(_position, _sprite.Width, _sprite.Height))
        {
            _position = previousPosition;
        }

        if (isMoving)
        {
            _sprite.Update(gameTime);
        }
        else
        {
            _sprite.CurrentFrame = 0;
        }

        // Clamp player position within room bounds
        ClampToBounds(roomBounds);
    }

    private bool HandleInput(GameTime gameTime)
    {
        float speed = MOVEMENT_SPEED;
        bool moved = false;

        // Sprint modifier
        if (Core.Input.Keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        Animation newAnimation = _sprite.Animation;

        // Movement input
        if (Core.Input.Keyboard.IsKeyDown(Keys.W) || Core.Input.Keyboard.IsKeyDown(Keys.Up))
        {
            _position.Y -= speed;
            newAnimation = _animUp;
            moved = true;
        }

        if (Core.Input.Keyboard.IsKeyDown(Keys.S) || Core.Input.Keyboard.IsKeyDown(Keys.Down))
        {
            _position.Y += speed;
            newAnimation = _animDown;
            moved = true;
        }

        if (Core.Input.Keyboard.IsKeyDown(Keys.A) || Core.Input.Keyboard.IsKeyDown(Keys.Left))
        {
            _position.X -= speed;
            newAnimation = _animLeft;
            moved = true;
        }

        if (Core.Input.Keyboard.IsKeyDown(Keys.D) || Core.Input.Keyboard.IsKeyDown(Keys.Right))
        {
            _position.X += speed;
            newAnimation = _animRight;
            moved = true;
        }
        

        // Update animation if changed
        if (_sprite.Animation != newAnimation && newAnimation != null)
        {
            _sprite.Animation = newAnimation;
        }

        return moved;
    }

    private void ClampToBounds(Rectangle roomBounds)
    {
        Rectangle bounds = Bounds;

        if (bounds.Left < roomBounds.Left)
        {
            _position.X = roomBounds.Left;
        }
        else if (bounds.Right > roomBounds.Right)
        {
            _position.X = roomBounds.Right - _sprite.Width;
        }

        if (bounds.Top < roomBounds.Top)
        {
            _position.Y = roomBounds.Top;
        }
        else if (bounds.Bottom > roomBounds.Bottom)
        {
            _position.Y = roomBounds.Bottom - _sprite.Height;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }
}