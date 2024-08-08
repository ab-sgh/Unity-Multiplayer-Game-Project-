**Note**: Due to some reason, the game scene doesn't load directly while opening the game for the first time. So, drag 'Game Scenes' from Assets folder to the Hierarchy tab. I'll try to fix this

# Multi-Player Game using Unity

This project is based on developing a multi-player game using the Untiy Engine. In this project, I am developing a top-down 2D shooter which will be based on a PvP based gameplay.


## Acknowledgements

 - [Unity-Learn](https://learn.unity.com/)
 - [Shooter Sprite Pack](https://assetstore.unity.com/packages/2d/characters/shooter-sprite-pack-63136)
 - [2D Map Asset](https://assetstore.unity.com/packages/2d/environments/pixel-art-top-down-basic-187605)
 - [Brackeys YT Channel](https://www.youtube.com/@Brackeys)


## Documentation

In this project, after getting through all the important basics of Unity, I started ideation of my project.   
I decided to go with 2D project as it less complicated in terms of physics and other mechanics.  
After importing some free assets from Unity Store, I was ready to proceed to the next part. With the help of Brackeys channel, I learned about the player movement in Unity. Since, I am working on a top- down game, it is cruicial to rotate player based on mouse movement, so I implemented that.   
Till now, I have made a basic structure of the game, with the player movement, shooting script, as well as the orientation based on mouse movement.  
Basic Features(till now):
W,A,S,D - for player movement    
Hold Shift - for sprint (have not made timer for this yet)   
Left Mousekey and Mousemovement - to shoot and rotate player aim.     

For the next part I am planning on implementing:
- Multi-player Features
- Health Bar  
- UI including winner, bestscore etc.  
- Some extra animations for aesthetics

## Implementing Multiplayer

  In the later part of the project, I have implemented multiplayer system to the game.
  Now the game is a 1v1 2d top down shooter where the two players spawn at opposite side of the map and whoever dies first loses the game.
  Primarily, I have used Netcode for Unity which is very helpful for making multiplayer games in Unity.
  **Note**: All the relevant scripts and Assets can be found in their corresponding folder
  




