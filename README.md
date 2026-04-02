# Flappy Bird Unity Project

This repository contains a Unity 2D Flappy Bird style game with:

- endless obstacle spawning
- collectible score pickups
- parallax scrolling background
- runtime audio system
- in-game settings menu for music and SFX volume
- pause on `Esc`

## Unity Version

Open the project with:

- `Unity 6000.4.0f1`

The exact editor version is defined in `ProjectSettings/ProjectVersion.txt`.

## Project Status

The current playable scene is:

- `Assets/Scenes/Collision.unity`

The current enabled build scene in `ProjectSettings/EditorBuildSettings.asset` is:

- `Assets/Scenes/Collision.unity`

Other scenes in the project:

- `Assets/Scenes/SampleScene.unity`
- `Assets/Scenes/Parallax.unity`
- `Assets/Free 2D Cartoon Parallax Background/Demo/Scenes/Demo_Scene.unity`

## How To Open And Run

1. Install Unity Hub.
2. Install Unity Editor `6000.4.0f1`.
3. Add this folder as a project in Unity Hub.
4. Open the project.
5. Open `Assets/Scenes/Collision.unity`.
6. Press `Play` in the Unity Editor.

If Unity shows an audio listener warning, make sure only one scene is open and only one `Main Camera` with an `Audio Listener` is active.

## Controls

- `Space`: start game
- `Left Mouse Button`: start game
- `Space`: flap
- `Left Mouse Button`: flap
- `Esc`: pause game and open settings
- `Esc` again: close settings and resume

When the player loses:

- `Space` or `Left Mouse Button`: restart

## Gameplay Overview

The gameplay loop is handled mainly by:

- `Assets/Scripts/GameManager.cs`
- `Assets/Scripts/PlayerController.cs`
- `Assets/Scripts/ObstacleSpawner.cs`
- `Assets/Scripts/Obstacle.cs`
- `Assets/Scripts/Collectible.cs`
- `Assets/Scripts/GameEvents.cs`

Current gameplay behavior:

- the player starts from a menu/idle state
- obstacles and collectibles spawn continuously after the run begins
- score increases when the player collects a collectible
- collision with hazards, ground, or ceiling triggers game over
- obstacle speed ramps up over time
- game over stops gameplay and allows restart input

## Audio System

The project includes a runtime audio manager:

- `Assets/Scripts/GameAudio.cs`

It supports:

- background music
- flap SFX
- score SFX
- hit SFX
- saved music volume
- saved SFX volume

Audio files are loaded from:

- `Assets/Resources/Audio`

Required clip names:

- `flap`
- `score`
- `hit`
- `bgm`

Examples:

- `Assets/Resources/Audio/flap.wav`
- `Assets/Resources/Audio/score.wav`
- `Assets/Resources/Audio/hit.wav`
- `Assets/Resources/Audio/bgm.mp3`

If a clip is missing, the game will still run and the Console will log which file name is missing.

## Settings Menu

The in-game settings UI is created at runtime by:

- `Assets/Scripts/SettingsMenuUI.cs`

Features:

- opens with `Esc`
- pauses the game while open
- uses Unity Input System UI input
- allows changing `Music` volume
- allows changing `SFX` volume
- stores values with `PlayerPrefs`

## Parallax Background

The parallax system is implemented in:

- `Assets/Scripts/ParallaxRepeatingLayer.cs`

The background uses multiple layers that move at different speeds to create depth during side scrolling.

Background art included in this repository is stored mainly under:

- `Assets/Free 2D Cartoon Parallax Background`

## Input System

This project uses the Unity Input System package.

Relevant files:

- `Assets/Settings/InputSystem_Actions.inputactions`
- `Packages/manifest.json`

The settings menu UI was adjusted to use Input System compatible UI input modules.

## Main Script Reference

- `Assets/Scripts/GameManager.cs`: game state, start flow, score text, game over, restart
- `Assets/Scripts/PlayerController.cs`: player flap input, animation, rotation, collision handling
- `Assets/Scripts/ObstacleSpawner.cs`: pooled spawning for obstacles and collectibles
- `Assets/Scripts/Obstacle.cs`: obstacle positioning and movement
- `Assets/Scripts/Collectible.cs`: collectible movement, animation, score trigger
- `Assets/Scripts/HazardTrigger.cs`: obstacle hit detection
- `Assets/Scripts/GameSettings.cs`: core gameplay constants
- `Assets/Scripts/GameEvents.cs`: shared gameplay events
- `Assets/Scripts/GameAudio.cs`: background music and sound effects
- `Assets/Scripts/SettingsMenuUI.cs`: runtime settings menu and pause behavior
- `Assets/Scripts/ParallaxRepeatingLayer.cs`: looping parallax background movement

## Folder Structure

Important folders in this repository:

- `Assets/Scenes`: main Unity scenes
- `Assets/Scripts`: gameplay and systems code
- `Assets/Prefab`: gameplay prefabs
- `Assets/Resources/Audio`: runtime-loaded audio clips
- `Assets/Sprite`: player and gameplay sprites
- `Assets/Materials`: materials used in the scene
- `Assets/Settings`: render pipeline and input assets
- `Packages`: Unity package manifest and lock file
- `ProjectSettings`: Unity project configuration
- `Test`: built game output included in this repository

## Build

To create a Windows build from Unity:

1. Open `File > Build Profiles` or `File > Build Settings` depending on your Unity UI.
2. Confirm `Assets/Scenes/Collision.unity` is included and enabled.
3. Choose Windows.
4. Click `Build`.

This repository already includes a build output folder:

- `Test`

## Known Notes

- Keep only one active scene camera with an `Audio Listener`.
- If you open multiple scenes additively, Unity may report two audio listeners.
- If you change audio files, keep the required names in `Assets/Resources/Audio`.
- The game uses runtime-generated settings UI instead of a prebuilt editor canvas prefab.

## Repository Purpose

This repository now represents the full current local Unity project state, including:

- source scene files
- scripts
- audio assets
- project settings
- included build output in `Test`

## License / Assets

Check third-party asset license terms before redistributing commercial builds, especially for:

- background packs
- fonts
- imported audio files
