using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;
using UnityEngine.UI;

public class SuperHayContainerView : View<ISuperHayContainerViewModel>
{

	public Image FillImage;

	public void SetFillAmount(float fillAmount)
	{
		FillImage.fillAmount = fillAmount;
	}
}
