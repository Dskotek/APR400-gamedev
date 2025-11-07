using System;
using System.Collections.Generic;
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
    public float Width => _sprite.Width;
    public float Height => _sprite.Height;

    protected Enemy(AnimatedSprite sprite, Vector2 position, float speed)
    {
        _sprite = sprite;
        _position = position;
        _speed = speed;
    }

    public virtual void Update(GameTime gameTime, Vector2 playerPosition, Level level, List<Enemy> otherEnemies = null)
    {
        _sprite.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }
}
