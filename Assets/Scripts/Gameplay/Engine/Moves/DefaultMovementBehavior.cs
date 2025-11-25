using System.Collections.Generic;

namespace Gameplay.Engine.Moves
{
    /// <summary>
    /// The default movement behavior used by most tiles that rely on standard
    /// directional, step-based movement rules.
    ///
    /// <para>
    /// This behavior delegates movement calculation to
    /// <see cref="MovementCalculator"/>, using the internally stored
    /// <see cref="MovementRules"/> together with contextual rules provided in the
    /// <see cref="MoveContext"/> when evaluating available moves.
    /// </para>
    ///
    /// <para>
    /// Tiles compose this behavior to gain movement capabilities without needing
    /// to implement movement logic themselves. For example:
    /// <list type="bullet">
    ///     <item><description>A tile may move orthogonally up to three steps.</description></item>
    ///     <item><description>A tile may move diagonally but cannot pass obstacles.</description></item>
    ///     <item><description>A tile may have strict step limits while a card modifies movement on a specific turn.</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// This class is sealed because it is intended to be a simple, reusable default.
    /// More exotic movement behaviors (jumping, teleporting, pattern movement, etc.)
    /// should be implemented by creating additional <see cref="IMovementBehavior"/>
    /// implementations rather than subclassing this one.
    /// </para>
    /// </summary>
    public sealed class DefaultMovementBehavior : IMovementBehavior
    {
        /// <summary>
        /// The inherent movement rules associated with this behavior. These rules
        /// define the tile's base movement pattern (step count, directionality,
        /// obstacle interaction, etc.).
        ///
        /// <para>
        /// Implementations of <see cref="GetAvailableMoves"/> may choose to merge
        /// these rules with any additional movement constraints found in the
        /// <see cref="MoveContext"/>. This implementation passes responsibility to
        /// <see cref="MovementCalculator"/>.
        /// </para>
        /// </summary>
        public MovementRules MovementRules { get; set; }
        
        /// <summary>
        /// Creates a new default movement behavior using the supplied movement rules.
        /// </summary>
        /// <param name="movementRules">
        /// The base rules that determine how this tile is allowed to move when this
        /// behavior is applied.
        /// </param>
        public DefaultMovementBehavior(MovementRules movementRules)
        {
            MovementRules = movementRules;
        }
        
        /// <summary>
        /// Computes all legal movement options for a tile using this behavior and the
        /// provided <paramref name="context"/>.
        ///
        /// <para>
        /// Delegates the movement resolution to <see cref="MovementCalculator"/>,
        /// which performs directional raycasts and applies both contextual rules and
        /// the behavior's <see cref="MovementRules"/>.
        /// </para>
        ///
        /// <para>
        /// This method does not modify board state or tile positions.
        /// </para>
        /// </summary>
        /// <param name="context">
        /// Contains the board snapshot, the tile's current position, and any
        /// additional movement constraints applying to this specific movement action.
        /// </param>
        ///
        /// <returns>
        /// A list of <see cref="MoveOption"/> objects that represent all valid
        /// destinations available to the tile under this behavior.
        /// </returns>
        public IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context)
        {
            return MovementCalculator.GetMoves(context);
        }
    }
}