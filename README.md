# Chrono Savior

## Description
"Chrono Savior" is a 2D adventure game with shooter, adventure, and action genres. The game involves time travel and missions to save the Universe's future. Players take on the role of a temporal guardian, navigating through various missions and sub-missions to ensure the safety of the Universe.

## Table of Contents
- [Installation](#installation)
- [Usage](#usage)
- [Controls](#controls)
- [Menu Flowchart](#menu-flowchart)
- [Prototype 2 Details](#Prototype-2)
- [Contact](#contact)

## Installation
To run "Chrono Savior" on your local machine, follow these steps:

1. **Clone the repository**

   

2. **Open Unity**
- Make sure you have Unity LTS v2022.3.30f1 installed.
- Open Unity Hub, click on "Add -> Add project from disk," and select the `Chrono Savior` folder inside the repository.

## Usage
To play the game or test its features:

1. **Open the Unity editor.**
2. **Load the main scene:**
- Navigate to the `Assets/Scenes` directory and load the <b>Main Menu</b> scene.
3. **Play the game:**
- Press the play button in the Unity editor to start the game.
- All controls are discussed in detail in the next section.

## Controls
### Movement:
- **Keyboard:** In Ground Combat Simulation use the W, A, S, D keys to move the player character in the desired direction. And in the Space Shooter use W, S to move the ship in the up and down direction.

### Aiming and Shooting:
- **Mouse:** Aim with the mouse cursor and left-click to shoot (can hold to fire for ground combat simulation).

### Special Abilities:
- **Keyboard:** Use Z to activate special ability.

### Weapon Swapping:
- **Keyboard:** 1, 2, 3 to cycle between weapons.

### Game Pausing
- **Keyboard:** Escape key to to pause the game

## Menu Flowchart
**Main Menu:**
- Start
  - Infinite Mode
    - Space Travel
    - Ground Combat
  - Campaign Mode
    - Load New Session (currently only space shooter level added - inprogress)
    - Load Last Session (feature not added yet)
- Options
    - Volume
- Store (feature not added yet)
- Instruction (feature not added yet)

**Game Over Screen:**
- Restart
- Options
    - Volume
- Main Menu

**Pause Screen:**
- Resume
- Options
- Main Menu

## Prototype 2
### Fixes 
- UI showing bullet count #/inf changed to show #/MAX
- The sprite slips when the gun is facing left, making gun rotation feel natural
- Null checks/Defensive coding implemented
- Mathf.Clamp used to clean up if/else blocks
- Spacecraft bounds fixed
- Used SerializeField everywhere instead of layer int
- Object pooling on bullets 
- sqrMagnitude used instead of square root
- Code made consistent throughout

### Additions
- Added proper main menu
- Added pause menu and resume functionality
- Added sounds
- Added volume slider in options
- Added UI for space mode
- Added power-ups for space shooter mode
- Art updated
- Campaign mode progress started - space shooter section done


## Contact
For any questions, reach out to Team 1:

- **Email:** 26100182@lums.edu.pk, 26100053@lums.edu.pk
- **GitHub:** [maazKashif-999](https://github.com/maazKashif-999)

