using System;
using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;
using Zenject;

public class DynamicView<TViewModel> : View, IPoolable<TViewModel, IMemoryPool>, IDisposable
{
	protected TViewModel  ViewModelField;
	private   IMemoryPool pool;

	public TViewModel ViewModel
	{
		get { return ViewModelField; }
	}

	public void OnSpawned(TViewModel viewModel, IMemoryPool pool1)
	{
		ViewModelField = viewModel;
		transform.position = Vector3.zero;
		pool = pool1;
	}

	public void OnDespawned()
	{
		pool = null;
	}

	public void Dispose()
	{
		pool.Despawn(this);
	}
}
