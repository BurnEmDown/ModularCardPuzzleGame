using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public class GripperTile : ModuleTile
    {
        public GripperTile(int id, string typeKey) : base(id, typeKey,
            new DefaultMovementBehavior(new MovementRules(1, true, true)),
            new DefaultAbilityBehavior())
        {
            
        }
    }
}