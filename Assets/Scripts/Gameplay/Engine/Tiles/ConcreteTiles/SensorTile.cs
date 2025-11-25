using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public class SensorTile : ModuleTile
    {
        public SensorTile(int id, string typeKey) : base(id, typeKey,
            new DefaultMovementBehavior(new MovementRules(10, false, true, ObstaclePassRule.CanPassThrough)),
            new DefaultAbilityBehavior())
        {
            
        }
    }
}