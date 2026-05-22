# Blox

A physics-aligned 3D puzzle game built in Unity, heavily inspired by the classic flash game *Bloxorz*.

## Features
* **Dynamic Grid Movement:** Precise math-driven rolling system using linear interpolation (Lerp) and SmoothStep easing curves for fluid, tactile movement.
* **Orientation-Based Snapping:** Automatic height and coordinate correction ensuring the block dynamically updates its footprint based on whether it is standing vertically or lying horizontally.
* **Edge Tipping Physics:** Custom raycasting framework that detects partial footprint support, causing the block to realistically tip over the edge of platforms into empty space instead of ghosting through geometry.
* **Upright Win Condition:** Strict vertical validation requiring the block to stand completely upright on designated goal tiles to trigger level completion.

## Project Architecture
* **`BlockMovement.cs`:** The core engine handling input assembly, structural bounds calculations, world axis projections, rolling animations, grid lock algorithms, and spatial raycast safety checks.

## Development Stack
* **Engine:** Unity
* **Input Architecture:** Unity New Input System
* **Language:** C#