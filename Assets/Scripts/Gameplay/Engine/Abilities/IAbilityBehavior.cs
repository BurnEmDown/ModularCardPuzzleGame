using System.Collections.Generic;

namespace Gameplay.Engine.Abilities
{
    /// <summary>
    /// Defines an ability behavior that can be attached to a tile to provide
    /// reusable, composable ability logic.
    ///
    /// <para>
    /// Unlike the tile itself, which simply declares that it <em>can</em> use an ability
    /// via <c>IAbilityTile</c>, an <see cref="IAbilityBehavior"/> contains the actual
    /// rules and logic governing when the ability is available and which actions
    /// the ability can perform.
    /// </para>
    ///
    /// <para>
    /// Each tile that supports abilities may compose one or more concrete behaviors
    /// (e.g., <c>LaserAbilityBehavior</c>, <c>GrappleAbilityBehavior</c>, etc.) instead
    /// of inheriting ability functionality through a class hierarchy. This ensures
    /// high flexibility and avoids limitations of single inheritance.
    /// </para>
    ///
    /// <para>
    /// Implementations should not directly modify board state. Instead, they should
    /// describe available ability options, while the tile or game controller handles
    /// executing the selected action.
    /// </para>
    /// </summary>
    public interface IAbilityBehavior
    {
        /// <summary>
        /// Indicates whether the tile's ability is currently available.
        ///
        /// <para>
        /// Availability may depend on factors such as:
        /// <list type="bullet">
        ///     <item><description>Whether the ability has already been used this turn.</description></item>
        ///     <item><description>Board or game state conditions.</description></item>
        ///     <item><description>Cooldowns or resource constraints.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        bool IsAbilityAvailable { get; }

        /// <summary>
        /// Computes the set of possible ability actions that can be taken from the
        /// current <paramref name="context"/>.
        ///
        /// <para>
        /// Implementations interpret <see cref="AbilityContext"/> to determine
        /// available targets, ranges, or effects. This method does not execute the
        /// ability; it only enumerates options.
        /// </para>
        ///
        /// <para>
        /// If no ability options are available (e.g., due to cooldown, invalid state,
        /// or inability to target anything), an empty collection is returned.
        /// </para>
        /// </summary>
        /// <param name="context">
        /// A snapshot of relevant game state for evaluating the ability, such as
        /// board layout, tile positions, and turn information.
        /// </param>
        /// <returns>
        /// A read-only list of <see cref="AbilityOption"/> objects representing all
        /// valid actions the tile may perform with its ability.
        /// </returns>
        IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context);
    }
}