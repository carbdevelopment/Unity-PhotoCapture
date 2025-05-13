# Unity Photo Capture System

Simple in-game photo capture system for Unity. Take, preview, and save screenshots during gameplay.

## Features
- Take screenshots in-game
- Camera zoom functionality
- Flash effect and sound
- Photo preview system
- Automatic photo saving

## Setup
1. Add PhotoCapture script to any GameObject
2. Assign required references in Inspector (acording to Canvas pregab):
   - Main Camera 
   - Photo Preview (PhotoFrameBG)
   - Photo UI (canvas)
   - Flash Effect (GameObject)
   - Camera Sound (AudioSource)

## Controls
- TAB: Toggle photo mode
- Mouse Wheel: Zoom in/out
- Left Click: Take photo
- Right Click: Exit photo view

## Photo Storage
Photos are automatically saved to: `[Project_Path]/Assets/Photos`
