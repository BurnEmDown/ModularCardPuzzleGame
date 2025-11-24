using System.Collections.Generic;
using System.Linq;
using static Gameplay.Engine.Board.Structs;

namespace Gameplay.Engine.Abilities
{
    public class AbilityOption
    {
        private readonly IEnumerable<CellPos> targetCells;

        // Could be a target cell, or multiple cells, or a pattern key.
        public IReadOnlyList<CellPos> TargetCells { get; }

        // Optional: extra payload (direction, mode, etc.)
        // public string ModeKey { get; }

        public AbilityOption(IEnumerable<CellPos> targetCells)
        {
            this.targetCells = targetCells;
            TargetCells = targetCells.ToList();
        }
    }
}