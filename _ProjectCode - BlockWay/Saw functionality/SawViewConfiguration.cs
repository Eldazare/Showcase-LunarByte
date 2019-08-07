using LunarByte.MVVM;
using LunarByte.MVVM.Configurations;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SawViewConfiguration), menuName = "Saw/Configurations/View")]
public class SawViewConfiguration : ViewConfiguration<ISawViewModel, SawViewModel, SawView, SawModel
	, LevelModel, PlayerModel, ScoreModel, SaveModel, TimerService, SocialService, SaveService>
{
	[SerializeField] private SawVisualSettings visualSettings;

	protected override void Configure(SawViewModel  viewModel,
	                                  SawView       view,
	                                  SawModel      sawModel,
	                                  LevelModel    levelModel,
	                                  PlayerModel   playerModel,
	                                  ScoreModel    scoreModel,
	                                  SaveModel     saveModel,
	                                  TimerService  timerService,
	                                  SocialService socialService,
	                                  SaveService   saveService)
	{
		Debug.Log("Configure: " + nameof(SawViewConfiguration));

		// Set Visual values from Settings
		view.CutHayLength = visualSettings.CutHayLength;
		view.MaxMovementSpeed = visualSettings.MaxMovementSpeed;
		view.Radius = visualSettings.Radius;
		view.XMargin = visualSettings.XMargin;
		view.SawParentHeight = visualSettings.SawParentHeight;
		view.SawHandleHeight = visualSettings.SawHandleHeight;
		view.FlyingDuration = visualSettings.FlyingDuration;
		view.FlyingVelocity = visualSettings.FlyingVelocity;
		view.FlyingRotationVelocity = visualSettings.FlyingRotationVelocity;

		// BaseScale And SuperSaw scale bindings
		viewModel
			.Bind<SawViewModel, float>(superSawScale =>
				                           viewModel.SuperSawScaleMultiplier = superSawScale)
			.ToProperty(sawModel, m => m.SuperSawScaleMultiplier,
			            nameof(SawModel.SuperSawScaleMultiplier));

		viewModel.Bind<SawViewModel, float>(sawScaleBase => viewModel.SawScaleBase = sawScaleBase)
		         .ToProperty(sawModel, m => m.SawScaleBase, nameof(SawModel.SawScaleBase));

		view.Bind<SawView, float>(view.OnSuperSawScaleChange).ToProperty(
			viewModel, vm => vm.SuperSawScaleMultiplier,
			nameof(SawViewModel.SuperSawScaleMultiplier));

		view.Bind<SawView, float>(view.OnBaseScaleChanged).ToProperty(
			viewModel, vm => vm.SawScaleBase,
			nameof(SawViewModel.SawScaleBase));

		viewModel.Bind<SawViewModel, float>(sawAutoSpeed => viewModel.SawAutoSpeed = sawAutoSpeed)
		         .ToProperty(sawModel, m => m.SawAutoSpeed, nameof(SawModel.SawAutoSpeed));

		viewModel.Bind<SawViewModel, float>(sawCurve => viewModel.SawCurve = sawCurve)
		         .ToProperty(sawModel, m => m.SawCurve, nameof(SawModel.SawCurve));

		viewModel.Bind<SawViewModel, float>(sawPos => viewModel.SawPos = sawPos)
		         .ToProperty(sawModel, m => m.SawPos, nameof(SawModel.SawPos));

		viewModel
			.Bind<SawViewModel, float>(sawRotationSpeed =>
				                           viewModel.SawRotationSpeed = sawRotationSpeed)
			.ToProperty(sawModel, m => m.SawRotationSpeed, nameof(SawModel.SawRotationSpeed));

		viewModel
			.Bind<SawViewModel, bool>(superSawActive => viewModel.SuperSawActive = superSawActive)
			.ToProperty(playerModel, m => m.SuperSawActive, nameof(PlayerModel.SuperSawActive));

		viewModel.Bind<SawViewModel, bool>(sawActive => viewModel.SawActive = sawActive)
		         .ToProperty(playerModel, m => m.SawActive, nameof(PlayerModel.SawActive));

		view.Bind<SawView, bool>(view.ResetSaw)
		    .ToProperty(viewModel, vm => vm.SawActive, nameof(SawViewModel.SawActive));

		view.Bind<SawView, float>(view.OnSawMove)
		    .ToProperty(viewModel, vm => vm.SawPos, nameof(SawViewModel.SawPos));

		view.Bind<SawView, float>(view.OnSawAutoSpeedChange)
		    .ToProperty(viewModel, vm => vm.SawAutoSpeed, nameof(SawViewModel.SawAutoSpeed));

		view.Bind<SawView, bool>(view.SetSuperSawstate)
		    .ToProperty(viewModel, vm => vm.SuperSawActive, nameof(SawViewModel.SuperSawActive));

		viewModel.EndLevelEvent.AddListener((state, levelObjectIndex) =>
		{
			levelModel.EndLevel(state, playerModel.Points, levelObjectIndex);

			if (state == LevelEndState.Complete)
			{
				socialService.UnlockAchievement(AchievementIDConstants.TheBeginningOfAJourneyID);
				socialService.IncrementAchievement(AchievementIDConstants.GettingThereID, 34);
			}
			else
			{
				socialService.UnlockAchievement(AchievementIDConstants.PractiseMakesPerfectID);
			}
			scoreModel.AddScore(new ScoreObject(playerModel.Points, "Player"), saveModel);
			socialService.AddScore(playerModel.Points, $"leaderboard_{levelModel.CurrentPrestige+1}{levelModel.CurrentLevel}");
			playerModel.UpdateLevelPoints(levelModel, saveService);

			levelModel.ResetTapCount();
			timerService.CancelAllTimers();
		});

		viewModel.ToggleDirectionEvent.AddListener(tapped =>
		{
			if (tapped)
			{
				levelModel.AddTapCount();
			}
		});
		viewModel.GivePointsEvent.AddListener(playerModel.GivePlayerPoints);

		viewModel.GatherSuperHayEvent.AddListener(x => playerModel.GatherSuperHay(
			                                          x, sawModel.SuperHayContainerMax,
			                                          () =>
			                                          {
				                                          socialService.UnlockAchievement(
					                                          AchievementIDConstants.FeverID);
			                                          }));

		viewModel.StartDecreasingSuperHayEvent.AddListener(() => timerService.AddConstantTimer(
			                                                   sawModel.SuperSawDuration,
			                                                   x => playerModel.SetSuperHay(x,
			                                                                                sawModel
				                                                                                .SuperHayContainerMax /
			                                                                                sawModel
				                                                                                .SuperSawDuration),
			                                                   playerModel.DeactivateSuperSaw));
	}
}
