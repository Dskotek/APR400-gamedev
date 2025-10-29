using System;
using System.Collections.Generic;
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
    public override void Update(GameTime gameTime, Vector2 playerPosition, List<Enemy> otherEnemies = null)
    {
        base.Update(gameTime, playerPosition);

        Vector2 separationForce = Vector2.Zero;

        if (otherEnemies != null)
        {
            foreach (var enemy in otherEnemies)
            {
                if (enemy != this)
                {
                    Vector2 diff = _position - enemy.Position;
                    float distance = diff.Length();

                    if (distance < 80f && distance > 0)
                    {
                        diff.Normalize();
                        separationForce += diff * (80f - distance) * 0.3f;
                    }
                }

            }
        }

        Vector2 direction = playerPosition - _position;
        if (direction != Vector2.Zero)
        {
            direction.Normalize();

            Vector2 finalDirection = (direction * 2.0f) + separationForce;
            if (finalDirection != Vector2.Zero)
            {
                finalDirection.Normalize();
            }

            _position += finalDirection * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
