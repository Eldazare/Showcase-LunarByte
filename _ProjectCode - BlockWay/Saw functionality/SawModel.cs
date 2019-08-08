using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;

public class SawModel : Model, ISawModel
{
    private float SawAutoSpeedField;
    private float SawCurveField;
    private float SawPosField;
    private float SawRotationSpeedField;

    private int SuperHayContainerMaxField;
    private float SuperSawScaleMultiplierField;
    private float SawScaleBaseField;
    private float SuperSawDurationField;

    public int SuperHayContainerMax
    {
	get { return SuperHayContainerMaxField; }
	set
	{
	    SuperHayContainerMaxField = value;
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

    public float SuperSawDuration
    {
	get { return SuperSawDurationField; }
	set
	{
	    SuperSawDurationField = value;
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

    public float SawCurve
    {
	get { return SawCurveField; }
	set
	{
	    SawCurveField = value;
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

    public float SawRotationSpeed
    {
	get { return SawRotationSpeedField; }
	set
	{
	    SawRotationSpeedField = value;
	    OnPropertyChanged();
	}
    }
}

public interface ISawModel : IObservableProperties
{
    int   SuperHayContainerMax { get; }
    float SuperSawScaleMultiplier { get; }
    float SawScaleBase { get; }
    float SuperSawDuration { get; }

    float SawAutoSpeed { get; }
    float SawCurve { get; }
    float SawPos { get; }
    float SawRotationSpeed { get; }
}
