# Procedural Dungeon Generator — roguelite maps in Unity

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

This is a procedural dungeon generator.

Not a level editor.  
Not a full game.  
Not wizardry, even if it sometimes looks like it.

It just generates maps at runtime.  
By code.  
And somehow the code is still behaving.

This project is one of my first “serious” procedural generation experiments in Unity. The goal is to build a base for grid-based dungeons with different algorithms and parameters you can poke until something breaks.

---

## What this is

A small Unity project focused on procedural 2D dungeon generation, built around:

- multiple generation algorithms
- a flexible parameter UI using `ScriptableObject`
- pixel-art style layouts for top-down, roguelike, and rogue-lite games

If you’ve ever played a game where the map is generated on the fly and everyone pretends that was a carefully planned feature, you already get the idea.

This project is about building that kind of system in Unity without turning the whole thing into a maintenance crime scene.

---

## Map types

Right now it supports:

- **Simple Random Walk**
  - a noisy, organic layout
  - good for cave-like dungeons and maps that look mildly unhinged

- **Corridors**
  - random walks stretched into mostly straight corridors
  - feels more like connected halls and rooms

- **BSP Rooms**
  - Binary Space Partitioning to split space, carve rooms, and connect them
  - more structured, more “real dungeon,” less “I clicked randomly and it worked”

Each map type has its own parameter panel, so you can tune the values until the map stops looking like an accident and starts looking like a deliberate one.

---

## Parameter system

The core of the project is the map parameter UI, driven by `ScriptableObject`.

### `MapGeneratorSO`
This holds all configurable values:

- Random Walk:
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

### `MapParametersUI`
This connects the UI to the parameters:

- uses `TMP_InputField`, `Slider`, `Toggle`, and `Dropdown`
- shows only the relevant panel depending on the selected map type:
  - Simple
  - Corridors
  - BSP Rooms
- has two modes:
  - **Default mode**: uses `defaultParameters`
  - **Custom mode**: lets you edit `customParameters` directly and make your own problems

Validation is included to stop obviously cursed input like:

- `minRoomWidth > dungeonWidth`
- absurd offsets
- values that look like they came from a machine trying to guess numbers

If something is invalid:

- a warning animation appears
- the bad values are not applied to the `ScriptableObject`
- generation is blocked until the input stops being nonsense

---

## How to use it

You have two options.

### 1. Just run the build

1. Go to the Releases section of this repository.
2. Download the latest build for your platform.
3. Unzip it and run the executable.
4. Pick a map type and tweak some parameters.
5. Press **Generate** until you get:
   - a layout you like, or
   - a layout you hate but keep anyway for emotional reasons

### 2. Open the project in Unity

1. Clone or download the repository.
2. Open it in **Unity 2022.3+**.
3. Open the main scene, which contains:
   - the dungeon generator
   - the parameter UI
   - the camera controller
4. Hit **Play** and experiment:
   - switch between **Default** and **Custom**
   - try **Simple / Corridors / BSP Rooms**
   - feed it weird values and see how quickly the warning animation starts judging you

---

## Controls

- **WASD / Arrow Keys** → move the camera
- **Mouse Wheel** → zoom in / out
- **Left Click + Drag** → pan the camera

Zoom and drag are disabled while the cursor is over the UI, so you don’t accidentally drag the map while fighting with sliders.

---

## Project status

- dungeon generation: working
- multiple algorithms: implemented
- parameter UI: wired to `ScriptableObject` configs
- validation: prevents generation when inputs are invalid
- bugs: statistically guaranteed
- ready to extend: add your own algorithms, tiles, or game logic on top

---

## Project structure

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
    │   │   └── (abstract classes / base classes for generators)
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
