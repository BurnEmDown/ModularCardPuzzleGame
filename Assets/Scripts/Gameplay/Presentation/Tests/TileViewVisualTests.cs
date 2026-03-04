using NUnit.Framework;
using Gameplay.Presentation.Tiles;
using Gameplay.Engine.Tiles;
using Gameplay.Engine.Moves;
using Gameplay.Engine.Abilities;
using UnityEngine;
using static Gameplay.Engine.Board.Structs;

namespace Gameplay.Presentation.Tests
{
    /// <summary>
    /// Tests for TileView's visual feedback features (selection indicators).
    /// </summary>
    [TestFixture]
    public class TileViewVisualTests
    {
        private GameObject tileViewGameObject;
        private TileView tileView;
        private GameObject selectionIndicator;

        [SetUp]
        public void Setup()
        {
            // Create a test tile
            var tile = new ModuleTile(1, "TestTile",
                new DefaultMovementBehavior(new MovementRules(1, true, false)),
                new DefaultAbilityBehavior());

            // Create TileView GameObject
            tileViewGameObject = new GameObject("TileView");
            tileView = tileViewGameObject.AddComponent<TileView>();
            tileViewGameObject.AddComponent<SpriteRenderer>();

            // Create selection indicator child
            selectionIndicator = new GameObject("SelectionIndicator");
            selectionIndicator.transform.SetParent(tileViewGameObject.transform);
            selectionIndicator.AddComponent<SpriteRenderer>();
            selectionIndicator.SetActive(false); // Start inactive

            // Initialize TileView
            tileView.Init(tile, new CellPos { X = 0, Y = 0 });

            // Use reflection to set the private selectionIndicator field
            var field = typeof(TileView).GetField("selectionIndicator", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(tileView, selectionIndicator);
        }

        [TearDown]
        public void Teardown()
        {
            if (tileViewGameObject != null)
                Object.DestroyImmediate(tileViewGameObject);
        }

        [Test]
        public void ShowSelection_ActivatesSelectionIndicator()
        {
            // Arrange
            Assert.IsFalse(selectionIndicator.activeSelf, "Selection indicator should start inactive");

            // Act
            tileView.ShowSelection();

            // Assert
            Assert.IsTrue(selectionIndicator.activeSelf, "Selection indicator should be active after ShowSelection");
        }

        [Test]
        public void HideSelection_DeactivatesSelectionIndicator()
        {
            // Arrange
            selectionIndicator.SetActive(true);
            Assert.IsTrue(selectionIndicator.activeSelf, "Selection indicator should start active");

            // Act
            tileView.HideSelection();

            // Assert
            Assert.IsFalse(selectionIndicator.activeSelf, "Selection indicator should be inactive after HideSelection");
        }

        [Test]
        public void ShowSelection_ThenHideSelection_TogglesIndicatorCorrectly()
        {
            // Arrange
            Assert.IsFalse(selectionIndicator.activeSelf);

            // Act & Assert - Show
            tileView.ShowSelection();
            Assert.IsTrue(selectionIndicator.activeSelf, "Should be active after show");

            // Act & Assert - Hide
            tileView.HideSelection();
            Assert.IsFalse(selectionIndicator.activeSelf, "Should be inactive after hide");
        }

        [Test]
        public void ShowSelection_WithNullIndicator_DoesNotThrowException()
        {
            // Arrange - Create TileView without selection indicator
            var tileViewNoIndicator = new GameObject("TileViewNoIndicator").AddComponent<TileView>();
            tileViewNoIndicator.gameObject.AddComponent<SpriteRenderer>();
            
            var tile = new ModuleTile(2, "TestTile2",
                new DefaultMovementBehavior(new MovementRules(1, true, false)),
                new DefaultAbilityBehavior());
            tileViewNoIndicator.Init(tile, new CellPos { X = 0, Y = 0 });

            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => tileViewNoIndicator.ShowSelection());
            Assert.DoesNotThrow(() => tileViewNoIndicator.HideSelection());

            // Cleanup
            Object.DestroyImmediate(tileViewNoIndicator.gameObject);
        }

        [Test]
        public void MultipleShowSelection_Calls_KeepsIndicatorActive()
        {
            // Arrange
            Assert.IsFalse(selectionIndicator.activeSelf);

            // Act - Call multiple times
            tileView.ShowSelection();
            tileView.ShowSelection();
            tileView.ShowSelection();

            // Assert
            Assert.IsTrue(selectionIndicator.activeSelf, "Should remain active after multiple calls");
        }

        [Test]
        public void MultipleHideSelection_Calls_KeepsIndicatorInactive()
        {
            // Arrange
            selectionIndicator.SetActive(true);

            // Act - Call multiple times
            tileView.HideSelection();
            tileView.HideSelection();
            tileView.HideSelection();

            // Assert
            Assert.IsFalse(selectionIndicator.activeSelf, "Should remain inactive after multiple calls");
        }
    }
}
