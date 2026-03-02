using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Presentation.Effects
{
    /// <summary>
    /// Shows a visual highlight when the mouse hovers over this GameObject.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Automatically detects pointer enter/exit events using Unity's EventSystem and
    /// toggles the assigned hover indicator GameObject. This component is self-contained
    /// and can be attached to any interactive GameObject with a collider.
    /// </para>
    /// <para>
    /// <strong>Usage:</strong>
    /// Attach this component alongside a collider (2D or 3D) to add hover feedback. Commonly used for:
    /// - Interactive tiles
    /// - UI elements
    /// - Selectable game objects
    /// - Buttons and controls
    /// </para>
    /// <para>
    /// <strong>Requirements:</strong>
    /// - EventSystem must be present in the scene
    /// - GameObject must have a Collider or Collider2D
    /// - Hover indicator should be a child GameObject with visual components (SpriteRenderer, Image, etc.)
    /// </para>
    /// <para>
    /// <strong>Architecture:</strong>
    /// This follows the single responsibility principle by separating hover visual effects
    /// from core view logic (e.g., TileView). It can be reused across different object types
    /// without coupling to specific game logic.
    /// </para>
    /// </remarks>
    public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("Child GameObject that displays when the mouse hovers over this object (e.g., subtle highlight or glow).")]
        [SerializeField] private GameObject hoverIndicator = null!;

        private void OnEnable()
        {
            // Ensure hover indicator is hidden when component is enabled
            if (hoverIndicator != null)
                hoverIndicator.SetActive(false);
        }

        /// <summary>
        /// Called when the mouse pointer enters this GameObject's collider area.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowHover();
        }

        /// <summary>
        /// Called when the mouse pointer exits this GameObject's collider area.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            HideHover();
        }

        /// <summary>
        /// Shows the hover visual indicator.
        /// </summary>
        private void ShowHover()
        {
            if (hoverIndicator != null)
                hoverIndicator.SetActive(true);
        }

        /// <summary>
        /// Hides the hover visual indicator.
        /// </summary>
        private void HideHover()
        {
            if (hoverIndicator != null)
                hoverIndicator.SetActive(false);
        }
    }
}
