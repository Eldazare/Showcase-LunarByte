using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PoolableMonoPool<TPoolable> : MemoryPool<TPoolable>
    where TPoolable : PoolableMonoBehaviour
{
    Transform _originalParent;

    [Inject]
    public PoolableMonoPool()
    {
    }

    protected override void OnCreated(TPoolable item)
    {
        item.gameObject.SetActive(false);
        _originalParent = item.transform.parent;
    }

    protected override void OnDestroyed(TPoolable item)
    {
        GameObject.Destroy(item.gameObject);
    }

    protected override void OnDespawned(TPoolable item)
    {
        item.OnDespawned();
        item.gameObject.SetActive(false);

        if (item.transform.parent != _originalParent)
        {
            item.transform.SetParent(_originalParent, false);
        }
    }

    protected override void Reinitialize(TPoolable item)
    {
        item.gameObject.SetActive(true);
        item.OnSpawned(this);
    }
}

public class PoolableMonoPool<TParam1, TPoolable> : MemoryPool<TParam1, TPoolable>
    where TPoolable : PoolableMonoBehaviour<TParam1>
{
    Transform _originalParent;

    [Inject]
    public PoolableMonoPool()
    {
    }

    protected override void OnCreated(TPoolable item)
    {
        item.gameObject.SetActive(false);
        _originalParent = item.transform.parent;
    }

    protected override void OnDestroyed(TPoolable item)
    {
        GameObject.Destroy(item.gameObject);
    }

    protected override void OnDespawned(TPoolable item)
    {
        item.OnDespawned();
        item.gameObject.SetActive(false);

        if (item.transform.parent != _originalParent)
        {
            item.transform.SetParent(_originalParent, false);
        }
    }

    protected override void Reinitialize(TParam1 param1, TPoolable item)
    {
        item.gameObject.SetActive(true);
        item.OnSpawned(param1, this);
    }
}

public class PoolableMonoPool<TParam1, TParam2, TPoolable> : MemoryPool<TParam1, TParam2, TPoolable>
    where TPoolable : PoolableMonoBehaviour<TParam1, TParam2>
{
    Transform _originalParent;

    [Inject]
    public PoolableMonoPool()
    {
    }

    protected override void OnCreated(TPoolable item)
    {
        item.gameObject.SetActive(false);
        _originalParent = item.transform.parent;
    }

    protected override void OnDestroyed(TPoolable item)
    {
        GameObject.Destroy(item.gameObject);
    }

    protected override void OnDespawned(TPoolable item)
    {
        item.OnDespawned();
        item.gameObject.SetActive(false);

        if (item.transform.parent != _originalParent)
        {
            item.transform.SetParent(_originalParent, false);
        }
    }

    protected override void Reinitialize(TParam1 param1, TParam2 param2, TPoolable item)
    {
        item.gameObject.SetActive(true);
        item.OnSpawned(param1, param2, this);
    }
}