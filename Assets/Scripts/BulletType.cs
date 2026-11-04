// The kinds of bullets the BulletFactory knows how to make.
// The pool keeps a separate queue for each one.
public enum BulletType
{
    Player,  // torpedo fired by the player, hits enemies
    Enemy    // shot fired by an enemy sub, hits the player
}
