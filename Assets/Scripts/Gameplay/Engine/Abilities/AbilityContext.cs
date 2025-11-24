using Gameplay.Engine.Board;
using static Gameplay.Engine.Board.Structs;

namespace Gameplay.Engine.Abilities
{
    public class AbilityContext
    {
        public IBoardState Board { get; }
        public CellPos CurrentPosition { get; }

        public AbilityContext(IBoardState board, CellPos currentPosition)
        {
            Board = board;
            CurrentPosition = currentPosition;
        }
    }
}