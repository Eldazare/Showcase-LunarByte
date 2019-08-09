using System;
using LunarByte.MVVM.Configurations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarByte.MVVM;

[CreateAssetMenu(fileName = nameof(LevelViewConfigurationWall), menuName = "LevelGeneration/ViewConfig")]
public class LevelViewConfigurationWall : ViewConfiguration<ILevelViewModelWall, LevelViewModelWall, LevelViewWall, LevelModelWall, PlayerModel, InputModel, ILevelDatasContainer>
{
    protected override void Configure(
            LevelViewModelWall viewModel, 
            LevelViewWall view, 
            LevelModelWall levelModel, 
            PlayerModel playerModel, 
            InputModel inputModel,
            ILevelDatasContainer levelDataContainer)
    {
        Debug.Log($"Configuring {nameof(LevelViewConfigurationWall)}");

	//Property binds
        viewModel.Bind<LevelViewModelWall, ILevelDataWall>(levelData => viewModel.LevelData = levelData)
		.ToProperty(levelModel, m => m.LevelData, nameof(LevelModelWall.LevelData));
		
        view.Bind<LevelViewWall, ILevelDataWall>(view.LevelDataChanged)
		.ToProperty(viewModel, vm => vm.LevelData, nameof(LevelViewModelWall.LevelData));

        viewModel.Bind<LevelViewModelWall, LevelCommand>(command => viewModel.Command = command)
		.ToProperty(levelModel, m => m.Command, nameof(LevelModelWall.Command));
		
        view.Bind<LevelViewWall, LevelCommand>(view.ReceiveLevelCommand)
		.ToProperty(viewModel, vm => vm.Command, nameof(LevelViewModelWall.Command));

        viewModel.Bind<LevelViewModelWall, int>(endLevelKeys => viewModel.EndLevelKeys = endLevelKeys)
	        .ToProperty(playerModel, m => m.KeyRing[0], nameof(PlayerModel.KeyRing));

        view.Bind<LevelViewWall, int>((int i) => view.AttemptRemoveEndDoorKeys(0))
            .ToProperty(viewModel, vm => vm.EndLevelKeys, nameof(LevelViewModelWall.EndLevelKeys));

	//Event binds
        viewModel.ResetPlayerModelEvent.AddListener(() => playerModel.ResetPlayer());
        viewModel.ResetInputEvent.AddListener(() => inputModel.ResetInput());
        viewModel.ChooseNextLevelEvent.AddListener(() => levelModel.SetNextLevel(levelDataContainer));  
	viewModel.AttemptRemoveAllKeysEvent.AddListener((int ind, Action successAction) => playerModel.AttemptRemoveMaxKeys(ind, successAction, levelModel.KeysMax[ind]));
        viewModel.UpdateMaxTileObjectsEvent.AddListener((int[] keys, int collectibles) => levelModel.UpdateMaxTileObjects(keys, collectibles));
    }
}
