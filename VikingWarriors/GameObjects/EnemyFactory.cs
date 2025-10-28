using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Graphics;

namespace VikingWarriors.GameObjects;

public static class EnemyFactory
{
    public static Enemy CreateSkeleton(ContentManager Content, Vector2 position)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/skeletons.xml");
        AnimatedSprite skeletonSprite = atlas.CreateAnimatedSprite("skeleton-animation");
        return new Skeleton(skeletonSprite, position, 80f);
    }

    public static Enemy CreateZombie(ContentManager Content, Vector2 position, int zombieType = 1)
    {
        string atlasFile = $"images/zombies{zombieType}.xml";
        string animationPrefix = $"zombie{zombieType}";

        TextureAtlas zombieAtlas = TextureAtlas.FromFile(Content, atlasFile);
        Animation zombieDown = zombieAtlas.GetAnimation($"{animationPrefix}-down");
        Animation zombieRight = zombieAtlas.GetAnimation($"{animationPrefix}-right");
        Animation zombieUp = zombieAtlas.GetAnimation($"{animationPrefix}-up");
        Animation zombieLeft = zombieAtlas.GetAnimation($"{animationPrefix}-left");

        AnimatedSprite sprite = new AnimatedSprite(zombieDown);

        return new Zombie(sprite, position, 50.0f, zombieDown, zombieRight, zombieUp, zombieLeft);
    }
    public static Enemy CreateRandomZombie(ContentManager Content, Vector2 position)
    {
        int zombieType = Random.Shared.Next(1, 7);
        return CreateZombie(Content, position, zombieType);
    }
}
