#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Engine.Tiles;
using static Gameplay.Engine.Board.Structs;

namespace Gameplay.Engine.Board
{
    /// <summary>
    /// Concrete implementation of <see cref="IBoardState"/> that stores and manages
    /// tile placement inside a 2D grid.
    /// 
    /// This class maintains two synchronized data structures:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             A 2D array (<c>Tile?[,]</c>) representing the grid layout.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             A dictionary mapping each placed <see cref="Tile"/> instance to its
    ///             current <see cref="CellPos"/> on the board.
    ///         </description>
    ///     </item>
    /// </list>
    ///
    /// All public modification methods keep these structures consistent. Direct access
    /// to the underlying array is intentionally restricted to preserve invariants 
    /// (e.g. no overlapping tiles, correct position tracking).
    ///
    /// Higher-level systems—such as movement logic, objective evaluation, and the
    /// game controller—interact with this class through safe query and mutation
    /// operations (e.g. <see cref="TryPlaceTile"/>, <see cref="MoveTile"/>, 
    /// <see cref="SwapTiles"/>).
    /// </summary>
    public class BoardState : IBoardState
    {
        /// <inheritdoc/>
        public int Width { get; }

        /// <inheritdoc/>
        public int Height { get; }

        /// <summary>
        /// Internal 2D array storing tile references at each grid cell.
        /// A value of <c>null</c> represents an empty cell.
        /// </summary>
        private readonly Tile?[,] tiles;

        /// <summary>
        /// Maps each placed tile to its current position on the board.
        /// This dictionary is always kept in sync with <see cref="tiles"/>.
        /// </summary>
        private readonly Dictionary<Tile, CellPos> tilePositions;

        /// <summary>
        /// Creates a new empty board with the specified dimensions.
        /// </summary>
        /// <param name="width">Number of columns in the board.</param>
        /// <param name="height">Number of rows in the board.</param>
        public BoardState(int width, int height)
        {
            Width = width;
            Height = height;
            tiles = new Tile?[width, height];
            tilePositions = new Dictionary<Tile, CellPos>();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="pos"/> is outside the board boundaries.
        /// </exception>
        public Tile? GetTileAt(CellPos pos)
        {
            if (!IsInsideBounds(pos))
                throw new ArgumentOutOfRangeException(nameof(pos),
                    $"Position {pos.X}, {pos.Y} is outside the board bounds.");

            return tiles[pos.X, pos.Y];
        }

        /// <inheritdoc/>
        public IReadOnlyList<CellPos> GetAllTilePositions()
        {
            // Provide a snapshot; external code cannot modify the internal state.
            return tilePositions.Values.ToList();
        }

        /// <inheritdoc/>
        public IReadOnlyList<Tile> GetAllTiles()
        {
            // Provide a snapshot to avoid exposing internal collections.
            return tilePositions.Keys.ToList();
        }

        /// <inheritdoc/>
        public bool TryPlaceTile(CellPos pos, Tile tile)
        {
            if (!IsInsideBounds(pos)) return false;
            if (tiles[pos.X, pos.Y] != null) return false;

            tiles[pos.X, pos.Y] = tile;
            tilePositions[tile] = pos;
            return true;
        }

        /// <summary>
        /// Moves a tile from one position to another on the board.
        /// 
        /// This method does not perform occupancy checks on the target cell.
        /// It is the caller's responsibility to ensure the move is legal.
        /// </summary>
        /// <param name="from">The current tile position.</param>
        /// <param name="to">The destination position.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if either <paramref name="from"/> or <paramref name="to"/> 
        /// lies outside the board boundaries.
        /// </exception>
        public void MoveTile(CellPos from, CellPos to)
        {
            if (!IsInsideBounds(from))
                throw new ArgumentOutOfRangeException(nameof(from));

            if (!IsInsideBounds(to))
                throw new ArgumentOutOfRangeException(nameof(to));

            var tile = tiles[from.X, from.Y];
            tiles[from.X, from.Y] = null;
            tiles[to.X, to.Y] = tile;

            if (tile != null)
                tilePositions[tile] = to;
        }

        /// <summary>
        /// Swaps the tiles located at two board positions.
        ///
        /// If either position is empty, <c>null</c> tiles are swapped accordingly.
        /// Caller is responsible for ensuring the swap is legal under game rules.
        /// </summary>
        /// <param name="posA">First position to swap.</param>
        /// <param name="posB">Second position to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if either position lies outside board bounds.
        /// </exception>
        public void SwapTiles(CellPos posA, CellPos posB)
        {
            if (!IsInsideBounds(posA))
                throw new ArgumentOutOfRangeException(nameof(posA));

            if (!IsInsideBounds(posB))
                throw new ArgumentOutOfRangeException(nameof(posB));

            (tiles[posA.X, posA.Y], tiles[posB.X, posB.Y]) =
                (tiles[posB.X, posB.Y], tiles[posA.X, posA.Y]);

            if (tiles[posA.X, posA.Y] != null)
                tilePositions[tiles[posA.X, posA.Y]!] = posA;

            if (tiles[posB.X, posB.Y] != null)
                tilePositions[tiles[posB.X, posB.Y]!] = posB;
        }

        /// <inheritdoc/>
        public bool IsInsideBounds(CellPos pos)
        {
            return pos.X >= 0 && pos.X < Width &&
                   pos.Y >= 0 && pos.Y < Height;
        }
    }
}