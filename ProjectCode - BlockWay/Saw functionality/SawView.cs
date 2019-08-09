using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LunarByte.MVVM;
using UnityEngine;
using UnityEngine.Events;

public class SawView : View<ISawViewModel>
{
	[SerializeField] private SawControllerAuto SawController;
    [SerializeField] private SphereCollider BladeCollider;
    [SerializeField] private SawBladeComponent SawComponent;

    //FromSettings
    [HideInInspector] public float CutHayLength;
    [HideInInspector] public float MaxMovementSpeed;
    [HideInInspector] public float Radius;
    [HideInInspector] public float XMargin;
    [HideInInspector] public float SawParentHeight;
    [HideInInspector] public float SawHandleHeight;
    [HideInInspector] public float FlyingDuration;
    [HideInInspector] public Vector3 FlyingVelocity;
    [HideInInspector] public Vector3 FlyingRotationVelocity;

    //Bound
    [HideInInspector] private float SuperSawScale;
    [HideInInspector] private float BaseSawScale;
    [HideInInspector] private float SuperSawScaleMulti;
    [HideInInspector] public float SawAutoSpeed; // Used by SawControllerAuto
    private bool SuperSawActive;

	//Optimization
	[SerializeField] private LayerMask CutHayLayer;

    protected override void OnViewAwake()
    {
        SawComponent.OnCollisionAction = Collide;
    }

    private void ActivateSuperSaw()
	{
        SawController.SetSawBladeScale(SuperSawScale);
		SuperSawActive = true;
		ViewModel.StartDecreasingSuperHay.Dispatch();
        BigVibrate();
    }

	private void DeactivateSuperSaw()
	{
        SawController.SetSawBladeScale(BaseSawScale);
        SuperSawActive = false;
	}

	public void OnSuperSawScaleChange(float newScale)
	{
		SuperSawScaleMulti = newScale;
		UpdateSuperSawScale();

	}

	public void OnBaseScaleChanged(float newScale)
	{
		BaseSawScale = newScale;
		UpdateSuperSawScale();

	}

	private void UpdateSuperSawScale()
	{
		SuperSawScale = BaseSawScale * SuperSawScaleMulti;
	}

	public void SetSuperSawstate(bool active)
	{
		if (active)
		{
			ActivateSuperSaw();
        }
        else
		{
			DeactivateSuperSaw();
		}
    }

	public void Collide(LevelObjectType levelObjectType, GameObject go, int index, Collider col)
	{
		switch (levelObjectType)
		{
			case LevelObjectType.Hay:
				GivePoints(10);
				CutLevelObject(go, col);
				ParticleManagerView.Instance.CreateParticle(go.transform.position, ParticleType.Hay);

                break;

			case LevelObjectType.Rock:
				if (SuperSawActive)
				{
					CutLevelObject(go, col);
					// go.Recycle();
					ParticleManagerView.Instance.CreateParticle(go.transform.position, ParticleType.Rock);
                }
				else
				{
					EndGame(LevelEndState.Fail, index);
				}
				BigVibrate();

				break;
			case LevelObjectType.EndLevelBlock:
				EndGame(LevelEndState.Complete, index);

				break;

			case LevelObjectType.SuperHay:
				GivePoints(100);
				GatherSuperHay(1);
				CutLevelObject(go, col);
                ParticleManagerView.Instance.CreateParticle(go.transform.position,
				                                            ParticleType.SuperHay);

                break;
		}
	}

	private void SmallVibrate()
	{
        if (!Application.isEditor)
        {
#if UNITY_ANDROID
            Vibration.Vibrate(100);
#elif UNITY_IOS
		Vibration.VibratePop();
#endif
        }
    }

	private void BigVibrate()
	{
        if (!Application.isEditor)
        {
#if UNITY_ANDROID
            Vibration.Vibrate(500);
#elif UNITY_IOS
		Vibration.VibratePeek();
#endif
        }
    }

    private void CutLevelObject(GameObject go, Collider col)
	{
		go.transform.localPosition = Vector3.Scale(go.transform.localPosition, new Vector3(1, -1, 1));
        go.transform.localPosition += Vector3.up * CutHayLength;
        // col.enabled = false;
        go.layer = Helper.ToLayer(CutHayLayer);
    }

    private void GivePoints(int points)
    {
	    ViewModel.GivePoints.Dispatch(points);
    }

    private void GatherSuperHay(int amount)
    {
	    ViewModel.GatherSuperHay.Dispatch(amount);
    }

	public void EndGame(LevelEndState endState, int levelObjectIndex)
	{
        ViewModel.EndGame.Dispatch(endState, levelObjectIndex);

        if (endState == LevelEndState.Fail)
        {
	        SawController.SetSawMovement(false);
			SawController.SendBladeFlying();
        }
        BladeCollider.enabled = false;
    }

	public void ResetSaw(bool state)
	{
		SawController.ResetSaw();
		BladeCollider.enabled = true;
    }

	public void OnSawAutoSpeedChange(float amount)
	{
        SawAutoSpeed = amount;
	}

	public void OnSawMove(float amount)
	{
		SawController.ApplySawPos();
	}
}

public interface ICollidable
{
	void Collide(LevelObjectType lot, GameObject go, int index, Collider col);
}
