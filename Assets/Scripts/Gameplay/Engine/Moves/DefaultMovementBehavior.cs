using System.Collections.Generic;

namespace Gameplay.Engine.Moves
{
    public sealed class DefaultMovementBehavior : IMovementBehavior
    {
        public MovementRules MovementRules { get; set; }
        
        public DefaultMovementBehavior(MovementRules movementRules)
        {
            MovementRules = movementRules;
        }
        
        public IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context)
        {
            return MovementCalculator.GetMoves(context);
        }
    }
}