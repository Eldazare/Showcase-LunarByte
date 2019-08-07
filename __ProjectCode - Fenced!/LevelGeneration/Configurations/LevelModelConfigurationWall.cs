using LunarByte.MVVM.Configurations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelModelConfigurationWall), menuName = "LevelGeneration/ModelConfig")]
public class LevelModelConfigurationWall : ModelConfiguration<ILevelModelWall, LevelModelWall, ILevelDatasContainer>
{
    protected override void Configure(LevelModelWall model, ILevelDatasContainer container)
    {
        Debug.Log($"Configuring {nameof(LevelModelConfigurationWall)}");
        if (container.LevelDatas.Length > 0)
        {
            int[] zeroArr = new int[10] { 0,0,0,0,0,0,0,0,0,0};
            model.UpdateMaxTileObjects(zeroArr, 0);
            model.SetLevelData(container, 0);
            model.Command = LevelCommand.None;
        } else
        {
            Debug.LogError("Bound LevelDatasContainer doesn't contain any level data.");
        }
    }
}
