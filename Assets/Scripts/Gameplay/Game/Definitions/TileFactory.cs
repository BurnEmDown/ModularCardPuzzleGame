using Gameplay.Engine.Moves;
using Gameplay.Engine.Tiles;

namespace Gameplay.Game.Definitions
{
    /// <summary>
    /// Provides a Unity-layer factory for creating engine-level <see cref="ModuleTile"/>
    /// instances from <see cref="TileDefinition"/> ScriptableObjects.
    ///
    /// <para>
    /// This factory serves as the adapter between Unity configuration assets
    /// (<see cref="TileDefinition"/>) and the engine's pure C# tile model.
    /// It translates ScriptableObject fields into a <see cref="TileConfig"/>,
    /// which is then passed to the engine-level <see cref="EngineTileFactory"/>.
    /// </para>
    ///
    /// <para>
    /// This class must remain in the Unity/game layer because it depends on
    /// ScriptableObjects and other Unity-specific data structures. It should not
    /// be referenced by engine code or engine tests.
    /// </para>
    ///
    /// <para>
    /// As additional tile features are implemented (such as display names,
    /// ability behaviors, or visual representations), they can be added to the
    /// <c>TileDefinition</c> asset and mapped here into <see cref="TileConfig"/>.
    /// The commented-out lines indicate optional fields that will be enabled as
    /// their corresponding systems are introduced.
    /// </para>
    /// </summary>
    public static class TileFactory
    {
        /// <summary>
        /// Creates a fully configured engine-level <see cref="ModuleTile"/> using
        /// the values stored in the supplied <see cref="TileDefinition"/>.
        ///
        /// <para>
        /// This method performs no logic or validation beyond simple translation.
        /// If a <see cref="TileDefinition"/> contains malformed or incomplete data,
        /// the resulting tile will reflect those values. Higher-level systems may
        /// add validation later if needed.
        /// </para>
        ///
        /// <para>
        /// The constructed tile contains:
        /// </para>
        /// <list type="bullet">
        ///     <item><description>
        ///     A type key identifying tile category.
        ///     </description></item>
        ///     <item><description>
        ///     Movement rules derived from the ScriptableObject.
        ///     </description></item>
        ///     <item><description>
        ///     Ability behavior (when implemented).
        ///     </description></item>
        /// </list>
        ///
        /// <para>
        /// After configuration translation, this method delegates final object
        /// creation to the engine-level <see cref="EngineTileFactory"/>.
        /// </para>
        /// </summary>
        /// <param name="def">
        /// The Unity <see cref="TileDefinition"/> asset containing tile
        /// configuration data.
        /// </param>
        /// <returns>
        /// A newly created <see cref="ModuleTile"/> instance configured according
        /// to the provided definition.
        /// </returns>
        public static ModuleTile CreateTile(TileDefinition def)
        {
            var config = new TileConfig
            {
                TypeKey     = def.typeKey,
                // DisplayName = def.displayName,
                MovementRules = new MovementRules(
                    def.maxMoveDistance,
                    def.allowOrthogonal,
                    def.allowDiagonal,
                    def.passRule
                ),
                // AbilityBehavior = CreateAbilityBehavior(def.abilityKind)
            };

            return EngineTileFactory.CreateTile(config);
        }
    }
}