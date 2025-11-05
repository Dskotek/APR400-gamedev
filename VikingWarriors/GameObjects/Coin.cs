using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace VikingWarriors.GameObjects;

public class Coin
{
    private AnimatedSprite _sprite;
    private Vector2 _position;
    private Vector2 _velocity;
    private float _speed;
    private SoundEffect _bounceSound;
    private SoundEffect _collectSound;

    public Vector2 Position => _position;
    public Rectangle Bounds => new Rectangle(
        (int)_position.X,
        (int)_position.Y,
        (int)_sprite.Width,
        (int)_sprite.Height
    );

    public Coin(AnimatedSprite sprite, Vector2 startPosition, float speed, 
                SoundEffect bounceSound, SoundEffect collectSound)
    {
        _sprite = sprite;
        _position = startPosition;
        _speed = speed;
        _bounceSound = bounceSound;
        _collectSound = collectSound;
        
        AssignRandomVelocity();
    }

    public void Update(GameTime gameTime, Rectangle playableBounds)
    {
        _sprite.Update(gameTime);

        Vector2 newPosition = _position + _velocity;

        Rectangle newBounds = new Rectangle(
            (int)newPosition.X,
            (int)newPosition.Y,
            (int)_sprite.Width,
            (int)_sprite.Height
        );

        Vector2 normal = Vector2.Zero;

        // Check left and right bounds
        if (newBounds.Left < playableBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newPosition.X = playableBounds.Left;
        }
        else if (newBounds.Right > playableBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newPosition.X = playableBounds.Right - _sprite.Width;
        }

        // Check top and bottom bounds
        if (newBounds.Top < playableBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newPosition.Y = playableBounds.Top;
        }
        else if (newBounds.Bottom > playableBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newPosition.Y = playableBounds.Bottom - _sprite.Height;
        }

        // Bounce if hit a wall
        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            _velocity = Vector2.Reflect(_velocity, normal);
            _bounceSound?.Play();
        }

        _position = newPosition;
    }

    public void Respawn(int screenWidth, int screenHeight)
    {
        // Divide the screen into grid based on coin size
        int totalColumns = screenWidth / (int)_sprite.Width;
        int totalRows = screenHeight / (int)_sprite.Height;

        // Choose random position
        int column = Random.Shared.Next(0, totalColumns);
        int row = Random.Shared.Next(0, totalRows);

        _position = new Vector2(column * _sprite.Width, row * _sprite.Height);
        
        // Assign new random velocity
        AssignRandomVelocity();
        
        // Play collect sound
        _collectSound?.Play();
    }

    private void AssignRandomVelocity()
    {
        // Generate a random angle
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to direction vector
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Apply speed
        _velocity = direction * _speed;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }
}
