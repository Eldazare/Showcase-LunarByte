using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEndLevelDoor : WallBase, ICollidableDoor
{
    [SerializeField] private int KeyIndexField;
    [SerializeField] private Collider DoorCollider;
    [SerializeField] private MeshRenderer DoorMeshRenderer;


	// Fade "Animation"
	[SerializeField] private float DoorFadeTime;
	[SerializeField] private AnimationCurve DoorFadeAnimationCurve;
    private Coroutine DoorFadeCoroutine;
	private float ElapsedTime;
	private Material DoorMaterial;
	private static readonly string TileShaderDropKeyword = "_SliceAmount";

    // Door functionality
    public int KeyIndex { get { return KeyIndexField; } }
	private bool Opened = false;

	void Start()
	{
		DoorMaterial = DoorMeshRenderer.material;
    }

    public void OnEncounter(PlayerView playerView)
    {
	    if (!Opened)
	    {
		    return;
	    }
	    DoorCollider.enabled = false;
	    DoorMeshRenderer.enabled = false;
        playerView.ResetDrawInputMovement();
	    playerView.EndGame();
    }

	// Called by LevelView
    public void OpenDoor()
    {
	    Opened = true;
	    DoorFadeCoroutine = StartCoroutine(_AnimateDoorFade(true));
    }

    protected override void RevertRuntimeChanges()
    {
        DoorCollider.enabled = true;
        DoorMeshRenderer.enabled = true;
        Opened = false;
        StopAllCoroutines();
        DoorFadeCoroutine = null;
        DoorMaterial.SetFloat(TileShaderDropKeyword, DoorFadeAnimationCurve.Evaluate(0f));
    }

    void OnEnable()
    {
	    if (DoorFadeCoroutine != null)
	    {
		    StartCoroutine(_AnimateDoorFade(false));
	    }
    }


    private IEnumerator _AnimateDoorFade(bool resetElapsedTime = true)
    {
	    if (resetElapsedTime)
	    {
		    ElapsedTime = 0.0f;
	    }

	    while (ElapsedTime < DoorFadeTime)
	    {
		    ElapsedTime += Time.deltaTime;
		    DoorMaterial.SetFloat(TileShaderDropKeyword, DoorFadeAnimationCurve.Evaluate(ElapsedTime / DoorFadeTime));
		    yield return 0;
	    }
	    DoorFadeCoroutine = null;
	    DoorMeshRenderer.enabled = false;
    }
}
