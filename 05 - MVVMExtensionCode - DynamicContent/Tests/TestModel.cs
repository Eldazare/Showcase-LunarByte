using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;

public class TestModel : Model, ITestModel
{
	private  int valueField;

	public int Value
	{
		get { return valueField; }
		set
		{
			valueField = value;
			OnPropertyChanged();
		}
	}
}

public interface ITestModel : IObservableProperties
{
	int Value { get; }
}
