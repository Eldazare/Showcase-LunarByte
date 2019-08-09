using System;
using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;
using Zenject;

public class TestView : DynamicView<ITestViewModel>
{
	public int someValue;

	public void ChangeSomeValue(int val)
	{
		someValue = val;
	}
}




