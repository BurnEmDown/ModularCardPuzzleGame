using System.Collections.Generic;
using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public class LaserTile : Tile, IAbilityTile, IMovingTile
    {
        public bool IsAbilityAvailable { get; }
        
        public LaserTile(int id, string typeKey) : base(id, typeKey)
        {
            
        }
        
        public bool CanUseAbility(AbilityContext context)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}