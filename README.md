# The Parallax Background

This project includes a parallax background system for a Flappy Bird style game in Unity.

The background is built with multiple layers that move at different speeds:
- far layers move slowly
- near layers move faster
- each layer loops continuously to create an endless side-scrolling effect

The parallax logic is implemented in [ParallaxRepeatingLayer.cs](C:/Users/Admin/OneDrive/Desktop/Flappy-bird-main/Assets/Scripts/ParallaxRepeatingLayer.cs).

## How It Works

Each background layer uses two duplicated sprite objects placed side by side.
During play:
- every layer moves to the left
- the movement speed is controlled by `Start Scroll Speed`, `Max Scroll Speed`, and `Acceleration`
- `Parallax Multiplier` determines how fast each layer moves relative to the others
- when one sprite moves out of view, it is repositioned to the right side to create a seamless loop

## Example Setup

- Far layer: low `Parallax Multiplier` such as `0.2`
- Mid layer: medium `Parallax Multiplier` such as `0.5`
- Near layer: high `Parallax Multiplier` such as `0.9`

This creates the depth effect expected from a parallax background in side-scrolling games.
