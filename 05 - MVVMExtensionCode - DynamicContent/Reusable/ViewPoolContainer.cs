using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;
using Zenject;

public class ViewPoolContainer<T, TViewModel>
	where T : DynamicView<TViewModel>, IPoolable<TViewModel, IMemoryPool>
{
	protected List<MonoMemoryPool<TViewModel, T>> Pools { get; set; }
	protected bool initialized = false;

	protected GameObject[] prefabList;
	protected DiContainer diContainer;
	protected MemoryPoolSettings memoryPoolSettings;

    public ViewPoolContainer(GameObject[]       prefabs,
	                         DiContainer        container,
	                         MemoryPoolSettings poolSettings = null)
    {
	    prefabList = prefabs;
	    diContainer = container;
	    memoryPoolSettings = poolSettings;
    }

    public T SpawnFromPool(int poolIndex, TViewModel viewModel)
    {
	    if (!initialized)
	    {
		    InitializePools();
	    }
	    return Pools[poolIndex].Spawn(viewModel);
    }

    private void InitializePools()
    {
	    initialized = true;
		Pools = new List<MonoMemoryPool<TViewModel, T>>();
	    if (memoryPoolSettings == null)
	    {
		    memoryPoolSettings = new MemoryPoolSettings(10, 100, PoolExpandMethods.Double);
	    }

	    //Logger.Log($"Creating {nameof(ViewPoolContainer<T, TViewModel>)}");

	    foreach (GameObject prefab in prefabList)
	    {
		    //Logger.Log($"Creating Pool for View type {nameof(T)}");

		    if (prefab.GetComponent<T>() != null)
		    {
			    var pool = diContainer.Instantiate<PoolableViewPool<TViewModel, T>>(new object[]
			    {
				    memoryPoolSettings, new ComponentFromPrefabFactory<T>(prefab, diContainer)
			    });
			    Pools.Add(pool);
		    }
		    else
		    {
			    Debug.LogError("Prefab without correct view given to ViewPoolContainer.");
		    }
	    }

    }


}

public class PoolableViewPool<TViewModel, T> : MonoMemoryPool<TViewModel, T>
	where T : View, IPoolable<TViewModel, IMemoryPool>
{
	protected override void OnDespawned(T component)
	{
		component.OnDespawned();
		base.OnDespawned(component);
    }

	protected override void Reinitialize(TViewModel viewModel, T component)
	{
		component.OnSpawned(viewModel, this);
	}
}

public class ComponentFromPrefabFactory<T> : IFactory<T>
	where T : Component
{
	private readonly DiContainer containerField;
	private readonly GameObject  prefabField;

	public ComponentFromPrefabFactory(
		GameObject  prefab,
		DiContainer container)
	{
		containerField = container;
		prefabField = prefab;
	}

	public T Create()
	{
		return containerField.InstantiatePrefabForComponent<T>(prefabField);
	}
}
