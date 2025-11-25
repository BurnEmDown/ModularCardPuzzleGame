using System.Collections.Generic;

namespace Gameplay.Engine.Moves
{
    public interface IMovementBehavior
    {
        MovementRules MovementRules { get; set; }
        
        IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context);
    }
}