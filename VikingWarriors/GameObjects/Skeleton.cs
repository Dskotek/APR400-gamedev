using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace VikingWarriors.GameObjects;

public class Skeleton : Enemy
{
    public Skeleton(AnimatedSprite sprite, Vector2 position, float speed)
        : base(sprite, position, speed)
    {
        _sprite.Scale = new Vector2(3.0f, 3.0f);
    }
    public override void Update(GameTime gameTime, Vector2 playerPosition)
    {
        base.Update(gameTime, playerPosition);

        Vector2 direction = playerPosition - _position;
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (playerPosition.X < _position.X)
        {
            _sprite.Effects = SpriteEffects.FlipHorizontally;
        }
        else
        {
            _sprite.Effects = SpriteEffects.None;
        }

    }
}
