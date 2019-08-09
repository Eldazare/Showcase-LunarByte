using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static Zenject.SignalSubscription;

public abstract class WallBase : PoolableMonoBehaviour
{

}

public interface ICollidableDoor
{
    void OnEncounter(PlayerView playerView);
}