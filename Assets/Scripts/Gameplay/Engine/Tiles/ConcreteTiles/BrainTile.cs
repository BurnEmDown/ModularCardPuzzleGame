using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public class BrainTile : ModuleTile
    {
        public BrainTile(int id, string typeKey) : base(id, typeKey,
            new DefaultMovementBehavior(new MovementRules(10, true, false)),
            new DefaultAbilityBehavior())
        {
            
        }
    }
}