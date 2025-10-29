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
    // Defines the slime animated sprite.
    private AnimatedSprite _hero;

    private Animation _heroDown, _heroLeft, _heroUp, _heroRight;

    private Animation _currentHeroAnimation;

    private AnimatedSprite _coin;

    // Tracks the position of the slime.
    private Vector2 _heroPosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    private Vector2 _coinPosition;

    private Vector2 _coinVelocity;

    private TileMap _tileMap;

    private Texture2D _tileset;

    private Rectangle _roomBounds;

    private SpriteFont _font;

    private int _score;
    private int _coinsCollected;
    private const int COINS_PER_ZOMBIE = 5;

    private const int COINS_PER_SKELETON = 15;

    private Vector2 _scoreTextPosition;

    private Vector2 _scoreTextOrigin;

    private List<Rectangle> _tileRects = new();

    private List<Enemy> _enemies = new List<Enemy>();

    private SoundEffect _bounceSoundeEffect;

    private SoundEffect _coinCollectSoundEffect;

    private Song _themeSong;

    public override void Initialize()
    {
        base.Initialize();

        _coinPosition = new Vector2(_hero.Width + 400, 400);
        // _skeletonPosition = new Vector2(_hero.Width + 200, 100);

        AssignRandomCoinVelocity();

        _scoreTextPosition = new Vector2(_roomBounds.Left, _tileMap.TileHeight * 0.5f);

        float _scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, _scoreTextYOrigin);

        // Starts to play the background music
        Core.Audio.PlaySong(_themeSong);

    }
    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        // TextureAtlas skeletonAtlas = TextureAtlas.FromFile(Content, "images/skeletons.xml");
        TextureAtlas heroDownAtlas = TextureAtlas.FromFile(Content, "images/player-down.xml");
        TextureAtlas heroLeftAtlas = TextureAtlas.FromFile(Content, "images/player-left.xml");
        TextureAtlas heroUpAtlas = TextureAtlas.FromFile(Content, "images/player-up.xml");
        TextureAtlas heroRightAtlas = TextureAtlas.FromFile(Content, "images/player-right.xml");

        TextureAtlas coinAtlas = TextureAtlas.FromFile(Content, "images/coin.xml");


        // Create the slime animated sprite from the atlas.
        /*_hero = heroAtlas.CreateAnimatedSprite("hero-animation");
        _hero.Scale = new Vector2(4.0f, 4.0f);*/

        _heroDown = heroDownAtlas.GetAnimation("hero-down");
        _heroLeft = heroLeftAtlas.GetAnimation("hero-left");
        _heroUp = heroUpAtlas.GetAnimation("hero-up");
        _heroRight = heroRightAtlas.GetAnimation("hero-right");
        _hero = new AnimatedSprite(_heroDown); // Starta med nedåt
        _hero.Scale = new Vector2(2.5f, 2.5f);

        _coin = coinAtlas.CreateAnimatedSprite("coin-animation");
        _coin.Scale = new Vector2(0.5f, 0.5f);



        //_enemies.Add(EnemyFactory.CreateSkeleton(Content, new Vector2(400, 300)));
        _enemies.Add(EnemyFactory.CreateRandomZombie(Content, new Vector2(300, 300)));



        // _skeleton = skeletonAtlas.CreateAnimatedSprite("skeleton-animation");
        // _skeleton.Scale = new Vector2(3.0f, 3.0f);

        _tileRects.Clear();
        _tileRects.Add(new Rectangle(0, 0, 64, 64)); //Topp left
        _tileRects.Add(new Rectangle(64, 0, 64, 64)); //Top mid
        _tileRects.Add(new Rectangle(128, 0, 64, 64));  //Top right
        _tileRects.Add(new Rectangle(0, 64, 64, 64));   //Mid left
        _tileRects.Add(new Rectangle(64, 64, 64, 64));  //Mid mid
        _tileRects.Add(new Rectangle(128, 64, 64, 64)); //Mid right
        _tileRects.Add(new Rectangle(0, 128, 64, 64));  //Bot left
        _tileRects.Add(new Rectangle(64, 128, 64, 64)); //Bot mid
        _tileRects.Add(new Rectangle(128, 128, 64, 64)); //Bot right
        _tileRects.Add(new Rectangle(512, 0, 64, 64)); //Dirt top left
        _tileRects.Add(new Rectangle(576, 0, 64, 64)); //dirt top right
        _tileRects.Add(new Rectangle(512, 64, 64, 64)); //Dirt bot left
        _tileRects.Add(new Rectangle(576, 64, 64, 64)); //Dirt bot right
        int[,] mapData = new int[,]
        {
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
            {3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 4, 5},
            {3, 4, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 11, 12, 4, 5},
            {3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5},
            {6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8}
        };
        int[,] decorationData = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 13, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 13, 0, 0, 0, 14, 0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        };
        _tileRects.Add(new Rectangle(0, 644, 63, 123));  //13 green tree
        _tileRects.Add(new Rectangle(383, 793, 128, 38)); //14 fence

        _tileset = Content.Load<Texture2D>("images/RPGpack_sheet");
        _tileMap = new TileMap(_tileset, 64, 64, mapData, _tileRects, decorationData);

        // Skapa spelplanens gränser baserat på tilemappen
        // mapData.GetLength(1) ger bredden (antal kolumner)
        // mapData.GetLength(0) ger höjden (antal rader)
        _roomBounds = new Rectangle(
            64,  // En tile in från vänster för väggarna
            64, // En tile ner från toppen för väggarna
            (mapData.GetLength(1) - 2) * 64,   // Bredd minus två tiles för väggarna
            (mapData.GetLength(0) - 2) * 64  // Höjd minus två tiles för väggarna
        );

        _bounceSoundeEffect = Content.Load<SoundEffect>("audio/bounce");

        _coinCollectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        _themeSong = Content.Load<Song>("audio/theme");

        _font = Content.Load<SpriteFont>("fonts/04B_30");
    }

    public override void Update(GameTime gameTime)
    {
        // Update the hero animated sprite.

        bool isHeroMoving = false;

        _coin.Update(gameTime);

        foreach (var enemy in _enemies)
        {
            enemy.Update(gameTime, _heroPosition, _enemies);
        }

        // _skeleton.Update(gameTime);

        // Check for keyboard input and handle it.
        isHeroMoving = CheckKeyboardInput();

        if (isHeroMoving)
        {
            _hero.Update(gameTime);
        }
        else
        {
            _hero.CurrentFrame = 0;
        }

        // Creating a bounding circle for the slime
        //Circle heroBounds = new Circle(
        //  (int)(_heroPosition.X + (_hero.Width * 0.5f)),
        //  (int)(_heroPosition.Y + (_hero.Height * 0.5f)),
        //  (int)(_hero.Width * 0.5f)
        // );
        Rectangle heroBounds = new Rectangle(
             (int)_heroPosition.X,
             (int)_heroPosition.Y,
             (int)_hero.Width,
             (int)_hero.Height
        );



        // Kontrollera att hjälten stannar inom spelplanens gränser
        if (heroBounds.Left < _roomBounds.Left)
        {
            _heroPosition.X = _roomBounds.Left;
        }
        else if (heroBounds.Right > _roomBounds.Right)
        {
            _heroPosition.X = _roomBounds.Right - _hero.Width;
        }

        if (heroBounds.Top < _roomBounds.Top)
        {
            _heroPosition.Y = _roomBounds.Top;
        }
        else if (heroBounds.Bottom > _roomBounds.Bottom)
        {
            _heroPosition.Y = _roomBounds.Bottom - _hero.Height;
        }


        /*Vector2 directionToHeroCoin = _heroPosition - _coinPosition;
        if (directionToHeroCoin != Vector2.Zero)
        {
            directionToHeroCoin.Normalize();
            _coinVelocity = directionToHeroCoin * MOVEMENT_SPEED;
        }*/

        //TODO Ändra SLIME
        /* Vector2 directionToSlimeSkeleton = _heroPosition - _skeletonPosition;
         if (directionToSlimeSkeleton != Vector2.Zero)
         {
             directionToSlimeSkeleton.Normalize();
             _skeletonPosition += directionToSlimeSkeleton * MOVEMENT_SPEED * 0.5f;
         }
         if (_heroPosition.X < _skeletonPosition.X)
         {
             _skeleton.Effects = SpriteEffects.FlipHorizontally;
         }
         else
         {
             _skeleton.Effects = SpriteEffects.None;
         }*/




        Vector2 newCoinPosition = _coinPosition + _coinVelocity;

        /*Circle coinBounds = new Circle(
            (int)(newCoinPosition.X + (_coin.Width * 0.5f)),
            (int)(newCoinPosition.Y + (_coin.Height * 0.5f)),
            (int)(_coin.Width * 0.5f)
        );*/
        Rectangle coinBounds = new Rectangle(
            (int)newCoinPosition.X,
            (int)newCoinPosition.Y,
            (int)_coin.Width,
            (int)_coin.Height
        );

        Vector2 normal = Vector2.Zero;

        if (coinBounds.Left < _roomBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newCoinPosition.X = _roomBounds.Left;
        }
        else if (coinBounds.Right > _roomBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newCoinPosition.X = _roomBounds.Right - _coin.Width;
        }

        if (coinBounds.Top < _roomBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newCoinPosition.Y = _roomBounds.Top;
        }
        else if (coinBounds.Bottom > _roomBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newCoinPosition.Y = _roomBounds.Bottom - _coin.Height;
        }




        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            _coinVelocity = Vector2.Reflect(_coinVelocity, normal);
            _bounceSoundeEffect.Play();
        }

        _coinPosition = newCoinPosition;

        if (heroBounds.Intersects(coinBounds))
        {
            // Öka poängen när hjälten kolliderar med myntet
            _score += 100;

            _coinsCollected++;

            if (_coinsCollected % COINS_PER_ZOMBIE == 0)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition();
                _enemies.Add(EnemyFactory.CreateRandomZombie(Content, spawnPosition));
            }
            if (_coinsCollected % COINS_PER_SKELETON == 0)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition();
                _enemies.Add(EnemyFactory.CreateSkeleton(Content, spawnPosition));
            }



            // Divide the width  and height of the screen into equal columns and
            // rows based on the width and height of the coin.
            int totalColumns = Core.GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_coin.Width;
            int totalRows = Core.GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_coin.Height;

            // Choose a random row and column based on the total number of each
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);

            // Change the coin position by setting the x and y values equal to
            // the column and row multiplied by the width and height.
            _coinPosition = new Vector2(column * _coin.Width, row * _coin.Height);

            _coinCollectSoundEffect.Play();

            // Assign a new random velocity to the coin
            AssignRandomCoinVelocity();
        }

        base.Update(gameTime);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        int edge = Random.Shared.Next(4);
        return edge switch
        {
            0 => new Vector2(_roomBounds.Left, Random.Shared.Next(_roomBounds.Top, _roomBounds.Bottom)), // Left edge
            1 => new Vector2(_roomBounds.Right, Random.Shared.Next(_roomBounds.Top, _roomBounds.Bottom)), // Right edge
            2 => new Vector2(Random.Shared.Next(_roomBounds.Left, _roomBounds.Right), _roomBounds.Top), // Top edge
            3 => new Vector2(Random.Shared.Next(_roomBounds.Left, _roomBounds.Right), _roomBounds.Bottom), // Bottom edge
            _ => new Vector2(_roomBounds.Left, Random.Shared.Next(_roomBounds.Top, _roomBounds.Bottom)),
        };
    }

    private void AssignRandomCoinVelocity()
    {
        // Generate a random angle.
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to a direction vector.
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed.
        _coinVelocity = direction * MOVEMENT_SPEED;
    }

    private bool CheckKeyboardInput()
    {
        // If the space key is held down, the movement speed increases by 1.5
        float speed = MOVEMENT_SPEED;
        bool heroMoved = false;
        if (Core.Input.Keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        Animation newAnimation = _hero.Animation;
        SpriteEffects newEffects = _hero.Effects;

        // If the W or Up keys are down, move the slime up on the screen.
        if (Core.Input.Keyboard.IsKeyDown(Keys.W) || Core.Input.Keyboard.IsKeyDown(Keys.Up))
        {
            _heroPosition.Y -= speed;
            newAnimation = _heroUp;
            newEffects = SpriteEffects.None;
            heroMoved = true;
        }

        // if the S or Down keys are down, move the slime down on the screen.
        if (Core.Input.Keyboard.IsKeyDown(Keys.S) || Core.Input.Keyboard.IsKeyDown(Keys.Down))
        {
            _heroPosition.Y += speed;
            newAnimation = _heroDown;
            newEffects = SpriteEffects.None;
            heroMoved = true;
        }

        // If the A or Left keys are down, move the slime left on the screen.
        if (Core.Input.Keyboard.IsKeyDown(Keys.A) || Core.Input.Keyboard.IsKeyDown(Keys.Left))
        {
            _heroPosition.X -= speed;
            newAnimation = _heroLeft;
            newEffects = SpriteEffects.None;
            heroMoved = true;

        }

        // If the D or Right keys are down, move the slime right on the screen.
        if (Core.Input.Keyboard.IsKeyDown(Keys.D) || Core.Input.Keyboard.IsKeyDown(Keys.Right))
        {
            _heroPosition.X += speed;
            newAnimation = _heroRight;
            newEffects = SpriteEffects.None;
            heroMoved = true;

        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.LeftAlt))
    {
        Core.Audio.SongVolume += 0.1f;
        Core.Audio.SoundEffectVolume += 0.1f;
    }

    // If the - button was pressed, decrease the volume.
    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.LeftControl))
    {
        Core.Audio.SongVolume -= 0.1f;
        Core.Audio.SoundEffectVolume -= 0.1f;
    }
        if (_hero.Animation != newAnimation && newAnimation != null)
        {
            _hero.Animation = newAnimation;
            _currentHeroAnimation = newAnimation;
        }
        _hero.Effects = newEffects;
        return heroMoved;
    }
    
    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tileMap.Draw(Core.SpriteBatch);

        // Draw the hero sprite.
        _hero.Draw(Core.SpriteBatch, _heroPosition);

        // Draw the coin sprite.
        _coin.Draw(Core.SpriteBatch, _coinPosition);

        foreach (var enemy in _enemies)
        {
            enemy.Draw(Core.SpriteBatch);
        }

        // Draw the score
        Core.SpriteBatch.DrawString(
            _font,              // spriteFont
            $"Score: {_score}", // text
            _scoreTextPosition, // position
            Color.White,        // color
            0.0f,               // rotation
            _scoreTextOrigin,   // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );

       // _skeleton.Draw(SpriteBatch, _skeletonPosition);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

        base.Draw(gameTime);
    }


}
