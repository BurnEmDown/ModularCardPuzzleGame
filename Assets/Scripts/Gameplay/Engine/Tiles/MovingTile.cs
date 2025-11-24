using System.Collections.Generic;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles
{
    public abstract class MovingTile : Tile, IMovingTile
    {
        /// <summary>
        /// Base movement rules for this tile (its inherent pattern).
        /// </summary>
        protected abstract MovementRules MovementRules { get; set; }
        
        protected MovingTile(int id, string typeKey) : base(id, typeKey) { }

        public virtual IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context)
        {
            return MovementCalculator.GetMoves(context);
        }
    }
}