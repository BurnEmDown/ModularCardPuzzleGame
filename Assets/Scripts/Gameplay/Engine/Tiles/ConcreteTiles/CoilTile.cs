using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public class CoilTile : ModuleTile
    {
        public CoilTile(int id, string typeKey) : base(id, typeKey,
            new DefaultMovementBehavior(new MovementRules(10, true, true, ObstaclePassRule.MustPassThrough)),
            new DefaultAbilityBehavior())
        {
            
        }
    }
}