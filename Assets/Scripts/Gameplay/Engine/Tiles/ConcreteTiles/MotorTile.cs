using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public class MotorTile : ModuleTile
    {
        public MotorTile(int id, string typeKey) : base(id, typeKey,
            new DefaultMovementBehavior(new MovementRules(1, true, false, ObstaclePassRule.PushObstacles)),
            new DefaultAbilityBehavior())
        {
            
        }
    }
}