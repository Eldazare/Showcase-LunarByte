using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PoolableMonoBehaviour : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    private IMemoryPool Pool;

    public void Dispose()
    {
        RevertRuntimeChanges();
        Pool.Despawn(this);
    }

    public void OnDespawned()
    {
        Pool = null;
    }

    public void OnSpawned(IMemoryPool pool)
    {
        Pool = pool;
    }

    protected abstract void RevertRuntimeChanges(); // Returns to state where it was pooled

    public void ResetToDefault() // Returns to state where it was after taking out of pool
    {
        RevertRuntimeChanges();
    }
}

public abstract class PoolableMonoBehaviour<TParam> : MonoBehaviour, IPoolable<TParam, IMemoryPool>, IDisposable
{
    private IMemoryPool Pool;

    public void Dispose()
    {
        RevertRuntimeChanges();
        Pool.Despawn(this);
    }

    public void OnDespawned()
    {
        Pool = null;
    }

    public void OnSpawned(TParam p1, IMemoryPool pool)
    {
        Pool = pool;
        OnObjectSpawned(p1);
    }

    protected abstract void RevertRuntimeChanges();

    public abstract void ResetToDefault();

    protected abstract void OnObjectSpawned(TParam p1);
}

public abstract class PoolableMonoBehaviour<TParam, TParam2> : MonoBehaviour, IPoolable<TParam, TParam2, IMemoryPool>, IDisposable
{
    private IMemoryPool Pool;

    public void Dispose()
    {
        RevertRuntimeChanges();
        Pool.Despawn(this);
    }

    public void OnDespawned()
    {
        Pool = null;
    }

    public void OnSpawned(TParam p1, TParam2 p2, IMemoryPool pool)
    {
        Pool = pool;
        OnObjectSpawned(p1,p2);
    }

    protected abstract void RevertRuntimeChanges();

    public abstract void ResetToDefault();

    protected abstract void OnObjectSpawned(TParam p1, TParam2 p2);
}