# Zimad Test Assignment - 2D Game Prototype

[![Unity Version](https://img.shields.io/badge/Unity-6.0.58-%23000000.svg?logo=unity)](https://unity.com/releases/editor/archive)
[![CodeFactor](https://www.codefactor.io/repository/github/llarean/zimad/badge)](https://www.codefactor.io/repository/github/llarean/zimad)
[![GitHub Last Commit](https://img.shields.io/github/last-commit/LLarean/zimad)](https://github.com/llarean/zimad/graphs/commit-activity)

> **Note:** Graphic assets provided by ZiMAD. Code implementation and optimization by author.

## Assignment Requirements

- [x] **UI Layout** - Implement responsive UI matching provided mockup with optimized sprite slicing for different screen ratios
- [x] **Asset Optimization** - Minimize graphic resources size while maintaining visual quality
- [x] **Character Animator** - Configure animator with `Run` (Bool) parameter and proper state transitions using PlayerController component
- [x] **Weapon VFX** - Create self-destroying ground impact particle effect
- [x] **Render Optimization** - Achieve minimal possible draw calls

---

## Demo

### Gameplay Video
![Gameplay Demo](https://github.com/LLarean/zimad/blob/main/Screenshots/Movie_004.gif?raw=true)

### WebGL Build
[Play in Browser](#) *(coming soon)*

**System Requirements:**
- Modern browser with WebGL support
- Recommended: Chrome, Firefox, Edge
- Stable internet connection

---

## Screenshots

<table>
  <tr>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/Screenshot_428x926.jpg?raw=true" alt="428x926 Layout"/>
      <p align="center"><em>428x926</em></p>
    </td>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/Screenshot_3440x1440.jpg?raw=true" alt="3440x1440 Layout"/>
      <p align="center"><em>3440x1440</em></p>
    </td>
  </tr>
  <tr>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/Animator.jpg?raw=true" alt="Animator Setup"/>
      <p align="center"><em>Character Animator State Machine</em></p>
    </td>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/FrameDebugger.png?raw=true" alt="Draw Calls"/>
      <p align="center"><em>Frame Debugger: 6 Draw Calls Total</em></p>
    </td>
  </tr>
</table>

---

## Technical Implementation

### 1. UI Optimization & Responsive Layout

**Sprite Optimization:**
- Minimal base sprites: **16x16** and **32x32** pixels
- 9-Slice scaling for borders and panels
- Single **Sprite Atlas** with Crunch compression

**Responsive Canvas:**
```
Canvas Scaler:
├─ UI Scale Mode: Scale With Screen Size
├─ Reference Resolution: 1920x1080
└─ Match: 0.5 (balanced width/height)
```

**Layout Strategy:**
- Anchor-based positioning (no hardcoded coordinates)
- Flexible containers adapt to screen aspect ratio
- Tested on 16:9, 18:9, 19.5:9, 4:3 ratios

**Result:** UI scales correctly from iPhone SE to iPad Pro without breaking layout.

---

### 2. Gameplay Rendering via RenderTexture

**Architecture:**
```
Gameplay Camera → RenderTexture (1024x1024)
                       ↓
                 RawImage (UI)
                       ↓
                  User sees game inside UI frame
```

**Benefits:**
- Gameplay isolated from UI layer (clean culling masks)
- Easy masking with rounded corners
- Independent resolution control

**Camera Settings:**
```
Gameplay Camera:
├─ Target Texture: GameplayWindow
├─ Culling Mask: Everything except UI
└─ Clear Flags: Solid Color

RenderTexture:
├─ Size: 1024x1024
├─ Anti-aliasing: none
└─ Filter Mode: Bilinear
```

---

### 3. Character Animation System

**Animator Controller:**

**States:**
- `Idle` - Standing still (default state)
- `Run` - Movement animation
- `Attack` - Periodic attack during idle

**Parameters:**
- `Run` (Bool) - Movement flag for PlayerController
- `IdleAttack` (Trigger) - Random idle attack

**Transitions:**
```
Idle ⟷ Run:
├─ Condition: _lastPosition != transform.position
├─ Has Exit Time: OFF (instant response)
├─ Transition Duration: 0.25s
└─ Interruption Source: Current State

Idle → IdleAttack:
├─ Condition: IdleAttack trigger (random 2-5s)
├─ Has Exit Time: OFF
└─ Transition Duration: 0.25s

IdleAttack → Idle:
├─ Has Exit Time: ON (play full animation)
├─ Exit Time: 0.57
└─ Transition Duration: 0.25s
```

**Integration with PlayerController:**
```csharp
_playerController.MoveForward();
_playerController.MoveBack();
_playerController.StopMoving();
_playerController.CreateFX("AttackEffect");
```

---

### 4. Weapon Hit VFX System

**Particle System Configuration:**
```
Main Module:
├─ Duration: 0.3s
├─ Start Lifetime: 0.15-0.25s
├─ Start Speed: 0.7-1
├─ Max Particles: 20
├─ Play On Awake: OFF
└─ Stop Action: Destroy

Emission:
├─ Rate over Time: 20
└─ Rate over Distance: 0

Shape:
├─ Shape: Circle
└─ Radius: 0.2

Size over Lifetime:
└─ Curve: 0.5 → 0.9

Color over Lifetime:
└─ Alpha: 255 → 200 → 0 (fade out)

Rederer:
└─ Material: AttackEffect
```

**Spawning via Animation Event:**
```csharp
// Animation Event at attack frame 15
private void OnAttackHit()
{
    _playerController.CreateFX("AttackEffect");
}
```

**Why Animation Events?**
- Frame-perfect timing (VFX synced with visual impact)
- No manual timing code needed
- Artist can adjust timing in animation editor

---

### 5. Render Optimization

**Draw Calls Breakdown:**

| Element | Draw Calls | Reason |
|---------|-----------|---------|
| UI Sprites (Atlas) | 1 | Batched via Sprite Atlas |
| Mask (Stencil Write) | 1 | Unity Mask component requirement |
| Text (Buttons) | 1 | TextMeshPro |
| RawImage (RenderTexture) | 1 | Dynamic texture (can't batch) |
| Mask (Stencil Clear) | 1 | Unity Mask component requirement |
| Border Overlay | 1 | Visual quality decision* |
| **Total** | **6** | **Optimized** |

**\*Design Decision: Border Overlay (+1 draw call)**

The border is rendered as a separate draw call **above** the masked RawImage to hide mask edge artifacts and ensure clean rounded corners.

**Trade-off Analysis:**
- ✅ Clean visual quality (no aliasing/pixelation)
- ✅ Professional appearance
- ❌ +1 draw call

**Alternative considered:** Merging border into atlas would save the call, but border must render *after* mask operations to properly hide artifacts. The visual improvement justifies the additional draw call.

**Mask Component (2 draw calls unavoidable):**

Unity's Mask component uses **stencil buffer** operations:
1. Stencil Write - defines mask area
2. Stencil Clear - cleanup after masked content

This is a **hardware-level GPU operation** and cannot be batched. Alternative solutions (RectMask2D, custom shader) don't support rounded corners with the same visual quality.

---

### 6. Code Architecture Decisions

#### Why `Update()` for IdleAttack instead of Coroutine?

**Current Implementation:**
```csharp
private void Update()
{
    if (Time.time >= _nextIdleAttackTime && IsIdle())
    {
        _animator.SetTrigger("IdleAttack");
        ScheduleNextIdleAttack();
    }
}
```

**Rationale:**
- ✅ **Performance**: Simple comparison (~0.001ms/frame) vs coroutine overhead
- ✅ **Zero GC allocation**: Update doesn't create objects
- ✅ **Precise timing**: Checked every frame (frame-accurate)

**Coroutine Alternative:**
```csharp
// Would require:
StartCoroutine(IdleAttackRoutine()); // IEnumerator allocation
yield return new WaitForSeconds(X);  // Object allocation
```

**Conclusion:** Update is optimal for lightweight frame-by-frame checks. Coroutines are better for complex sequences with multiple delays.

---

## Project Structure

```
Assets/
├── Animation/
├── Resources/
├── Scripts/
├── Scenes/
├── Sprites/
├── Shaders/
└── _ZiMAD/
    ├── Animations/
    │   └── SwordMan.controller
    │
    ├── Prefabs/
    │   ├── Button.prefab
    │   └── sword_man Variant.prefab
    │
    ├── Scripts/
    │   ├── EntryPoint.cs
    │   ├── GameplayPresenter.cs
    │   ├── GameplayView.cs
    │   └── IdleAttack.cs
    │
    ├── Art/
    │   ├── Materials/
    │   ├── Textures/
    │   ├── Sprites/
    │   │   ├── Sources/      # Original sprites (16x16, 32x32)
    │   │   └── Atlases/
    │   │       └── GameplayView.spriteatlas
    │
    └── Scenes/
       └── Gameplay.unity

```

---

## Quick Start

```bash
# 1. Clone repository
git clone https://github.com/LLarean/zimad.git

# 2. Open in Unity 6000.0.x

# 3. Open scene: _ZiMAD/Scenes/Gameplay.unity

# 4. Press Play
```

---

## Performance Results (UI)

| Metric        | Target | Achieved    |
|-------------------|--------|-------------|
| Draw Calls    | < 15   | **6**       |
| Batches       | < 20   | **6**       |
| Sprite Atlas Size | - | **2.2 KB**  |

**Test Environment:** Unity 6000.0.58, Built-in Pipeline, 1920x1080

---

## Technologies

- **Unity** 6000.0.58 LTS
- **C#** .NET Standard 2.1
- **Built-in Render Pipeline**
- **Sprite Atlas** - Crunch compression
- **Animation Events** - VFX timing
- **RenderTexture** - Camera-to-UI system

---

## Author

**LLarean**  
GitHub: [@LLarean](https://github.com/llarean)  
Email: llarean@yandex.com

---

## Credits

- **Provided by ZiMAD**: All graphic assets, core scripts (PlayerController, shaders)
- **Implementation & Optimization**: LLarean (UI system, animation controller, VFX, performance optimization)
- **Technical Test**: ZimAD
