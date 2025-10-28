using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace VikingWarriors.GameObjects;

public class Zombie : Enemy
{
    private Animation _zombieDown, _zombieRight, _zombieUp, _zombieLeft;
    public Zombie(AnimatedSprite sprite, Vector2 position, float speed,
                Animation zombieDown, Animation zombieRight, Animation zombieUp, Animation zombieLeft)
        : base(sprite, position, speed)
    {
        _sprite.Scale = new Vector2(2.0f, 2.0f);
        _zombieDown = zombieDown;
        _zombieRight = zombieRight;
        _zombieUp = zombieUp;
        _zombieLeft = zombieLeft;
        
        // Set initial animation
        _sprite.Animation = _zombieDown;
    }
    public override void Update(GameTime gameTime, Vector2 playerPosition, List<Enemy> otherEnemies = null)
    {
        base.Update(gameTime, playerPosition);


        Vector2 direction = playerPosition - _position;
        Vector2 separationForce = Vector2.Zero;

        if(otherEnemies != null)
        {
            foreach(var enemy in otherEnemies)
            {
                if (enemy != this)
                {
                    Vector2 diff = _position - enemy.Position;
                    float distance = diff.Length();

                    if(distance < 80f && distance > 0)
                    {
                        diff.Normalize();
                        separationForce += diff * (80f - distance) * 0.3f;
                    }
                }
                
            }
        }
        if(direction != Vector2.Zero)
        {
            direction.Normalize();

            Vector2 finalDirection = (direction * 2.0f) + separationForce;
            if (finalDirection != Vector2.Zero)
            {
                finalDirection.Normalize();
            }

            _position += finalDirection * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Animation newAnimation = _sprite.Animation;
            SpriteEffects newEffects = SpriteEffects.None;

            if (Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                if (direction.X > 0)
                {
                    newAnimation = _zombieRight;
                }
                else
                {
                    newAnimation = _zombieLeft;
                }
            }
            else
            {
                if (direction.Y > 0)
                {
                    newAnimation = _zombieDown;
                }
                else
                {
                    newAnimation = _zombieUp;
                }
            }
            if (_sprite.Animation != newAnimation)
            {
                _sprite.Animation = newAnimation;
            }
            _sprite.Effects = newEffects;
        }


    }
}