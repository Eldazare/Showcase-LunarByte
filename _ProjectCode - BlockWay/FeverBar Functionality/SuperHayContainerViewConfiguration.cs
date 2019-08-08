using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using LunarByte.MVVM.Configurations;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SuperHayContainerViewConfiguration), menuName = "SuperSaw/Configurations/View")]
public class SuperHayContainerViewConfiguration : ViewConfiguration<ISuperHayContainerViewModel, SuperHayContainerViewModel, SuperHayContainerView, PlayerModel, SawModel>
{
	protected override void Configure(SuperHayContainerViewModel viewModel, SuperHayContainerView view, PlayerModel model, SawModel sawModel)
	{
		
		viewModel.Bind<SuperHayContainerViewModel, float>(fillPercentage => viewModel.FillPercentage = fillPercentage)
			.To(model, m => m.SuperHayCollected, nameof(PlayerModel.SuperHayCollected))
			.UsingConverter((int x) => (float) x / sawModel.SuperHayContainerMax);


		view.Bind<SuperHayContainerView, float>(view.SetFillAmount)
			.ToProperty(viewModel, vm => vm.FillPercentage, nameof(SuperHayContainerViewModel.FillPercentage));
	}
}
