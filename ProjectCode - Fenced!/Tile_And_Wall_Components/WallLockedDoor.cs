using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLockedDoor : WallBase, ICollidableDoor
{
    [SerializeField] private int KeyIndexField;
    [SerializeField] private Collider doorCollider;
    [SerializeField] private MeshRenderer doorMesh;

    public int KeyIndex { get { return KeyIndexField; } }

    public void OnUnlock()
    {
        doorMesh.enabled = false;
        doorCollider.enabled = false;
    }

    public void OnEncounter(PlayerView playerView)
    {
        playerView.AttemptRemoveKey(KeyIndexField, OnUnlock);
    }

    protected override void RevertRuntimeChanges()
    {
        doorMesh.enabled = true;
        doorCollider.enabled = true;
    }
}
