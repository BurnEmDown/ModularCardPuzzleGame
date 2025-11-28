using Gameplay.Game.Definitions;
using Gameplay.Game.Services.Interfaces;

namespace Gameplay.Game.Services
{
    /// <summary>
    /// Provides centralized access to core runtime services used by the game.
    ///
    /// <para>
    /// <see cref="GameServices"/> acts as a simple service locator for
    /// high-level systems such as the tile library, allowing gameplay code
    /// to access these systems without needing to manage explicit references.
    /// </para>
    ///
    /// <para>
    /// This class is part of the Unity/game layer and should be initialized
    /// during application startup (for example, in a dedicated bootstrap
    /// MonoBehaviour). The engine layer should never depend on or reference
    /// this class.
    /// </para>
    ///
    /// <para>
    /// The current implementation exposes only a single service
    /// (<see cref="TileLibrary"/>), but it can be expanded as needed to
    /// include additional global services such as audio, localization,
    /// settings, or save/load systems.
    /// </para>
    /// </summary>
    public static class GameServices
    {
        /// <summary>
        /// Provides access to the game's <see cref="ITileLibrary"/>, which
        /// supplies tile definitions to runtime systems such as level
        /// generation, tile rendering, and gameplay logic.
        ///
        /// <para>
        /// This property is initialized by <see cref="Init"/> and should not
        /// be set elsewhere. Consumers should treat it as read-only.
        /// </para>
        /// </summary>
        public static ITileLibrary TileLibrary { get; private set; }

        /// <summary>
        /// Initializes the set of global game services.
        ///
        /// <para>
        /// This method must be called during game startup before any code
        /// attempts to access <see cref="TileLibrary"/>. It constructs a
        /// <see cref="TileLibraryService"/> using the supplied
        /// <see cref="TileLibrarySO"/> and assigns it to the globally
        /// accessible property.
        /// </para>
        ///
        /// <para>
        /// This method may be expanded in the future to initialize additional
        /// services as the project grows.
        /// </para>
        /// </summary>
        /// <param name="librarySO">
        /// The ScriptableObject containing tile definition data. Must not
        /// be <c>null</c>.
        /// </param>
        public static void Init(TileLibrarySO librarySO)
        {
            TileLibrary = new TileLibraryService(librarySO);
        }
    }
}