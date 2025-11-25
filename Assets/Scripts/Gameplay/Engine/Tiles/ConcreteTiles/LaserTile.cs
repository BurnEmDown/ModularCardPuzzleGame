using System.Collections.Generic;
using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles.ConcreteTiles
{
    public sealed class LaserTile : Tile, IMovingTile, IAbilityTile
    {
        public bool IsAbilityAvailable { get; }

        private int maxMoveDistance = 10;
        
        private readonly IMovementBehavior movementBehavior;
        private readonly IAbilityBehavior abilityBehavior;
        
        public LaserTile(int id, string typeKey) : base(id, typeKey)
        {
            movementBehavior = new DefaultMovementBehavior(new MovementRules(maxMoveDistance, true, true, ObstaclePassRule.CannotPassThrough));
        }

        public IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context)
        {
            return abilityBehavior.GetAbilityOptions(context);
        }
        
        public IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context)
        {
            return movementBehavior.GetAvailableMoves(context);
        }
    }
}