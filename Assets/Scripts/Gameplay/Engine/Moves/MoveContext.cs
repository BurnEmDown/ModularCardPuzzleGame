using Gameplay.Engine.Board;
using static Gameplay.Engine.Board.Structs;

namespace Gameplay.Engine.Moves
{
    /// <summary>
    /// Provides all information required for a tile to compute its valid movement options.
    ///
    /// A <see cref="MoveContext"/> bundles together:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             A read-only snapshot of the board (<see cref="IBoardState"/>), allowing
    ///             movement logic to inspect obstacles, tile positions, and boundaries.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             The tile's current grid position (<see cref="CurrentPosition"/>).
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             The movement constraints (<see cref="MovementRules"/>) applying to this
    ///             particular move.
    ///         </description>
    ///     </item>
    /// </list>
    ///
    /// <para>
    /// This context is passed into <c>IMovingTile.GetAvailableMoves</c>, enabling the tile's
    /// movement logic to calculate all valid <see cref="MoveOption"/> destinations. Tiles
    /// must treat the board reference as read-only; board mutation is the responsibility of
    /// the game controller or board state manager.
    /// </para>
    /// </summary>
    public class MoveContext
    {
        /// <summary>
        /// A read-only view of the board used to evaluate movement paths, detect
        /// obstacles, validate boundaries, and inspect other tiles.
        ///
        /// Implementations of <see cref="IBoardState"/> exposed here do not allow
        /// mutation and serve as a stable snapshot for movement evaluation.
        /// </summary>
        public IBoardState Board { get; }

        /// <summary>
        /// The current grid position of the tile attempting to move.
        ///
        /// Movement logic uses this as the origin for computing reachable cells.
        /// </summary>
        public CellPos CurrentPosition { get; }

        /// <summary>
        /// Movement constraints that apply to this move.
        /// These rules are typically determined externally (e.g. by a command card)
        /// and are combined with tile-specific movement logic to determine which
        /// destinations are valid.
        /// </summary>
        public MovementRules Rules { get; }

        /// <summary>
        /// Creates a new context object containing the board snapshot, origin position,
        /// and movement constraints for evaluating tile movement options.
        /// </summary>
        /// <param name="board">The board state used to evaluate movement.</param>
        /// <param name="currentPosition">The tile’s starting position.</param>
        /// <param name="rules">Movement rules applying to this action.</param>
        public MoveContext(IBoardState board, CellPos currentPosition, MovementRules rules)
        {
            Board = board;
            CurrentPosition = currentPosition;
            Rules = rules;
        }
    }
}