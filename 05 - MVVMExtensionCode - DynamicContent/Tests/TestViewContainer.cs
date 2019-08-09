using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestViewContainer : DynamicViewContainer<TestView, ITestViewModel>
{

	public int theCount = 0;
	public int listeners = 0;
	public int subscriptions = 0;


    void Update()
	{
		theCount = Views.Count;
		listeners = Listeners.Count;
		subscriptions = Subscriptions.Count;
	}


	override 
    protected void DefaultReorderAction(int index)
    {
	    Views[index].transform.localPosition = new Vector3(index, 0, 0);
    }
}
