using LunarByte.MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModelWall : Model, ILevelModelWall
{
    private ILevelDataWall LevelDataField;
    private LevelCommand LevelCommandField;
    private int LevelIndexField;

    private int[] KeysMaxField;
    private int CollectiblesMaxField;

    public ILevelDataWall LevelData {
        get { return LevelDataField; }
        set
        {
            LevelDataField = value;
            OnPropertyChanged();
        }
    }

    public LevelCommand Command
    {
        get { return LevelCommandField; }
        set
        {
            LevelCommandField = value;
            OnPropertyChanged();
        }
    }

    public int LevelIndex
    {
        get { return LevelIndexField; }
        set
        {
	        LevelIndexField = value;
            OnPropertyChanged();
        }
    }

    public int[] KeysMax
    {
        get { return KeysMaxField; }
        set
        {
            KeysMaxField = value;
            OnPropertyChanged();
        }
    }

    public int CollectiblesMax
    {
        get { return CollectiblesMaxField; }
        set
        {
            CollectiblesMaxField = value;
            OnPropertyChanged();
        }
    }
}

public interface ILevelModelWall : IObservableProperties
{
    ILevelDataWall LevelData { get; }
    LevelCommand Command { get; }
    int LevelIndex { get; }
    int[] KeysMax { get; }
    int CollectiblesMax { get; }
}

public enum LevelCommand { None, Reset, Generate, Dispose}
