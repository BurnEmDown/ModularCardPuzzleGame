using System.Collections.Generic;
using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public sealed class LaserTile : MovingTile, IAbilityTile
    {
        public bool IsAbilityAvailable { get; }

        private int maxMoveDistance = 10;
        protected override MovementRules MovementRules { get; set; }
        
        public LaserTile(int id, string typeKey) : base(id, typeKey)
        {
            MovementRules = new MovementRules(maxMoveDistance, true, true, ObstaclePassRule.CannotPassThrough);
        }
        
        public bool CanUseAbility(AbilityContext context)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}