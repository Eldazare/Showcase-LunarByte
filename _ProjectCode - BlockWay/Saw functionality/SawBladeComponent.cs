using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBladeComponent : MonoBehaviour, ICollidable
{
    public Action<LevelObjectType, GameObject, int, Collider> OnCollisionAction { private get; set; }


    public void Collide(LevelObjectType levelObjectType, GameObject go, int index, Collider col)
    {
        OnCollisionAction.Invoke(levelObjectType, go, index, col);
    }


}
