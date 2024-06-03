# Character and Solution Management Game

This project is a Unity-based game where players can manage characters and solutions by creating and validating scriptable objects from JSON files and interacting with them in the game scene. It is a simple puzzle game with dragging and dropping UI elements into appropriate slots, showcasing the C# coding skills.

## Requirements

- Unity (version 2022.3.30f1 or later)
- JSON data files: `characters.json` and `solutions.json`
- Character images corresponding to entries in `characters.json`

## Setup and Usage
### Preparing the Game Objects
1. Ensure you have empty Inventory Slots (children of a GameObject tagged with "CharacterInventory") to spawn Character mugshots.
2. Ensure you have empty Solution Slots (children of a GameObject tagged with "SolutionInventory") to spawn Solutions.

### Creating Character Scriptable Objects
1. Open the Editor Tool by navigating to Tools -> Convert Character JSON to ScriptableObject.
2. The JSON File Path text field should already be filled with the path to characters.json.
3. Click the Convert button.
4. The scriptable objects will be created in the Assets/ScriptableObjects/CharacterData directory.

### Creating Character Mugshots
1. Open the Character Scriptable Object in the Unity Inspector.
2. Scroll to the "Create a Mugshot of a Character from its ScriptableObject" section.
3. Click the Create Character Mugshot button.
4. The mugshot will appear as a new child object of the first free Inventory Slot in the scene.

### Validating Character Scriptable Objects
1. Open the Editor Tool by navigating to Tools -> Character Data Validator.
2. Drag a Character Scriptable Object into the Character Data field.
3. Click the Validate button.
4. Review the validation log in the selected Scriptable Object's Validator Log text field in the Unity Inspector.

### Creating Solution Scriptable Objects
1. Open the Editor Tool by navigating to Tools -> Convert Solutions JSON to ScriptableObject.
2. The JSON File Path text field should already be filled with the path to solutions.json.
3. Click the Convert button.
4. The scriptable objects will be created in the Assets/ScriptableObjects/SolutionData directory.

### Adding Solution Data to Slots
1. Open the Solution Scriptable Object in the Unity Inspector.
2. Scroll to the "Add Solution Data to Inventory Slot" section.
3. Click the Add Solution Data to Slot button.
4. The solution will appear in the scene view as a SolutionInfo script component added to the first empty Solution Slot.

### Playing the Game
1. Spawn a couple of solutions into available Solution Slots and a couple of Characters into available Inventory Slots.
2. Click the Play button to start the game.
3. Drag and drop the character mugshots from the inventory to the solution slots to solve the puzzle. Each Solution Slot will only accept the correct Character based on specified attributes.

## Credits
- Project by Adrianna Jankowska
- Unity Version: 2022.3.30f1
- Date: 2024.06.03

## File Structure

```plaintext
Assets/
├── Resources/
│   ├── characters.json
│   ├── solutions.json
│   ├── CharacterImages/
│   │   ├── character1.png
│   │   ├── character2.png
│   │   └── ...
├── ScriptableObjects/
│   ├── CharacterData/
│   │   ├── character1.asset
│   │   ├── character2.asset
│   │   └── ...
│   ├── SolutionData/
│   │   ├── solution1.asset
│   │   ├── solution2.asset
│   │   └── ...

