using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

    // Defines the bat animated sprite.
    private AnimatedSprite _bat;

    // Tracks the position of the slime.
    private Vector2 _heroPosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    // Tracks the position of the bat.
    private Vector2 _batPosition;

    // Tracks the velocity of the bat.
    private Vector2 _batVelocity;

    private TileMap _tileMap;

    private Texture2D _tileset;

    private List<Rectangle> _tileRects = new();

    private List<Enemy> _enemies = new List<Enemy>();

    public Game1() : base("Dungeon Slime", 1280, 768, false)
    {
        Core.ExitOnEscape = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Set the initial position of the bat to be 10px
        // to the right of the slime.
        _batPosition = new Vector2(_hero.Width + 10, 0);
       // _skeletonPosition = new Vector2(_hero.Width + 200, 100);

        // Assign the initial random velocity to the bat.
        AssignRandomBatVelocity();
    }

    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
       // TextureAtlas skeletonAtlas = TextureAtlas.FromFile(Content, "images/skeletons.xml");
        TextureAtlas heroAtlas = TextureAtlas.FromFile(Content, "images/hero-animations.xml");


        // Create the slime animated sprite from the atlas.
        /*_hero = heroAtlas.CreateAnimatedSprite("hero-animation");
        _hero.Scale = new Vector2(4.0f, 4.0f);*/

        _heroDown = heroAtlas.GetAnimation("hero-down");
        _heroLeft = heroAtlas.GetAnimation("hero-left");
        _heroUp = heroAtlas.GetAnimation("hero-up");
        _hero = new AnimatedSprite(_heroDown); // Starta med nedåt
        _hero.Scale = new Vector2(4.0f, 4.0f);

        // Create the bat animated sprite from the atlas.
        _bat = atlas.CreateAnimatedSprite("bat-animation");
        _bat.Scale = new Vector2(4.0f, 4.0f);


        
        _enemies.Add(EnemyFactory.CreateSkeleton(Content, new Vector2(400, 300)));

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

    }

    protected override void Update(GameTime gameTime)
    {
        // Update the slime animated sprite.

        bool isHeroMoving = false;

        // Update the bat animated sprite.
        _bat.Update(gameTime);

        foreach (var enemy in _enemies)
        {
            enemy.Update(gameTime, _heroPosition);
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

        // Create a bounding rectangle for the screen.
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        // Creating a bounding circle for the slime
        Circle heroBounds = new Circle(
            (int)(_heroPosition.X + (_hero.Width * 0.5f)),
            (int)(_heroPosition.Y + (_hero.Height * 0.5f)),
            (int)(_hero.Width * 0.5f)
        );

        

        // Use distance based checks to determine if the slime is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // move it back inside.
        if (heroBounds.Left < screenBounds.Left)
        {
            _heroPosition.X = screenBounds.Left;
        }
        else if (heroBounds.Right > screenBounds.Right)
        {
            _heroPosition.X = screenBounds.Right - _hero.Width;
        }

        if (heroBounds.Top < screenBounds.Top)
        {
            _heroPosition.Y = screenBounds.Top;
        }
        else if (heroBounds.Bottom > screenBounds.Bottom)
        {
            _heroPosition.Y = screenBounds.Bottom - _hero.Height;
        }


        Vector2 directionToHero = _heroPosition - _batPosition;
        if (directionToHero != Vector2.Zero)
        {
            directionToHero.Normalize();
            _batVelocity = directionToHero * MOVEMENT_SPEED;
        }

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
        
        

        // Calculate the new position of the bat based on the velocity.
        Vector2 newBatPosition = _batPosition + _batVelocity;

        // Create a bounding circle for the bat.
        Circle batBounds = new Circle(
            (int)(newBatPosition.X + (_bat.Width * 0.5f)),
            (int)(newBatPosition.Y + (_bat.Height * 0.5f)),
            (int)(_bat.Width * 0.5f)
        );

        Vector2 normal = Vector2.Zero;

        // Use distance based checks to determine if the bat is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // reflect it about the screen edge normal.
        if (batBounds.Left < screenBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Left;
        }
        else if (batBounds.Right > screenBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Right - _bat.Width;
        }

        if (batBounds.Top < screenBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Top;
        }
        else if (batBounds.Bottom > screenBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Bottom - _bat.Height;
        }

        // If the normal is anything but Vector2.Zero, this means the bat had
        // moved outside the screen edge so we should reflect it about the
        // normal.
        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            _batVelocity = Vector2.Reflect(_batVelocity, normal);
        }

        _batPosition = newBatPosition;

        if (heroBounds.Intersects(batBounds))
        {
            // Divide the width  and height of the screen into equal columns and
            // rows based on the width and height of the bat.
            int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
            int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

            // Choose a random row and column based on the total number of each
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);

            // Change the bat position by setting the x and y values equal to
            // the column and row multiplied by the width and height.
            _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

            // Assign a new random velocity to the bat
            AssignRandomBatVelocity();
        }


        base.Update(gameTime);
    }

    private void AssignRandomBatVelocity()
    {
        // Generate a random angle.
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to a direction vector.
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed.
        _batVelocity = direction * MOVEMENT_SPEED;
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

        // Draw the slime sprite.
        _hero.Draw(SpriteBatch, _heroPosition);

        // Draw the bat sprite.
        _bat.Draw(SpriteBatch, _batPosition);

        foreach (var enemy in _enemies)
        {
            enemy.Draw(SpriteBatch);
        }

       // _skeleton.Draw(SpriteBatch, _skeletonPosition);

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}