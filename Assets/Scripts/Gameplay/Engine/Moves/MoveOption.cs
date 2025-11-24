using static Gameplay.Engine.Board.Structs;

namespace Gameplay.Engine.Moves
{
    /// <summary>
    /// Represents a single valid movement outcome for a tile.
    /// 
    /// A <see cref="MoveOption"/> is produced by tile movement logic
    /// (see <c>IMovingTile.GetAvailableMoves</c>) and describes a legal destination
    /// the tile may move to under the current <see cref="MoveContext"/> and
    /// <see cref="MovementRules"/>.
    /// 
    /// This class does not apply the movement itself; it simply describes one
    /// allowable result. The board controller or game controller is responsible
    /// for performing the actual relocation of the tile.
    /// 
    /// Future variants of this class may include:
    /// <list type="bullet">
    ///   <item><description>the full movement path used to reach <see cref="Destination"/></description></item>
    ///   <item><description>movement cost or step count</description></item>
    ///   <item><description>special move tags (e.g. jump, teleport)</description></item>
    ///   <item><description>warnings or effects triggered upon arrival</description></item>
    /// </list>
    /// </summary>
    public class MoveOption
    {
        /// <summary>
        /// The grid position the tile may legally move to.
        /// 
        /// Movement logic guarantees that this destination is reachable under the
        /// supplied movement rules. It does not imply that the tile has moved yet—
        /// the caller must explicitly perform the move using board logic.
        /// </summary>
        public CellPos Destination { get; }

        /// <summary>
        /// Creates a new <see cref="MoveOption"/> pointing to the given destination.
        /// </summary>
        /// <param name="destination">A legal tile destination.</param>
        public MoveOption(CellPos destination)
        {
            Destination = destination;
        }
    }
}