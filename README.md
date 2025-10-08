# Zimad Test Assignment - 2D Game Prototype

[![Unity Version](https://img.shields.io/badge/Unity-6.0.58-%23000000.svg?logo=unity)](https://unity.com/releases/editor/archive)
[![CodeFactor](https://www.codefactor.io/repository/github/llarean/zimad/badge)](https://www.codefactor.io/repository/github/llarean/zimad)
[![GitHub Last Commit](https://img.shields.io/github/last-commit/LLarean/zimad)](https://github.com/llarean/zimad/graphs/commit-activity)

> **Note:** Graphic assets provided by ZiMAD. Code implementation and optimization by author.

---

## Assignment Requirements

- [x] Responsive UI layout with optimized sprite slicing
- [x] Minimized graphic resources while maintaining visual quality
- [x] Character animator with proper state transitions
- [x] Self-destroying weapon VFX effect
- [x] Minimal draw calls optimization

---

## Demo

### Gameplay Video
![Gameplay Demo](https://github.com/LLarean/zimad/blob/main/Screenshots/Movie_004.gif?raw=true)

### WebGL Build
[Play in Browser](https://llarean.github.io/zimad-webgl-demo/)

---

## Screenshots

<table>
  <tr>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/Screenshot_428x926.jpg?raw=true" alt="Mobile Layout"/>
      <p align="center"><em>Mobile (428×926)</em></p>
    </td>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/Screenshot_3440x1440.jpg?raw=true" alt="Ultrawide Layout"/>
      <p align="center"><em>Ultrawide (3440×1440)</em></p>
    </td>
  </tr>
  <tr>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/Animator.jpg?raw=true" alt="Animator"/>
      <p align="center"><em>Animation State Machine</em></p>
    </td>
    <td width="50%">
      <img src="https://github.com/LLarean/zimad/blob/main/Screenshots/DrawCalls.jpg?raw=true" alt="Optimization"/>
      <p align="center"><em>Frame Debugger</em></p>
    </td>
  </tr>
</table>

---

## Technical Implementation

### 1. UI Optimization

**Sprite Atlas:**
- Base sprites: 16×16 and 32×32 pixels
- 9-Slice scaling for scalable elements
- Crunch compression for size reduction

**Responsive Layout:**
- Anchor-based positioning (no hardcoded values)
- Flexible containers adapt to screen aspect ratio
- Tested: 4:3, 16:9, 18:9, 21:9 aspect ratios

**Result:** ~2.3 KB atlas, 6 draw calls

---

### 2. Gameplay Rendering

**RenderTexture System:**
- Gameplay camera renders to 1024×1024 texture
- RawImage displays texture in UI
- Isolated rendering layers (gameplay/UI)

**Benefits:**
- Clean layer separation
- Easy masking with rounded corners
- Independent resolution control

---

### 3. Character Animation

**States:**
- `Idle` - Default state
- `Run` - Movement (triggered by position change)
- `IdleAttack` - Random attack (2-5 sec intervals)

**Transitions:**
- Fast response: Has Exit Time OFF, Duration 0.25s
- Smooth blending between states
- Animation Events for frame-perfect VFX timing

---

### 4. Weapon VFX

**Particle System:**
- Duration: 0.3s, Self-destroying
- 20 particles in circular burst
- Fade-out animation (alpha 255→0)
- Spawned via Animation Event at impact frame

---

### 5. Render Optimization

#### UI Draw Calls (6 total):
1. UI sprites (atlas batching)
2. Mask stencil write
3. Text rendering
4. RawImage (RenderTexture)
5. Mask stencil clear
6. Border overlay (hides mask artifacts)

**Design Decision:**
The border is rendered as a separate draw call above the masked RawImage to hide mask edge artifacts and ensure clean rounded corners.

#### Gameplay Draw Calls (3 total):
1. Ground/background (Sorting Order 0)
2. Character (Sorting Order 10)
3. Grass/foreground (Sorting Order 20)

**Design Decision:**
Grass renders above character legs for correct depth. Different Sorting Orders prevent batching, but visual quality justifies the trade-off.

**Total: 9 draw calls**

---

### 6. Sprite Atlas Configuration

**Location Atlas:**
- Max Size: 4096
- Compression: Normal Quality, Crunch 50
- Contains: ground, grass, decorations
- Size: ~340 KB

**Character Atlas:**
- Max Size: 256
- Compression: High Quality, Crunch 75
- Contains: all knight sprites
- Size: ~32 KB

**Optimization Results:**
- Before: 15-18 textures, ~10+ MB, 15-30 draw calls
- After: 2 atlases, ~350 KB, 3 draw calls

---

## Project Structure

```
Assets/_ZiMAD/
├── Animations/
│   └── SwordMan.controller
├── Prefabs/
│   ├── Button.prefab
│   └── sword_man Variant.prefab
├── Scripts/
│   ├── EntryPoint.cs
│   ├── GameplayPresenter.cs
│   ├── GameplayView.cs
│   └── IdleAttack.cs
├── Art/
│   ├── Materials/
│   ├── Textures/
│   └── Sprites/
│       ├── Sources/
│       └── Atlases/
│           ├── GameplayView.spriteatlas
│           ├── Location.spriteatlas
│           └── Character.spriteatlas
└── Scenes/
    └── Gameplay.unity
```

---

## Quick Start

```bash
# Clone repository
git clone https://github.com/LLarean/zimad.git

# Open in Unity 6000.0.x

# Play: _ZiMAD/Scenes/Gameplay.unity
```

---


## Author

**LLarean**  
GitHub: [@LLarean](https://github.com/llarean)  
Email: llarean@yandex.com

---

## Credits

- **Assets & Base Scripts**: ZiMAD
- **Implementation & Optimization**: LLarean
