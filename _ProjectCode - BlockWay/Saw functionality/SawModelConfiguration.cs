using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM.Configurations;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SawModelConfiguration), menuName = "Saw/Configurations/" + nameof(SawModelConfiguration))]
public class SawModelConfiguration : ModelConfiguration<ISawModel, SawModel, ISawSettings, SaveModel>
{
    protected override void Configure(SawModel model, ISawSettings settings, SaveModel saveModel)
    {
	Debug.Log($"Configure {nameof(SawModelConfiguration)}");

	if (saveModel.SaveObjectOld.CurrentSawSpeed != 0)
	{
	    model.SawAutoSpeed = saveModel.SaveObjectOld.CurrentSawSpeed;
	}
	else
	{
	    model.SawAutoSpeed = settings.SawAutoSpeed;

	}
        model.SawCurve = settings.SawCurve;
	model.SawPos = settings.SawPos;
	model.SawRotationSpeed = settings.SawRotationSpeed;
	model.SuperHayContainerMax = settings.SuperHayContainerMax;
	model.SuperSawDuration = settings.SuperSawDuration;
	model.SuperSawScaleMultiplier = settings.SuperSawScaleMultiplier;
	model.SawScaleBase = settings.SawScaleBase;
    }
}
