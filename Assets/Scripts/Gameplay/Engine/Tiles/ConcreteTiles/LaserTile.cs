using System.Collections.Generic;
using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public sealed class LaserTile : ModuleTile
    {
        public LaserTile(int id, string typeKey) : base(id, typeKey,
            new DefaultMovementBehavior(new MovementRules(10, true, true)),
            new DefaultAbilityBehavior())
        {
            
        }
    }
}