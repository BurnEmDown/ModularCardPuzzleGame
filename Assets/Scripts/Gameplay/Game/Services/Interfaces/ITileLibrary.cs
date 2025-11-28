using Gameplay.Game.Definitions;

namespace Gameplay.Game.Services.Interfaces
{
    /// <summary>
    /// Provides a unified, runtime-accessible interface for retrieving
    /// <see cref="TileDefinition"/> objects used throughout the game.
    ///
    /// <para>
    /// This abstraction decouples gameplay systems from the underlying data
    /// source that stores tile definitions. Implementations may draw from a
    /// <see cref="TileLibrarySO"/>, load assets dynamically, utilize
    /// Addressables, or even generate definitions procedurally—without
    /// requiring changes in the systems that consume tile data.
    /// </para>
    ///
    /// <para>
    /// Game code should depend on this interface rather than directly
    /// referencing ScriptableObjects or Unity-specific APIs to ensure
    /// testability, maintainability, and flexibility in how definitions
    /// are stored and accessed.
    /// </para>
    /// </summary>
    public interface ITileLibrary
    {
        /// <summary>
        /// Retrieves all available <see cref="TileDefinition"/> objects stored
        /// in the library.
        ///
        /// <para>
        /// The returned array represents the complete set of tile types that the
        /// game can instantiate or reference at runtime. This may include tiles
        /// available in the current rule set, tiles unlockable across levels, or
        /// the full global catalog depending on the implementation.
        /// </para>
        /// </summary>
        /// <returns>
        /// An array containing all tile definitions known to the library.
        /// </returns>
        TileDefinition[] GetAllTiles();
        
        /// <summary>
        /// Retrieves a <see cref="TileDefinition"/> based on its unique
        /// <c>typeKey</c> value.
        ///
        /// <para>
        /// The <c>typeKey</c> serves as an identifier assigned within each
        /// <see cref="TileDefinition"/> asset. It allows gameplay systems to look
        /// up tile definitions without needing direct references to assets.
        /// </para>
        ///
        /// <para>
        /// If no matching tile is found, implementations may return
        /// <c>null</c> or throw an exception depending on how strictly the
        /// game enforces data integrity.
        /// </para>
        /// </summary>
        /// <param name="typeKey">
        /// The type identifier associated with a tile definition.
        /// </param>
        /// <returns>
        /// The tile definition associated with the given <c>typeKey</c>, if one
        /// exists.
        /// </returns>
        TileDefinition GetTileByTypeKey(string typeKey);
    }
}
