using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataWall : ScriptableObject, ILevelDataWall
{
    [SerializeField] private int SizeField;
    [SerializeField] private int StartPositionXField;
    [SerializeField] private int StartPositionYField;
    [SerializeField] private int[] TilesField;
    [SerializeField] private int[] WallsField;


    public string Name { get{ return name; } }
    public int Size
    {
        get { return SizeField; }
        set { SizeField = value; }
    }

    public int StartPositionX
    {
        get { return StartPositionXField; }
        set { StartPositionXField = value; }
    }
    public int StartPositionY
    {
        get { return StartPositionYField; }
        set { StartPositionYField = value; }
    }
    public int[] Tiles
    {
        get { return TilesField; }
        set { TilesField = value; }
    }
    public int[] Walls
    {
        get { return WallsField; }
        set { WallsField = value; }
    }



    public bool IsInitializedCorrectly(out string error)
    {
        if (!(Size > 0))
        {
            error = ($"{nameof(Size)} is negative or 0");
            return false;
        }
        if (!(StartPositionX >= 0 && StartPositionX < Size))
        {
            error = ($"{nameof(StartPositionX)} is not between 0 and {nameof(Size)}");
            return false;
        }
        if (!(StartPositionY >= 0 && StartPositionY < Size))
        {
            error = ($"{nameof(StartPositionY)} is not between 0 and {nameof(Size)}");
            return false;
        }
        if (Tiles.Length != Size * Size)
        {
            error = ($"{nameof(Tiles)} array length is not {nameof(Size)} times {nameof(Size)}");
            return false;
        }
        if (Walls.Length != (Size+1) * (2*Size+1))
        {
            error = ($"{nameof(Walls)} array length is not {nameof(Size)}+1 times 2*{nameof(Size)}+1");
            return false;
        }
        error = "";
        return true;
    }
}

public interface ILevelDataWall
{
    string Name { get; }
    int Size { get; }
    int StartPositionX { get; }
    int StartPositionY { get; }
    int[] Tiles { get; }
    int[] Walls { get; }
}
