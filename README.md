# Archery Game

An immersive archery experience built in Unity, focused on realistic bow mechanics, physics-based arrow flight, and interactive gameplay. This project explores intuitive player interaction, aiming systems, and satisfying feedback in a virtual environment.

# Features  
1. Realistic Bow Mechanics  
Draw, aim, and release using physics-based interactions.
2. Target Practice System  
Shoot at targets with scoring feedback and scoreboard.
3. Menu Naviagtion  
Allow users to navigate through menus to allow customization of gameplay.
4. Gamemodes  
Include a time trial gamemode where players must hit as many targets as possible before time runs out.

# Built With
Unity (Game Engine)  
C# (Scripting)  
XR Interaction Toolkit  
Blender / External Assets  

# Getting Started
Prerequisites:  
Install Unity Hub  
Recommended Unity version: Version 6000.4.0f1  

# Clone the repository:
git clone https://github.com/RJones-Code/ArcheryGame.git  
Open the project using Unity Hub  
  
Load the main scene:  
Assets/Scenes/MainMenu.unity  

Open the game scene and keep unloaded:  
Assets/Scenes/GameScene.unity  

Make sure XR Device simulator is toggled OFF in both scenes if using occulus or virtual headset.  
Press Play to start.  

# Project Structure
Assets/  
├── Bow         (Bow & Arrow Models, Prefabs, Scripts, Textures, and Materials)  
├── Environment (Environment Object, Terrain Data)  
├── HandModels  (Models and Prefabs for unity hands)  
├── Scenes      (Scenes and all lighting data)  
├── Scripts     (Scripts used for the main gameplay loop, Main Menu, and Game Scene menus)  
├── Sounds      (Sounds used for bow effects shoot, drop, pull, and impact, scene music, and audio mixer)  
├── Target      (Target Models, Prefabs, Scripts, Textures, and Materials)  
├── UI          (UI assets for menus, and font file (chomsky))  
└── WeaponRack  (Weapon Rack Texture and Script)  

# Future Improvements
More Gamemodes  

# Contributing
Russell Jones, Sean Gao, Ishan Phadke, Julia Sokolowski

# License
This project is licensed under the MIT License.  

# Acknowledgments
- Occulus Hand Models: https://developers.meta.com/horizon/downloads/package/oculus-hand-models/
- Low Poly Environment: https://assetstore.unity.com/packages/3d/environments/low-poly-environment-nature-free-lowpoly-medieval-fantasy-series-187052 
- Sunny Valley Studio: https://www.patreon.com/posts/74926653 
- Main Menu Music: HeatlyBros - Option Menu https://www.youtube.com/watch?v=krAEJH0SuGY 
- Game Scene Music: HeatlyBros - Battle Fantasy https://www.youtube.com/watch?v=BTBVX2uGA00 
