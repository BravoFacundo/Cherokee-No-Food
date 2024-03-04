*Este readme está escrito en INGLÉS. Si lo prefieres, puedes leerlo en [español](README.es.md).*

# Cherokee No Food

[Work in progress] Shooting gallery videogame with billboard graphics. Made in Unity, C#.

## Technologies

Developed using:
- C# Language
- Unity 2022.3.7f1

*You can see the code [here](Assets/_Scripts/).*

## Idea and development

This videogame is designed as a technical-aesthetic challenge, using 2D images to build a 3D world, which results in a diorama-style scenario.

The difficulty of the challenge lies mainly in the fact that 2D images may fail to reproduce or communicate aspects of a 3D world, such as the object's size or the depth perception of the environment. 

These aspects affect the gameplay and the player's user experience, therefore, the game design is kept simple, with a fixed perspective and a low need for precision.

## Implementation

Some elements of the game were kept as 3D assets instead of images to facilitate depth perception for the player. The projectiles that are thrown in the game, some rocks or plants on the ground and lights on the stage.

The game's scenario is composed of a set of layers that make up an image of the ground and the background. The actual ground on which the objects in the world rest is invisible and follows the same perspective.

Finally, using Shaders the images that make up the background, such as trees or stars in the sky can be animated. Shaders are also used so that the images used in the environment and the enemies can receive lights from the scene appropriately.

## Image gallery

![Github_CherokeeNoFood_01](https://github.com/BravoFacundo/CherokeeNoFood/assets/88951560/ce057646-a589-4327-8400-24ecd378ede1)
![Github_CherokeeNoFood_02](https://github.com/BravoFacundo/CherokeeNoFood/assets/88951560/b812c944-8ef3-41b6-8caa-71933d20f65d)
![Github_CherokeeNoFood_03](https://github.com/BravoFacundo/CherokeeNoFood/assets/88951560/4f038edb-aba4-49b1-9904-9446d36659a0)
![Github_CherokeeNoFood_04](https://github.com/BravoFacundo/CherokeeNoFood/assets/88951560/922fcb7d-0805-47ea-b4df-5dc9ad5dc631)

## Links and credits

The game's art, such as backgrounds, characters and 3D models were created particularly for this project by collaborators.

2D Artists:
- Dose (Jose Bayugar): [Behance](https://www.behance.net/bayugarj79c4) - [Instagram](https://www.instagram.com/dose_jb/)
- Ikumi: [Instagram](https://www.instagram.com/ikumi_arte/)

3D Models:
- Geronimo Calderon: [Artstation](https://scarymons7ers.artstation.com/).

