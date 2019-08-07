LevelViewWall.cs
Generates and resets the level.
Tile and Wall arrays are bound from LevelModelWall, LevelCommands can be given via LevelExtension method.
Generate method first returns all GameObjects to pool and then pulls new Tiles and Walls from pool according to the bound data.
Tiles and Walls can be Disposed, LevelView doesn't have to know what pool they belong to to return them to pool.
