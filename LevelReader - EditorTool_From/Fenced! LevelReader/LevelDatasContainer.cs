using LunarByte.MVVM.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelDatasContainer), menuName = "LevelGeneration/LevelDatasContainer")]
public class LevelDatasContainer : ScriptableObject, ILevelDatasContainer
{ 
    [SerializeField] private LevelDataWall[] LevelDatasField;

    public ILevelDataWall[] LevelDatas { get { return LevelDatasField; } }



    public void IsInitializedCorrectly()
    {
        if(LevelDatasField.Length == 0)
        {
            Debug.LogError($"{nameof(LevelDatasContainer)} has no LevelData! (There's no levels to load)");
        }
        string potentialError;
        for(int i = 0; i < LevelDatasField.Length;i++)
        {
            LevelDatasField[i].AssertNotNull();
            if (!LevelDatasField[i].IsInitializedCorrectly(out potentialError))
            {
                Debug.LogError($"Error in {nameof(LevelDataWall)} at index {i} with error message: {potentialError}");
            }
        }
    }
}

public interface ILevelDatasContainer
{
    ILevelDataWall[] LevelDatas { get; }
}
