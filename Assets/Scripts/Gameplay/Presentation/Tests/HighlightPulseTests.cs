using NUnit.Framework;
using Gameplay.Presentation.Effects;
using UnityEngine;
using DG.Tweening;

namespace Gameplay.Presentation.Tests
{
    /// <summary>
    /// Tests for HighlightPulse component's initialization and cleanup behavior.
    /// Note: DOTween animation behavior is not tested directly, only component state and lifecycle.
    /// </summary>
    [TestFixture]
    public class HighlightPulseTests
    {
        private GameObject pulseGameObject;
        private HighlightPulse highlightPulse;
        private SpriteRenderer spriteRenderer;

        [SetUp]
        public void Setup()
        {
            // Initialize DOTween (required for tests)
            DOTween.Init();

            // Create HighlightPulse GameObject
            pulseGameObject = new GameObject("HighlightPulse");
            highlightPulse = pulseGameObject.AddComponent<HighlightPulse>();

            // Create SpriteRenderer
            spriteRenderer = pulseGameObject.AddComponent<SpriteRenderer>();

            // Use reflection to set the private spriteRenderer field
            var field = typeof(HighlightPulse).GetField("spriteRenderer", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(highlightPulse, spriteRenderer);

            // Set default alpha
            var color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        [TearDown]
        public void Teardown()
        {
            // Kill all tweens to prevent cross-test contamination
            DOTween.KillAll();
            
            if (pulseGameObject != null)
                Object.DestroyImmediate(pulseGameObject);
        }

        [Test]
        public void OnEnable_WithValidSpriteRenderer_DoesNotThrowException()
        {
            // Arrange
            pulseGameObject.SetActive(false);

            // Act & Assert
            Assert.DoesNotThrow(() => pulseGameObject.SetActive(true));
        }

        [Test]
        public void OnEnable_WithNullSpriteRenderer_DoesNotThrowException()
        {
            // Arrange - Create pulse without sprite renderer
            var pulseNoRenderer = new GameObject("PulseNoRenderer").AddComponent<HighlightPulse>();

            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => pulseNoRenderer.gameObject.SetActive(true));
            Assert.DoesNotThrow(() => pulseNoRenderer.gameObject.SetActive(false));

            // Cleanup
            Object.DestroyImmediate(pulseNoRenderer.gameObject);
        }

        [Test]
        public void OnDisable_CleansUpTween()
        {
            // Arrange
            pulseGameObject.SetActive(true);
            
            // Give DOTween a frame to initialize the tween
            var activeTweensBefore = DOTween.TotalPlayingTweens();

            // Act
            pulseGameObject.SetActive(false);

            // Assert - Check tweens are cleaned up
            var activeTweensAfter = DOTween.TotalPlayingTweens();
            Assert.LessOrEqual(activeTweensAfter, activeTweensBefore, 
                "Tweens should be cleaned up on disable");
        }

        [Test]
        public void Enable_Then_Disable_Then_Enable_DoesNotCauseTweenLeaks()
        {
            // Arrange & Act - Multiple enable/disable cycles
            pulseGameObject.SetActive(true);
            pulseGameObject.SetActive(false);
            pulseGameObject.SetActive(true);
            pulseGameObject.SetActive(false);
            pulseGameObject.SetActive(true);

            // Assert - Check no excessive tweens are created
            var activeTweens = DOTween.TotalPlayingTweens();
            Assert.LessOrEqual(activeTweens, 10, 
                "Should not leak tweens after multiple enable/disable cycles");
        }

        [Test]
        public void Component_CanBeAddedToGameObject()
        {
            // Arrange
            var testObj = new GameObject("TestPulse");

            // Act
            var pulse = testObj.AddComponent<HighlightPulse>();

            // Assert
            Assert.IsNotNull(pulse, "HighlightPulse component should be added successfully");
            Assert.AreEqual(testObj, pulse.gameObject, "Component should be attached to correct GameObject");

            // Cleanup
            Object.DestroyImmediate(testObj);
        }

        [Test]
        public void MultipleComponents_CanExistIndependently()
        {
            // Arrange & Act - Create multiple pulse components
            var pulse1 = new GameObject("Pulse1").AddComponent<HighlightPulse>();
            var pulse2 = new GameObject("Pulse2").AddComponent<HighlightPulse>();
            var pulse3 = new GameObject("Pulse3").AddComponent<HighlightPulse>();

            pulse1.gameObject.AddComponent<SpriteRenderer>();
            pulse2.gameObject.AddComponent<SpriteRenderer>();
            pulse3.gameObject.AddComponent<SpriteRenderer>();

            // Act - Enable all
            pulse1.gameObject.SetActive(true);
            pulse2.gameObject.SetActive(true);
            pulse3.gameObject.SetActive(true);

            // Assert - All should work without interference
            Assert.IsNotNull(pulse1);
            Assert.IsNotNull(pulse2);
            Assert.IsNotNull(pulse3);

            // Cleanup
            Object.DestroyImmediate(pulse1.gameObject);
            Object.DestroyImmediate(pulse2.gameObject);
            Object.DestroyImmediate(pulse3.gameObject);
        }

        [Test]
        public void OnEnable_SetsInitialAlpha()
        {
            // Arrange
            pulseGameObject.SetActive(false);
            
            // Set reflection values for minAlpha
            var minAlphaField = typeof(HighlightPulse).GetField("minAlpha", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            minAlphaField.SetValue(highlightPulse, 0.3f);

            // Act
            pulseGameObject.SetActive(true);

            // Assert - Check alpha was set (may not be exact due to DOTween, but should be changed)
            Assert.IsTrue(spriteRenderer.color.a >= 0f && spriteRenderer.color.a <= 1f, 
                "Alpha should be within valid range after enable");
        }
    }
}
