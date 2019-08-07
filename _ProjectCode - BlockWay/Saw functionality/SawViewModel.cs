using LunarByte.MVVM;

public class SawViewModel : ViewModel, ISawViewModel
{
	private bool  SawActiveField;
	private float SawAutoSpeedField;
	private float SawCurveField;
	private bool SuperSawActiveField;
	private float SawMovementSpeedField;
	private float SawPosField;
	private float SawRotationSpeedField;
	private float SuperSawScaleMultiplierField;
	private float SawScaleBaseField;
    private string SuperSawTextField;
    private string LevelFailTextField;
    private string LevelCompleteTextField;

    public Event<LevelEndState, int> EndLevelEvent { get; } = new Event<LevelEndState, int>();
	public Event<int> GivePointsEvent { get; } = new Event<int>();
	public Event<bool> ToggleDirectionEvent { get; } = new Event<bool>();
	public Event<int> GatherSuperHayEvent { get; } = new Event<int>();
    public Event StartDecreasingSuperHayEvent { get; } = new Event();

    public float SawMovementSpeed
	{
		get { return SawMovementSpeedField; }
		set
		{
			SawMovementSpeedField = value;
			OnPropertyChanged();
		}
	}

    public float SuperSawScaleMultiplier
    {
	    get { return SuperSawScaleMultiplierField; }
	    set
	    {
		    SuperSawScaleMultiplierField = value;
		    OnPropertyChanged();
	    }
    }

    public float SawScaleBase
    {
	    get { return SawScaleBaseField; }
	    set
	    {
		    SawScaleBaseField = value;
			OnPropertyChanged();
	    }
    }

    public IDispatchableEvent<LevelEndState, int> EndGame
	{
		get { return EndLevelEvent; }
	}

	public IDispatchableEvent<int> GivePoints
	{
		get { return GivePointsEvent; }
	}

	public float SawRotationSpeed
	{
		get { return SawRotationSpeedField; }
		set
		{
			SawRotationSpeedField = value;
			OnPropertyChanged();
		}
	}

	public IDispatchableEvent<bool> ToggleDirection
	{
		get { return ToggleDirectionEvent; }
	}
	
	public IDispatchableEvent<int> GatherSuperHay
	{
		get { return GatherSuperHayEvent; }
	}

	public IDispatchableEvent StartDecreasingSuperHay
	{
		get { return StartDecreasingSuperHayEvent; }
	}

	public bool SawActive
	{
		get { return SawActiveField; }
		set
		{
			SawActiveField = value;
			OnPropertyChanged();
		}
	}

	public float SawAutoSpeed
	{
		get { return SawAutoSpeedField; }
		set
		{
			SawAutoSpeedField = value;
			OnPropertyChanged();
		}
	}

	public float SawPos
	{
		get { return SawPosField; }
		set
		{
			SawPosField = value;
			OnPropertyChanged();
		}
	}

	public float SawCurve
    {
        get { return SawCurveField; }
        set
        {
            SawCurveField = value;
			OnPropertyChanged();
        }
    }
	public bool SuperSawActive
	{
		get { return SuperSawActiveField; }
		set
		{
			SuperSawActiveField = value;
			OnPropertyChanged();
		}
    }
}

public interface ISawViewModel : IObservableProperties
{
    float SawMovementSpeed { get; }
	IDispatchableEvent<LevelEndState, int> EndGame { get; }
	IDispatchableEvent<int> GivePoints { get; }
	IDispatchableEvent<int> GatherSuperHay { get; }

	IDispatchableEvent StartDecreasingSuperHay { get; }
	bool SawActive { get; }
	bool SuperSawActive { get; }
	float SawAutoSpeed { get; }
	float SawPos       { get; }
	float SawCurve     { get; }

	float SuperSawScaleMultiplier { get; }
	float SawScaleBase { get; }

    IDispatchableEvent<bool> ToggleDirection { get; }

	float                                  SawRotationSpeed { get; }
}
