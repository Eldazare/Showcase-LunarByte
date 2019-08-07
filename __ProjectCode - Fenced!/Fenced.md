**LevelViewWall.cs** <br/>
Generates and resets the level. <br/>
Tile and Wall arrays are bound from LevelModelWall, LevelCommands can be given via LevelExtension method.<br/>
Generate method first returns all GameObjects to pool and then pulls new Tiles and Walls from pool according to the bound data.<br/>
Tiles and Walls can be Disposed, LevelView doesn't have to know what pool they belong to to return them to pool.<br/>

**LevelGameConfig.cs** <br/>
Tile and Wall pools are bound here. <br/>
PoolablePoolContainer contains pools for each prefab given to it. This way we can pool different objects without having to address them by type (only by index). This is why it's important to have....<br/>

**PrefabSettingsWall.cs**<br/>
Determined in Unity Editor.<br/>
Makes sure that A specific prefab is bound to specific Char used in LevelDefinition CSV-files.<br/>
-> We can be sure that specific Char index is equal to specific prefab index.<br/>
Also holds initial pool sizes for each object.<br/>


**LevelSelectionView.cs**<br/>
Dynamically generates buttons depending on the amount of levels loaded.<br/>
Initializes each button to load correct level upon press.<br/>



**Tiles and Walls**<br/>
Contain definition to what happens when Player enters the tile/wall.<br/>
Walls and Tiles trigger events through PlayerView, thus affecting the game state.<br/>


**FragileTile functionality**<br/>
On drop, FragileTile disables it's normal collider and activates a collider that's of collision layer "Wall", so it acts as a Wall afterwards.<br/>
