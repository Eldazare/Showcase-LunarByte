using System;
using LunarByte.MVVM;
using System.Collections;
using System.Collections.Generic;

public class LevelViewModelWall : ViewModel, ILevelViewModelWall
{
    private ILevelDataWall LevelDataField;
    private LevelCommand CommandField;
    private int EndLevelKeysField;

    public ILevelDataWall LevelData
    {
        get { return LevelDataField; }
        set
        {
            LevelDataField = value;
            OnPropertyChanged();
        }
    }

    public LevelCommand Command
    {
        get { return CommandField; }
        set
        {
            CommandField = value;
            OnPropertyChanged();
        }
    }

    public int EndLevelKeys
    {
	    get { return EndLevelKeysField; }
	    set
	    {
		    EndLevelKeysField = value;
		    OnPropertyChanged();
	    }
    }

    public Event ResetPlayerModelEvent { get; } = new Event();

    public IDispatchableEvent ResetPlayerModel { get { return ResetPlayerModelEvent; } }

    public Event ResetInputEvent { get; } = new Event();

    public IDispatchableEvent ResetInput { get { return ResetInputEvent; } }

    public Event<int[],int> UpdateMaxTileObjectsEvent { get; } = new Event<int[], int>();

    public IDispatchableEvent<int[], int> UpdateMaxTileObjects { get { return UpdateMaxTileObjectsEvent; } }

    public Event ChooseNextLevelEvent { get; } = new Event();

    public IDispatchableEvent ChooseNextLevel { get { return ChooseNextLevelEvent; } }

    public Event<int, Action> AttemptRemoveAllKeysEvent = new Event<int, Action>();

	public IDispatchableEvent<int, Action> AttemptRemoveAllKeys {  get { return AttemptRemoveAllKeysEvent; } }
}

public interface ILevelViewModelWall
{
    ILevelDataWall LevelData { get; }
    LevelCommand Command { get; }
    int EndLevelKeys { get; }

    IDispatchableEvent ResetPlayerModel { get; }
    IDispatchableEvent ResetInput { get; }
    IDispatchableEvent<int[], int> UpdateMaxTileObjects { get; }
    IDispatchableEvent ChooseNextLevel{ get; }

    IDispatchableEvent<int, Action> AttemptRemoveAllKeys { get; }
}
