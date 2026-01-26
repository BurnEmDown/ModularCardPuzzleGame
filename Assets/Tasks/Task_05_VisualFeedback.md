# Task 04: Add Visual Feedback

**Estimated Time:** 45-60 minutes  
**Prerequisites:** Task_03c_Documentation.md completed  
**Status:** Not Started

---

## Context

Movement execution works (Task 03), but the visual experience is minimal:
- No visual indication of selected tile
- No tile movement animation (instant teleport)
- Highlights exist but are basic
- No hover feedback

This task adds polish to make the game feel responsive and clear.

**Current State:**
- ✅ Tiles can be selected
- ✅ Moves can be executed
- ✅ Basic green highlights show destinations
- ❌ No selected tile visual
- ❌ No movement animation
- ❌ No hover feedback

**Goal:** Add visual polish for selection, movement, and interaction feedback.

---

## Goals

1. Add visual feedback for selected tiles (outline/glow)
2. Animate tile movement (smooth transition, not teleport)
3. Improve move preview highlights (better visuals)
4. Add hover feedback (optional - nice-to-have)

---

## Implementation Steps

### Step 1: Add Selection Visual to TileView
**File:** `/Assets/Scripts/Gameplay/Presentation/Tiles/TileView.cs`

**Add fields:**
```csharp
[Header("Selection Visual")]
[SerializeField] private GameObject selectionIndicator = null!; // Child object with outline sprite
```

**Add methods:**
```csharp
/// <summary>
/// Shows visual indication that this tile is selected.
/// </summary>
public void ShowSelection()
{
    if (selectionIndicator != null)
        selectionIndicator.SetActive(true);
}

/// <summary>
/// Hides selection visual.
/// </summary>
public void HideSelection()
{
    if (selectionIndicator != null)
        selectionIndicator.SetActive(false);
}
```

**Unity Setup:**
1. Open TileView prefab
2. Create child GameObject: "SelectionIndicator"
3. Add SpriteRenderer with:
   - Sprite: Circle or square outline
   - Color: Yellow/white (R:1, G:1, B:0, A:0.8)
   - Sorting order: +1 (above tile sprite)
4. Set SelectionIndicator inactive by default
5. Assign to `selectionIndicator` field in inspector

### Step 2: Update TileSelectionController to Use Visuals
**File:** `/Assets/Scripts/Gameplay/Game/Controllers/TileSelectionController.cs`

**Update SelectTile method:**
```csharp
private void SelectTile(TileView tileView)
{
    // Deselect previous tile if any
    if (selectedTileView != null)
    {
        selectedTileView.HideSelection(); // ADD THIS
        DeselectTile();
    }

    selectedTileView = tileView;
    selectedTileView.ShowSelection(); // ADD THIS
    
    Debug.Log($"[TileSelectionController] Selected: {tileView.Tile.TypeKey} at ({tileView.BoardPosition.X}, {tileView.BoardPosition.Y})");

    OnTileSelected?.Invoke(tileView);
}
```

**Update DeselectTile method:**
```csharp
private void DeselectTile()
{
    if (selectedTileView == null)
        return;

    selectedTileView.HideSelection(); // ADD THIS
    Debug.Log($"[TileSelectionController] Deselected: {selectedTileView.Tile.TypeKey}");
    selectedTileView = null;

    OnTileDeselected?.Invoke();
}
```

### Step 3: Add Movement Animation with DOTween
**File:** `/Assets/Scripts/Gameplay/Presentation/Board/BoardPresenter.cs`

**Prerequisites:**
- Install DOTween from Unity Asset Store or OpenUPM
- Run `Tools → Demigiant → DOTween Utility Panel → Setup DOTween`
- Add `DOTWEEN_ENABLED` scripting define symbol

**Add field:**
```csharp
[Header("Animation")]
[SerializeField] private float moveDuration = 0.3f;
[SerializeField] private Ease moveEase = Ease.OutCubic;
```

**Replace MoveView method:**
```csharp
/// <summary>
/// Updates a single view mapping and animates the view to the new position using DOTween.
/// </summary>
public void MoveView(CellPos from, CellPos to)
{
    if (!viewsByPos.TryGetValue(from, out var view) || view == null)
        return;

    viewsByPos.Remove(from);
    viewsByPos[to] = view;

    view.SetBoardPosition(to);

    // Animate move using DOTween
    Vector3 targetPosition = BoardToWorld(to);
    view.transform.DOMove(targetPosition, moveDuration)
        .SetEase(moveEase)
        .SetAutoKill(true);
}
```

**Add using statement:**
```csharp
using DG.Tweening;
```

**Why DOTween?**
- ✅ More performant than coroutines (pooled tweeners, optimized updates)
- ✅ Built-in easing functions (Ease.OutCubic, Ease.InOutQuad, etc.)
- ✅ Auto-cleanup (SetAutoKill prevents memory leaks)
- ✅ Easy to chain, sequence, and combine animations
- ✅ Supports pause/resume, speed scaling, and callbacks
- ✅ Industry standard for Unity tweening

### Step 4: Improve Move Preview Highlights
**Update MoveHighlight prefab in Unity:**

1. Open MoveHighlight prefab
2. Update SpriteRenderer:
   - Color: Bright cyan (R:0, G:1, B:1, A:0.4)
   - Add slight scale pulse (optional - see Step 5)
3. Add border/outline:
   - Create child GameObject: "Border"
   - Add SpriteRenderer with white outline sprite
   - Scale slightly larger than fill
   - Color: White (R:1, G:1, B:1, A:0.6)

**Alternative: Use Shader for Glow Effect**
```csharp
// Add to MoveHighlight prefab component
[SerializeField] private Material glowMaterial;
// Assign material with glow shader (Unity default: Sprites/Default + emission)
```

### Step 5: Add Highlight Pulse Animation with DOTween (Optional)
**File:** Create `/Assets/Scripts/Gameplay/Presentation/Effects/HighlightPulse.cs`

```csharp
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Presentation.Effects
{
    /// <summary>
    /// Pulses a sprite renderer's alpha for visual emphasis using DOTween.
    /// </summary>
    public class HighlightPulse : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null!;
        [SerializeField] private float pulseDuration = 1f;
        [SerializeField] private float minAlpha = 0.3f;
        [SerializeField] private float maxAlpha = 0.6f;

        private Tweener pulseTween;

        private void OnEnable()
        {
            if (spriteRenderer == null)
                return;

            // Kill existing tween if any
            pulseTween?.Kill();

            // Create looping alpha pulse
            pulseTween = spriteRenderer.DOFade(maxAlpha, pulseDuration)
                .From(minAlpha)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetAutoKill(false);
        }

        private void OnDisable()
        {
            pulseTween?.Kill();
        }
    }
}
```

**Add to MoveHighlight prefab:**
1. Add HighlightPulse component
2. Assign spriteRenderer reference
3. Adjust pulseDuration/alpha values to taste

**Advantages over Update():**
- ✅ No per-frame math calculations (DOTween handles it)
- ✅ Auto-pauses when object disabled
- ✅ Smooth sine easing built-in
- ✅ Easy to modify timing without code changes

### Step 6: Add Hover Feedback (Optional)
**File:** `/Assets/Scripts/Gameplay/Presentation/Tiles/TileView.cs`

**Add interface:**
```csharp
public class TileView : MonoBehaviour, IUserInteractionTarget, IPointerEnterHandler, IPointerExitHandler
```

**Add fields:**
```csharp
[Header("Hover Visual")]
[SerializeField] private GameObject hoverIndicator = null!;
```

**Add methods:**
```csharp
public void OnPointerEnter(PointerEventData eventData)
{
    if (hoverIndicator != null)
        hoverIndicator.SetActive(true);
}

public void OnPointerExit(PointerEventData eventData)
{
    if (hoverIndicator != null)
        hoverIndicator.SetActive(false);
}
```

**Unity Setup:**
1. Create child GameObject: "HoverIndicator"
2. Add SpriteRenderer with subtle white overlay
3. Color: White (R:1, G:1, B:1, A:0.2)
4. Set inactive by default
5. Assign to `hoverIndicator` field

### Step 7: Add Using Statements
**Required in various files:**
```csharp
using UnityEngine.EventSystems; // For IPointerEnterHandler, IPointerExitHandler
using System.Collections; // For IEnumerator
```

---

## Test Checklist

### Visual Verification
- [ ] Click tile - yellow/white outline appears
- [ ] Click different tile - outline moves to new tile
- [ ] Click same tile to deselect - outline disappears
- [ ] Execute move - tile smoothly animates to destination (not instant)
- [ ] Animation takes ~0.3 seconds (adjust moveDuration if needed)
- [ ] Move highlights have cyan color and are clearly visible
- [ ] (Optional) Highlights pulse gently
- [ ] (Optional) Hovering over tile shows subtle highlight

### Animation Quality
- [ ] Movement animation is smooth (no jitter)
- [ ] Tile reaches exact destination position
- [ ] Multiple moves in sequence work correctly
- [ ] Animation doesn't block other interactions
- [ ] Selection visual stays with tile during movement

### Performance Check
- [ ] No frame drops during move animation
- [ ] Multiple highlights don't cause lag
- [ ] Selection visual toggles instantly (no delay)

### Edge Cases
- [ ] Deselect during animation - no visual artifacts
- [ ] Select new tile during animation - previous animation completes
- [ ] Rapid selection changes - visuals stay consistent

---

## Success Criteria

- ✅ DOTween Tweeners:** Pooled and optimized by DOTween internally
- **Pooled Highlights:** Already pooled via PoolManager (no instantiation cost)
- **Auto-Kill:** Tweens automatically clean up when complete (no memory leaks)
- **Simultaneous Animations:** DOTween efficiently handles multiple concurrent tweens

### Alternative Approaches
**Manual Coroutines (if DOTween not available):**
```csharp
// Fallback without DOTween (less performant, more code)
private IEnumerator AnimateTileMove(TileView view, Vector3 targetPosition)
{
    Vector3 startPosition = view.transform.position;
    float elapsed = 0f;
    while (elapsed < moveDuration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / moveDuration);
        t = 1f - Mathf.Pow(1f - t, 3f); // Ease out cubic
        view.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        yield return null;
    }
    view.transform.position = targetPosition;
}
```

**Scriptable Animation Curves with DOTween:**
```csharp
[SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
view.transform.DOMove(targetPosition, moveDuration).SetEase(moveCurve);sive but visible)
- **Easing Function:** Ease-out cubic feels natural for movement
- **Highlight Pulse:** Subtle pulse (2 Hz) draws attention without distraction

### Performance Considerations
- **Coroutines:** One coroutine per moving tile is fine (<10 simultaneous moves)
- **Pooled Highlights:** Already pooled via PoolManager (no instantiation cost)
- **SpriteRenderer Updates:** Minimal CPU cost for alpha/position updates

### Alternative Approaches
**DOTDOTween not found"** → Install DOTween and run Setup, add DOTWEEN_ENABLED scripting define
- **"Animation is jerky"** → Check moveDuration > 0 and DOTween is properly initialized
- **"Tile snaps to position"** → Verify DOTween is installed and tween is being created
- **"Animation doesn't stop"** → Tweens auto-kill by default; check SetAutoKill(true) is set
- **"Multiple animations conflict"** → DOTween automatically handles this; new tweens override old ones
// If using DOTween library (more powerful animation)
view.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutCubic);
```

**Scriptable Animation Curves:**
```csharp
[SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
// Use moveCurve.Evaluate(t) instead of hardcoded easing
```

### Common Issues
- **"Selection visual doesn't appear"** → Check selectionIndicator is assigned and has active SpriteRenderer
- **"Animation is jerky"** → Ensure moveDuration > 0 and Time.deltaTime is accumulating correctly
- **"Tile snaps to position"** → Check coroutine is actually starting (StartCoroutine called)
- **"Hover doesn't work"** → Verify EventSystem in scene and TileView has Collider2D

### Next Task
After visuals work, proceed to **Task_04b_VisualTestVerification.md** to document visual test cases.

### Related Files
- `/Assets/Scripts/Gameplay/Presentation/Tiles/TileView.cs`
- `/Assets/Scripts/Gameplay/Presentation/Board/BoardPresenter.cs`
- `/Assets/Scripts/Gameplay/Game/Controllers/TileSelectionController.cs`
- `/Assets/Prefabs/TileView.prefab`
- `/Assets/Prefabs/MoveHighlight.prefab`
