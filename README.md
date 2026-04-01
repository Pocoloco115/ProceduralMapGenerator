# 🎲 Procedural Dungeon Generator – Rogue-Lite Style Maps in Unity

<div align="center">
  <img src="Media/gameplay.gif" alt="Game Preview" width="600"/>
</div>

<br>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2022.3+-000000?logo=unity&logoColor=white&style=for-the-badge" alt="Unity">
  <img src="https://img.shields.io/badge/Made%20with-C%23-239120?logo=c-sharp&logoColor=white&style=for-the-badge" alt="C#">
  <img src="https://img.shields.io/badge/.NET-8.0+-512BD4?logo=.net&logoColor=white&style=for-the-badge" alt=".NET">
  <img src="https://img.shields.io/badge/Pixel%20Dungeons-Yes-ff69b4?style=for-the-badge" alt="Pixel Dungeons">
</p>

---

It’s a **procedural dungeon generator**.

Not a level editor.  
Not a full game (yet).  

Just maps.  
Generated at runtime.  
By code I actually understand (most days).

This is one of my first “serious” procedural generation projects in Unity –  
a toolkit to create **grid-based dungeons** with different algorithms and parameters.

---

## 🗺️ What Is This?

A small Unity project focused on **procedural 2D dungeon generation**, built around:

- Different **generation algorithms** (map types)
- A flexible **parameter UI** using `ScriptableObject`
- Pixel-art style layouts meant for top-down / roguelike / rogue-lite games

If you’ve ever played:

- Any roguelike
- Any “generate random dungeon” game

You already know *why* this exists.

This project is about **how** to build that kind of map system in Unity.

---

## 🧮 Map Types (a.k.a. Ways to Suffer Less Designing Levels)

Currently supported map types:

- **Simple Random Walk**
  - A noisy, organic layout
  - Great for cave-like rooms and messy dungeons

- **Corridors**
  - Random walks extended by straight-ish corridors
  - Feels more like hallways connecting rooms

- **BSP Rooms**
  - Binary Space Partitioning to carve out rooms and connect them
  - A bit more “structured dungeon generator” style

Each type has its own parameter panel, so you can tweak it until it breaks. Then tweak it back.

---

## 🎛 Parameter System (a.k.a. UI of Questionable Power)

The core of the project is the **map parameter UI**, driven by `ScriptableObject`s:

- `MapGeneratorSO` – holds all configurable values:
  - Random walk:
    - `walkLength`
    - `iterations`
    - `startRandomlyEachIteration`
  - Corridors:
    - `corridorLength`
    - `corridorCount`
    - `roomPercent`
    - `corridorWideningMode`
  - BSP Rooms:
    - `dungeonWidth`, `dungeonHeight`
    - `minRoomWidth`, `minRoomHeight`
    - `offset`
    - `randomWalkRooms`

- `MapParametersUI` – binds UI ↔ parameters:
  - Uses **TMP_InputField**, **Slider**, **Toggle**, **Dropdown**
  - Shows only the relevant panels depending on the map type:
    - Simple
    - Corridors
    - BSP Rooms
  - Two modes:
    - **Default mode** → uses predefined `defaultParameters`
    - **Custom mode** → lets you edit `customParameters` with full control

Validation is included so that:

- If your inputs are invalid (e.g. `minRoomWidth > dungeonWidth`, offsets absurdos…):
  - It **shows a warning animation** 
  - It **does not apply** the bad values to the ScriptableObject
  - And you can prevent map generation when validation fails

---

## ▶️ How to Use

You’ve got two options:

### 1. Just Play with It (Build Release)

1. Go to the **Releases** section of this repository.
2. Download the latest build for your platform.
3. Unzip and run the executable.
4. Pick a **map type** and tweak some parameters.
5. Spam the **Generate** button until you get:
   - a layout you love, or
   - a layout you absolutely hate (but still kinda want to keep).

### 2. Open the Project in Unity

1. Clone or download this repository.
2. Open it in **Unity 2022.3+**.
3. Open the main scene that contains:
   - The **dungeon generator**,
   - The **parameter UI**, and
   - The **camera controller**.
4. Hit **Play** and experiment:
   - Switch between **Default** and **Custom** modes.
   - Try different **Map Types** (Simple / Corridors / BSPRooms).
   - Break it with weird values, then angrily fix them when the warning animation triggers.

---

## ⌨️ Controls

- **WASD / Arrow Keys** → Move the camera around the dungeon  
- **Mouse Wheel** → Zoom in / out (speed scales with zoom level)  
- **Left Click + Drag** → Pan the camera  
- Zoom and drag are **disabled when the cursor is over UI**, so you don’t move the map while adjusting sliders and inputs.

---

## 📦 Project Status

- 🧱 Dungeon generation → Working  
- 🧮 Multiple algorithms → Implemented (Random Walk, Corridors, BSP Rooms)  
- 🎛 Parameter UI → Fully hooked to `ScriptableObject` configs  
- 🚫 Validation → Prevents generation when inputs are invalid (with warning animation)  
- 🐞 Bugs → Statistically inevitable  
- 🔧 Ready to extend → Add your own algorithms, tiles, or full game logic on top

---

## 🗂 Project Structure

```text
Assets/
└── Scripts/
    ├── Camera/
    │   └── (camera controllers, input handling)
    ├── Data/
    │   └── ScriptableObject/
    │       └── (MapGeneratorSO and related data assets)
    ├── Game/
    │   ├── Abstractions/
    │   │   └── (interfaces / base classes for generators)
    │   ├── Algorithms/
    │   │   └── (Random Walk, Corridors, BSPRooms implementations)
    │   ├── Data/
    │   │   └── MapCases/
    │   │       └── (preset map configurations / cases)
    │   ├── Editor/
    │   │   └── (custom inspectors / editor tooling)
    │   └── Manager/
    │       └── (high-level map generation orchestration)
    └── UI/
        └── (MapParametersUI, menus, warning animations, etc.)
