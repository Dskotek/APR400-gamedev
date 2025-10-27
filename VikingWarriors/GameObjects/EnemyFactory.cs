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
}
