using NUnit.Framework;
using Gameplay.Presentation.Effects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Presentation.Tests
{
    /// <summary>
    /// Tests for HoverHighlight component's pointer interaction behavior.
    /// </summary>
    [TestFixture]
    public class HoverHighlightTests
    {
        private GameObject hoverHighlightGameObject;
        private HoverHighlight hoverHighlight;
        private GameObject hoverIndicator;

        [SetUp]
        public void Setup()
        {
            // Create HoverHighlight GameObject
            hoverHighlightGameObject = new GameObject("HoverHighlight");
            hoverHighlight = hoverHighlightGameObject.AddComponent<HoverHighlight>();

            // Create hover indicator child
            hoverIndicator = new GameObject("HoverIndicator");
            hoverIndicator.transform.SetParent(hoverHighlightGameObject.transform);
            hoverIndicator.AddComponent<SpriteRenderer>();
            hoverIndicator.SetActive(true); // Start active for testing

            // Use reflection to set the private hoverIndicator field
            var field = typeof(HoverHighlight).GetField("hoverIndicator", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(hoverHighlight, hoverIndicator);
        }

        [TearDown]
        public void Teardown()
        {
            if (hoverHighlightGameObject != null)
                Object.DestroyImmediate(hoverHighlightGameObject);
        }

        [Test]
        public void OnEnable_HidesHoverIndicator()
        {
            // Arrange
            hoverIndicator.SetActive(true);
            hoverHighlightGameObject.SetActive(false);

            // Act
            hoverHighlightGameObject.SetActive(true);

            // Assert
            Assert.IsFalse(hoverIndicator.activeSelf, "Hover indicator should be hidden on enable");
        }

        [Test]
        public void OnPointerEnter_ShowsHoverIndicator()
        {
            // Arrange
            hoverIndicator.SetActive(false);
            var eventData = new PointerEventData(EventSystem.current);

            // Act
            hoverHighlight.OnPointerEnter(eventData);

            // Assert
            Assert.IsTrue(hoverIndicator.activeSelf, "Hover indicator should be visible after pointer enter");
        }

        [Test]
        public void OnPointerExit_HidesHoverIndicator()
        {
            // Arrange
            hoverIndicator.SetActive(true);
            var eventData = new PointerEventData(EventSystem.current);

            // Act
            hoverHighlight.OnPointerExit(eventData);

            // Assert
            Assert.IsFalse(hoverIndicator.activeSelf, "Hover indicator should be hidden after pointer exit");
        }

        [Test]
        public void OnPointerEnter_ThenOnPointerExit_TogglesIndicatorCorrectly()
        {
            // Arrange
            hoverIndicator.SetActive(false);
            var eventData = new PointerEventData(EventSystem.current);

            // Act & Assert - Enter
            hoverHighlight.OnPointerEnter(eventData);
            Assert.IsTrue(hoverIndicator.activeSelf, "Should be visible after enter");

            // Act & Assert - Exit
            hoverHighlight.OnPointerExit(eventData);
            Assert.IsFalse(hoverIndicator.activeSelf, "Should be hidden after exit");
        }

        [Test]
        public void OnPointerEnter_WithNullIndicator_DoesNotThrowException()
        {
            // Arrange - Create HoverHighlight without indicator
            var hoverNoIndicator = new GameObject("HoverNoIndicator").AddComponent<HoverHighlight>();
            var eventData = new PointerEventData(EventSystem.current);

            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => hoverNoIndicator.OnPointerEnter(eventData));
            Assert.DoesNotThrow(() => hoverNoIndicator.OnPointerExit(eventData));

            // Cleanup
            Object.DestroyImmediate(hoverNoIndicator.gameObject);
        }

        [Test]
        public void MultipleOnPointerEnter_Calls_KeepsIndicatorVisible()
        {
            // Arrange
            hoverIndicator.SetActive(false);
            var eventData = new PointerEventData(EventSystem.current);

            // Act - Call multiple times
            hoverHighlight.OnPointerEnter(eventData);
            hoverHighlight.OnPointerEnter(eventData);
            hoverHighlight.OnPointerEnter(eventData);

            // Assert
            Assert.IsTrue(hoverIndicator.activeSelf, "Should remain visible after multiple enter calls");
        }

        [Test]
        public void MultipleOnPointerExit_Calls_KeepsIndicatorHidden()
        {
            // Arrange
            hoverIndicator.SetActive(true);
            var eventData = new PointerEventData(EventSystem.current);

            // Act - Call multiple times
            hoverHighlight.OnPointerExit(eventData);
            hoverHighlight.OnPointerExit(eventData);
            hoverHighlight.OnPointerExit(eventData);

            // Assert
            Assert.IsFalse(hoverIndicator.activeSelf, "Should remain hidden after multiple exit calls");
        }

        [Test]
        public void Disable_Then_Enable_ResetsIndicatorToHidden()
        {
            // Arrange
            hoverIndicator.SetActive(true);

            // Act
            hoverHighlightGameObject.SetActive(false);
            hoverHighlightGameObject.SetActive(true);

            // Assert
            Assert.IsFalse(hoverIndicator.activeSelf, "Indicator should be hidden after re-enable");
        }
    }
}
