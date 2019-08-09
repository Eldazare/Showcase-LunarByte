using System;
using System.Collections.Generic;
using LunarByte.MVVM;
using LunarByte.MVVM.Configurations;
using UnityEngine;
using UnityEngine.Events;

public abstract class DynamicViewContainer<TView, TViewModel> : MonoBehaviour,
                                                                IDynamicViewContainer<TView>
	where TView : DynamicView<TViewModel>, IDisposable
{
	public readonly List<TView> Views         = new List<TView>();
	public          UnityEvent  OnViewAdded   = new UnityEvent();
	public          UnityEvent  OnViewRemoved = new UnityEvent();

	protected Action<int> ReorderAction;

	public void Add(TView view)
	{
		Views.Add(view);
		ReorderObject(Views.Count - 1);
		OnViewAdded.Invoke();
	}

	public void Remove(TView view)
	{
		Views.Remove(view);
		view.Dispose();
		ReorderAll();
		OnViewRemoved.Invoke();
	}

	public TView RemoveAt(int index)
	{
		TView retVal = Views[index];
		Views.RemoveAt(index);
		retVal.Dispose();
		ReorderAll();
		OnViewRemoved.Invoke();

		return retVal;
	}

	public void Replace(int index, TView view)
	{
		TView retVal = Views[index];
		Views.RemoveAt(index);
		Views.Insert(index, view);
		ReorderObject(index);
		retVal.Dispose();
	}

	public void Clear()
	{
		foreach (TView v in Views)
		{
			v.Dispose();
		}

		Views.Clear();
	}

	public void Insert(int index, TView view)
	{
		Views.Insert(index, view);
		ReorderAll();
	}

	public void Move(int startIndex, int targetIndex)
	{
		TView item = Views[startIndex];
		Views.RemoveAt(startIndex);
		Views.Insert(targetIndex, item);
		ReorderAll();
	}

	public TView Get(int index)
	{
		return Views[index];
	}

	public             List<Subscription> Subscriptions { get; } = new List<Subscription>();
	public             List<Subscription> Listeners     { get; } = new List<Subscription>();
	protected abstract void               DefaultReorderAction(int index);

	public void SetReorderAction(Action<int> action)
	{
		ReorderAction = action;
		ReorderAll();
	}

	public void ReorderObject(int index)
	{
		if (Views.Count > index)
		{
			if (ReorderAction != null)
			{
				ReorderAction.Invoke(index);
			}
			else
			{
				DefaultReorderAction(index);
			}
		}
	}

	protected void ReorderAll()
	{
		for (var i = 0; i < Views.Count; i++)
		{
			ReorderObject(i);
		}
	}
}

