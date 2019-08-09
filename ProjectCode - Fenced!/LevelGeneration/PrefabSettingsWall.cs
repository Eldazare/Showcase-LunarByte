using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PrefabSettingsWall), menuName = ("Settings/" + nameof(PrefabSettingsWall)))]
public class PrefabSettingsWall : ScriptableObject
{

    [SerializeField] private LevelObjectStructWall[] LevelObjectArray;
    [SerializeField] private LevelObjectStructWall[] WallObjectArray;

    public GameObject[] TilePrefabList { get; private set; }
    public GameObject[] WallPrefabList { get; private set; }

    public char[] TileCharList { get; private set; }
    public char[] WallCharList { get; private set; }

    public int[] TilePoolInitialSizes { get; private set; }
    public int[] WallPoolInitialSizes { get; private set; }


    public void OnValidate()
    {
        //Debug.Log("OnValidate");
        List<char> charsL = new List<char>();
        List<GameObject> gObjects = new List<GameObject>();
        List<int> poolSizes = new List<int>();

        foreach (var obj in LevelObjectArray)
        {
            charsL.Add(obj.csvIdentifier);
            gObjects.Add(obj.prefab);
            poolSizes.Add(obj.poolInitialSize);
        }
        TileCharList = charsL.ToArray();
        TilePrefabList = gObjects.ToArray();
        TilePoolInitialSizes = poolSizes.ToArray();

        charsL.Clear();
        gObjects.Clear();
        poolSizes.Clear();

        foreach (var obj in WallObjectArray)
        {
            charsL.Add(obj.csvIdentifier);
            gObjects.Add(obj.prefab);
            poolSizes.Add(obj.poolInitialSize);
        }
        WallCharList = charsL.ToArray();
        WallPrefabList = gObjects.ToArray();
        WallPoolInitialSizes = poolSizes.ToArray();
    }
}

[Serializable]
public struct LevelObjectStructWall
{
    public char csvIdentifier;
    public GameObject prefab;
    public int poolInitialSize;
}