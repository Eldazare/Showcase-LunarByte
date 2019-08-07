using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelModelWallExtensions
{
    public static void SetLevelData(this LevelModelWall model, ILevelDatasContainer dataContainer, int levelIndex)
    {
        model.LevelIndex = levelIndex;
        model.LevelData = dataContainer.LevelDatas[levelIndex];
    }

    public static void GiveLevelCommand(this LevelModelWall model, LevelCommand command)
    {
        model.Command = command;
        model.Command = LevelCommand.None;
    }

    public static void SetNextLevel(this LevelModelWall model, ILevelDatasContainer container)
    {
        int newLevelIndex = model.LevelIndex+1;
        if (newLevelIndex >= container.LevelDatas.Length)
        {
            newLevelIndex = 0;
        }
        model.SetLevelData(container, newLevelIndex);
    }

    public static void UpdateMaxTileObjects(this LevelModelWall model, int[] maxKeys, int maxCollectibles)
    {
        model.KeysMax = maxKeys;
        model.CollectiblesMax = maxCollectibles;
    }
}
