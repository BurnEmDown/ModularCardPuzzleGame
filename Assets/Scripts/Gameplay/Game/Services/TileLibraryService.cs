using Gameplay.Game.Definitions;
using Gameplay.Game.Services.Interfaces;

namespace Gameplay.Game.Services
{
    /// <summary>
    /// Concrete implementation of <see cref="ITileLibrary"/> that provides
    /// runtime access to tile definitions stored in a <see cref="TileLibrarySO"/>.
    ///
    /// <para>
    /// This service acts as the bridge between Unity-side data (ScriptableObject
    /// definitions) and gameplay systems that require access to tile types. By
    /// depending on the <see cref="ITileLibrary"/> interface rather than the
    /// ScriptableObject itself, game code remains decoupled from Unity APIs and
    /// is easier to test, mock, or refactor.
    /// </para>
    ///
    /// <para>
    /// The service performs simple lookups such as retrieving all tile
    /// definitions or searching for a definition by its <c>typeKey</c>. More
    /// advanced behaviors (such as filtering by rule set, generating tiles, or
    /// integrating with Addressables) can be added in future implementations
    /// without affecting consumer code.
    /// </para>
    /// </summary>
    public sealed class TileLibraryService : ITileLibrary
    {
        /// <summary>
        /// The underlying ScriptableObject containing the tile definition
        /// data used by this service.
        ///
        /// <para>
        /// This object should be assigned at application startup (for example,
        /// through a bootstrap component) and is treated as immutable at runtime.
        /// </para>
        /// </summary>
        private readonly TileLibrarySO data;
        
        /// <summary>
        /// Creates a new tile library service that reads from the specified
        /// <see cref="TileLibrarySO"/>.
        ///
        /// <para>
        /// This constructor does not clone or modify the data; it simply
        /// provides a safe access layer over the ScriptableObject fields.
        /// </para>
        /// </summary>
        /// <param name="data">
        /// The ScriptableObject containing the collection of tile definitions.
        /// Must not be <c>null</c>.
        /// </param>
        public TileLibraryService(TileLibrarySO data)
        {
            this.data = data;
        }
        
        /// <summary>
        /// Returns all tile definitions currently stored in the library.
        ///
        /// <para>
        /// This array represents the complete set of tile types available to
        /// the game according to the associated <see cref="TileLibrarySO"/>.
        /// No filtering or rule-based logic is applied by this method.
        /// </para>
        /// </summary>
        public TileDefinition[] GetAllTiles() => data.tileDefinitions;

        /// <summary>
        /// Retrieves a specific <see cref="TileDefinition"/> by its unique
        /// <c>typeKey</c> value.
        ///
        /// <para>
        /// This lookup performs a simple linear search through the stored tile
        /// definitions. If no matching definition is found, this method returns
        /// <c>null</c>.
        /// </para>
        ///
        /// <para>
        /// Future implementations may optimize lookups using dictionaries or
        /// other data structures if needed, without requiring changes to
        /// consumer code.
        /// </para>
        /// </summary>
        /// <param name="typeKey">
        /// The unique string identifier associated with a tile type.
        /// </param>
        /// <returns>
        /// The <see cref="TileDefinition"/> with the given <c>typeKey</c>, or
        /// <c>null</c> if no match exists.
        /// </returns>
        public TileDefinition GetTileByTypeKey(string typeKey) =>
            System.Array.Find(data.tileDefinitions, tile => tile.typeKey == typeKey);
    }
}