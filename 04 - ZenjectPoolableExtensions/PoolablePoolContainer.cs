using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PoolablePoolContainer<T> : IInitializable
    where T : PoolableMonoBehaviour
{
    protected List<MemoryPool<T>> Pools { get; set; }

    // From Constructor
    protected GameObject[] PrefabList;
    protected DiContainer DiContainer;
    private int[] InitialPoolSizes;

    // Placeholders
    private MemoryPoolSettings MemoryPoolSettings;
    private GameObject Prefab;

    public PoolablePoolContainer(GameObject[] prefabs,
                                DiContainer container,
                                int[] initialPoolSizes)
    {
        Pools = new List<MemoryPool<T>>();
        PrefabList = prefabs;
        DiContainer = container;
        InitialPoolSizes = initialPoolSizes;

        if (prefabs.Length != initialPoolSizes.Length)
        {
            Debug.LogError("Prefab array and initialPoolSizes don't have matching length");
        }
    }

    public void Initialize()
    {
        for (int i = 0; i < PrefabList.Length; i++)
        {
            Prefab = PrefabList[i];
            if (Prefab.GetComponent<T>() != null)
            {
                MemoryPoolSettings = new MemoryPoolSettings(InitialPoolSizes[i], 100, PoolExpandMethods.OneAtATime);
                var pool = DiContainer.Instantiate<PoolableMonoPool<T>>(new object[]
                {
                    MemoryPoolSettings, new ComponentFromPrefabFactory<T>(Prefab, DiContainer)
                });
                Pools.Add(pool);
            }
            else
            {
                Debug.LogError($"Prefab without correct component given to ViewPoolContainer. With name {Prefab.name}. Component wanted is of type {typeof(T)}");
            }
        }

        Debug.Log($"PoolContainer Initialized, Pool count: {Pools.Count}");
    }

    public T SpawnFromPool(int prefabIndex)
    {
        if (Pools.Count == 0)
        {
            Debug.LogError("PoolablePoolContainer: Either PrefabList had 0 objects or Initialization failed. Check wherever this is bound.");
        }

        if (prefabIndex < Pools.Count)
        {
            return Pools[prefabIndex].Spawn();
        }
        else
        {
            Debug.LogError($"Invalid prefabIndex given to PoolContainer. (PoolCount = {Pools.Count}, indexGiven = {prefabIndex})");
            return null;
        }
    }
}
