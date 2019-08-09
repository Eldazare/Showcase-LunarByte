using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static Zenject.SignalSubscription;

public abstract class TileBase : PoolableMonoBehaviour, ICheckable
{

    // Note: Collision layers dictate that only Player can collide with Tiles (Tiles-layer)
    // Draw Input checks Floor -Layer (thus we need child object)

    [SerializeField] private TileItemBase TileItem;

    protected abstract void OnTileVariantEnter(PlayerView playerView);

    protected abstract void OnTileVariantExit(PlayerView playerView);

    protected abstract void TileReInitialize();

    public void Check()
    {
        if (TileItem == null)
        {
            if (GetComponentInChildren<TileItemBase>() != null)
            {
                Debug.LogError($"Possible missing reference in prefab: {gameObject.name}");
            }
        }
    }

    public void OnTileEnter(PlayerView playerView)
    {
        if (TileItem != null)
        {
            TileItem.Interact(playerView);
        }
        OnTileVariantEnter(playerView);
    }

    public void OnTileExit(PlayerView playerView)
    {
        OnTileVariantExit(playerView);
    }

    protected override void RevertRuntimeChanges()
    {
        if(TileItem != null)
        {
            TileItem.ReInitialize();
        }
        TileReInitialize();
    }

    public TileItemEnum TileObjectType(out int index)
    {
        if (TileItem == null)
        {
            index = -1;
            return TileItemEnum.None;
        }
        return TileItem.GetTileItemEnum(out index);
    }
}
