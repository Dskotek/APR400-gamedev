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
        return new Skeleton(skeletonSprite, position, 100f);
    }

    public static Enemy CreateZombie(ContentManager Content, Vector2 position)
    {
        TextureAtlas zombieAtlas = TextureAtlas.FromFile(Content, "images/zombies.xml");
        Animation zombieDown = zombieAtlas.GetAnimation("zombie-down");
        Animation zombieRight = zombieAtlas.GetAnimation("zombie-right");
        Animation zombieUp = zombieAtlas.GetAnimation("zombie-up");
        Animation zombieLeft = zombieAtlas.GetAnimation("zombie-left");

        AnimatedSprite sprite = new AnimatedSprite(zombieDown);

        return new Zombie(sprite, position, 50.0f, zombieDown, zombieRight, zombieUp, zombieLeft);
    }
}
