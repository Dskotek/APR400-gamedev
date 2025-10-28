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

namespace VikingWarriors;

public class Game1 : Core
{
    // Defines the slime animated sprite.
    private AnimatedSprite _hero;

    private Animation _heroDown, _heroLeft, _heroUp;

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

    private Vector2 _scoreTextPosition;

    private Vector2 _scoreTextOrigin;

    private List<Rectangle> _tileRects = new();

    private List<Enemy> _enemies = new List<Enemy>();

    private SoundEffect _bounceSoundeEffect;

    private SoundEffect _coinCollectSoundEffect;

    private Song _themeSong;

    public Game1() : base("Viking Warriors", 1280, 768, false)
    {
        Core.ExitOnEscape = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _coinPosition = new Vector2(_hero.Width + 400, 400);
        // _skeletonPosition = new Vector2(_hero.Width + 200, 100);

        AssignRandomCoinVelocity();

        _scoreTextPosition = new Vector2(_roomBounds.Left, _tileMap.TileHeight * 0.5f);

        float _scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, _scoreTextYOrigin);

        // Starts to play the background music
        Audio.PlaySong(_themeSong);
        
    }

    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        // TextureAtlas skeletonAtlas = TextureAtlas.FromFile(Content, "images/skeletons.xml");
        TextureAtlas heroAtlas = TextureAtlas.FromFile(Content, "images/hero-animations.xml");
        
        TextureAtlas coinAtlas = TextureAtlas.FromFile(Content, "images/coin.xml");


        // Create the slime animated sprite from the atlas.
        /*_hero = heroAtlas.CreateAnimatedSprite("hero-animation");
        _hero.Scale = new Vector2(4.0f, 4.0f);*/

        _heroDown = heroAtlas.GetAnimation("hero-down");
        _heroLeft = heroAtlas.GetAnimation("hero-left");
        _heroUp = heroAtlas.GetAnimation("hero-up");
        _hero = new AnimatedSprite(_heroDown); // Starta med nedåt
        _hero.Scale = new Vector2(4.0f, 4.0f);

        _coin = coinAtlas.CreateAnimatedSprite("coin-animation");
        _coin.Scale = new Vector2(0.5f, 0.5f);



        _enemies.Add(EnemyFactory.CreateSkeleton(Content, new Vector2(400, 300)));
        _enemies.Add(EnemyFactory.CreateZombie(Content, new Vector2(300, 300)));
        _enemies.Add(EnemyFactory.CreateZombie(Content, new Vector2(200, 300)));
        _enemies.Add(EnemyFactory.CreateZombie(Content, new Vector2(100, 300)));
        _enemies.Add(EnemyFactory.CreateZombie(Content, new Vector2(400, 300)));


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
        _tileset = Content.Load<Texture2D>("images/RPGpack_sheet");
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
        _tileMap = new TileMap(_tileset, 64, 64, mapData, _tileRects);

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

    protected override void Update(GameTime gameTime)
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

        // Check for gamepad input and handle it.
        CheckGamePadInput();

        if (isHeroMoving)
        {
            _hero.Update(gameTime);
        }
        else
        {
            _hero.CurrentFrame = 0;
        }

        // Creating a bounding circle for the slime
        Circle heroBounds = new Circle(
            (int)(_heroPosition.X + (_hero.Width * 0.5f)),
            (int)(_heroPosition.Y + (_hero.Height * 0.5f)),
            (int)(_hero.Width * 0.5f)
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

        Circle coinBounds = new Circle(
            (int)(newCoinPosition.X + (_coin.Width * 0.5f)),
            (int)(newCoinPosition.Y + (_coin.Height * 0.5f)),
            (int)(_coin.Width * 0.5f)
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

            // Divide the width  and height of the screen into equal columns and
            // rows based on the width and height of the coin.
            int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_coin.Width;
            int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_coin.Height;

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
        if (Input.Keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        Animation newAnimation = _hero.Animation;
        SpriteEffects newEffects = _hero.Effects;

        // If the W or Up keys are down, move the slime up on the screen.
        if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
        {
            _heroPosition.Y -= speed;
            newAnimation = _heroUp;
            newEffects = SpriteEffects.None;
            heroMoved = true;
        }

        // if the S or Down keys are down, move the slime down on the screen.
        if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
        {
            _heroPosition.Y += speed;
            newAnimation = _heroDown;
            newEffects = SpriteEffects.None;
            heroMoved = true;
        }

        // If the A or Left keys are down, move the slime left on the screen.
        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        {
            _heroPosition.X -= speed;
            newAnimation = _heroLeft;
            newEffects = SpriteEffects.FlipHorizontally;
            heroMoved = true;

        }

        // If the D or Right keys are down, move the slime right on the screen.
        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        {
            _heroPosition.X += speed;
            newAnimation = _heroLeft;
            newEffects = SpriteEffects.None;
            heroMoved = true;

        }
        if (_hero.Animation != newAnimation && newAnimation != null)
        {
            _hero.Animation = newAnimation;
            _currentHeroAnimation = newAnimation;
        }
        _hero.Effects = newEffects;
        return heroMoved;
    }

    private void CheckGamePadInput()
    {
        GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];

        // If the A button is held down, the movement speed increases by 1.5
        // and the gamepad vibrates as feedback to the player.
        float speed = MOVEMENT_SPEED;
        if (gamePadOne.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

        // Check thumbstick first since it has priority over which gamepad input
        // is movement.  It has priority since the thumbstick values provide a
        // more granular analog value that can be used for movement.
        if (gamePadOne.LeftThumbStick != Vector2.Zero)
        {
            _heroPosition.X += gamePadOne.LeftThumbStick.X * speed;
            _heroPosition.Y -= gamePadOne.LeftThumbStick.Y * speed;
        }
        else
        {
            // If DPadUp is down, move the slime up on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadUp))
            {
                _heroPosition.Y -= speed;
            }

            // If DPadDown is down, move the slime down on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadDown))
            {
                _heroPosition.Y += speed;
            }

            // If DPapLeft is down, move the slime left on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadLeft))
            {
                _heroPosition.X -= speed;
            }

            // If DPadRight is down, move the slime right on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadRight))
            {
                _heroPosition.X += speed;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tileMap.Draw(SpriteBatch);

        // Draw the hero sprite.
        _hero.Draw(SpriteBatch, _heroPosition);

        // Draw the coin sprite.
        _coin.Draw(SpriteBatch, _coinPosition);

        foreach (var enemy in _enemies)
        {
            enemy.Draw(SpriteBatch);
        }

        // Draw the score
        SpriteBatch.DrawString(
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
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}