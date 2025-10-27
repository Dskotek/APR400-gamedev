using System;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingWarriors.GameObjects;

public abstract class Enemy
{
    protected AnimatedSprite _sprite;
    protected Vector2 _position;
    protected float _speed;

    public Vector2 Position => _position;

    protected Enemy(AnimatedSprite sprite, Vector2 position, float speed)
    {
        _sprite = sprite;
        _position = position;
        _speed = speed;
    }

    public virtual void Update(GameTime gameTime, Vector2 playerPosition)
    {
        _sprite.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }
}
