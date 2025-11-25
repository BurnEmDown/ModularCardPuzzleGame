using System.Collections.Generic;

namespace Gameplay.Engine.Moves
{
    /// <summary>
    /// Defines a movement behavior that can be attached to a tile in order to
    /// determine how that tile is able to move on the board.
    ///
    /// <para>
    /// Movement behaviors encapsulate reusable movement logic. A movement behavior
    /// typically holds a set of inherent <see cref="MovementRules"/> describing the
    /// tile’s default movement capabilities and uses these rules to compute legal
    /// movement destinations via <see cref="GetAvailableMoves"/>.
    /// </para>
    ///
    /// <para>
    /// Different tiles may compose different movement behaviors to achieve unique
    /// movement patterns. For example:
    /// <list type="bullet">
    ///     <item><description>A default straight-line behavior based on directional rays.</description></item>
    ///     <item><description>A jump behavior that ignores adjacency rules.</description></item>
    ///     <item><description>A behavior that teleports to specific positions.</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// This interface does not apply the movement itself; it only computes which
    /// destinations are legal. Movement is executed by game controller or board logic.
    /// </para>
    /// </summary>
    public interface IMovementBehavior
    {
        /// <summary>
        /// The base movement rules associated with this behavior.
        ///
        /// <para>
        /// These rules define the inherent movement capabilities such as step count,
        /// allowed directions, and obstacle interaction. They may be combined with
        /// additional movement constraints provided in a <see cref="MoveContext"/>
        /// (for example, card effects or temporary state modifications).
        /// </para>
        /// </summary>
        MovementRules MovementRules { get; set; }
        
        /// <summary>
        /// Computes all legal movement destinations for a tile using this behavior,
        /// given the current <paramref name="context"/> which contains board state,
        /// the tile's current position, and externally supplied movement rules.
        ///
        /// <para>
        /// Implementations decide how to combine <see cref="MovementRules"/> with the
        /// contextual rules from <paramref name="context"/>. A typical implementation
        /// merges the two rule sets and uses a helper such as
        /// <see cref="MovementCalculator"/> to perform ray-based movement checks.
        /// </para>
        ///
        /// <para>
        /// The board is treated as read-only. Calling this method must not mutate
        /// board state or tile positions.
        /// </para>
        /// </summary>
        /// <param name="context">
        /// Provides the board snapshot, tile position, and additional movement rules
        /// that apply to this movement action.
        /// </param>
        ///
        /// <returns>
        /// A read-only list of <see cref="MoveOption"/> objects representing all legal
        /// movement destinations the tile may move to.
        /// </returns>
        IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context);
    }
}