using DG.Tweening;
using UnityEngine;

namespace Gameplay.Presentation.Effects
{
    /// <summary>
    /// Pulses a sprite renderer's alpha for visual emphasis using DOTween.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Creates a looping fade animation between min and max alpha values using DOTween's
    /// optimized tweening system. The animation automatically starts on enable and stops
    /// on disable, with proper cleanup to prevent memory leaks.
    /// </para>
    /// <para>
    /// <strong>Usage:</strong>
    /// Attach this component to any GameObject with a SpriteRenderer to add a subtle
    /// pulsing effect. Commonly used for:
    /// - Move destination highlights
    /// - Interactive UI elements
    /// - Collectible items
    /// - Selection indicators
    /// </para>
    /// <para>
    /// <strong>Performance:</strong>
    /// Uses DOTween's pooled tweeners which are significantly more efficient than
    /// manual Update() loops. Multiple instances can run simultaneously without
    /// performance impact.
    /// </para>
    /// </remarks>
    public class HighlightPulse : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null!;
        
        [Tooltip("Duration of one complete pulse cycle (fade out + fade in).")]
        [SerializeField] private float pulseDuration = 1f;
        
        [Tooltip("Minimum alpha value (most transparent).")]
        [SerializeField] private float minAlpha = 0.3f;
        
        [Tooltip("Maximum alpha value (most opaque).")]
        [SerializeField] private float maxAlpha = 0.6f;

        private Tweener? pulseTween;

        private void OnEnable()
        {
            if (spriteRenderer == null)
            {
                Debug.LogWarning("[HighlightPulse] SpriteRenderer not assigned!");
                return;
            }

            // Kill existing tween if any
            pulseTween?.Kill();

            // Set initial alpha to minimum
            Color color = spriteRenderer.color;
            color.a = minAlpha;
            spriteRenderer.color = color;

            // Create looping alpha pulse with smooth sine easing
            // Duration is halved because yoyo makes one complete cycle = fade out + fade in
            pulseTween = DOTween.To(
                () => spriteRenderer.color.a,
                alpha =>
                {
                    Color c = spriteRenderer.color;
                    c.a = alpha;
                    spriteRenderer.color = c;
                },
                maxAlpha,
                pulseDuration / 2
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetAutoKill(false);
        }

        private void OnDisable()
        {
            // Clean up tween when disabled
            pulseTween?.Kill();
        }
    }
}
