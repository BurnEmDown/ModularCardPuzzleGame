using System.Collections.Generic;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles
{
    public interface IMovingTile
    {
        IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context);
    }
}