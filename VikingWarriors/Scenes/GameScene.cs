using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using VikingWarriors.GameObjects;
using MonoGameLibrary.Scenes;

namespace VikingWarriors.Scenes;

public class GameScene : Scene
{
    private Player _player;
    private Coin _coin;
    private Level _level;
    private Score _score;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;
    private const int COINS_PER_ZOMBIE = 5;
    private const int COINS_PER_SKELETON = 15;

    private List<Enemy> _enemies = new List<Enemy>();

    private SoundEffect _bounceSoundeEffect;
    private SoundEffect _coinCollectSoundEffect;
    private Song _themeSong;

    public override void Initialize()
    {
        base.Initialize();

        // Starts to play the background music
        Core.Audio.PlaySong(_themeSong);

    }
    public override void LoadContent()
    {
        // Load level
        _level = new Level();
        _level.LoadContent(Content);

        // Create the texture atlas from the XML configuration file.
        TextureAtlas heroDownAtlas = TextureAtlas.FromFile(Content, "images/player-down.xml");
        TextureAtlas heroLeftAtlas = TextureAtlas.FromFile(Content, "images/player-left.xml");
        TextureAtlas heroUpAtlas = TextureAtlas.FromFile(Content, "images/player-up.xml");
        TextureAtlas heroRightAtlas = TextureAtlas.FromFile(Content, "images/player-right.xml");

        TextureAtlas coinAtlas = TextureAtlas.FromFile(Content, "images/coin.xml");

        // Create player with animations
        Animation heroDown = heroDownAtlas.GetAnimation("hero-down");
        Animation heroLeft = heroLeftAtlas.GetAnimation("hero-left");
        Animation heroUp = heroUpAtlas.GetAnimation("hero-up");
        Animation heroRight = heroRightAtlas.GetAnimation("hero-right");
        
        _player = new Player(heroDown, heroLeft, heroUp, heroRight, Vector2.Zero);

        // Load audio
        _bounceSoundeEffect = Content.Load<SoundEffect>("audio/bounce");
        _coinCollectSoundEffect = Content.Load<SoundEffect>("audio/collect");
        _themeSong = Content.Load<Song>("audio/theme");

        // Create coin
        AnimatedSprite coinSprite = coinAtlas.CreateAnimatedSprite("coin-animation");
        coinSprite.Scale = new Vector2(0.5f, 0.5f);
        _coin = new Coin(coinSprite, new Vector2(400, 400), MOVEMENT_SPEED, _bounceSoundeEffect, _coinCollectSoundEffect);

        // Create initial enemies
        _enemies.Add(EnemyFactory.CreateRandomZombie(Content, new Vector2(300, 300)));

        // Load font and create score
        SpriteFont font = Content.Load<SpriteFont>("fonts/04B_31");
        Vector2 scorePosition = new Vector2(_level.PlayableBounds.Left, _level.TileHeight * 0.5f);
        _score = new Score(font, scorePosition);
    }

    public override void Update(GameTime gameTime)
    {
        // Handle volume controls
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.LeftAlt))
        {
            Core.Audio.SongVolume += 0.1f;
            Core.Audio.SoundEffectVolume += 0.1f;
        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.LeftControl))
        {
            Core.Audio.SongVolume -= 0.1f;
            Core.Audio.SoundEffectVolume -= 0.1f;
        }

        // Update player
        _player.Update(gameTime, _level.PlayableBounds);

        // Update coin
        _coin.Update(gameTime, _level.PlayableBounds);

        // Update enemies
        foreach (var enemy in _enemies)
        {
            enemy.Update(gameTime, _player.Position, _enemies);
        }

        /*if (Core.Input.Keyboard.IsKeyDown(Keys.Z))
        {
            _enemies.Add(EnemyFactory.CreateRandomZombie(Content, new Vector2(300, 300)));
        }  Stress testing  */

        // Check collision between player and coin
        if (_player.Bounds.Intersects(_coin.Bounds))
        {
            HandleCoinCollection();
        }

        base.Update(gameTime);
    }

    private void HandleCoinCollection()
    {
        // Add points and increment coin count
        _score.AddScore(100);
        _score.IncrementCoinsCollected();

        // Spawn zombies every N coins
        if (_score.CoinsCollected % COINS_PER_ZOMBIE == 0)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            _enemies.Add(EnemyFactory.CreateRandomZombie(Content, spawnPosition));
        }
        
        // Spawn skeletons every N coins
        if (_score.CoinsCollected % COINS_PER_SKELETON == 0)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            _enemies.Add(EnemyFactory.CreateSkeleton(Content, spawnPosition));
        }

        // Respawn coin at new random location
        _coin.Respawn(
            Core.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Core.GraphicsDevice.PresentationParameters.BackBufferHeight
        );
    }

    private Vector2 GetRandomSpawnPosition()
    {
        int edge = Random.Shared.Next(4);
        return edge switch
        {
            0 => new Vector2(_level.PlayableBounds.Left, Random.Shared.Next(_level.PlayableBounds.Top, _level.PlayableBounds.Bottom)), // Left edge
            1 => new Vector2(_level.PlayableBounds.Right, Random.Shared.Next(_level.PlayableBounds.Top, _level.PlayableBounds.Bottom)), // Right edge
            2 => new Vector2(Random.Shared.Next(_level.PlayableBounds.Left, _level.PlayableBounds.Right), _level.PlayableBounds.Top), // Top edge
            3 => new Vector2(Random.Shared.Next(_level.PlayableBounds.Left, _level.PlayableBounds.Right), _level.PlayableBounds.Bottom), // Bottom edge
            _ => new Vector2(_level.PlayableBounds.Left, Random.Shared.Next(_level.PlayableBounds.Top, _level.PlayableBounds.Bottom)),
        };
    }
    
    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _level.Draw(Core.SpriteBatch);

        // Draw the player sprite
        _player.Draw(Core.SpriteBatch);

        // Draw the coin sprite
        _coin.Draw(Core.SpriteBatch);

        foreach (var enemy in _enemies)
        {
            enemy.Draw(Core.SpriteBatch);
        }

        // Draw the score
        _score.Draw(Core.SpriteBatch);

       // _skeleton.Draw(SpriteBatch, _skeletonPosition);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

        base.Draw(gameTime);
    }
}
