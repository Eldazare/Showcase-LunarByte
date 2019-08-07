using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFragile : TileBase
{
    [SerializeField] private Collider TileCollider;
    [SerializeField] private Collider SolidChildCollider;
    [SerializeField] private MeshRenderer TileRenderer;
    [SerializeField] private Collider FloorChildCollider;

    [Range(1,10)]
    [SerializeField] private int NumberOfSteps = 1;

    private bool TileDropped;
    private int CurrentSteps;

	// Tile drop animation
	[SerializeField] private float TileDropTime = 1.0f;
	[SerializeField] private AnimationCurve TileDropAnimationCurve;
	private Material TileMaterial;
	private static readonly string TileShaderDropKeyword = "_SliceAmount";
	private Coroutine TileDropCoroutine;
	private float ElapsedTime;

    private void Start()
    {
	    TileMaterial = TileRenderer.material;
        RevertRuntimeChanges();
    }

    protected override void OnTileVariantEnter(PlayerView playerView)
    {
        if (!TileDropped)
        {
            CurrentSteps--;
        }
    }

    protected override void OnTileVariantExit(PlayerView playerView)
    {
        if (CurrentSteps <= 0 && !TileDropped)
        {
            TileDropped = true;
            TileCollider.enabled = false;
            FloorChildCollider.enabled = false;

            TileDropCoroutine = StartCoroutine(_AnimateTileDrop());
        }
    }

    protected override void TileReInitialize()
    {
        CurrentSteps = NumberOfSteps;
        TileRenderer.enabled = true;

        TileDropped = false;
        TileCollider.enabled = true;
        FloorChildCollider.enabled = true;
        SolidChildCollider.enabled = false;


        StopAllCoroutines();
        TileDropCoroutine = null;
        TileMaterial.SetFloat(TileShaderDropKeyword, TileDropAnimationCurve.Evaluate(0f));
    }

    void OnEnable()
    {
	    if (TileDropCoroutine != null)
	    {
		    StartCoroutine(_AnimateTileDrop(false));
	    }
    }


    private IEnumerator _AnimateTileDrop(bool resetElapsedTime = true)
    {
	    if (resetElapsedTime)
	    {
		    ElapsedTime = 0.0f;
	    }

	    while (ElapsedTime < TileDropTime)
	    {
		    ElapsedTime += Time.deltaTime;
			TileMaterial.SetFloat(TileShaderDropKeyword, TileDropAnimationCurve.Evaluate(ElapsedTime / TileDropTime));
		    yield return 0;
	    }
	    TileRenderer.enabled = false;
	    SolidChildCollider.enabled = true;
	    TileDropCoroutine = null;
    }
}
