using UnityEngine;

namespace Gameplay.Game.Definitions
{
    /// <summary>
    /// A ScriptableObject that stores references to all available
    /// <see cref="TileDefinition"/> assets used by the game.
    ///
    /// <para>
    /// This asset acts as a central registry or catalog of tile types.
    /// Systems that need to access tile definitions—such as level
    /// generators, UI previews, or tile factories—can reference this asset
    /// rather than needing to manually locate or load definitions.
    /// </para>
    ///
    /// <para>
    /// Although this object contains the data, gameplay systems should
    /// typically access tile definitions through a dedicated service or
    /// wrapper (such as a <c>TileLibraryService</c>) to avoid coupling the
    /// rest of the codebase directly to ScriptableObjects or Unity APIs.
    /// This keeps the architecture clean and allows the underlying data
    /// source to change later (e.g., loading from Resources or Addressables)
    /// without affecting game logic.
    /// </para>
    /// </summary>
    [CreateAssetMenu(menuName = "Rovelike/Tile Library")]
    public class TileLibrarySO : ScriptableObject
    {
        /// <summary>
        /// The list of tile definitions available in the game. Each entry
        /// represents a unique tile type with its own rules, metadata, and
        /// configuration used to create engine-level tile instances at runtime.
        ///
        /// <para>
        /// This array is typically populated in the Unity Inspector by
        /// dragging <see cref="TileDefinition"/> assets into the list.
        /// </para>
        /// </summary>
        public TileDefinition[] tileDefinitions;
    }
}