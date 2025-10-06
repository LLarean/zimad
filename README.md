# Zimad Test Assignment - 2D Prototype

[![Unity Version](https://img.shields.io/badge/Unity-6.0.58-%23000000.svg?logo=unity)](https://unity.com/releases/editor/archive)
[![CodeFactor](https://www.codefactor.io/repository/github/llarean/zimad/badge)](https://www.codefactor.io/repository/github/llarean/zimad)
[![GitHub Last Commit](https://img.shields.io/github/last-commit/LLarean/zimad)](https://github.com/llarean/zimad/graphs/commit-activity)

## Task Overview

This project is a technical implementation of a 2D game prototype with optimized UI, character animation system, and VFX effects.

### Requirements Implemented:
- [x] Responsive UI layout with optimized sprite atlasing
- [x] Minimized graphic resources without visual quality loss
- [x] Character animator with state machine and transitions
- [x] Self-destroying weapon hit VFX system
- [x] Render optimization with minimal draw calls

---

## Demo

### Video Demonstration
![Watch Gameplay Demo](https://github.com/LLarean/zimad/blob/main/Screenshots/Movie_004.gif?raw=true)

### WebGL Build
[Play in Browser](#) *(link to WebGL build if available)*

---

## Screenshots

### UI Implementation
![GameWindow](https://github.com/LLarean/zimad/blob/main/Screenshots/Screen_1.jpg?raw=true)  
![GameWindow](https://github.com/LLarean/zimad/blob/main/Screenshots/Screen_2.jpg?raw=true)  
*Responsive UI adapting to different screen sizes*

### Animation
![Animator](https://github.com/LLarean/zimad/blob/main/Screenshots/Animator.jpg?raw=true)
*Character animation*

### Optimization Results
![FrameDebugger](https://github.com/LLarean/zimad/blob/main/Screenshots/FrameDebugger.jpg?raw=true)
*Draw calls optimization showcase*

---

## Technical Implementation

### 1. UI Optimization

**Approach:**
- Created minimal base sprites (16x16, 32x32 pixels)
- Utilized 9-Slice technique for scalable UI elements
- Consolidated all UI sprites into a single Sprite Atlas with compression

**Canvas Setup:**
```
Canvas Scaler:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920x1080
- Screen Match Mode: Match Width Or Height (.5f)
```

**Result:** Single atlas texture, reduced memory footprint, adaptive layout for various screen ratios.

---

### 2. Gameplay Rendering

**Camera-to-UI System:**
- Dedicated camera renders gameplay to RenderTexture
- RawImage component displays the texture in UI
- Isolates gameplay rendering from UI layer

**Benefits:**
- Clean separation of concerns
- Easy to mask/frame gameplay area
- Better performance control

**Settings:**
```
Gameplay Camera:
- Target Texture: GameplayRT (1024x1024)
- Culling Mask: Not UI
- Clear Flags: Solid Color

Canvas:
- Render Mode: Screen Space - Overlay
```

---

### 3. Character Animation System

**Animator Setup:**

States:
- `Idle` - Default state
- `Run` - Movement animation
- `IdleAttack` - Periodic idle animation variant

**Parameters:**
- `Speed` (Float) - Controls Idle <-> Run transitions
- `Run` (Bool) - Triggers run animation
- `IdleAttack` (Trigger) - Triggers idle attack variant

**Transitions:**
```
Idle → Run: Speed > 0.1
Run → Idle: Speed < 0.1
Idle → IdleAttack: Random periodic trigger
Any State → Attack: Attack trigger
```

**Optimization:**
- Has Exit Time: OFF (for responsive gameplay)
- Transition Duration: 0.05-0.1s (smooth but fast)
- Interruption Source: Current State

---

### 4. VFX System

**Weapon Hit Effect:**
- Particle System with burst emission
- Self-destruction via timed Destroy()
- Spawned via Animation Events for precise timing

**Particle Settings:**
```
Duration: 0.5s
Max Particles: 15
Emission: Burst (15 particles at once)
Size over Lifetime: 0 → 1 → 1.2 (growth curve)
Color over Lifetime: White → Transparent (fade out)
Shape: Circle with radial spread
```

**Implementation:**
```csharp
// Animation Event calls this method
private void OnAttackHit()
{
    ParticleSystem fx = Instantiate(attackFX, hitPosition, Quaternion.identity);
    Destroy(fx.gameObject, 1f);
}
```

---

### 5. Render Optimization

**Achieved Results:**
- Batched UI elements via Sprite Atlas
- Minimized state changes
- Efficient layer culling setup
- RectMask2D instead of Mask component

**Draw Call Breakdown:**
- UI: ~2-3 batches (single atlas)
- Gameplay: ~5-8 batches (sprites + particles)
- Total: **< 15 draw calls** on target devices

**Optimization Techniques:**
- Sprite Atlas with platform-specific compression
- Layer-based culling masks
- Disabled unnecessary Canvas components
- Static batching for environment objects

---

## Project Structure

```
Assets/
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
    │       ├── Sources/          # Original minimal sprites (16x16, 32x32)
    │       └── Atlases/          # Sprite Atlas assets
    │           └── GameplayView.spriteatlas
    │
    └── Scenes/
        └── Gameplay.unity
```

---

## Build Settings

**Target Platforms:**
- Standalone (Windows/Mac/Linux)
- WebGL
- Mobile (iOS/Android)

**Unity Version:** 6000.0.x LTS

**Render Pipeline:** Built-in

---

## Key Features

- **Adaptive UI** - Works on 16:9, 18:9, 4:3 aspect ratios
- **Memory Efficient** - Minimal sprite sizes with quality scaling
- **Smooth Animations** - Fast transitions with Animation Events
- **Performance Optimized** - < 15 draw calls average
- **Clean Architecture** - Modular component-based design

---

## How to Run

1. Clone the repository
```bash
git clone https://github.com/yourusername/zimad-test-assignment.git
```

2. Open in Unity (version 6000.0.x or later)

3. Open `_ZiMAD/Scenes/Gameplay.unity`

4. Press Play

---

## Performance Metrics

| Metric | Value |
|--------|----|
| Draw Calls | 5 |
| Batches | 8-10 |
| Texture Memory | ~2-3 MB |
| FPS (Target Device) | 60 FPS |

---

## Technologies Used

- **Unity** - 6000.0.x LTS
- **C#** - .NET Standard 2.1
- **Sprite Atlas** - Texture optimization
- **Animation Events** - VFX timing
- **RenderTexture** - Camera-to-UI rendering
- **Particle System** - Visual effects

---

## Notes

- All sprites optimized to minimal dimensions (16x16, 32x32)
- Sprite Atlas uses platform-specific compression (Crunch)
- Animation system uses StateMachineBehaviour for clean code
- VFX spawned via Animation Events for frame-perfect timing
- Camera renders to 1024x1024 RenderTexture for UI display

---

## Author

**LLarean**
- GitHub: [@LLarean](https://github.com/llarean)
- Email: llarean@yandex.com

---

## License

This project was created as a test assignment for Zimad.

---

## Acknowledgments

Test assignment provided by Zimad.