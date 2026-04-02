# Flappy Bird Unity Project Presentation

## Slide 1: Project Introduction

**Project Name:** Flappy Bird Unity Project

**Goals:**

- build a 2D Flappy Bird style game in Unity
- implement an endless runner gameplay loop
- add a settings interface and audio system

**Speaker Notes:**

Hello everyone. Today we would like to present our Flappy Bird project built with Unity. This is a 2D endless runner style game where the player controls a bird, avoids obstacles, collects points, and tries to survive for as long as possible.

---

## Slide 2: Technologies Used

- Unity `6000.4.0f1`
- `C#`
- Unity Input System
- TextMesh Pro
- URP 2D

**Speaker Notes:**

For this project, we used Unity version 6000.4.0f1 and C# for scripting. We also used the Unity Input System for modern input handling, TextMesh Pro for UI text, and URP 2D for rendering support in the game.

---

## Slide 3: Main Features

- player can fly using `Space` or left mouse click
- obstacles spawn continuously
- collectibles increase the score
- game over happens on collision
- quick restart is supported
- background music and sound effects are included
- settings menu allows volume adjustment
- press `Esc` to pause and open settings

**Speaker Notes:**

The main features of the game include player flight control, continuous obstacle spawning, collectible-based scoring, game over handling, and a quick restart system. We also added background music, sound effects, and an in-game settings menu for better user experience.

---

## Slide 4: Gameplay Flow

**Gameplay sequence:**

1. enter the game at the menu state
2. press to start
3. fly through narrow gaps
4. collect items to gain points
5. collide and trigger game over
6. press to restart

**Speaker Notes:**

At the beginning, the game shows a menu state. The player starts by clicking the mouse or pressing the space bar. Then the bird moves through obstacles while collecting items for points. If the player collides with an obstacle, the ground, or the ceiling, the game ends and can be restarted immediately.

---

## Slide 5: Script Architecture

**Important scripts:**

- `GameManager.cs`: manages game state, score, game over, and restart
- `PlayerController.cs`: controls the bird, animation, and collision
- `ObstacleSpawner.cs`: spawns obstacles and collectibles
- `Obstacle.cs`: handles obstacle movement
- `Collectible.cs`: handles items and score collection
- `GameAudio.cs`: manages music and sound effects
- `SettingsMenuUI.cs`: creates the runtime settings menu

**Speaker Notes:**

From an architecture perspective, we separated responsibilities into multiple scripts. For example, GameManager handles the game state, PlayerController manages player input and animation, ObstacleSpawner generates gameplay objects, and GameAudio plus SettingsMenuUI were added to improve the overall player experience.

---

## Slide 6: Obstacles And Scoring System

- obstacles spawn over time
- collectibles appear together with obstacles
- object pooling is used to reuse objects
- game speed increases over survival time

**Speaker Notes:**

One important part of the project is the use of object pooling. Instead of creating and destroying obstacles and collectibles repeatedly, we reuse them. This helps improve performance. In addition, the game speed gradually increases over time to make the gameplay more challenging.

---

## Slide 7: Audio System

- background music `bgm`
- flap sound `flap`
- score sound `score`
- hit sound `hit`
- audio files are loaded from `Assets/Resources/Audio`

**Speaker Notes:**

We added an audio system to make the game more engaging. The game currently includes background music, flap sound effects, score sounds, and hit sounds. These audio files are managed centrally and loaded from the Resources folder so they can be replaced or expanded easily.

---

## Slide 8: Settings And Pause

- opens with `Esc`
- pauses the game immediately
- adjusts `Music` volume separately
- adjusts `SFX` volume separately
- stores settings using `PlayerPrefs`

**Speaker Notes:**

An important improvement in this project is the in-game settings menu. When the player presses Esc, the game pauses and opens the settings panel. The player can adjust music and sound effect volume separately. These settings are saved using PlayerPrefs, so the selected values remain available the next time the game runs.

---

## Slide 9: Visuals And UI

- multi-layer parallax background
- creates a sense of depth during movement
- uses 2D sprites for player, obstacles, and items
- includes score text and game over UI

**Speaker Notes:**

For visuals, the game uses a multi-layer parallax background to create depth while moving. The player, obstacles, and items are all built with 2D sprites. In addition, the UI clearly shows the score and the game over state.

---

## Slide 10: Challenges And Solutions

**Problems we encountered:**

- `2 Audio Listener` warning
- conflict between legacy input and the new Input System
- settings opened but flap input still worked
- git push conflict with the remote repository

**Solutions:**

- keep only one camera with an `Audio Listener`
- switch UI input to `InputSystemUIInputModule`
- block gameplay input while settings are open
- merge remote changes and resolve conflicts before pushing

**Speaker Notes:**

During development, we faced several practical issues such as duplicate Audio Listeners, input conflicts, and GitHub merge conflicts. These problems were solved by standardizing the camera setup, using the correct Input System UI module, blocking gameplay input while paused, and carefully merging code before pushing.

---

## Slide 11: Results Achieved

- the game runs in Unity
- the main scene is playable
- audio and settings are implemented
- the game can be paused with `Esc`
- the full project has been pushed to GitHub

**Speaker Notes:**

At the current stage, the game runs properly in Unity, the main scene is playable, audio and settings are implemented, the pause system works, and the full project is managed through GitHub. This makes the project easier to demo, easier to maintain, and easier to continue as a team project.

---

## Slide 12: Future Improvements

- add a more polished main menu
- add a high score system
- add skin or background selection
- add scene transition effects
- build a polished `.exe` version for presentation

**Speaker Notes:**

In the future, we can improve the project by adding a better main menu, saving high scores, allowing players to choose skins or backgrounds, adding transition effects, and preparing a polished executable build for demonstration.

---

## Slide 13: Closing

**Thank you for listening**

**Speaker Notes:**

That is the end of our presentation about the Flappy Bird Unity project. Thank you for listening, and we are ready to answer any questions.

---

## Possible Defense Questions

### 1. Why did your team choose Unity?

Because Unity is very suitable for 2D games, supports C# well, has a visual editor, and makes it easier to manage scenes, sprites, animation, and builds.

### 2. Why did you use object pooling?

Because obstacles and collectibles appear continuously. If we create and destroy objects too often, performance can drop. Object pooling allows reuse and improves efficiency.

### 3. Why is the audio settings feature important?

Because it improves user experience. Players can adjust music and sound effects based on their preference, which makes the game feel more complete and professional.

### 4. Why did you use the new Input System?

Because the new Input System is more flexible, works better with keyboard, mouse, and UI input, and is more suitable for future expansion.

### 5. What part of the project is the most valuable?

The most valuable part is turning a basic Unity project into a more complete game with gameplay, audio, settings, pause functionality, and GitHub-based source control.
